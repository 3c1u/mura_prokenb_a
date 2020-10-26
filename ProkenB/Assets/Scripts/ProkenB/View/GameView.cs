using System;
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

        private ReactiveProperty<GameModel.GameLifecycle> m_lifecycle = new ReactiveProperty<GameModel.GameLifecycle>(GameModel.GameLifecycle.NotInitialized);
        public GameModel.GameLifecycle Lifecycle
        {
            get => m_lifecycle.Value;
            set => m_lifecycle.Value = value;
        }
        public IObservable<GameModel.GameLifecycle> LifecycleChanged => m_lifecycle.AsObservable();

        private ReactiveProperty<int> m_totalPlayers = new ReactiveProperty<int>(0);

        public int TotalPlayers
        {
            get => m_totalPlayers.Value;
            set => m_totalPlayers.Value = value;
        }

        private float m_timer = 0f;

        void Awake()
        {
            m_photonView = GetComponent<PhotonView>();

            // GameViewが自分で作られた場合は，GameView
            m_isMaster = m_photonView.IsMine;
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
                    if (TotalPlayers >= 2)
                    {
                        m_photonView.RPC("GameReady", RpcTarget.All);
                    }
                    break;
                case GameModel.GameLifecycle.Ready:
                    if (m_timer >= 5.0f)
                    {
                        m_photonView.RPC("StartGame", RpcTarget.All);
                    }
                    break;
                case GameModel.GameLifecycle.Playing:
                    break;
                case GameModel.GameLifecycle.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [PunRPC]
        void StartGame()
        {
            Debug.Log("game started");

            Lifecycle = GameModel.GameLifecycle.Playing;
        }

        [PunRPC]
        void GameReady()
        {
            Debug.Log("game is ready");

            Lifecycle = GameModel.GameLifecycle.Ready;
            m_timer = 0;
        }
    }
}
