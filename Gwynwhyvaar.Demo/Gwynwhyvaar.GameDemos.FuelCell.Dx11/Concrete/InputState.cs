using System;
using System.Collections.Generic;
using System.Diagnostics;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Concrete
{
    public class InputState : IInputState
    {
        private readonly int _maxGamePadInputs;

        public KeyboardState CurrentKeyboardState;
        public readonly GamePadState[] CurrentGamePadStates;

        public KeyboardState LastKeyboardState;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        public bool GamePadsAvailable = false;

        public TouchCollection TouchState;

        public readonly List<GestureSample> Gestures = new List<GestureSample>();
        public InputState()
        {
            _maxGamePadInputs = GameConstants.MaxGamePadInputs;

            CurrentKeyboardState = new KeyboardState();
            CurrentGamePadStates = new GamePadState[_maxGamePadInputs];

            for (int x = 0; x < _maxGamePadInputs; x++)
            {
                var capabilities = GamePad.GetCapabilities(x);
                Trace.TraceInformation("{0} :{1} - DisplayName: {2} GamePadType: {3}", DateTime.UtcNow, x, capabilities.DisplayName, capabilities.GamePadType);
            }
            LastKeyboardState = new KeyboardState();
            LastGamePadStates = new GamePadState[_maxGamePadInputs];

            GamePadWasConnected = new bool[_maxGamePadInputs];
        }
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            GamePadsAvailable = false;

            for (int x = 0; x < _maxGamePadInputs; x++)
            {
                LastGamePadStates[x] = CurrentGamePadStates[x];
                CurrentGamePadStates[x] = GamePad.GetState((PlayerIndex)x);

                // keep track of whether a gamepad has ever been connected, so we can detect when unplugged.
                if (CurrentGamePadStates[x].IsConnected)
                {
                    GamePadsAvailable = true;
                    GamePadWasConnected[x] = true;
                }
            }

            TouchState = TouchPanel.GetState();
            Gestures.Clear();

            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }
        }

        private bool IsKeyPressed(Keys key) => CurrentKeyboardState.IsKeyDown(key);

        private bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;

                int i = ((int)playerIndex);

                if (i > _maxGamePadInputs)
                {
                    return false;
                }
                return CurrentGamePadStates[i].IsButtonDown(button);
            }
            else
            {
                // Accept input from any player ..
                return (IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                    IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                    IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                    IsButtonPressed(button, PlayerIndex.Four, out playerIndex));
            }
        }
        private bool IsNewKeyPressed(Keys key) => (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        private bool IsKeyHeld(Keys key) => (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyDown(key));
        private bool IsKeyReleased(Keys key) => (CurrentKeyboardState.IsKeyUp(key) && LastKeyboardState.IsKeyDown(key));
        private bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = ((int)playerIndex);
                if (i > _maxGamePadInputs)
                {
                    return false;
                }
                return (CurrentGamePadStates[i].IsButtonDown(button) && LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }
        private bool IsButtonHeld(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int x = ((int)playerIndex);

                if (x > _maxGamePadInputs)
                {
                    return false;
                }
                return (CurrentGamePadStates[x].IsButtonDown(button) && LastGamePadStates[x].IsButtonUp(button));
            }
            else
            {
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }
        private bool IsButtonReleased(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int x = ((int)playerIndex);
                if (x > _maxGamePadInputs)
                {
                    return false;
                }
                return (CurrentGamePadStates[x].IsButtonUp(button) && LastGamePadStates[x].IsButtonDown(button));
            }
            else
            {
                return (IsButtonReleased(button, PlayerIndex.One, out playerIndex) ||
                    IsButtonReleased(button, PlayerIndex.Two, out playerIndex) ||
                    IsButtonReleased(button, PlayerIndex.Three, out playerIndex) ||
                    IsButtonReleased(button, PlayerIndex.Four, out playerIndex));
            }
        }
    }
}
