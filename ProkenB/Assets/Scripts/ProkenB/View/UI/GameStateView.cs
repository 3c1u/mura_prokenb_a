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
        private int m_totalPlayers = 0;

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

        public int TotalPlayers
        {
            get => m_totalPlayers;
            set
            {
                m_totalPlayers = value;
                UpdateLabel();
            }
        }

        void Start()
        {
            m_text.text = "";
        }

        private void OnDestroy()
        {
            m_text = null;
        }

        void UpdateLabel()
        {
            if (!m_text)
            {
                return;
            }
            
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
            
            m_text.text = $"{m_totalPlayers} player{(m_totalPlayers < 2 ? "" : "s")}\n{state}{(m_isMaster ? " (master)" : "")}";
        }
    }
}
