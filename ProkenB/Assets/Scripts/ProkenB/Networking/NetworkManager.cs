using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Threading.Tasks;

namespace ProkenB.Networking
{
    /// <summary>
    /// PUNを利用した対戦を管理するクラス．
    /// </summary>
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private TaskCompletionSource<bool> m_taskCompletionSource = null;

        void Awake()
        {
        }

        void Start()
        {
        }

        public async Task ConnectAsync()
        {
            m_taskCompletionSource = new TaskCompletionSource<bool>();

            // まともなコールバック処理くらいさせろカス
            PhotonNetwork.ConnectUsingSettings();

            await m_taskCompletionSource.Task;
        }

        /// <summary>
        /// Photonのサーバーに接続された際に呼び出されるコールバック
        /// </summary>
        public override void OnConnectedToMaster()
        {
            // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
            PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
        }

        /// <summary>
        /// ルームに入ったときに呼び出されるコールバック．
        /// </summary>
        public override void OnJoinedRoom()
        {
            m_taskCompletionSource.SetResult(true);
        }
    }
}
