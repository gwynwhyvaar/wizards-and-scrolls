using System;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Constants
{
    public class GameConstants
    {
        // camera constants
        // todo: some of these can be in config setting
        public const float NearClip = 0.01f; 
        public const float FarClip = 2000.0f;
        public const float ViewAngle = 45.0f;

        // player -avatar constants
        public const float Velocity = 0.45f;
        public const float TurnSpeed = 0.025f;
        public const float HeightOffset = 0;

        // The MaxRange member is used to prevent the player -avatar from moving off the playing field
        public const int MaxRange = 98;

        // scrolls, clouds , foliage and varying objects settings
        public const int NumScrolls = 12;
        public const int NumRockBarriers = 40;
        public const int NumCloudsBarriers = 50;
        public const int NumTombstones = 10;
        public const int NumTrees = 20;
        public const int NumObelisk = 1;
        public const int NumFoliage = 2560;
        public const int MinDistance = 10;
        public const int MaxDistance = 90;
        public const int MaxRangeTerrain = 98;
        public const int NumPowerUpCount = 1;
        public const int PowerUpBonusSeconds = 10;

        //bounding sphere scaling factors
        public const float WizardBoundingSphereFactor = .7f;
        public const float ScrollBoundingSphereFactor = .5f;
        public const float RockBarrierBoundingSphereFactor = .7f;

        // game timer 
        public static readonly TimeSpan RoundTime = TimeSpan.FromSeconds(60.25);

        // display text
        public const string TimeRemainingText = "Time Remaining: ";
        public const string ScrollsFoundText = "Scrolls Retrieved: ";
        public const string GameWonText = "Level {0} Completed !\nScore: ";
        public const string GameLostText = "Game Lost !";
        public const string PlayAgainText = "Press Enter/Start to play again or Esc/Back to quit";
        public const string ProceedToNextLevelText = "Press Enter/Start to proceed to Next Level";
        public const string Instructions1Text = "Retrieve all Scrolls before time runs out.";
        public const string Instructions2Text = "Control Wizard using keyboard (A, D, W, S) or the left thumbstick.";
        public const string LevelText = "Level: ";
        public const string HighScoreText = "High-score: ";

        // inut state management -this handles the amount of gamepads supported
        public const int MaxGamePadInputs = 4;

        // multi-sample count
        public const int MultiSampleCount = 32;

        // fullscreen settings
        public const bool IsFullScreen = false;
    }
}
