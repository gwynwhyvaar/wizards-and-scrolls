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
        private Random _random;
        private Wizard _wizard;

        private Scroll[] _scrolls;
        private RockBarrier[] _rockBarriers;

        private DrawModel _drawModel;
        private GameObject _groundGameObject;
        private CameraObject _gameCameraObject;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // input keyboard state ..
        private KeyboardState _lastKeyboardState;
        private KeyboardState _currentKeyboardState;

        // input gamepad state ..
        private GamePadState _lastGamepadState;
        private GamePadState _currentGamepadState;

        private GameObjectPosition _gameObjectPostion;

        public FuelCellGameHome()
        {
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
            _groundGameObject.Model = Content.Load<Model>("3d/terrain");
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

            _wizard.Update(_currentGamepadState, _currentKeyboardState, _rockBarriers);
            _gameCameraObject.Update(_wizard.ForwardDirection, _wizard.Position, aspectRatio);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // draw game objects
            // 1. the terrain
            _drawModel.DrawTerrain(_groundGameObject.Model, _gameCameraObject);
            // 2. the scroll
            foreach (Scroll scroll in _scrolls)
            {
                scroll.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            }
            // 3. the rock barriers
            foreach (RockBarrier rockBarrier in _rockBarriers)
            {
                rockBarrier.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            }
            // 4. the player avatar -zee the wizard ov war!
            _wizard.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            base.Draw(gameTime);
        }
    }
}
