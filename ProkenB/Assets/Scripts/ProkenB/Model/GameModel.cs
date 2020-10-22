using System;
using System.Collections.Generic;
using UniRx;

namespace ProkenB.Model
{
    public class GameModel
    {
        private List<PlayerModel> m_players = new List<PlayerModel>();
        public List<PlayerModel> Players => m_players;

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
            set => m_lifecycle.SetValueAndForceNotify(value);
        }

        public IObservable<GameLifecycle> LifecycleAsObservable => m_lifecycle.AsObservable();
    }
}
