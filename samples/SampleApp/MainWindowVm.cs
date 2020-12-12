using System.Collections.ObjectModel;
using System.Windows.Controls;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SvgResourceGenerator;
using YiSA.Foundation.Common.Extensions;
using YiSA.WPF.Common;

namespace SampleApp
{
    public class MainWindowVm : DisposableBindable
    {
        public IReactiveProperty<PageInfoVm> Current { get; }
        
        public ObservableCollection<PageInfoVm> Pages { get; }

        public MainWindowVm()
        {
            Pages = new ObservableCollection<PageInfoVm>()
            {
                new PageInfoVm("Rotation Icon Demo",IconType.Icon_Spinner_solid,"TODO",new ShowCase1Vm()).DisposeBy(Disposables),
                new PageInfoVm("Icon List",IconType.Icon_List_solid,"TODO",new ShowCase2Vm()).DisposeBy(Disposables),
            };
            
            Current = new ReactiveProperty<PageInfoVm>(Pages[1])
                .DisposeBy(Disposables);
        }
    }
}