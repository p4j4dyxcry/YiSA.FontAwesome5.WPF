using System;
using System.Linq;
using System.Windows;

namespace SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ThemeManager _themeManager;
        private void SwitchTheme_OnClick(object sender, RoutedEventArgs e)
        {
            _themeManager ??= new ThemeManager(App.Current.Resources);
            _themeManager.SwitchTheme();
        }
    }
}