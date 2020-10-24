using System;
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

        private PlayerView View => m_view ?? (m_view = gameObject.AddComponent<PlayerView>());

        private void Awake()
        {
            // ここでモデルへの追加と初期化を行う
            var model = new PlayerModel();
            GameManager.Instance.Model.Players.Add(model);
            Bind(model);
        }

        public void Bind(PlayerModel model, bool synchronizeToModel = false)
        {
            m_model = model;

            if (synchronizeToModel)
            {
                // TODO: モデルに同期
                View.Position = model.Position;
            }
            else
            {
                // TODO: モデルを同期
                m_model.Position = View.Position;
            }

            // 位置の同期
            View.PositionChanged
                .Do(v => m_model.Position = v)
                .Subscribe();

            m_model.PositionAsObservable
                .Do(v => View.Position = v)
                .Subscribe();

            // 削除
            View.OnDestroyAsObservable()
                .Do(_ => m_model.Destroy())
                .Subscribe();
        }
    }
}
