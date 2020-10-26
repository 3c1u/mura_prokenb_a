using System;
using ProkenB.Model;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace ProkenB.View.UI
{
    public class GameStateView : MonoBehaviour
    {
        [SerializeField]
        private Text m_text = null;
        
        private GameModel.GameLifecycle m_lifecycle;
        private bool m_isMaster = false;

        public bool IsMaster
        {
            get => m_isMaster;
            set
            {
                m_isMaster = value;
                UpdateLabel();
            }
        }

        public GameModel.GameLifecycle Lifecycle
        {
            get => m_lifecycle;
            set
            {
                m_lifecycle = value;
                UpdateLabel();
            }
        }

        void UpdateLabel()
        {
            string state;
            
            switch (m_lifecycle)
            {
                case GameModel.GameLifecycle.NotInitialized:
                    state = "not ready";
                    break;
                case GameModel.GameLifecycle.Ready:
                    state = "ready";
                    break;
                case GameModel.GameLifecycle.Playing:
                    state = "playing";
                    break;
                case GameModel.GameLifecycle.Finish:
                    state = "finish";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var master = m_isMaster ? " (master)" : "";
            m_text.text = $"{state}{master}";
        }
    }
}
