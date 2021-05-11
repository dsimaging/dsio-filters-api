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
        public static Dictionary<FILTER_TYPE, string> GetValues()
        {
            Dictionary<FILTER_TYPE, string> _filterParamList = new Dictionary<FILTER_TYPE, string>();


            _filterParamList.Add(FILTER_TYPE.SELECT_FILTER, @"{
'enhancementMode': 'edgePro',
'lutInfo': {
                'gamma': 2.3,
'slope': 65535,
'offset': 0,
'totalGrays': 4096,
'minimumGray': 3612,
'maximumGray': 418
}
        }");

            _filterParamList.Add(FILTER_TYPE.SUPREME_FILTER, @"{
  'task': 'general',
  'binningMode': 'binned2X2',
  'sharpness': 70,
  'lutInfo': {
                'gamma': 2.3,
    'slope': 65535,
    'offset': 0,
    'totalGrays': 4096,
    'minimumGray': 3612,
    'maximumGray': 418
  }
        }");

            _filterParamList.Add(FILTER_TYPE.AE_FILTER, @"{
  'task': 'general',
  'sharpness': 70,
  'lutInfo': {
                'gamma': 2.3,
    'slope': 65535,
    'offset': 0,
    'totalGrays': 4096,
    'minimumGray': 3612,
    'maximumGray': 418
  }
        }");

            _filterParamList.Add(FILTER_TYPE.UNMAP_FILTER, @"{
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
