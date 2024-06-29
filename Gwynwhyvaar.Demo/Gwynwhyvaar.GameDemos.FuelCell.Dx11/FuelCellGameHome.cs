using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Concrete;
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

        public FuelCellGameHome()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _gameCameraObject = new CameraObject();
            _groundGameObject = new GameObject();

            // init and place the scrolls
            _scrolls = new Scroll[1];
            _scrolls[0] = new Scroll();
            _scrolls[0].LoadContent(Content, "scroll");
            _scrolls[0].Position = new Vector3(0, 0, 15);
            
            // init and place the rock barriers;
            _rockBarriers =new RockBarrier[3];
            _rockBarriers[0] = new RockBarrier();
            _rockBarriers[1] = new RockBarrier();
            _rockBarriers[2] = new RockBarrier();

            _rockBarriers[0].LoadContent(Content, "boulder_mesh_01");
            _rockBarriers[0].Position = new Vector3(0, 0, 30);

            _rockBarriers[1].LoadContent(Content, "boulder_mesh_01");
            _rockBarriers[1].Position = new Vector3(15, 0, 30);

            _rockBarriers[2].LoadContent(Content, "boulder_mesh_01");
            _rockBarriers[2].Position = new Vector3(-15, 0, 30);

            // init and place the player avatar ** THE MOST IMPORTANT!
            _wizard =new Wizard();
            _wizard.LoadContent(Content, "wizard_ov_war");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            float rotation = 0.0f;
            Vector3 position =Vector3.Zero;
            _gameCameraObject.Update(rotation, position, _graphics.GraphicsDevice.Viewport.AspectRatio);
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
