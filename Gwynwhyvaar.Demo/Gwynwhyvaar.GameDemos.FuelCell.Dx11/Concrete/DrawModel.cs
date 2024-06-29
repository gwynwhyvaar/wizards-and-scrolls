using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Concrete
{
    public class DrawModel : IDrawModelInterface
    {
        public void DrawTerrain(Model model, CameraObject gameCamera)
        {
            try
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach(BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World =Matrix.Identity;
                        effect.View =gameCamera.ViewMatrix;
                        effect.Projection = gameCamera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
            catch (Exception ex)
            {
                // log it
                throw new Exception(ex.Message);
            }
        }
    }
}
