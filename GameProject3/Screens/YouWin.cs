using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameProject3.StateManagement;


namespace GameProject3.Screens
{
    class YouWin : GameScreen
    {
        ContentManager _content;
        Texture2D _background;
        SpriteFont _menufont;
        TimeSpan _displayTime;
        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _background = _content.Load<Texture2D>("whirl1");
            _menufont = _content.Load<SpriteFont>("menufont");
            _displayTime = TimeSpan.FromSeconds(3);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _displayTime -= gameTime.ElapsedGameTime;
            if (_displayTime <= TimeSpan.Zero) System.Environment.Exit(0);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(_menufont, "You Win!", new Vector2(275, 200), Color.Green, 0, new Vector2(0,0),2.0f,SpriteEffects.None,0);
            ScreenManager.SpriteBatch.DrawString(_menufont, "You  have  earned  your  rest.", new Vector2(225, 275), Color.Green, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.End();
        }
    }
}
