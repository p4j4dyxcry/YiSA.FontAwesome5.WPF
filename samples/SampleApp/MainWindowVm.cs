using System.Collections.ObjectModel;
using System.Windows.Controls;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using YiSA.Foundation.Common.Extensions;
using YiSA.WPF.Common;

namespace SampleApp
{
    public class MainWindowVm : DisposableBindable
    {
        public IReactiveProperty<Bindable> Current { get; }
        
        public ObservableCollection<PageInfoVm> Pages { get; }

        public MainWindowVm()
        {
            Pages = new ObservableCollection<PageInfoVm>()
            {
                new PageInfoVm("Page1","TODO",new ShowCase1Vm()).DisposeBy(Disposables)
            };
            
            Current = new ReactiveProperty<Bindable>(Pages[0].Page)
                .DisposeBy(Disposables);
        }
    }
}