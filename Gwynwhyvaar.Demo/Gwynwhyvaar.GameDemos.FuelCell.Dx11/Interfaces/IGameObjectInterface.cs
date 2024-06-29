using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces
{
    public interface IGameObjectInterface
    {
        public void LoadContent(ContentManager content, string modelName);
        public void Draw(Matrix view, Matrix projection);
    }
}
