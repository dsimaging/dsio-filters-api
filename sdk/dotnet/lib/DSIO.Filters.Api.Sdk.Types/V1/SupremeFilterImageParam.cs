using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.IO.FiltersApi.V1.Types
{
    public class SupremeFilterImageParam
    {
        public TaskName TaskName { get; set; }
        public BinningMode BinningMode { get; set; }
        public int Sharpness { get; set; }
        public LutInfo LutInfo { get; set; }
    }
}
