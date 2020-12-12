using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SvgResourceGenerator;
using YiSA.Foundation.Common.Extensions;
using YiSA.WPF.Common;

namespace SampleApp
{
    public class ShowCase2Vm : DisposableBindable
    {
        private readonly IconType[] _icons = Enum.GetValues(typeof(IconType)).OfType<IconType>().ToArray();
        public IEnumerable<IconType> Icons => _icons.Where(_filter);
        public IReactiveProperty<string> SearchWord { get; }
        public IReactiveProperty<IconType> Current { get; }
        public IReactiveProperty<int> Size { get; }
        public IReactiveProperty<double> Angle { get; }
        public IReactiveProperty<double> Opacity { get; }
        public IReactiveProperty<bool> IsEnabled { get; }
        public IReactiveProperty<Color> Foreground { get; }
        public IReactiveProperty<Color> Background { get; }
        public IReactiveProperty<Color> Disable { get; }
        public Color[] Colors { get; } = {
            System.Windows.Media.Colors.IndianRed,
            System.Windows.Media.Colors.CornflowerBlue,
            System.Windows.Media.Colors.LightPink,
            System.Windows.Media.Colors.NavajoWhite,
            System.Windows.Media.Colors.LawnGreen,
            Color.FromArgb(128,128,128,128), 
            System.Windows.Media.Colors.Transparent,
        };
        
        public IReactiveProperty<VerticalAlignment> Vertical { get; }
        
        public VerticalAlignment[] VerticalAlignments { get; } = {
            VerticalAlignment.Top,
            VerticalAlignment.Center,
            VerticalAlignment.Bottom,
        };

        public IReactiveProperty<HorizontalAlignment> Horizontal { get; }
        
        public HorizontalAlignment[] HorizontalAlignments { get; } = {
            HorizontalAlignment.Left,
            HorizontalAlignment.Center,
            HorizontalAlignment.Right,
        };
        
        public IReactiveProperty<Stretch> StretchVal { get; }

        public Stretch[] Stretches { get; } = {
            Stretch.Fill,
            Stretch.None,
            Stretch.Uniform,
            Stretch.UniformToFill,
        };
        
        public IReadOnlyReactiveProperty<string> Xaml { get; }

        public ShowCase2Vm()
        {
            Current = new ReactivePropertySlim<IconType>(_icons[0]).DisposeBy(Disposables);
            SearchWord = new ReactivePropertySlim<string>("alt").DisposeBy(Disposables);
            SearchWord.Throttle(TimeSpan.FromMilliseconds(100), UIDispatcherScheduler.Default)
                .Subscribe(_=>OnPropertyChanged(nameof(Icons)))
                .DisposeBy(Disposables);
            
            Foreground = new ReactivePropertySlim<Color>(System.Windows.Media.Colors.IndianRed).DisposeBy(Disposables);
            Background = new ReactivePropertySlim<Color>(System.Windows.Media.Colors.Transparent).DisposeBy(Disposables);
            Disable    = new ReactivePropertySlim<Color>(Color.FromArgb(128,128,128,128)).DisposeBy(Disposables);
            Size       = new ReactivePropertySlim<int>(128).DisposeBy(Disposables);
            IsEnabled  = new ReactiveProperty<bool>(true).DisposeBy(Disposables);
            Vertical   = new ReactivePropertySlim<VerticalAlignment>(VerticalAlignment.Center).DisposeBy(Disposables);
            Horizontal = new ReactivePropertySlim<HorizontalAlignment>(HorizontalAlignment.Center).DisposeBy(Disposables);
            StretchVal = new ReactivePropertySlim<Stretch>(Stretch.Uniform).AddTo(Disposables);
            Angle      = new ReactivePropertySlim<double>(0d).DisposeBy(Disposables);
            Opacity    = new ReactivePropertySlim<double>(1d).DisposeBy(Disposables);

            Xaml = Observable.Merge(Current.ToUnit(),
                    Foreground.ToUnit(), Background.ToUnit(),
                    Disable.ToUnit()   , Size.ToUnit(),
                    IsEnabled.ToUnit() , Vertical.ToUnit(),
                    Horizontal.ToUnit(), StretchVal.ToUnit(),
                    Angle.ToUnit()     , Opacity.ToUnit())
                .Throttle(TimeSpan.FromMilliseconds(100), UIDispatcherScheduler.Default)
                .Select(x => _buildXaml())
                .ToReadOnlyReactivePropertySlim()
                .DisposeBy(Disposables);
        }

        private string _buildXaml()
        {
            var builder = new StringBuilder();
            builder.AppendLine( "<fontAwesome5:GeometryIcon ");
            builder.AppendLine($"        Icon=\"{Current.Value}\" Width=\"{Size.Value}\" Height=\"{Size.Value}\"");
            builder.AppendLine($"        Foreground=\"{Foreground.Value}\" Background=\"{Background.Value}\" DisableForegroundBrush=\"{Disable.Value}\"");
            builder.AppendLine($"        HorizontalAlignment=\"{Horizontal.Value}\" VerticalAlignment=\"{Vertical.Value}\" Stretch=\"{StretchVal.Value}\"");
            builder.AppendLine($"        IsEnabled=\"{IsEnabled.Value}\" Angle=\"{Angle.Value}\" Opacity=\"{Opacity.Value}\"/>");
            return builder.ToString();
        }

        private bool _filter(IconType iconType)
        {
            if (string.IsNullOrWhiteSpace(SearchWord.Value))
                return true;
            if (Current.Value == iconType)
                return true;
            var word = SearchWord.Value.ToLower();
            if (iconType.ToString().ToLower().Contains(word) is false)
                return false;
            return true;
        }
    }
}