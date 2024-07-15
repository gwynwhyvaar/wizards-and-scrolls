using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Extensions;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models
{
    public record class CloudsGameObject : GameObject, IGameObjectInterface
    {
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

        public void LoadContent(ContentManager content, string modelName)
        {
            Model = content.Load<Model>($"3d/{modelName}");
        }
    }
}
