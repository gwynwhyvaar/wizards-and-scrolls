using System;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Concrete;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Constants;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Enums;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11
{
    public class WizardScrollsGameHome : Game
    {
        private int _retrievedScrolls = 0;

        private TimeSpan _startTime, _roundTimer, _roundTime;

        private float _aspectRatio;

        private Song _backgroundMusic;

        private Random _random;

        private GameStateEnum _currentGameState = GameStateEnum.Loading;

        private Wizard _wizard;

        private Scroll[] _scrolls;
        private RockBarrier[] _rockBarriers;
        private CloudsGameObject[] _clouds;

        private DrawModel _drawModel;
        private GameObject _groundGameObject, _boundingSphere;
        private CameraObject _gameCameraObject;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _statsFont;

        private GameObjectPosition _gameObjectPostion;

        private IInputState _inputState;
        public WizardScrollsGameHome()
        {
            _roundTime = GameConstants.RoundTime;
            _random = new Random();
            _gameObjectPostion = new GameObjectPosition();
            _graphics = new GraphicsDeviceManager(this);

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = false;
            _graphics.PreferMultiSampling = true;

            _graphics.PreparingDeviceSettings += _graphics_PreparingDeviceSettings;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // set a default resolution of 853 x480
            _graphics.PreferredBackBufferWidth = 853;
            _graphics.PreferredBackBufferHeight = 480;

            _inputState = new InputState(this);
        }
        private void _graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            _graphics.PreferMultiSampling = true;
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 16;
        }
        protected override void Initialize()
        {
            _gameCameraObject = new CameraObject();
            _groundGameObject = new GameObject();
            _boundingSphere = new GameObject();
            _aspectRatio = _graphics.GraphicsDevice.Viewport.AspectRatio;
            // init the scrolls
            _scrolls = new Scroll[GameConstants.NumScrolls];
            for (int index = 0; index < _scrolls.Length; index++)
            {
                _scrolls[index] = new Scroll();
                _scrolls[index].LoadContent(Content, "scroll_small");
            }
            // init the rock barriers
            InitializeGameField();
            // init and place the player avatar ** THE MOST IMPORTANT!
            _wizard = new Wizard();
            _wizard.LoadContent(Content, "wizard_ov_war");

            _drawModel = new DrawModel();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // load the bounding sphere
            _boundingSphere.SetModel(Content.Load<Model>("3d/sphere1uR"));
            // load the terrain
            _groundGameObject.SetModel(Content.Load<Model>("3d/terrain"));
            // load font
            _statsFont = Content.Load<SpriteFont>("fonts/StatsFont");
            // load the background music
            _backgroundMusic = Content.Load<Song>("audio/the-time-is-upon-us-in_game");
        }
        protected override void Update(GameTime gameTime)
        {
            _inputState.Update();

            if (_inputState.PlayerExit(PlayerIndex.One))
            {
                Exit();
            }

            if (_currentGameState == GameStateEnum.Loading)
            {
                if (_inputState.StartGame(PlayerIndex.One))
                {
                    ResetGame(gameTime, _aspectRatio);
                }
            }

            if ((_currentGameState == GameStateEnum.Running))
            {
                // _wizard.Update(_currentGamepadState, _currentKeyboardState, _rockBarriers);
                _wizard.Update(_inputState, _rockBarriers);
                _gameCameraObject.Update(_wizard.ForwardDirection, _wizard.Position, _aspectRatio);

                _retrievedScrolls = 0;

                foreach (Scroll scroll in _scrolls)
                {
                    scroll.Update(_wizard.BoundingSphere);
                    if (scroll.IsRetrieved)
                    {
                        _retrievedScrolls++;
                    }
                }
                if (_retrievedScrolls == GameConstants.NumScrolls)
                {
                    _currentGameState = GameStateEnum.Won;
                }
                _roundTimer -= gameTime.ElapsedGameTime;
                if ((_roundTimer < TimeSpan.Zero) && (_retrievedScrolls != GameConstants.NumScrolls))
                {
                    _currentGameState = GameStateEnum.Lost;
                }
            }

            if ((_currentGameState == GameStateEnum.Won) || (_currentGameState == GameStateEnum.Lost))
            {
                if (MediaPlayer.State == MediaState.Playing)
                {
                    try
                    {
                        MediaPlayer.Stop();
                    }
                    catch { }
                }
                if (_inputState.StartGame(PlayerIndex.One))
                {
                    ResetGame(gameTime, _aspectRatio);
                }
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.DeepSkyBlue);
            _graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            switch (_currentGameState)
            {
                case GameStateEnum.Loading:
                    // load splash screen
                    DrawSplashScreen();
                    break;
                case GameStateEnum.Running:
                    // load the running gameplay
                    DrawGameplayScreen();
                    break;
                case GameStateEnum.Won:
                    // draw won screen
                    DrawWinOrLossScreen(GameConstants.GameWonText);
                    break;
                case GameStateEnum.Lost:
                    // draw lost creen
                    DrawWinOrLossScreen(GameConstants.GameLostText);
                    break;
            }
            base.Draw(gameTime);
        }
        private void DrawStats()
        {
            float xOffSetText, yOffSetText;

            string text1 = GameConstants.TimeRemainingText;
            string text2 = $"{GameConstants.ScrollsFoundText} {_retrievedScrolls.ToString()} of {GameConstants.NumRockBarriers.ToString()}";

            Rectangle rectSafeArea;

            text1 += (_roundTimer.Seconds).ToString();

            rectSafeArea = GraphicsDevice.Viewport.TitleSafeArea;

            xOffSetText = rectSafeArea.X;
            yOffSetText = rectSafeArea.Y;

            Vector2 textSize = _statsFont.MeasureString(text1);
            Vector2 positionText = new Vector2(xOffSetText + 10, yOffSetText);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_statsFont, text1, positionText, Color.White);
            positionText.Y += textSize.Y;

            _spriteBatch.DrawString(_statsFont, text2, positionText, Color.White);
            _spriteBatch.End();

            ResetRenderStates();
        }
        private void ResetRenderStates()
        {
            //re-enable depth buffer after sprite batch disablement
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
        private void DrawSplashScreen()
        {
            float xOffsetText, yOffsetText;
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Vector2 centerText;

            _graphics.GraphicsDevice.Clear(Color.Maroon);

            xOffsetText = yOffsetText = 0;
            Vector2 instructionTextSize = _statsFont.MeasureString(GameConstants.Instructions1Text);
            Vector2 positionText;
            centerText = new Vector2(instructionTextSize.X / 2, instructionTextSize.Y / 2);

            yOffsetText = (viewportSize.Y / 2 - centerText.Y);
            xOffsetText = (viewportSize.X / 2 - centerText.X);

            positionText = new Vector2(xOffsetText, yOffsetText);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_statsFont, GameConstants.Instructions1Text, positionText, Color.White);

            instructionTextSize = _statsFont.MeasureString(GameConstants.Instructions2Text);

            centerText = new Vector2(instructionTextSize.X / 2, instructionTextSize.Y / 2);
            yOffsetText = (viewportSize.Y / 2 - centerText.Y) + _statsFont.LineSpacing;
            xOffsetText = (viewportSize.X / 2 - centerText.X);
            positionText = new Vector2(xOffsetText, yOffsetText);

            _spriteBatch.DrawString(_statsFont, GameConstants.Instructions2Text, positionText, Color.LightGray);
            _spriteBatch.End();

            ResetRenderStates();
        }
        private void DrawGameplayScreen()
        {
            // 1. the terrain
            _drawModel.DrawTerrain(_groundGameObject.Model, _gameCameraObject);
            // 2. the scroll
            foreach (Scroll scroll in _scrolls)
            {
                if (!scroll.IsRetrieved)
                {
                    scroll.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
                }
            }
            // 3. the rock barriers
            foreach (RockBarrier rockBarrier in _rockBarriers)
            {
                rockBarrier.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            }
            // 4. draw the clouds ...
            foreach (CloudsGameObject cloud in _clouds)
            {
                cloud.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            }

            // 5. the player avatar -zee the wizard ov war!
            _wizard.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);

            DrawStats();
        }
        private void ResetGame(GameTime gameTime, float aspectRatio)
        {
            _wizard.Reset();
            _gameCameraObject.Update(_wizard.ForwardDirection, _wizard.Position, _aspectRatio);

            InitializeGameField();

            _retrievedScrolls = 0;
            _startTime = gameTime.TotalGameTime;
            _roundTimer = _roundTime;
            _currentGameState = GameStateEnum.Running;

            try
            {
                MediaPlayer.Stop();
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.5f;
                MediaPlayer.Play(_backgroundMusic);
            }
            catch { }
        }
        private void InitializeGameField()
        {
            // init the rock barriers
            _rockBarriers = new RockBarrier[GameConstants.NumRockBarriers];
            int randomRockBarrier = _random.Next(3);
            string rockBarrierName = null;
            for (int x = 0; x < GameConstants.NumRockBarriers; x++)
            {
                switch (randomRockBarrier)
                {
                    case 0:
                        rockBarrierName = "boulder_mesh_01";
                        break;
                    case 1:
                        rockBarrierName = "boulder_mesh_02";
                        break;
                    case 2:
                        rockBarrierName = "boulder_mesh_03";
                        break;
                }
                _rockBarriers[x] = new RockBarrier();
                _rockBarriers[x].LoadContent(Content, rockBarrierName);
                // reset the random value
                randomRockBarrier = _random.Next(3);
            }
            // init the clouds
            _clouds = new CloudsGameObject[GameConstants.NumCloudsBarriers];
            // loop through the cloud game objects
            for(int x = 0; x < GameConstants.NumCloudsBarriers; x++)
            {
                _clouds[x] = new CloudsGameObject();
                _clouds[x].LoadContent(Content, "cloud");
            }
            // PlaceScrollsAndRocks();
            _gameObjectPostion.PlaceScrollsAndRockBarriers(_scrolls, _rockBarriers, _random, _clouds);
        }
        private void DrawWinOrLossScreen(string gameResult)
        {
            float xOffsetText, yOffsetText;

            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Vector2 centerText;

            xOffsetText = yOffsetText = 0;

            Vector2 resultText = _statsFont.MeasureString(gameResult);
            Vector2 playAgainSizeText = _statsFont.MeasureString(GameConstants.PlayAgainText);
            Vector2 positionText;

            centerText = new Vector2(resultText.X / 2, resultText.Y / 2);

            yOffsetText = (viewportSize.Y / 2 - centerText.Y);
            xOffsetText = (viewportSize.X / 2 - centerText.X);
            positionText = new Vector2(xOffsetText, yOffsetText);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_statsFont, gameResult, positionText, Color.Red);

            centerText = new Vector2(playAgainSizeText.X / 2, playAgainSizeText.Y / 2);
            yOffsetText = (viewportSize.Y / 2 - centerText.Y) + (float)_statsFont.LineSpacing;
            xOffsetText = (viewportSize.X / 2 - centerText.X);
            positionText = new Vector2(xOffsetText, yOffsetText);

            _spriteBatch.DrawString(_statsFont, GameConstants.PlayAgainText, positionText, Color.AntiqueWhite);
            _spriteBatch.End();

            ResetRenderStates();
        }
    }
}
