using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.IO.FiltersApi.V1.Types
{
    public class SelectFilterImageParam
    {
        public EnhancementMode EnhancementMode { get; set; }
        public LutInfo LutInfo { get; set; }
    }
}
