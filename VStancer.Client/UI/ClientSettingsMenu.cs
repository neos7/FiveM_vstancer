﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VStancer.Client.Scripts;
using MenuAPI;
using static VStancer.Client.UI.MenuUtilities;

namespace VStancer.Client.UI
{
    internal class ClientSettingsMenu : Menu
    {
        private readonly ClientSettingsScript _script;

        private MenuCheckboxItem IgnoreEmptyPresetsCheckboxItem { get; set; }

        internal event BoolPropertyChanged BoolPropertyChanged;

        public ClientSettingsMenu(ClientSettingsScript script, string name = Globals.ScriptName, string subtitle = "Client Settings Menu") : base(name, subtitle)
        {
            _script = script;

            _script.ClientSettingsChanged += (sender, args) => { Update(); };

            Update();

            OnCheckboxChange += CheckboxChange;
        }

        private void CheckboxChange(Menu menu, MenuCheckboxItem menuItem, int itemIndex, bool newCheckedState)
        {
            if (string.IsNullOrEmpty(menuItem.ItemData))
                return;

            BoolPropertyChanged?.Invoke(menuItem.ItemData, newCheckedState);
        }

        internal void Update()
        {
            ClearMenuItems();

            if (_script.ClientSettings == null)
                return;

            IgnoreEmptyPresetsCheckboxItem = new MenuCheckboxItem(
                "Ignore Empty Presets",
                "If checked the incomplete presets will only apply partially and all the rest will be kept in the current status, otherwise missing data will just be reset.",
                _script.ClientSettings.IgnoreEmptyPresets)
            {
                ItemData = nameof(_script.ClientSettings.IgnoreEmptyPresets)
            };

            AddMenuItem(IgnoreEmptyPresetsCheckboxItem);
        }
    }
}