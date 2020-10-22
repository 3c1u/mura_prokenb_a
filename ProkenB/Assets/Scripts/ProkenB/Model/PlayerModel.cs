using UnityEngine;
using UniRx;
using System;

namespace ProkenB.Model
{
    public class PlayerModel
    {
        private ReactiveProperty<Vector3> m_position = new ReactiveProperty<Vector3>(new Vector3());

        /// <summary>
        /// プレイヤーの位置
        /// </summary>
        public Vector3 Position
        {
            get => m_position.Value;
            set => m_position.SetValueAndForceNotify(value);
        }

        /// <summary>
        /// プレイヤー位置のObservableオブジェクト．
        /// </summary>
        public IObservable<Vector3> PositionAsObservable => m_position.AsObservable();

        private ReactiveProperty<float?> m_goalTime = new ReactiveProperty<float?>(null);

        /// <summary>
        /// ゴールした時間．`null`ならまた未達成．
        /// </summary>
        public float? GoalTime
        {
            get => m_goalTime.Value;
            set => m_goalTime.SetValueAndForceNotify(value);
        }

        /// <summary>
        /// ゴールしたかどうか．
        /// </summary>
        public bool HasReachedGoal => m_goalTime.Value != null;
    }
}
