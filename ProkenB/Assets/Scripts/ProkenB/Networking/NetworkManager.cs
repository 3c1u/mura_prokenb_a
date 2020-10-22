using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

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
        }

        void Start()
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
        }
        
        /// <summary>
        /// Photonのサーバーに接続された際に呼び出されるコールバック
        /// </summary>
        public override void OnConnectedToMaster() {
            // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
            PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
        }
        
        /// <summary>
        /// ルームに入ったときに呼び出されるコールバック．
        /// </summary>
        public override void OnJoinedRoom(){
            Debug.Log("room joined");
        }
    }
}
