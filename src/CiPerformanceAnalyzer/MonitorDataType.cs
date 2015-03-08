using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiPerformanceAnalyzer
{
    public enum MonitorDataType
    {
        MDT_CPU_CFG_T_BEGIN                     = 1,
        MDT_CPU_CFG_T_END                       = 2,
        MDT_CPU_CFG_R_BEGIN                     = 3,
        MDT_CPU_CFG_R_END                       = 4,

        MDT_CPU_NEW_CYCLE_T_BEGIN               = 5,
        MDT_CPU_NEW_CYCLE_T_END                 = 6,
        MDT_CPU_NEW_CYCLE_R_BEGIN               = 7,
        MDT_CPU_NEW_CYCLE_R_END                 = 8,

        MDT_CPU_INPUT_T_BEGIN                   = 9,
        MDT_CPU_INPUT_T_END                     = 10,
        MDT_CPU_INPUT_R_BEGIN                   = 11,
        MDT_CPU_INPUT_R_END                     = 12,

        MDT_CPU_RESULT_T_BEGIN                  = 13,
        MDT_CPU_RESULT_T_END                    = 14,
        MDT_CPU_RESULT_R_BEGIN                  = 15,
        MDT_CPU_RESULT_R_END                    = 16,

        MDT_CPU_HEART_BEAT_T_BEGIN              = 17,
        MDT_CPU_HEART_BEAT_T_END                = 18,
        MDT_CPU_HEART_BEAT_R_BEGIN              = 19,
        MDT_CPU_HEART_BEAT_R_END                = 20,

        MDT_SERIES_CFG_T_BEGIN                  = 21,
        MDT_SERIES_CFG_T_END                    = 22,
        MDT_SERIES_CFG_R_BEGIN                  = 23,
        MDT_SERIES_CFG_R_END                    = 24,

        MDT_SERIES_NEW_CYCLE_T_BEGIN            = 25,
        MDT_SERIES_NEW_CYCLE_T_END              = 26,
        MDT_SERIES_NEW_CYCLE_R_BEGIN            = 27,
        MDT_SERIES_NEW_CYCLE_R_END              = 28,

        MDT_SERIES_INPUT_T_BEGIN                = 29,
        MDT_SERIES_INPUT_T_END                  = 30,
        MDT_SERIES_INPUT_R_BEGIN                = 31,
        MDT_SERIES_INPUT_R_END                  = 32,

        MDT_SERIES_RESULT_T_BEGIN               = 33,
        MDT_SERIES_RESULT_T_END                 = 34,
        MDT_SERIES_RESULT_R_BEGIN               = 35,
        MDT_SERIES_RESULT_R_END                 = 36,

        MDT_SERIES_HEART_BEAT_T_BEGIN           = 37,
        MDT_SERIES_HEART_BEAT_T_END             = 38,
        MDT_SERIES_HEART_BEAT_R_BEGIN           = 39,
        MDT_SERIES_HEART_BEAT_R_END             = 40,

        MDT_INTRRUPT_BEGIN                      = 41, /*用来检测定时中断*/
        MDT_CI_CYCLE_BEGIN                      = 42, /*联锁周期开始*/
        MDT_CI_CYCLE_END                        = 43, /*联锁周期结束*/

        MDT_EEU_T_BEGIN                         = 44,
        MDT_EEU_T_END                           = 45,

        MDT_EEU_R_BEGIN                         = 46,
        MDT_EEU_R_END                           = 47,
    }
}
