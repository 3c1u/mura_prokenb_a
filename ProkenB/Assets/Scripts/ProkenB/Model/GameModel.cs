using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ProkenB.Model
{
    public class GameModel
    {
        private List<PlayerModel> m_players = new List<PlayerModel>();
        public List<PlayerModel> Players => m_players;
        private ReactiveProperty<PlayerModel> m_localPlayer = new ReactiveProperty<PlayerModel>(null);

        public PlayerModel LocalPlayer
        {
            get => m_localPlayer.Value;
            set => m_localPlayer.Value = value;
        }

        public IObservable<PlayerModel> LocalPlayerAsObservable => m_localPlayer.AsObservable();

        private ReactiveProperty<int> m_totalPlayers = new ReactiveProperty<int>(0);
        public IObservable<int> TotalPlayersAsObservable => m_totalPlayers.AsObservable();

        public int TotalPlayers
        {
            get => m_totalPlayers.Value;
            set => m_totalPlayers.Value = value;
        }

        public void AddPlayer(PlayerModel player, bool isLocal = false)
        {
            Debug.Log("player entered");
            m_players.Add(player);
            TotalPlayers++;

            if (isLocal)
            {
                LocalPlayer = player;
            }
        }

        public void RemovePlayer(PlayerModel player)
        {
            Debug.Log("player leaved");
            m_players.Remove(player);
            TotalPlayers--;
            if (LocalPlayer == player)
            {
                LocalPlayer = null;
            }
        }

        private ReactiveProperty<bool> m_isMaster = new ReactiveProperty<bool>(false);
        public IObservable<bool> IsMasterAsObservable => m_isMaster.AsObservable();

        public bool IsMaster
        {
            get => m_isMaster.Value;
            set => m_isMaster.Value = value;
        }

        public enum GameLifecycle
        {
            NotInitialized = -1,
            Ready = 0,
            Playing = 1,
            Finish = 2,
        }

        private ReactiveProperty<GameLifecycle> m_lifecycle = new ReactiveProperty<GameLifecycle>(GameLifecycle.NotInitialized);

        public GameLifecycle Lifecycle
        {
            get => m_lifecycle.Value;
            set => m_lifecycle.Value = value;
        }

        public IObservable<GameLifecycle> LifecycleAsObservable => m_lifecycle.AsObservable();
    }
}
