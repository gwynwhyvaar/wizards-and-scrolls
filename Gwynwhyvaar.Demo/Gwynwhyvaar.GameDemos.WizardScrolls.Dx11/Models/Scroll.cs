using System;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Constants;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Extensions;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models
{
    public record class Scroll : GameObject, IGameObjectInterface
    {
        public bool IsRetrieved { get; set; }

        private SoundEffect _scrollCollect;
        private SoundEffect ScrollCollect
        {
            get
            {
                return _scrollCollect;
            }
            set
            {
                _scrollCollect = value;
            }
        }
        public Scroll() : base()
        {
            IsRetrieved = false;
        }
        public void LoadContent(ContentManager contentManager, string modelName)
        {
            Model = contentManager.Load<Model>($"3d/{modelName}");
            ScrollCollect = contentManager.Load<SoundEffect>("audio/bonus-earned");

            Position = Vector3.Down;
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
                        // we make it glow ..
                        effect.EmissiveColor = Color.DarkViolet.ToVector3();
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
        public void Update(BoundingSphere wizardBoundingSphere)
        {
            if(wizardBoundingSphere.Intersects(this.BoundingSphere) &&!this.IsRetrieved)
            {
                this.IsRetrieved = true;

                ScrollCollect.Play();
            }
        }
    }
}
