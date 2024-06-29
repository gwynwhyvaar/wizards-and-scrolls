using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Concrete;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SharpDX.XInput;

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

        public FuelCellGameHome()
        {
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

            // init and place the scrolls
            _scrolls = new Scroll[1];
            _scrolls[0] = new Scroll();
            _scrolls[0].LoadContent(Content, "scroll_small");
            _scrolls[0].Position = new Vector3(-15, 0, 60);

            // init and place the rock barriers;
            _rockBarriers = new RockBarrier[3];
            _rockBarriers[0] = new RockBarrier();
            _rockBarriers[1] = new RockBarrier();
            _rockBarriers[2] = new RockBarrier();

            _rockBarriers[0].LoadContent(Content, "boulder_mesh_01");
            _rockBarriers[0].Position = new Vector3(0, 0, 30);

            _rockBarriers[1].LoadContent(Content, "boulder_mesh_02");
            _rockBarriers[1].Position = new Vector3(20, 0, 30);

            _rockBarriers[2].LoadContent(Content, "boulder_mesh_03");
            _rockBarriers[2].Position = new Vector3(-20, 0, 30);

            // init and place the player avatar ** THE MOST IMPORTANT!
            _wizard = new Wizard();
            _wizard.LoadContent(Content, "wizard_ov_war");
            // _wizard.Position = new Vector3(20, 0, 15);

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
            GraphicsDevice.Clear(Color.Azure);
            // draw game objects
            // 1. the terrain
            _drawModel.DrawTerrain(_groundGameObject.Model, _gameCameraObject);
            // 2. the scroll
            _scrolls[0].Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            // 3. the rock barriers
            foreach (RockBarrier barrier in _rockBarriers)
            {
                barrier.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            }
            // 4. the player avatar -zee the wizard ov war!
            _wizard.Draw(_gameCameraObject.ViewMatrix, _gameCameraObject.ProjectionMatrix);
            base.Draw(gameTime);
        }
    }
}
