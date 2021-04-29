using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.IO.FiltersApi.V1.Types
{
    /// <summary>
    /// TaskName
    /// </summary>
    public enum TaskName
    {
        General,
        Endodontic,
        Periodontic,
        Restorative,
        Hygiene
    };

    /// <summary>
    /// Sensor binning mode
    /// </summary>
    public enum BinningMode
    {
        Unbinned,
        Binned2X2
    }

    /// <summary>
    /// EnhancementMode
    /// </summary>
    public enum EnhancementMode
    {
        None,
        Smooth,
        EdgeLow,
        EdgeHigh,
        EdgePro
    };
}
