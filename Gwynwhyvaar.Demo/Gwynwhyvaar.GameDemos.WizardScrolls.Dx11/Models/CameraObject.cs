using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Constants;

using Microsoft.Xna.Framework;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models
{
    public class CameraObject
    {
        public Vector3 AvatarHeadOffset { get; set; } = new Vector3(0, 7, -15);
        public Vector3 TargetOffset { get; set; } = new Vector3(0, 5, 0);
        public Matrix ViewMatrix { get; set; } = Matrix.Identity;
        public Matrix ProjectionMatrix { get; set; } = Matrix.Identity;

        public void Update(float avataYaw, Vector3 position, float aspectRatio)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(avataYaw);

            Vector3 transformedHeadOffset = Vector3.Transform(AvatarHeadOffset, rotationMatrix);
            Vector3 transformedReference = Vector3.Transform(TargetOffset, rotationMatrix);

            Vector3 cameraPosition = position + transformedHeadOffset;
            Vector3 cameraTarget = position + transformedReference;

            // Calculate the camera's view and projection matrices based on current values.
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(GameConstants.ViewAngle), aspectRatio, GameConstants.NearClip, GameConstants.FarClip);
        }
    }
}
