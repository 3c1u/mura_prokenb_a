using UnityEngine;
using Photon.Pun;
using System.Collections;
// using Photon.Realtime;

namespace ProkenB.Networking
{
    /// <summary>
    /// PUNを利用した対戦を管理するクラス．
    /// </summary>
    public class NetworkManager : MonoBehaviour
    {
        /// <summary>
        /// NetworkManagerが生成（Instantiate）された直後に実行されるイベント．
        /// ネットワーク周りの初期化とかを行う．
        /// </summary>
        void Awake()
        {
        }

        IEnumerator Start()
        {
            // Photonのネットワークに接続する．
            if (PhotonNetwork.ConnectUsingSettings())
            {
                Debug.Log("Photon connected");
            }
            else
            {
                Debug.LogError("failed to connect to Photon Network");
            }

            yield break;
        }
    }
}
