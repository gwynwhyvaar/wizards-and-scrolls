using System;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models
{
    public record class FoliageGameObject : GameObject, IGameObjectInterface
    {
        private string _modelName;
        private string _modelPath;

        private readonly Guid _guid;

        public FoliageGameObject()
        {
            _guid = Guid.NewGuid();
        }

        public Guid ModelGuid
        {
            get { return _guid; }
        }
        public void Draw(Matrix view, Matrix projection)
        {
            throw new System.NotImplementedException();
        }

        public void LoadContent(ContentManager content, string modelName)
        {
            _modelPath = $"3d/{modelName}";
            _modelName = modelName;
            _tempModel = content.Load<Model>(_modelPath);
        }
    }
}
