using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models;

using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces
{
    public interface IDrawModelInterface
    {
        public void DrawTerrain(Model model, CameraObject gameCamera);
    }
}
