using System;
using Bolt;
using UnityEngine;
using UniRx;
using ProkenB.Game;
using Photon.Pun;
using ProkenB.Model;

namespace ProkenB.View
{
    public class PlayerView : MonoBehaviour
    {
        private Subject<Vector3> m_positionChanged = new Subject<Vector3>();
        public IObservable<Vector3> PositionChanged => m_positionChanged.AsObservable();
        public Subject<bool> m_destroy = new Subject<bool>();

        private Subject<float> m_goalReached = new Subject<float>();
        public IObservable<float> GoalReached => m_goalReached.AsObservable();

        private PhotonView m_photonView = null;
        private Rigidbody m_rigidBody = null;

        public bool IsLocal => m_photonView.IsMine;

        private readonly Vector3 m_cameraOffset = new Vector3(0, 20, -20) - new Vector3(10.10229f, 2.368004f, 21.034f);

        public Vector3 Position
        {
            get => gameObject.transform.position;
            set => gameObject.transform.position = value;
        }

        void Awake()
        {
            m_photonView = GetComponent<PhotonView>();
            m_rigidBody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (m_photonView && !m_photonView.IsMine)
            {
                return;
            }

            var cameraTr = Camera.main.transform;
            var position = transform.position;

            cameraTr.position = position + m_cameraOffset;
            m_positionChanged.OnNext(position);
        }

        private void FixedUpdate()
        {
            if (m_photonView && !m_photonView.IsMine)
            {
                return;
            }

            if (GameManager.Instance.Model.Lifecycle != GameModel.GameLifecycle.Playing)
            {
                m_rigidBody.velocity = Vector3.zero;
                return;
            }

            // 入力を取る
            var breath = GameManager.Instance.Detector.Breath;
            var _voice = GameManager.Instance.Detector.Voice;

            if (breath)
            {
                m_rigidBody.AddForce(new Vector3(0, 0, 60));
            }

            m_rigidBody.AddForce(new Vector3(0, 0, 200) * Input.GetAxis("Vertical"));
            m_rigidBody.AddForce(new Vector3(200, 0, 0) * Input.GetAxis("Horizontal"));
        }

        // Colliderから呼び出されてほしい
        public void NotifyGoalReached()
        {
            if (!IsLocal)
            {
                return;
            }

            m_photonView.RPC("GoalReachedMessage", RpcTarget.AllViaServer, GameManager.Instance.Timer);
        }

        [PunRPC]
        public void GoalReachedMessage(float time)
        {
            m_goalReached.OnNext(time);
        }
    }
}
