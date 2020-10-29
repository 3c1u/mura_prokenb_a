using System;
using ProkenB.Game;
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
        private bool m_hasReachedGoal = false;

        public bool HasReachedGoal
        {
            get => m_hasReachedGoal;
            set
            {
                m_hasReachedGoal = value;
                UpdateLabel();
            }
        }

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

        private void FixedUpdate()
        {
            UpdateLabel();
        }

        void UpdateLabel()
        {
            if (!m_text || !GameManager.Instance)
            {
                return;
            }

            string state;
            var timer = GameManager.Instance.Timer;

            switch (m_lifecycle)
            {
                case GameModel.GameLifecycle.NotInitialized:
                    state = "not ready";
                    break;
                case GameModel.GameLifecycle.Ready:
                    state = $"ready <start in {Mathf.FloorToInt(Constant.WAITTIME_GAME_START - timer)} seconds>";
                    break;
                case GameModel.GameLifecycle.Playing:
                    state = $"playing for {Mathf.FloorToInt(timer)} second(s)";
                    break;
                case GameModel.GameLifecycle.Finish:
                    state = "finish";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            m_text.text = $"{m_totalPlayers} player{(m_totalPlayers < 2 ? "" : "s")}" +
                          (m_hasReachedGoal ? ", reached" : ", not reached") +
                          $"\n{state}{(m_isMaster ? " (master)" : "")}";
        }
    }
}
