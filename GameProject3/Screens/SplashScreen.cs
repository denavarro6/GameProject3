using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameProject3.StateManagement;


namespace GameProject3.Screens
{
    class SplashScreen : GameScreen
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
            _displayTime = TimeSpan.FromSeconds(2);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _displayTime -= gameTime.ElapsedGameTime;
            if (_displayTime <= TimeSpan.Zero) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(_menufont,"Time  to  Go  to  Bed", new Vector2(275, 200), Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}
