using System;
using UnityEngine;

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
        
        // メンバ変数
        private GameObject m_mainPlayer = null;
        private GameObject m_stage = null;

        private static GameManager m_manager = null;
        public static GameManager Instance => m_manager ?? throw new NullReferenceException("GameManager not started");
        
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
            
            // TODO: モデルの初期化
            
            // TODO: プレイヤーの初期化処理はPlayerFactoryにやらせる（ここでPresenter/Viewも用意）
            m_mainPlayer = Instantiate(playerPrefab);
            
            // TODO: ステージの初期化処理もStageFactoryにやらせる
            m_stage = Instantiate(stagePrefab);
            
            // TODO: UIをUIFactoryにつくらせる
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
            
            // ここでちゃんとGameManagerを破棄する
            m_manager = null;
        }
    }
}
