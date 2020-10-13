using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

namespace ProkenB.Networking
{
    /// <summary>
    /// PUNを利用した対戦を管理するクラス．
    /// </summary>
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// NetworkManagerが生成（Instantiate）された直後に実行されるイベント．
        /// ネットワーク周りの初期化とかを行う．
        /// </summary>
        void Awake()
        {
            // Photonのネットワークに接続する．
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {

        }
    }
}
