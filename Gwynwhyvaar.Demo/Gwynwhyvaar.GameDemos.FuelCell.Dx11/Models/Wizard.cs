using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models
{
    public record class Wizard : GameObject, IGameObjectInterface
    {
        public float ForwardDirection { get; set; } = 0.0f;
        public int MaxRange { get; set; } = GameConstants.MaxRange;
        public Wizard() : base()
        {
        }

        public void LoadContent(ContentManager content, string modelName)
        {
            Model = content.Load<Model>($"3d/{modelName}");
        }

        public void Draw(Matrix view, Matrix projection)
        {
            try
            {
                Matrix worldMatrix =Matrix.Identity;
                Matrix rotationYMatrix =Matrix.CreateRotationX(ForwardDirection);
                Matrix translateMatrix = Matrix.CreateTranslation(Position);

                worldMatrix = rotationYMatrix * translateMatrix;
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World =worldMatrix;
                        effect.View = view;
                        effect.Projection = projection;

                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
