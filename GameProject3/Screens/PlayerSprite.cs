using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace GameProject3
{
    public class PlayerSprite
    {
        private GamePadState gamePadState;

        private SpriteBatch spriteBatch;

        Game game;

        Viewport viewport;

        private double animationTimer;

        private short animationFrame = 0;

        private KeyboardState keyboardState;

        private SoundEffect jump;

        private Texture2D texture;

        private Vector2 position = new Vector2(0, 0);

        Vector2 velocity;

        private bool flipped;

        private GraphicsDeviceManager graphics;
        Vector2 origin;
        Body body;
        /// <summary>
        /// color overlay of ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        public PlayerSprite(Body body, Texture2D image)
        {
            //this.game = game;

            //graphics = new GraphicsDeviceManager(this);
            //viewport = game.GraphicsDevice.Viewport;
            this.texture = image;
            this.body = body;
            this.position = new Vector2(100, 400);
        }
        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            //texture = content.Load<Texture2D>("player-spritemap");
            //jump = content.Load<SoundEffect>("jump");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            var state = 1;
            if (state == 1)
            {
                // Apply keyboard movement
                /*if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    position += new Vector2(0, -1);
                }*/
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    position += new Vector2(0, 1);
                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    position += new Vector2(-1, 0);
                    flipped = true;
                }
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    position += new Vector2(1, 0);
                    flipped = false;
                }
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 acceleration = new Vector2(0, 2);
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    acceleration = new Vector2(0, -7);
                    //jump.Play(.25f,0,0);
                }
                position += acceleration;
                if (position.Y < 32) position.Y = 32;
                if (position.Y > viewport.Height) position.Y = viewport.Height;
                if (position.X < 32) position.X = 32;
                if (position.X > viewport.Width) position.X = viewport.Width;

                //update the bounds
                /*bounds.X = position.X - 48;
                bounds.Y = position.Y - 48;*/
            }
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frme

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var source = new Rectangle(animationFrame * 150, 0, 48, 48);
            //spriteBatch.Draw(texture, body.Position, null, Color, body.Rotation, new Vector2(164,164), 0f, spriteEffects, 0);
            spriteBatch.Draw(texture, position, source, Color, 0, new Vector2(64, 64), 1, spriteEffects, 0);
        }
    }
}
