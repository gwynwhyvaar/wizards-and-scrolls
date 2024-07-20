using System;
using System.Collections.Generic;
using System.Diagnostics;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Constants;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Concrete
{
    public class InputState : IInputState
    {
        private readonly Game _game;
        private readonly int _maxGamePadInputs;

        public KeyboardState CurrentKeyboardState;
        public readonly GamePadState[] CurrentGamePadStates;

        public KeyboardState LastKeyboardState;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        public bool GamePadsAvailable = false;

        public TouchCollection TouchState;

        public readonly List<GestureSample> Gestures = new List<GestureSample>();
        public InputState(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game", "Game object cannot be null.");
            }
            _game = game;
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

            if (_game.Services.GetService(typeof(IInputState)) != null)
            {
                throw new ArgumentException("An Input state class is already register.");
            }
            _game.Services.AddService(typeof(IInputState), this);
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

        // helpers
        public Vector2 GetThumbStickLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetThumbStickLeft(controllingPlayer, out playerIndex);
        }
        public Vector2 GetThumbStickLeft(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int x = ((int)playerIndex);
                return CurrentGamePadStates[x].ThumbSticks.Left;
            }
            else
            {
                for (int index = 0; index < _maxGamePadInputs; index++)
                {
                    if (CurrentGamePadStates[index].IsConnected)
                    {
                        playerIndex = (PlayerIndex)index;
                        return CurrentGamePadStates[index].ThumbSticks.Left;
                    }
                }
                playerIndex = PlayerIndex.One;
                return Vector2.Zero;
            }
        }
        public Vector2 GetThumbStickRight(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetThumbStickRight(controllingPlayer, out playerIndex);
        }
        public Vector2 GetThumbStickRight(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int x = ((int)playerIndex);
                return CurrentGamePadStates[x].ThumbSticks.Right;
            }
            else
            {
                for (int x = 0; x < _maxGamePadInputs; x++)
                {
                    if (CurrentGamePadStates[x].IsConnected)
                    {
                        playerIndex = (PlayerIndex)x;
                        return CurrentGamePadStates[x].ThumbSticks.Right;
                    }
                }
                playerIndex = PlayerIndex.One;
                return Vector2.Zero;
            }
        }
        public float GetTriggerLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetTriggerLeft(controllingPlayer, out playerIndex);
        }
        public float GetTriggerLeft(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int x = ((int)playerIndex);
                return CurrentGamePadStates[x].Triggers.Left;
            }
            else
            {
                for (int x = 0; x < _maxGamePadInputs; x++)
                {
                    if (CurrentGamePadStates[x].IsConnected)
                    {
                        playerIndex = (PlayerIndex)x;
                        return CurrentGamePadStates[x].Triggers.Left;
                    }
                }
                playerIndex = PlayerIndex.One;
                return 0;
            }
        }
        public float GetTriggerRight(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetTriggerRight(controllingPlayer, out playerIndex);
        }
        public float GetTriggerRight(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int x = ((int)playerIndex);
                return CurrentGamePadStates[x].Triggers.Right;
            }
            else
            {
                for (int x = 0; x < _maxGamePadInputs; x++)
                {
                    if (CurrentGamePadStates[x].IsConnected)
                    {
                        playerIndex = (PlayerIndex)x;
                        return CurrentGamePadStates[x].Triggers.Right;
                    }
                }
                playerIndex = PlayerIndex.One;
                return 0;
            }
        }
        public bool PlayerExit(PlayerIndex? controllingPlayer) => IsNewKeyPressed(Keys.Escape) || IsNewButtonPress(Buttons.Back, controllingPlayer, out _);
        public bool StartGame(PlayerIndex? controllingPlayer) => IsNewKeyPressed(Keys.Enter) || IsNewButtonPress(Buttons.Start, controllingPlayer, out _);
        public float GetPlayerTurn(PlayerIndex? controllingPlayer)
        {
            float turnAmount = 0;
            Vector2 thumStickValue = GetThumbStickLeft(controllingPlayer);
            if (IsKeyHeld(Keys.A) || IsKeyHeld(Keys.Left))
            {
                turnAmount = 1;
            }
            else if (IsKeyHeld(Keys.D) || IsKeyHeld(Keys.Right))
            {
                turnAmount = -1;
            }
            else if (thumStickValue.X != 0)
            {
                turnAmount = -thumStickValue.X;
            }
            return turnAmount;
        }
        public Vector3 GetPlayerMove(PlayerIndex? controllingPlayer)
        {
            Vector3 movement = Vector3.Zero;
            Vector2 thumbstickValue = GetThumbStickLeft(controllingPlayer);
            if (IsKeyHeld(Keys.W) || IsKeyHeld(Keys.Up))
            {
                movement.Z = 1;
            }
            else if (IsKeyHeld(Keys.S)|| IsKeyHeld(Keys.Down))
            {
                movement.Z = -1;
            }
            else if (thumbstickValue.Y != 0)
            {
                movement.Z = thumbstickValue.Y;
            }
            return movement;
        }
    }
}
