using ProkenB.Model;
using ProkenB.View;
using UnityEngine;

namespace ProkenB.Presenter
{
    public class PlayerPresenter : MonoBehaviour
    {
        private PlayerView m_view = null;
        private PlayerModel m_model = null;

        private PlayerView View => m_view ?? (m_view = gameObject.AddComponent<PlayerView>());

        public void Bind(PlayerModel model, bool synchronizeToModel = true)
        {
            if (synchronizeToModel)
            {
                // TODO: モデルに同期
            }
            else
            {
                // TODO: モデルを同期
            }

            
        }
    }
}
