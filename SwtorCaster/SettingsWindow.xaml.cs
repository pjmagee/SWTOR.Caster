namespace SwtorCaster
{
    using System.Collections.Generic;
    using System.Windows;
    using Newtonsoft.Json;
    using Parser;

    public partial class SettingsWindow : Window
    {
        public Settings Settings { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
