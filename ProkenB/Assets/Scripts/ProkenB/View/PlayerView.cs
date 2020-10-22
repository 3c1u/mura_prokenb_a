using System;
using UnityEngine;
using UniRx;
using ProkenB.Game;
using Photon.Pun;

namespace ProkenB.View
{
    public class PlayerView : MonoBehaviour
    {
        private Subject<Vector3> m_positionChanged = new Subject<Vector3>();
        public IObservable<Vector3> PositionChanged => m_positionChanged.AsObservable();

        private Subject<float> m_goalReached = new Subject<float>();
        public IObservable<float> GoalReached => m_goalReached.AsObservable();

        // Photon関連
        private PhotonView  m_photonView = null;

        void Awake()
        {
            m_photonView = GetComponent<PhotonView>();
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (m_photonView && !m_photonView.IsMine)
            {
                return;
            }


        }

        // Colliderから呼び出されてほしい
        public void NotifyGoalReached(object sender)
        {
            m_goalReached.OnNext(GameManager.Instance.Now);
        }
    }
}
