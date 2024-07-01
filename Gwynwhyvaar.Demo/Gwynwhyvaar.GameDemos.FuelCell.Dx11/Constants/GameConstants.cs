namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants
{
    public class GameConstants
    {
        // camera constants
        // todo: some of these can be in config setting
        public const float NearClip = 0.01f; 
        public const float FarClip = 2000.0f;
        public const float ViewAngle = 45.0f;

        // player -avatar constants
        public const float Velocity = 0.75f;
        public const float TurnSpeed = 0.025f;
        public const float HeightOffset = 0;

        // The MaxRange member is used to prevent the player -avatar from moving off the playing field
        public const int MaxRange = 98;

        // scrolls and objects settings
        public const int NumScrolls = 12;
        public const int NumRockBarriers = 40;
        public const int MinDistance = 10;
        public const int MaxDistance = 90;
        public const int MaxRangeTerrain = 98;

        //bounding sphere scaling factors
        public const float WizardBoundingSphereFactor = .7f;
        public const float ScrollBoundingSphereFactor = .5f;
        public const float RockBarrierBoundingSphereFactor = .7f;
    }
}
