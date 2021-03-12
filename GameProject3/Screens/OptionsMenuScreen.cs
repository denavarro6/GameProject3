using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameArchitectureExample.StateManagement;

namespace GameArchitectureExample.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {
        

        private readonly MenuEntry _soundMenuEntry;
        private readonly MenuEntry _soundMenuEntryIncrease;
        private readonly MenuEntry _soundMenuEntryDecrease;
        private readonly MenuEntry _musicMenuEntry;
        private readonly MenuEntry _musicMenuEntryIncrease;
        private readonly MenuEntry _musicMenuEntryDecrease;
        private readonly InputAction _menuLeft;
        private readonly InputAction _menuRight;
        //private readonly MenuEntry _frobnicateMenuEntry;
        //private readonly MenuEntry _elfMenuEntry;

        //private static Ungulate _currentUngulate = Ungulate.Dromedary;
        private static int _currentSound = 100;
        private static int _currentMusic = 100;
        

        public OptionsMenuScreen() : base("Options")
        {
            _soundMenuEntry = new MenuEntry(string.Empty);
            _musicMenuEntry = new MenuEntry(string.Empty);
            _soundMenuEntryIncrease = new MenuEntry(string.Empty);
            _soundMenuEntryDecrease = new MenuEntry(string.Empty);
            _musicMenuEntryIncrease = new MenuEntry(string.Empty);
            _musicMenuEntryDecrease = new MenuEntry(string.Empty);
            //_frobnicateMenuEntry = new MenuEntry(string.Empty);
            //_elfMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _soundMenuEntry.Selected += SoundMenuEntrySelected;
            _musicMenuEntry.Selected += MusicMenuEntrySelected;
            _soundMenuEntryIncrease.Selected += SoundMenuEntryISelected;
            _soundMenuEntryDecrease.Selected += SoundMenuEntryDSelected;
            _musicMenuEntryIncrease.Selected += MusicMenuEntryISelected;
            _musicMenuEntryDecrease.Selected += MusicMenuEntryDSelected;
            //_frobnicateMenuEntry.Selected += FrobnicateMenuEntrySelected;
            //_elfMenuEntry.Selected += ElfMenuEntrySelected;
            back.Selected += OnCancel;
            MenuEntries.Add(_soundMenuEntryIncrease);
            MenuEntries.Add(_soundMenuEntry);
            MenuEntries.Add(_soundMenuEntryDecrease);
            MenuEntries.Add(_musicMenuEntryIncrease);
            MenuEntries.Add(_musicMenuEntry);
            MenuEntries.Add(_musicMenuEntryDecrease);
            //MenuEntries.Add(_frobnicateMenuEntry);
           //MenuEntries.Add(_elfMenuEntry);
            MenuEntries.Add(back);
            _menuLeft = new InputAction(
                new[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft },
                new[] { Keys.Left }, true);
            _menuRight = new InputAction(
                new[] { Buttons.DPadRight, Buttons.LeftThumbstickRight },
                new[] { Keys.Right }, true);
        }

        private void MusicMenuEntryDSelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMusic > 0 && _currentMusic <= 100)
                _currentMusic--;
            SetMenuEntryText();
        }

        private void MusicMenuEntryISelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMusic >= 0 && _currentMusic < 100)
                _currentMusic++;
            SetMenuEntryText();
        }

        private void SoundMenuEntryDSelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentSound > 0 && _currentSound <= 100)
                _currentSound--;
            SetMenuEntryText();
        }

        private void SoundMenuEntryISelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentSound >= 0 && _currentSound < 100)
                _currentSound++;
            SetMenuEntryText();
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _soundMenuEntryIncrease.Text = $"Increase Sound Effect Volume";
            _soundMenuEntry.Text = $"Sound Effect Volume: {_currentSound}";
            _soundMenuEntryDecrease.Text = $"Decrease Sound Effect Volume";
            _musicMenuEntryIncrease.Text = $"Increase Music Volume";
            _musicMenuEntry.Text = $"Music Volume: {_currentMusic}";
            _musicMenuEntryDecrease.Text = $"Decrease Music Volume";
            //_frobnicateMenuEntry.Text = $"Frobnicate: {(_frobnicate ? "on" : "off")}";
            //_elfMenuEntry.Text = $"elf: {_elf.ToString()}";
        }

        private void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {


            SetMenuEntryText();
        }
        /*public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if (_menuRight.Occurred(input, ControllingPlayer, out playerIndex))
            {

                if (_currentSound >= 0 && _currentSound <= 100 && _currentSoundSelected)
                    _currentSound++;
                if (_currentMusic >= 0 && _currentMusic <= 100 && _currentMusicSelected)
                    _currentMusic++;

                
            }

            if (_menuLeft.Occurred(input, ControllingPlayer, out playerIndex))
            {
               // _selectedEntry++;

                if (_currentMusic >= 0 && _currentMusic <= 100 && _currentMusicSelected)
                    _currentMusic--;
            }

            /*if (_menuSelect.Occurred(input, ControllingPlayer, out playerIndex))
                OnSelectEntry(_selectedEntry, playerIndex);
            else if (_menuCancel.Occurred(input, ControllingPlayer, out playerIndex))
                OnCancel(playerIndex);
        }*/
        private void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //KeyboardState keyboard = Keyboard.GetState();
            //Inpu
            /*_currentSoundSelected = false;
            _currentMusicSelected = true;*/
            SetMenuEntryText();
        }

        private void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
           //_frobnicate = !_frobnicate;
            SetMenuEntryText();
        }

        private void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //_elf++;
            SetMenuEntryText();
        }
    }
}
