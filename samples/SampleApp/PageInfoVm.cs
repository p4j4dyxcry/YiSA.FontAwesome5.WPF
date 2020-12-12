using System;
using SvgResourceGenerator;
using YiSA.Foundation.Common.Extensions;
using YiSA.WPF.Common;

namespace SampleApp
{
    public class PageInfoVm : DisposableBindable
    {
        public string Label { get; }
        public string Detail { get; }
        
        public IconType Icon { get; }
        
        public Bindable Page { get; }
        
        public PageInfoVm(string label,IconType icon, string detail, Bindable page)
        {
            Label = label;
            Detail = detail;
            Page = page;
            Icon = icon;

            if (page is IDisposable disposable)
                disposable.DisposeBy(Disposables);
        }
    }
}