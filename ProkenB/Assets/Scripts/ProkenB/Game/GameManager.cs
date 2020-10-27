using System;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using ProkenB.Detector;
using UnityEngine;
using ProkenB.Model;
using ProkenB.Presenter;
using UniRx;

namespace ProkenB.Game
{
    /// <summary>
    /// ゲーム内のライフサイクルを担うクラス．シーンの最初には基本的にコイツが置かれるし，
    /// コイツが一番偉い．
    ///
    /// つまり，ライフサイクル上ではこれがもっとも上位で，ほかのモデル，ビュー，プレゼンターは
    /// コイツと一生をともにする．DestroyOnLoadとかではないので，基本的にはシーンとともに一生を終える．儚い命よ...．
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks
    {
        /* GameMangerのPrefabにくっつけとくやつら．*/
        [SerializeField] private GameObject stagePrefab = null;
        [SerializeField] private MicrophoneSoundDetector detector = null;

        // メンバ変数
        private GameObject m_mainPlayer = null;
        private GameObject m_stage = null;

        // モデル
        private GameModel m_model = null;
        private TaskCompletionSource<bool> m_modelWaiter = new TaskCompletionSource<bool>();
        private object m_lock = new object();

        private static GameManager m_manager = null;

        public static GameManager Instance => m_manager ?? throw new NullReferenceException("GameManager not started");

        private GamePresenter m_presenter = null;

        private float m_startTime = 0;
        public float Now => Time.time - m_startTime;

        public MicrophoneSoundDetector Detector => detector;
        public GameModel Model
        {
            get => m_model;
            set
            {
                lock (m_lock)
                {
                    m_model = value;
                    m_modelWaiter?.SetResult(true);
                    m_modelWaiter = null;
                }
            }
        }

        /* */
        private ReactiveProperty<bool> m_isMaster = new ReactiveProperty<bool>();
        public bool IsMaster
        {
            get => m_isMaster.Value;
            private set => m_isMaster.Value = value;
        }

        public IObservable<bool> IsMasterAsObservable => m_isMaster.AsObservable();

        private bool m_roomJoined = false;

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
        /* */

        /// <summary>
        /// すべてのゲームオブジェクトが初期化されたあとに呼ばれる奴．
        ///
        /// ここで，ゲームシーン上にいろんなオブジェクトを配置して，ゲームモデルを構築する．
        /// </summary>
        void Awake()
        {
            if (m_manager)
            {
                throw new Exception("GameManager should not be instantiated twice");
            }

            m_manager = this;

            m_presenter = gameObject.AddComponent<GamePresenter>();

            // タイマーの初期化（本当はGameModelのステート変更によって初期化されるべき）
            m_startTime = Time.time;

            // Photonの初期化
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);

            m_customProperties["TotalPlayers"] = 0;
            m_customProperties["Lifecycle"] = (int)GameModel.GameLifecycle.NotInitialized;
        }

        public override void OnJoinedRoom()
        {
            IsMaster = PhotonNetwork.IsMasterClient;

            // プロパティの同期
            if (!IsMaster)
            {
                m_customProperties = PhotonNetwork.CurrentRoom.CustomProperties;

                Lifecycle = Lifecycle;
                TotalPlayers = TotalPlayers;

                if (Lifecycle == GameModel.GameLifecycle.Playing || Lifecycle == GameModel.GameLifecycle.Finish)
                {
                    Debug.LogError("an attempt to join the ongoing game should not occur");
                }
            }
            else
            {
                UpdateParameters();
            }

            m_roomJoined = true;

            var players = TotalPlayers;
            Debug.Log($"game initialized with {players} player(s)");

            if (players > Constant.MAX_PLAYERS)
            {
                // プレイヤー数上限の場合は，シーンの生成を行わない．
                Debug.LogError("too many players in the same room: we do not handle that");
            }

            // ステージの初期化
            m_stage = Instantiate(stagePrefab);

            // プレイヤーの配置
            m_mainPlayer = PhotonNetwork.Instantiate(
                "Player",
                new Vector3(10.10229f, 2.368004f, 21.034f)
                + new Vector3(20.0f, 0, 0) * players,
                Quaternion.identity);

            if (m_mainPlayer == null)
            {
                Debug.LogError("main player creation failed");
            }

            // UIをUIFactoryにつくらせる
        }

        private void UpdateParameters()
        {
            if (IsMaster && m_roomJoined)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(m_customProperties);
            }
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            m_ownershipChanged.OnNext(newMasterClient);

            if (!newMasterClient.IsLocal)
            {
                IsMaster = false;
                return;
            }

            IsMaster = true;

            Debug.Log("ownership moved to local");

            // マスターとしての処理を引き継ぐ
        }

        private void Update()
        {
            if (!m_roomJoined)
            {
                return;
            }

            m_timer += Time.deltaTime;

            if (!IsMaster)
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

        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            if (PhotonNetwork.IsMasterClient)
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

        /// <summary>
        /// ここでオブジェクトの後片付けを行う．
        /// </summary>
        void OnDestroy()
        {
            // TODO: Presenter（とその中のView）を破棄
            Destroy(m_mainPlayer);
            m_mainPlayer = null;

            Destroy(m_stage);
            m_stage = null;

            m_model = null;

            // ここでちゃんとGameManagerを破棄する
            m_manager = null;

            m_roomJoined = false;
        }
    }
}
