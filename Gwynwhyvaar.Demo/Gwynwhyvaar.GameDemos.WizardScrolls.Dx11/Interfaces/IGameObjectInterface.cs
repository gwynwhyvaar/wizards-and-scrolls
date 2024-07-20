using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces
{
    public interface IGameObjectInterface
    {
        public void LoadContent(ContentManager content, string modelName);
        public void Draw(Matrix view, Matrix projection);
    }
}
