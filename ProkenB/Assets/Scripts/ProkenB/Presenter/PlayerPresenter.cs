using System;
using Ludiq;
using ProkenB.Game;
using ProkenB.Model;
using ProkenB.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ProkenB.Presenter
{
    public class PlayerPresenter : MonoBehaviour
    {
        private PlayerView m_view = null;
        private PlayerModel m_model = null;

        private PlayerView View => m_view;

        private void Awake()
        {
            // ここでモデルへの追加と初期化を行う
            var model = new PlayerModel();

            m_view = gameObject.GetOrAddComponent<PlayerView>();
            m_model = model;

            GameManager.Instance.Model.AddPlayer(model, m_view.IsLocal);

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
                // TODO: モデルに同期
                View.Position = m_model.Position;
            }
            else
            {
                // TODO: モデルを同期
                m_model.Position = View.Position;
            }

            // 位置の同期
            View.PositionChanged
                .Subscribe(v => m_model.Position = v);

            m_model.PositionAsObservable
                .Subscribe(v => View.Position = v);

            View.GoalReached
                .Subscribe(v => m_model.GoalTime = v);

            // 削除
            var model = m_model;
            View.OnDestroyAsObservable()
                .Subscribe(_ => model.Destroy());
        }
    }
}
