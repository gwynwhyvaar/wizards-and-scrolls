using System;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Constants;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Extensions;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models
{
    public record class Wizard : GameObject, IGameObjectInterface
    {
        public float ForwardDirection { get; set; } = 0.0f;
        public int MaxRange { get; set; } = GameConstants.MaxRange;

        public Wizard() : base()
        {
            Position = new Vector3(0, GameConstants.HeightOffset, 0);
        }

        public void LoadContent(ContentManager content, string modelName)
        {
            _tempModel = content.Load<Model>($"3d/{modelName}");

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
                Matrix worldMatrix = Matrix.Identity;
                Matrix rotationYMatrix = Matrix.CreateRotationY(ForwardDirection);
                Matrix translateMatrix = Matrix.CreateTranslation(Position);

                worldMatrix = rotationYMatrix * translateMatrix;

                Matrix[] transforms = new Matrix[Model.Bones.Count];
                Model.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = transforms[mesh.ParentBone.Index] * worldMatrix;
                        effect.View = view;
                        effect.Projection = projection;
                        effect.SetSolidEffect();
                    }
                    mesh.Draw();
                }
            }
            catch (Exception ex)
            {
                ex.LogError();
                throw new Exception(ex.ToString());
            }
        }
        public void Update(IInputState inputState, RockBarrier[] rockBarriers)
        {
            Vector3 futurePosition = Position;

            ForwardDirection += inputState.GetPlayerTurn(PlayerIndex.One) * GameConstants.TurnSpeed;

            Matrix orientationMatrix = Matrix.CreateRotationY(ForwardDirection);

            Vector3 speed = Vector3.Transform(inputState.GetPlayerMove(PlayerIndex.One), orientationMatrix);

            speed *= GameConstants.Velocity;
            futurePosition = Position + speed;

            if (ValidateMovement(futurePosition, rockBarriers))
            {
                Position = futurePosition;
                BoundingSphere updatedSphere;
                updatedSphere = this.BoundingSphere;

                updatedSphere.Center.X = Position.X;
                updatedSphere.Center.Z = Position.Z;

                this.BoundingSphere = new BoundingSphere(updatedSphere.Center, updatedSphere.Radius);
            }
        }
        private bool ValidateMovement(Vector3 futurePosition, RockBarrier[] rockBarriers)
        {
            BoundingSphere futureBoundingSphere = this.BoundingSphere;
            futureBoundingSphere.Center.X = futurePosition.X;
            futureBoundingSphere.Center.Z = futurePosition.Z;
            // dont allow off-terrain movmement ..
            if ((Math.Abs(futurePosition.X) > MaxRange) || (Math.Abs(futurePosition.Z) > MaxRange))
            {
                return false;
            }
            // dont allow moving through a rock barrier
            if (CheckForBarrierCollision(futureBoundingSphere, rockBarriers))
            {
                return false;
            }
            return true;
        }
        private bool CheckForBarrierCollision(BoundingSphere wizardBoundingSphere, RockBarrier[] rockBarriers)
        {
            for (int i = 0; i < rockBarriers.Length; i++)
            {
                if (wizardBoundingSphere.Intersects(rockBarriers[i].BoundingSphere))
                {
                    return true;
                }
            }
            return false;
        }
        public void Reset()
        {
            Position = Vector3.Zero;
            ForwardDirection = 0f;
        }
    }
}
