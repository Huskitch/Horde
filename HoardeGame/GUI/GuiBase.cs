using System.Diagnostics;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    public delegate Vector2 PositionRecalculationDelegate();

    public enum HiddenState
    {
        Visible,
        Disabled,
        Hidden
    }

    public delegate void ButtonDelegate();
    public delegate void CustomDrawDelegate(GameTime gameTime, SpriteBatch spriteBatch, float interpolation);
    public delegate void CustomUpdateDelegate(GameTime gameTime, Point clickPoint, bool click);

    public class GuiBase
    {
        public static SpriteFont Font;

        protected readonly Main Game;
        public readonly string ID;
        public Vector2 Position;
        public Color Color = Color.White;
        public Color DisabledColor = Color.Gray;
        public HiddenState Visibility = HiddenState.Visible;

        public ButtonDelegate OnClick = () => { };
        public ButtonDelegate OnMouseOver = () => { };
        public CustomDrawDelegate CustomDraw = (time, spriteBatch, interpolation) => { };
        public CustomUpdateDelegate CustomUpdate = (time, point, click) => { };

        public GuiBase(GameState state, string id, bool noSub = false)
        {
            ID = id;

            if (noSub) return;

            state.OnCheck += Check;
            state.OnDraw += Draw;
        }

        public virtual void SetVisibility(HiddenState state)
        {
            Visibility = state;
        }

        public virtual void Check(GameTime gameTime, Point point, bool click)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
        }
    }
}
