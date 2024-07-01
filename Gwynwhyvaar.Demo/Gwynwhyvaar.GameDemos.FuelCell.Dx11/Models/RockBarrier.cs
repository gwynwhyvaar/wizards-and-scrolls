using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Extensions;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models
{
    public record class RockBarrier: GameObject, IGameObjectInterface
    {
        public string RockBarrierType { get; set; }
        public RockBarrier(): base()
        {
        }

        public void LoadContent(ContentManager contentManager, string modelName)
        {
            Model = contentManager.Load<Model>($"3d/{modelName}");
            Position = Vector3.Down;
            RockBarrierType = modelName;
            BoundingSphere = CalculateBoundingSphere();
            // .......
            BoundingSphere scaledSphere;
            scaledSphere = BoundingSphere;
            scaledSphere.Radius *= GameConstants.ScrollBoundingSphereFactor;
            BoundingSphere = new BoundingSphere(scaledSphere.Center, scaledSphere.Radius);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            try
            {
                Matrix translateMatrix = Matrix.CreateTranslation(Position);
                Matrix worldMatrix = translateMatrix;
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = worldMatrix;
                        effect.View = view;
                        effect.Projection = projection;

                        effect.SetSolidEffect();
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
