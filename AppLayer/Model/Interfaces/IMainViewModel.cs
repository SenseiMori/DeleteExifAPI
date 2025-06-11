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
        bool IsBestWeight { get; set; }
        bool IsNormalWeight { get; set; }
        bool IsExtraWeight { get; set; }
        bool IsBestCompress { get; set; }
        bool IsNormalCompress { get; set; }
        bool IsExtraCompress { get; set; }

    }
}
