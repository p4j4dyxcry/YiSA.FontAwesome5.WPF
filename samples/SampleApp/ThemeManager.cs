using System;
using System.Linq;
using System.Windows;

namespace SampleApp
{
    public class ThemeManager
    {
        private readonly ResourceDictionary _themeHolder;

        private ResourceDictionary[] Themes = new[]
        {
            new ResourceDictionary()
            {
                Source = new Uri("/MahApps.Metro;component/Styles/Themes/Dark.Blue.xaml",UriKind.RelativeOrAbsolute),                
            },
            new ResourceDictionary()
            {
                Source = new Uri("/MahApps.Metro;component/Styles/Themes/Dark.Orange.xaml",UriKind.RelativeOrAbsolute),
            },
            new ResourceDictionary()
            {
                Source = new Uri("/MahApps.Metro;component/Styles/Themes/Dark.Green.xaml",UriKind.RelativeOrAbsolute),
            },
            new ResourceDictionary()
            {
                Source = new Uri("/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml",UriKind.RelativeOrAbsolute),
            },
            new ResourceDictionary()
            {
                Source = new Uri("/MahApps.Metro;component/Styles/Themes/Light.Orange.xaml",UriKind.RelativeOrAbsolute),
            },
            new ResourceDictionary()
            {
                Source = new Uri("/MahApps.Metro;component/Styles/Themes/Light.Green.xaml",UriKind.RelativeOrAbsolute),
            },
        };

        public ThemeManager(ResourceDictionary themeHolder)
        {
            _themeHolder = themeHolder;
        }
        
        public void SwitchTheme()
        {
            var dictionary = _themeHolder.MergedDictionaries;
            var current = dictionary.First(x => x.Source.ToString().Contains("Themes"));
            dictionary.Remove(current);
            if (current.Source.ToString().Contains("Dark"))
            {
                dictionary.Add(Themes[4]);
            }
            else
            {
                dictionary.Add(Themes[2]);
            }
        }
    }
}