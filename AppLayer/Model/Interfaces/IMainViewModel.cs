using ModifierCore.Core.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Model.Interfaces
{
    public interface IMainViewModel
    {
        bool IsResize { get; }
        bool IsCompress { get; }
        bool IsRemove { get; set; }
        bool IsBestResolution { get; set; }
        bool IsNormalResolution { get; set; }
        bool IsExtraResolution { get; set; }
        bool IsBestCompress { get; set; }
        bool IsNormalCompress { get; set; }
        bool IsExtraCompress { get; set; }

    }
}
