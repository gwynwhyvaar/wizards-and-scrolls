using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Extensions;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models
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
            Model = content.Load<Model>($"3d/{modelName}");
        }

        public void Draw(Matrix view, Matrix projection)
        {
            try
            {
                Matrix worldMatrix = Matrix.Identity;
                Matrix rotationYMatrix = Matrix.CreateRotationY(ForwardDirection);
                Matrix translateMatrix = Matrix.CreateTranslation(Position);
              
                worldMatrix = rotationYMatrix * translateMatrix;

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
                throw new Exception(ex.ToString());
            }
        }
        public void Update(GamePadState gamePadState, KeyboardState keyboardState, RockBarrier[] rockBarriers)
        {
            Vector3 futurePosition = Position;
            float turnAmount = 0;

            if (keyboardState.IsKeyDown(Keys.A))
            {
                turnAmount = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                turnAmount = -1;
            }
            else if (gamePadState.ThumbSticks.Left.X != 0)
            {
                turnAmount = -gamePadState.ThumbSticks.Left.X;
            }
            ForwardDirection += turnAmount * GameConstants.TurnSpeed;
            Matrix orientationMatrix = Matrix.CreateRotationY(ForwardDirection);

            Vector3 movement = Vector3.Zero;
            if (keyboardState.IsKeyDown(Keys.W))
            {
                movement.Z = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                movement.Z = 1;
            }
            else if (gamePadState.ThumbSticks.Left.Y != 0)
            {
                movement.Z = gamePadState.ThumbSticks.Left.Y;
            }

            Vector3 speed = Vector3.Transform(movement, orientationMatrix);
            speed *= GameConstants.Velocity;
            futurePosition = Position + speed;

            if (ValidateMovement(futurePosition, rockBarriers))
            {
                Position = futurePosition;
            }
        }

        private bool ValidateMovement(Vector3 futurePosition, RockBarrier[] rockBarriers)
        {
            // dont allow off-terrain movmement ..
            if ((Math.Abs(futurePosition.X) > MaxRange) || (Math.Abs(futurePosition.Z) > MaxRange))
            {
                return false;
            }
            return true;
        }
    }
}
