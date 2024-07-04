using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Concrete;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11
{
    public class FuelCellGameHome : Game
    {
        private int _retrievedScrolls = 0;

        private TimeSpan _startTime, _roundTimer, _roundTime;

        private Random _random;
        private Wizard _wizard;

        private Scroll[] _scrolls;
        private RockBarrier[] _rockBarriers;

        private DrawModel _drawModel;
        private GameObject _groundGameObject, _boundingSphere;
        private CameraObject _gameCameraObject;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _statsFont;

        // input keyboard state ..
        private KeyboardState _lastKeyboardState;
        private KeyboardState _currentKeyboardState;

        // input gamepad state ..
        private GamePadState _lastGamepadState;
        private GamePadState _currentGamepadState;

        private GameObjectPosition _gameObjectPostion;

        public FuelCellGameHome()
        {
            _roundTime =GameConstants.RoundTime;
            _random = new Random();
            _gameObjectPostion = new GameObjectPosition();
            _graphics = new GraphicsDeviceManager(this);

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = false;
            _graphics.PreferMultiSampling = true;

            _graphics.PreparingDeviceSettings += _graphics_PreparingDeviceSettings;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _lastKeyboardState = new KeyboardState();
            _currentKeyboardState = new KeyboardState();

            _lastGamepadState = new GamePadState();
            _currentGamepadState = new GamePadState();

            // set a default resolution of 853 x480
            _graphics.PreferredBackBufferWidth = 853;
            _graphics.PreferredBackBufferHeight = 480;

            // _graphics.PreferredBackBufferFormat = SurfaceFormat.
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
            _boundingSphere =new GameObject();
            // init the scrolls
            _scrolls = new Scroll[GameConstants.NumScrolls];
            for (int index = 0; index < _scrolls.Length; index++)
            {
                _scrolls[index] = new Scroll();
                _scrolls[index].LoadContent(Content, "scroll_small");
            }

            // init the rock barriers
            _rockBarriers = new RockBarrier[GameConstants.NumRockBarriers];
            int randomRockBarrier = _random.Next(3);
            string rockBarrierName = null;
            for (int x = 0; x < _rockBarriers.Length; x++)
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
            _gameObjectPostion.PlaceScrollsAndRockBarriers(_scrolls, _rockBarriers, _random);
            // init and place the player avatar ** THE MOST IMPORTANT!
            _wizard = new Wizard();
            _wizard.LoadContent(Content, "wizard");

            _drawModel = new DrawModel();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // load the bounding sphere
            _boundingSphere.Model = Content.Load<Model>("3d/sphere1uR");
            // load the terrain
            _groundGameObject.Model = Content.Load<Model>("3d/terrain");
            // load font
            _statsFont = Content.Load<SpriteFont>("fonts/File");
        }

        protected override void Update(GameTime gameTime)
        {
            float aspectRatio = _graphics.GraphicsDevice.Viewport.AspectRatio;

            _lastKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _lastGamepadState = _currentGamepadState;
            _currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if((_lastKeyboardState.IsKeyDown(Keys.Enter) && (_lastKeyboardState.IsKeyUp(Keys.Enter))) || _currentGamepadState.Buttons.Start ==ButtonState.Pressed)
            {
                _roundTimer = _roundTime;
            }

            _wizard.Update(_currentGamepadState, _currentKeyboardState, _rockBarriers);

            _gameCameraObject.Update(_wizard.ForwardDirection, _wizard.Position, aspectRatio);

            // todo: checking the intersection between the scrolls and the wizard game object
            foreach(Scroll scroll in _scrolls)
            {
                scroll.Update(_wizard.BoundingSphere);
            }
            _roundTimer -= gameTime.ElapsedGameTime;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.DeepSkyBlue);
            _graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // 1. the terrain
            _drawModel.DrawTerrain(_groundGameObject.Model, _gameCameraObject);
            // 2. the scroll
            foreach (Scroll scroll in _scrolls)
            {
                if (!scroll.IsRetrieved)
                {
                    scroll.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
                    // ChangeRasterizerState(FillMode.WireFrame);
                    // draw the bounding sphere
                    //  scroll.DrawBoundingSphere(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix, _boundingSphere);
                    // reset the graphics device drawing mode to solid/ textured
                    // ChangeRasterizerState(FillMode.Solid);
                }
            }
            // 3. the rock barriers
            foreach (RockBarrier rockBarrier in _rockBarriers)
            {
                rockBarrier.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
                // ChangeRasterizerState(FillMode.WireFrame);
                // draw the bounding sphere
                // rockBarrier.DrawBoundingSphere(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix, _boundingSphere);
                // reset the graphics device drawing mode to solid/ textured
                // ChangeRasterizerState(FillMode.Solid);
            }
            // 4. the player avatar -zee the wizard ov war!
            _wizard.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);

            DrawStats();

            base.Draw(gameTime);
        }

        private RasterizerState ChangeRasterizerState(FillMode fillMode, CullMode cullMode = CullMode.None)
        {
            RasterizerState state = new RasterizerState()
            {
                FillMode = fillMode,
                CullMode = cullMode,
            };
            _graphics.GraphicsDevice.RasterizerState = state;
            return state;
        }

        private void DrawStats()
        {
            float xOffSetText, yOffSetText;

            string text1 =GameConstants.TimeRemainingText;
            string text2 =GameConstants.ScrollsFoundText +_retrievedScrolls.ToString()+ " of "+GameConstants.NumRockBarriers.ToString();

            Rectangle rectSafeArea;

            text1 += (_roundTimer.Seconds).ToString();

            rectSafeArea = GraphicsDevice.Viewport.TitleSafeArea;

            xOffSetText =rectSafeArea.X;
            yOffSetText =rectSafeArea.Y;

            Vector2 textSize =_statsFont.MeasureString(text1);
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
    }
}
