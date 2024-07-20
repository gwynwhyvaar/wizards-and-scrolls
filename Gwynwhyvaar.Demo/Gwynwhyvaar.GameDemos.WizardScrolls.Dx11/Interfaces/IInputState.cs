using Microsoft.Xna.Framework;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces
{
    public interface IInputState
    {
        /// <summary>
        /// Update the input state
        /// </summary>
        void Update();

        // thumbsticks
        public Vector2 GetThumbStickLeft(PlayerIndex? controllingPlayer);
        public Vector2 GetThumbStickLeft(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);
        public Vector2 GetThumbStickRight(PlayerIndex? controllingPlayer);
        public Vector2 GetThumbStickRight(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);

        float GetTriggerLeft(PlayerIndex? controllingPlayer);
        float GetTriggerLeft(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);
        float GetTriggerRight(PlayerIndex? controllingPlayer);
        float GetTriggerRight(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);

        // game actions 
        bool PlayerExit(PlayerIndex? controllingPlayer);
        bool StartGame(PlayerIndex? controllingPlayer);
        float GetPlayerTurn(PlayerIndex? controllingPlayer);
        Vector3 GetPlayerMove(PlayerIndex? controllingPlayer);
    }
}
