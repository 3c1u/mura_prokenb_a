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
        private GameView m_view = null;
        private GameModel m_model = null;

        private void Awake()
        {
            m_view = gameObject.GetOrAddComponent<GameView>();
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

            m_view.TotalPlayers = m_model.TotalPlayers;

            m_view.LifecycleChanged
                .Do(l => m_model.Lifecycle = l)
                .Subscribe();

            m_view.OwnershipChanged
                .Do(value => m_model.IsMaster = value.IsMasterClient)
                .Subscribe();

            m_model.TotalPlayersAsObservable
                .Do(value => m_view.TotalPlayers = value)
                .Subscribe();
        }
    }
}
