using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models
{
    public record class Scroll: GameObject, IGameObjectInterface
    {
        public bool IsRetrieved { get; set; }
        public Scroll(): base()
        {
            IsRetrieved = false;
        }
        public void LoadContent(ContentManager contentManager, string modelName)
        {
            Model = contentManager.Load<Model>($"3d/{modelName}");
            Position = Vector3.Down;
        }
        public void Draw(Matrix view, Matrix projection)
        {
            try
            {
                Matrix translateMatrix = Matrix.CreateTranslation(Position);
                Matrix worldMatrix =translateMatrix;
                if (!IsRetrieved)
                {
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
                        mesh.Draw();
                    }
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
