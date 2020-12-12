using System;
using YiSA.Foundation.Common.Extensions;
using YiSA.WPF.Common;

namespace SampleApp
{
    public class PageInfoVm : DisposableBindable
    {
        public string Label { get; }
        public string Detail { get; }
        public Bindable Page { get; }
        
        public PageInfoVm(string label, string detail, Bindable page)
        {
            Label = label;
            Detail = detail;
            Page = page;

            if (page is IDisposable disposable)
                disposable.DisposeBy(Disposables);
        }
    }
}