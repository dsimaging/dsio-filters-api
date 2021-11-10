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
    'enhancementMode': 'edgePro'
}");

            _filterParamList.Add(FilterType.Supreme, 
@"{
    'task': 'general',
    'sharpness': 70
}");

            _filterParamList.Add(FilterType.Ae, 
@"{
    'task': 'general',
    'sharpness': 70
}");

            _filterParamList.Add(FilterType.Unmap, 
@"");

            return _filterParamList;
        }
    }
}
