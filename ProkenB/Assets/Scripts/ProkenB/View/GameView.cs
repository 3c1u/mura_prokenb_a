using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using ProkenB.Model;
using UnityEngine;
using UniRx;

namespace ProkenB.View
{
    public class GameView : MonoBehaviourPunCallbacks
    {
        private PhotonView m_photonView = null;

        private bool m_isMaster = false;
        public bool IsMaster => m_isMaster;

        private Subject<Player> m_ownershipChanged = new Subject<Player>();
        public IObservable<Player> OwnershipChanged => m_ownershipChanged.AsObservable();

        private Subject<GameModel.GameLifecycle> m_lifecycle = new Subject<GameModel.GameLifecycle>();
        public GameModel.GameLifecycle Lifecycle
        {
            get => m_customProperties["Lifecycle"] is int value ? (GameModel.GameLifecycle)value : GameModel.GameLifecycle.NotInitialized;
            set
            {
                m_customProperties["Lifecycle"] = (int)value;
                m_lifecycle.OnNext(value);
                UpdateParameters();
            }
        }
        public IObservable<GameModel.GameLifecycle> LifecycleChanged => m_lifecycle.AsObservable();

        private Subject<int> m_totalPlayers = new Subject<int>();
        public IObservable<int> TotalPlayersChanged => m_totalPlayers.AsObservable();

        public int TotalPlayers
        {
            get => m_customProperties["TotalPlayers"] is int value ? value : 0;
            set
            {
                m_customProperties["TotalPlayers"] = value;
                m_totalPlayers.OnNext(value);
                UpdateParameters();
            }
        }

        private float m_timer = 0f;

        private Hashtable m_customProperties = new Hashtable();

        void Awake()
        {
            m_photonView = GetComponent<PhotonView>();

            // GameViewが自分で作られた場合は，マスターになる
            m_isMaster = PhotonNetwork.IsMasterClient;

            m_customProperties["TotalPlayers"] = 0;
            m_customProperties["Lifecycle"] = (int)GameModel.GameLifecycle.NotInitialized;
        }

        public override void OnJoinedRoom()
        {
            if (!m_isMaster)
            {
                m_customProperties = PhotonNetwork.LocalPlayer.CustomProperties;

                Lifecycle = Lifecycle;
                TotalPlayers = TotalPlayers;

                if (Lifecycle == GameModel.GameLifecycle.Playing || Lifecycle == GameModel.GameLifecycle.Finish)
                {
                    Debug.LogError("an attempt to join the ongoing game should not occur");
                }

                return;
            }

            UpdateParameters();
        }

        private void UpdateParameters()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(m_customProperties);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            m_ownershipChanged.OnNext(newMasterClient);

            if (!newMasterClient.IsLocal)
            {
                m_isMaster = false;
                return;
            }

            // 新しいマスタークライアントに所有権を渡す
            m_photonView.TransferOwnership(newMasterClient);
            m_isMaster = true;

            Debug.Log("ownership moved to local");

            // マスターとしての処理を引き継ぐ
        }

        private void Update()
        {
            m_timer += Time.deltaTime;

            if (!m_isMaster)
            {
                return;
            }

            switch (Lifecycle)
            {
                case GameModel.GameLifecycle.NotInitialized:
                    if (TotalPlayers >= Constant.MIN_PLAYERS)
                    {
                        Lifecycle = GameModel.GameLifecycle.Ready;
                        m_timer = 0;
                    }
                    break;
                case GameModel.GameLifecycle.Ready:
                    if (m_timer >= Constant.WAITTIME_GAME_START)
                    {
                        Lifecycle = GameModel.GameLifecycle.Playing;
                        m_timer = 0;
                    }

                    if (TotalPlayers == 1)
                    {
                        // 待機状態にロールバック
                        Lifecycle = GameModel.GameLifecycle.NotInitialized;
                        m_timer = 0;
                    }
                    break;
                case GameModel.GameLifecycle.Playing:
                    if (TotalPlayers == 1 || false) // TODO: ゴール判定
                    {
                        Lifecycle = GameModel.GameLifecycle.Finish;
                        m_timer = 0;
                    }
                    break;
                case GameModel.GameLifecycle.Finish:
                    if (false) // 全員のゴール判定
                    {
                        // TODO: シーン遷移とか
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
        {
            if (target.ActorNumber != photonView.OwnerActorNr || target.IsLocal)
            {
                return;
            }

            if (changedProps["TotalPlayers"] is int totalPlayers)
            {
                m_totalPlayers.OnNext(totalPlayers);
                m_customProperties["TotalPlayers"] = totalPlayers;
            }

            if (changedProps["Lifecycle"] is int lifecycle)
            {
                m_lifecycle.OnNext((GameModel.GameLifecycle)lifecycle);
                m_customProperties["Lifecycle"] = lifecycle;
            }
        }
    }
}
