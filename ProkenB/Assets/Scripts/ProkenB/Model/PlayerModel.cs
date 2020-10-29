using UnityEngine;
using UniRx;
using System;
using Photon.Pun.Demo.PunBasics;

namespace ProkenB.Model
{
    public class PlayerModel
    {
        private WeakReference<GameModel> m_parent = new WeakReference<GameModel>(Game.GameManager.Instance.Model);
        private ReactiveProperty<Vector3> m_position = new ReactiveProperty<Vector3>(new Vector3());

        /// <summary>
        /// プレイヤーの位置
        /// </summary>
        public Vector3 Position
        {
            get => m_position.Value;
            set => m_position.Value = value;
        }

        /// <summary>
        /// プレイヤー位置のObservableオブジェクト．
        /// </summary>
        public IObservable<Vector3> PositionAsObservable => m_position.AsObservable();

        private ReactiveProperty<float?> m_goalTime = new ReactiveProperty<float?>(null);

        public IObservable<float?> GoalTimeAsObservable => m_goalTime.AsObservable();

        /// <summary>
        /// ゴールした時間．`null`ならまた未達成．
        /// </summary>
        public float? GoalTime
        {
            get => m_goalTime.Value;
            set => m_goalTime.Value = value;
        }

        /// <summary>
        /// ゴールしたかどうか．
        /// </summary>
        public bool HasReachedGoal => m_goalTime.Value != null;

        private ReactiveProperty<bool> m_isActive = new ReactiveProperty<bool>(true);

        public bool IsActive
        {
            get => m_isActive.Value;
            set => m_isActive.Value = value;
        }

        public IObservable<bool> IsActiveAsObservable => m_isActive.AsObservable();

        public void Destroy()
        {
            GameModel parent = null;

            if (m_parent?.TryGetTarget(out parent) ?? false)
            {
                parent.RemovePlayer(this);
            }
        }
    }
}
