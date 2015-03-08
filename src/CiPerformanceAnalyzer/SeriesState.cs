using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiPerformanceAnalyzer
{
    public enum SeriesState
    {
        SERIES_PENDING = 0,        /*停机状态*/
        SERIES_CHECK = 1,        /*校核状态*/
        SERIES_PRIMARY = 2,        /*主机状态*/
        SERIES_SPARE = 3,        /*热备状态*/
    }
}
