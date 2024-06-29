using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Concrete;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11
{
    public class GameHome : Game
    {
        private DrawModel _drawModel;
        private GameObject _groundGameObject;
        private CameraObject _gameCameraObject;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GameHome()
        {
            _graphics = new GraphicsDeviceManager(this);

            _gameCameraObject = new CameraObject();
            _groundGameObject = new GameObject();
            _drawModel = new DrawModel();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            _drawModel.DrawTerrain(_groundGameObject.Model, _gameCameraObject);

            base.Draw(gameTime);
        }
    }
}
