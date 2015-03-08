using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiPerformanceAnalyzer
{
    public enum CpuState
    {
        CPU_STATE_NONE      = 0,        /*未知状态*/
        CPU_STATE_PRIMARY   = 1,        /*主CPU*/
        CPU_STATE_SLAVE     = 2,        /*从CPU*/
    }
}
