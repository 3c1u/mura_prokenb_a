using System;
using Ludiq;
using ProkenB.Game;
using ProkenB.Model;
using ProkenB.View;
using UniRx;
using UnityEngine;

namespace ProkenB.Presenter
{
    public class GamePresenter : MonoBehaviour
    {
        private GameManager m_view = null;
        private GameModel m_model = null;

        private void Awake()
        {
            m_view = GameManager.Instance;
            m_model = new GameModel();

            GameManager.Instance.Model = m_model;
            Bind();
        }

        private void OnDestroy()
        {
            m_view = null;
            m_model = null;
        }

        public void Bind(bool synchronizeToModel = false)
        {
            if (synchronizeToModel)
            {
                // TODO:
            }
            else
            {
                m_model.IsMaster = m_view.IsMaster;
                m_model.Lifecycle = m_view.Lifecycle;
            }

            var view = m_view;
            var model = m_model;

            model.TotalPlayers = view.TotalPlayers;
            model.IsMaster = view.IsMaster;

            view.LifecycleChanged
                .Subscribe(l => model.Lifecycle = l);

            view.OwnershipChanged
                .Subscribe(value => model.IsMaster = value.IsMasterClient);

            m_view.TotalPlayersChanged
                .Subscribe(value => model.TotalPlayers = value);

            model.TotalPlayersAsObservable
                .Subscribe(value => view.TotalPlayers = value);

            view.IsMasterAsObservable
                .Subscribe(value => model.IsMaster = value);
        }
    }
}
