using System;
using Photon.Pun;
using ProkenB.Detector;
using UnityEngine;
using ProkenB.Networking;
using ProkenB.Model;

namespace ProkenB.Game
{
    /// <summary>
    /// ゲーム内のライフサイクルを担うクラス．シーンの最初には基本的にコイツが置かれるし，
    /// コイツが一番偉い．
    ///
    /// つまり，ライフサイクル上ではこれがもっとも上位で，ほかのモデル，ビュー，プレゼンターは
    /// コイツと一生をともにする（NetworkManagerも！）．DestroyOnLoadとかではないので
    /// ，基本的にはシーンとともに一生を終える．儚い命よ...．
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /* GameMangerのPrefabにくっつけとくやつら．*/
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject stagePrefab = null;
        [SerializeField] private MicrophoneSoundDetector detector = null;

        // メンバ変数
        private GameObject m_mainPlayer = null;
        private GameObject m_stage = null;

        // モデル
        private GameModel m_model = null;

        private NetworkManager m_networkManager = null;

        private static GameManager m_manager = null;
        public static GameManager Instance => m_manager ?? throw new NullReferenceException("GameManager not started");

        private float m_startTime = 0;
        public float Now => Time.time - m_startTime;

        public MicrophoneSoundDetector Detector => detector;
        public GameModel Model => m_model;

        /// <summary>
        /// すべてのゲームオブジェクトが初期化されたあとに呼ばれる奴．
        ///
        /// ここで，ゲームシーン上にいろんなオブジェクトを配置して，ゲームモデルを構築する．
        /// </summary>
        async void Awake()
        {
            if (m_manager)
            {
                throw new Exception("GameManager should not be instantiated twice");
            }

            m_manager = this;

            // タイマーの初期化（本当はGameModelのステート変更によって初期化されるべき）
            m_startTime = Time.time;

            // ネットワークマネージャーの初期化
            m_networkManager = gameObject.AddComponent<NetworkManager>();

            await m_networkManager.ConnectAsync();
            SetupGame();
        }

        void SetupGame()
        {
            // モデルの初期化
            m_model = new GameModel();

            // ステージの初期化
            m_stage = Instantiate(stagePrefab);
            m_mainPlayer = PhotonNetwork.Instantiate(
                "Player",
                new Vector3(10.10229f, 2.368004f, 21.034f),
                Quaternion.identity);

            if (m_mainPlayer == null)
            {
                Debug.LogError("main player creation failed");
            }

            // UIをUIFactoryにつくらせる
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

            // TODO: モデルを破棄
            m_model = null;

            // ここでちゃんとGameManagerを破棄する
            m_manager = null;
        }

        void Update()
        {
            //
        }
    }
}
