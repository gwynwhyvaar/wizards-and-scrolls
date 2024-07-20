using System;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Extensions;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models
{
    public record class CloudsGameObject : GameObject, IGameObjectInterface
    {
        private string _modelName;
        private string _modelPath;

        private readonly Guid _guid;
        public CloudsGameObject()
        {
            _guid = Guid.NewGuid();
        }
        public Guid ModelGuid
        {
            get { return _guid; }
        }
        public string ModelPath
        {
            get
            {
                return _modelPath;
            }
        }
        public string ModelName
        {
            get { return _modelName; }
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
                ex.LogError();
                // throw it ..
                throw new Exception(ex.Message);
            }
        }

        public void LoadContent(ContentManager content, string modelName)
        {
            _modelPath = $"3d/{modelName}";
            _modelName = modelName;

            _tempModel = content.Load<Model>(_modelPath);
        }
    }
}
