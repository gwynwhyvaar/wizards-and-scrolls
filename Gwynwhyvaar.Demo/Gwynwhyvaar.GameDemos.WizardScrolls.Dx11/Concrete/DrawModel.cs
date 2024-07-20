using System;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Extensions;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Concrete
{
    public class DrawModel : IDrawModelInterface
    {
        public void DrawTerrain(Model model, CameraObject gameCamera)
        {
            try
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Identity;
                        effect.View = gameCamera.ViewMatrix;
                        effect.Projection = gameCamera.ProjectionMatrix;

                        effect.SetSolidEffect();
                    }
                    mesh.Draw();
                }
            }
            catch (Exception ex)
            {
                // log it
                ex.LogError();
                // throw ..
                throw new Exception(ex.Message);
            }
        }
    }
}
