using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSample.Types;

namespace WpfSample.Defaults
{
    public static class FilterDefaults
    {
        public static Dictionary<FilterType, string> GetValues()
        {
            Dictionary<FilterType, string> _filterParamList = new Dictionary<FilterType, string>();


            _filterParamList.Add(FilterType.Select, 
@"{
    'enhancementMode': 'edgePro',
    'lutInfo': 
    {
        'gamma': 2.3,
        'slope': 65535,
        'offset': 0,
        'totalGrays': 4096,
        'minimumGray': 3612,
        'maximumGray': 418
    }
}");

            _filterParamList.Add(FilterType.Supreme, 
@"{
    'taskName': 'general',
    'binningMode': 'Unbinned',
    'sharpness': 70,
    'lutInfo': 
    {
        'gamma': 2.3,
        'slope': 65535,
        'offset': 0,
        'totalGrays': 4096,
        'minimumGray': 3612,
        'maximumGray': 418
    }
}");

            _filterParamList.Add(FilterType.Ae, 
@"{
    'taskName': 'general',
    'sharpness': 70,
    'lutInfo': 
    {
        'gamma': 2.3,
        'slope': 65535,
        'offset': 0,
        'totalGrays': 4096,
        'minimumGray': 3612,
        'maximumGray': 418
    }
}");

            _filterParamList.Add(FilterType.Unmap, 
@"{
    'gamma': 2.3,
    'slope': 65535,
    'offset': 0,
    'totalGrays': 4096,
    'minimumGray': 3612,
    'maximumGray': 418
}");

            return _filterParamList;
        }
    }
}
