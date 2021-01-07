using SvgResourceGenerator;
using YiSA.Markup.Common;
using YiSA.WPF.Common;

namespace SampleApp
{
    public class ShowCase1Vm : DisposableBindable
    {
        public IconType Icon { get; } = IconType.Icon_Spinner_solid;

        public bool NewProperty{ get;set; }

    }
}