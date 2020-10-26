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

        private async void Start()
        {
            await GameManager.Instance.WaitForModel();
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
                .Do(value => gameStateView.Lifecycle = value)
                .Subscribe();

            m_model.IsMasterAsObservable
                .Do(value => gameStateView.IsMaster = value)
                .Subscribe();
        }
    }
}
