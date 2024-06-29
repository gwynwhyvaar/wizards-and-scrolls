namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants
{
    public class GameConstants
    {
        // camera constants
        // todo: some of these can be in config setting
        public const float NearClip = 1.0f;
        public const float FarClip = 1000.0f;
        public const float ViewAngle = 45.0f;

        // player -avatar constants
        public const float Velocity = 0.75f;
        public const float TurnSpeed = 0.025f;
        // The MaxRange member is used to prevent the player -avatar from moving off the playing field
        public const int MaxRange = 98;
    }
}
