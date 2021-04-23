using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject3.StateManagement;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using tainicom.Aether.Physics2D.Dynamics;
using GameProject3.Collisions;

namespace GameProject3.Screens
{
    public enum Direction
    {
        Down = 0,
        Right = 1,
        Up = 2,
        Left = 3,
    }
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreenLvl2 : GameScreen
    {
        public Game1 game;
        private ContentManager _content;
        private SpriteFont _gameFont;
        private World world;
        private SoundEffect _hitBed;
        private SoundEffect _hitBurger;
        // private int height = ScreenManager.GraphicsDevice.Viewport.Hieght;
        private Vector2 _playerPosition = new Vector2(100, 500);
        private Vector2 _enemyPosition1 = new Vector2(2820, 450);
        private Vector2 _enemyPosition2 = new Vector2(450, 450);
        private Vector2 _enemyPosition3 = new Vector2(1000, 450);
        private Vector2 _enemyPosition4 = new Vector2(1600,450);
        private bool _enemy1Land = true;
        private bool _enemy2Land = true;
        private bool _enemy3Land = true;
        private bool _enemy4Land = true;

        private BoundingRectangle _enemy1Bounds = new BoundingRectangle(new Vector2(2830, 450), 45, 45);
        private BoundingRectangle _enemy2Bounds = new BoundingRectangle(new Vector2(460, 450), 45, 45);
        private BoundingRectangle _enemy3Bounds = new BoundingRectangle(new Vector2(1010, 450), 45, 45);
        private BoundingRectangle _enemy4Bounds = new BoundingRectangle(new Vector2(1610, 450), 45, 60);
        //private BoundingRectangle[] _enemyBounds = new BoundingRectangle[] { new BoundingRectangle(new Vector2(140, 450),) }
        private PlayerSprite _player;
        private Texture2D _bed;
        private Texture2D _player1;
        private Texture2D _enemy;
        private SpriteBatch spriteBatch;
        private double animationTimer;
        private bool flipped;
        private SoundEffect _walk;
        Texture2D _floor;
        private BoundingRectangle _playerbounds = new BoundingRectangle(new Vector2(100, 500), 40, 50);
        private Direction Direction;
        private BoundingRectangle _bedBounds = new BoundingRectangle(new Vector2(3774-13, 480 - 12-14), (float)56.5, (float)26.75);

        private short animationFrame = 0;

        private readonly Random _random = new Random();

        Texture2D _background;
        Texture2D _hitbox;
        Song _backgroundMusic;

        bool _isPlaying = false;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreenLvl2()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _gameFont = _content.Load<SpriteFont>("gamefont");
            _background = _content.Load<Texture2D>("Background-3");
            _backgroundMusic = _content.Load<Song>("Ending");
            _bed = _content.Load<Texture2D>("bed");
            _hitBed = _content.Load<SoundEffect>("HitBed");
            _hitBurger = _content.Load<SoundEffect>("HitBurger");
            _hitbox = _content.Load<Texture2D>("redeclipse_bk");

            _floor = _content.Load<Texture2D>("floor2.0");
            world = new World();
            world.Gravity = Vector2.Zero;


            

            var top = 0;
            var bottom = ScreenManager.GraphicsDevice.Viewport.Height;
            var left = 0;
            var right = ScreenManager.GraphicsDevice.Viewport.Width;
            var edges = new Body[] {
            world.CreateEdge(new Vector2(left,top), new Vector2(right,top)),
            world.CreateEdge(new Vector2(left, top), new Vector2(left,bottom)),
            world.CreateEdge(new Vector2(left,bottom), new Vector2(right,bottom)),

            world.CreateEdge(new Vector2(right,top), new Vector2(right,bottom))
            };
            foreach (var edge in edges)
            {
                edge.BodyType = BodyType.Static;
            }
            var body = world.CreateRectangle(10, 20, 1, new Vector2(0, 480), 0, BodyType.Static);
            _walk = _content.Load<SoundEffect>("footstep");
            _player1 = _content.Load<Texture2D>("player-spritemap");
            _enemy = _content.Load<Texture2D>("hamburger");
            _player = new PlayerSprite(body, _player1);
            _player.LoadContent(_content);
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);


            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Initialize()
        {
            RainParticleSystem rain = new RainParticleSystem(this, new Rectangle(100, 20, 500, 100),ScreenManager.GraphicsDevice, _content);
            Components.Add(rain);
            base.Initialize();
        }

        public override void Deactivate()
        {
            _isPlaying = false;
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // Apply some random jitter to make the enemy move around.
                const float randomization = 10;

                //_enemyPosition.X += (float)(_random.NextDouble() - 0.5) * randomization;
                //_enemyPosition.Y += (float)(_random.NextDouble() - 0.5) * randomization;

                // Apply a stabilizing force to stop the enemy moving off the screen.
                //var targetPosition = new Vector2(
                //  ScreenManager.GraphicsDevice.Viewport.Width / 2 - _gameFont.MeasureString("Insert Gameplay Here").X / 2,
                //200);

                // _enemyPosition = Vector2.Lerp(_enemyPosition, targetPosition, 0.05f);



                //Move the mouse in direction it is moving
                if (_enemyPosition1.Y < 500 && _enemyPosition1.Y > 100 && _enemy1Land)
                {
                    _enemyPosition1.Y = 0;
                    _enemy1Bounds.Y = 0;
                    _enemy1Land = false;
                }
                else if (_enemyPosition1.Y >= 480)
                {
                    _enemy1Land = true;
                }
                else
                {
                    _enemyPosition1.Y += 8;
                    _enemy1Bounds.Y = _enemyPosition1.Y+8;
                }
                
                
                if (_enemyPosition2.Y < 500 && _enemyPosition2.Y > 100 && _enemy2Land)
                {
                    _enemyPosition2.Y = 0;
                    _enemy2Bounds.Y = 0;
                    _enemy2Land = false;
                }
                else if (_enemyPosition2.Y >= 480)
                {
                    _enemy2Land = true;
                }
                else
                {
                    _enemyPosition2.Y += 4;
                    _enemy2Bounds.Y = _enemyPosition2.Y+8;

                }

                if (_enemyPosition3.Y < 500 && _enemyPosition3.Y > 100 && _enemy3Land)
                {
                    _enemyPosition3.Y = 0;

                    _enemy3Bounds.Y = 0;
                    _enemy3Land = false;
                }
                else if (_enemyPosition3.Y >= 480)
                {
                    _enemy3Land = true;
                }
                else
                {
                    _enemyPosition3.Y += 2;
                    _enemy3Bounds.Y = _enemyPosition3.Y+8;

                }

                if (_enemyPosition4.Y < 500 && _enemyPosition4.Y > 100 && _enemy4Land)
                {
                    _enemyPosition4.Y = 0;
                    _enemy4Bounds.Y = 0;
                    _enemy4Land = false;
                }
                else if (_enemyPosition4.Y >= 480)
                {
                    _enemy4Land = true;
                }
                else
                {
                    _enemyPosition4.Y += 6;
                    _enemy4Bounds.Y = _enemyPosition4.Y+8;
                }
                //_enem.bounds = new BoundingRectangle(Position + new Vector2(0, 8), 40, 16);


                // This game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)

                _player.Update(gameTime);
            }

        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                MediaPlayer.Stop();
                _isPlaying = false;
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame == 1 || animationFrame == 3 || animationFrame == 6)
                            _walk.Play();
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    movement.X--;

                    flipped = true;
                }

                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame == 1 || animationFrame == 3 || animationFrame == 6)
                            _walk.Play();
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }

                    movement.X++;

                    flipped = false;
                }

                /*if (keyboardState.IsKeyDown(Keys.Up))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    movement.Y--;
                }*/

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    movement.Y++;
                }
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 acceleration = new Vector2(0, 2);
                /*if (keyboardState.IsKeyDown(Keys.Space))
                {
                    acceleration = new Vector2(0, -7);
                    //jump.Play(.25f,0,0);
                }*/
                _playerPosition += acceleration;
                if (_playerPosition.Y < 50) _playerPosition.Y = 50;
                if (_playerPosition.Y > ScreenManager.GraphicsDevice.Viewport.Height) _playerPosition.Y = ScreenManager.GraphicsDevice.Viewport.Height;
                if (_playerPosition.X < 64) _playerPosition.X = 64;
                if (_playerPosition.X > 3810) _playerPosition.X = 3810;
                _playerbounds.X = _playerPosition.X;
                _playerbounds.Y = _playerPosition.Y - 64;
                if (_playerbounds.CollidesWith(_bedBounds))
                {
                    MediaPlayer.Stop();
                    _hitBed.Play();
                    Thread.Sleep(2000);
                    ExitScreen();
                    //ExitScreen();
                }
                if (_playerbounds.CollidesWith(_enemy2Bounds) || _playerbounds.CollidesWith(_enemy1Bounds) || _playerbounds.CollidesWith(_enemy3Bounds) || _playerbounds.CollidesWith(_enemy4Bounds))
                {
                    _hitBurger.Play();
                    _playerPosition.X = 100;
                    _playerPosition.Y = 500;
                }

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 4f;

                if (!_isPlaying)
                {
                    MediaPlayer.Play(_backgroundMusic);
                    MediaPlayer.IsRepeating = true;
                    _isPlaying = true;
                }
                _player.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            float playerX = MathHelper.Clamp(_playerPosition.X, 300, 3340);
            float offsetX = 300 - playerX;

            Matrix transform = Matrix.CreateTranslation(offsetX, 0, 0);

            spriteBatch.Begin(transformMatrix: transform);
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            spriteBatch.Draw(_floor, new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height - 20), Color.White);
            //spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
            //spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
            // _enemyPosition, Color.DarkRed);
            //spriteBatch.Draw(_enemy, _enemyPosition, source, Color.White, 0, new Vector2(64, 64), 1, spriteEffects, 0);
            _player.Draw(gameTime, spriteBatch);
            // ScreenManager.SpriteBatch.
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frme

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            
            spriteBatch.Draw(_enemy, _enemyPosition1, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 0);
            var rect = new Rectangle((int)(_enemy1Bounds.X),
                                            (int)(_enemy1Bounds.Y),
                                            (int)(_enemy1Bounds.Width), (int)(_enemy1Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect, Color.White);
            var rect2 = new Rectangle((int)(_enemy2Bounds.X),
                                           (int)(_enemy2Bounds.Y),
                                           (int)(_enemy2Bounds.Width), (int)(_enemy2Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect2, Color.White);
            spriteBatch.Draw(_enemy, _enemyPosition2, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 0);
            
            
            spriteBatch.Draw(_enemy, _enemyPosition3, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 0);
            var rect3 = new Rectangle((int)(_enemy3Bounds.X),
                                           (int)(_enemy3Bounds.Y),
                                           (int)(_enemy3Bounds.Width), (int)(_enemy3Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect3, Color.White);

            spriteBatch.Draw(_enemy, _enemyPosition4, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 0);
            var rect4 = new Rectangle((int)(_enemy4Bounds.X),
                                           (int)(_enemy4Bounds.Y),
                                           (int)(_enemy4Bounds.Width), (int)(_enemy4Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect4, Color.White);


            var source = new Rectangle(animationFrame * 46, 150, 46, 50);
            spriteBatch.Draw(_player1, _playerPosition, source, Color.White, 0, new Vector2(0, 64), 1, spriteEffects, 0);
            var rect7 = new Rectangle((int)(_playerbounds.X),
                                           (int)(_playerbounds.Y),
                                           (int)(40), (int)(50));
            //spriteBatch.Draw(_hitbox, rect7, Color.White);

            spriteBatch.DrawString(_gameFont, "How  did  we  get  here?\nAnyways,  you  really  need  to  sleep.", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 300, 32), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "BED  THIS  WAY  ----->", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + 100, 150), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "Almost  there...", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + 1900, 100), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "You  forgot  to  have  dinner,  didn't  you...", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + 900, 125), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.Draw(_bed, new Vector2(3774, ScreenManager.GraphicsDevice.Viewport.Height - 12), null, Color.White, 0, new Vector2(64, 64), .25f, SpriteEffects.None, 0);
            
            var rect8 = new Rectangle((int)(_bedBounds.X),
                                           (int)(_bedBounds.Y),
                                           (int)(56.5), (int)(26.75));
            //spriteBatch.Draw(_player1, _playerPosition, Color.White);
            //spriteBatch.Draw(_hitbox, rect8, Color.White);

            spriteBatch.End();
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
