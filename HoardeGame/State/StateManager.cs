using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HoardeGame.State
{
    public class StateManager : IDisposable
    {
        public GameState ActiveGameState { get; private set; }
        public List<GameState> GameStates { get; private set; }

        public StateManager()
        {
            GameStates = new List<GameState>();
        }

        public void Dispose()
        {
            while (ActiveGameState != null) Pop();
        }

        public GameState Push(GameState gameState)
        {
            if (gameState == null) throw new ArgumentNullException("gameState");
            if (ActiveGameState != null) ActiveGameState.Pause();

            GameStates.Add(gameState);
            ActiveGameState = gameState;
            gameState.Start();

            return gameState;
        }

        public GameState Pop()
        {
            if (ActiveGameState == null) throw new StackOverflowException("There are no GameStates on the stack to pop");

            ActiveGameState.End();
            GameStates.Remove(ActiveGameState);

            if (GameStates.Count > 0)
            {
                ActiveGameState = GameStates[GameStates.Count - 1];
                ActiveGameState.Resume();
            }
            else ActiveGameState = null;

            return ActiveGameState;
        }

        public void Switch(GameState gameState)
        {
            if (gameState == null) throw new ArgumentNullException("gameState");

            if (ActiveGameState != null) ActiveGameState.Pause();
            if (!GameStates.Remove(gameState)) throw new ArgumentException(gameState + " is not on the stack");

            GameStates.Add(gameState);

            gameState.Resume();
            ActiveGameState = gameState;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < GameStates.Count; i++) GameStates[i].Update(gameTime);
        }

        public void Draw(GameTime gameTime, float interpolation)
        {
            for (int i = 0; i < GameStates.Count; i++) GameStates[i].Draw(gameTime, interpolation);
        }
    }
}
