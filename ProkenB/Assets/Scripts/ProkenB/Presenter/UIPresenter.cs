using System;
using Ludiq;
using ProkenB.Game;
using ProkenB.Model;
using ProkenB.View;
using ProkenB.View.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ProkenB.Presenter
{
    public class UIPresenter : MonoBehaviour
    {
        private GameModel m_model = null;

        [SerializeField]
        private GameStateView gameStateView = null;

        private void Start()
        {
            m_model = GameManager.Instance.Model;
            Bind();
        }

        private void OnDestroy()
        {
            m_model = null;
        }

        public void Bind()
        {
            gameStateView.Lifecycle = m_model.Lifecycle;
            gameStateView.IsMaster = m_model.IsMaster;

            m_model.LifecycleAsObservable
                .Subscribe(value => gameStateView.Lifecycle = value);

            m_model.TotalPlayersAsObservable
                .Subscribe(value => gameStateView.TotalPlayers = value);

            m_model.IsMasterAsObservable
                .Subscribe(value => gameStateView.IsMaster = value);

            m_model.LocalPlayerAsObservable
                .Subscribe(player =>
                    player?.GoalTimeAsObservable
                        .Subscribe(v => gameStateView.HasReachedGoal = v != null)
                );
        }
    }
}
