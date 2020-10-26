using System;
using System.Collections.Generic;
using UniRx;

namespace ProkenB.Model
{
    public class GameModel
    {
        private List<PlayerModel> m_players = new List<PlayerModel>();
        public List<PlayerModel> Players => m_players;

        private Subject<int> m_totalPlayersChanged = new Subject<int>();
        public IObservable<int> TotalPlayersAsObservable => m_totalPlayersChanged.AsObservable();
        public int TotalPlayers => Players.Count;

        public void AddPlayer(PlayerModel player)
        {
            m_players.Add(player);
            m_totalPlayersChanged.OnNext(TotalPlayers);
        }

        public void RemovePlayer(PlayerModel player)
        {
            m_players.Remove(player);
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
