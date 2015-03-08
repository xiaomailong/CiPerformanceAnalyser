using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Visifire.Charts;

namespace CiPerformanceAnalyzer
{
    public class ClientControlBase : UserControl
    {
        private int _maxPoints1 = 200;
        private int _maxPoints2 = 50;

        public ClientControlBase()
        {
            CpuNewCycleSendDataPoints = new DataPointCollection();
            CpuNewCycleRecvDataPoints = new DataPointCollection();

            CpuInputSendDataPoints = new DataPointCollection();
            CpuInputRecvDataPoints = new DataPointCollection();

            CpuResultSendDataPoints = new DataPointCollection();
            CpuResultRecvDataPoints = new DataPointCollection();

            CpuHeartBeatSendDataPoints = new DataPointCollection();
            CpuHeartBeatRecvDataPoints = new DataPointCollection();

            InterruptDataPoints = new DataPointCollection();
            CiCycleDataPoints = new DataPointCollection();

            return;
        }
        /*
         * 共有5种数据，分别为新周期，输入数据，输出数据，心跳信号，配置比较数据，其中配置比较数据
         * 不是运行过程当中数据，所以不做检测
         * 另外还有一种数据为心跳响应延迟检测数据
         */
        public DataPointCollection CpuNewCycleSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection CpuNewCycleRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection CpuInputSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection CpuInputRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection CpuResultSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection CpuResultRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection CpuHeartBeatSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection CpuHeartBeatRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection InterruptDataPoints 
        {
            get;
            set;
        }

        public DataPointCollection CiCycleDataPoints
        {
            get;
            set;
        }

        protected double DataPointsCollectionMin(DataPointCollection points)
        {
            double m = 1000.0;
            foreach (var point in points)
            {
                if (point != null)
                {
                    if (m > point.YValue)
                    {
                        m = point.YValue;
                    }
                }
            }
            return m;
        }

        protected void UpdatePointsData1(DataPointCollection points,uint count,double t_time)
        {
            if (points.Count > _maxPoints1)
            {
                points.RemoveAt(0);
            }

            points.Add(new DataPoint()
                    {
                    AxisXLabel = count.ToString(),
                    YValue = t_time
                    });
        }

        protected void UpdatePointsData2(DataPointCollection points,uint count,double t_time)
        {
            if (points.Count > _maxPoints2)
            {
                points.RemoveAt(0);
            }

            points.Add(new DataPoint()
                    {
                    AxisXLabel = count.ToString(),
                    YValue = t_time
                    });
        }

        public void Update(MonitorFrame frame)
        {
            UpdatePoints(frame);
            UpdateAxisYMin(frame);
        }

        protected void UpdatePoints(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_INTRRUPT_BEGIN:
                    UpdateInterrupt(frame);
                    break;
                case MonitorDataType.MDT_CPU_NEW_CYCLE_T_BEGIN:
                case MonitorDataType.MDT_CPU_NEW_CYCLE_T_END:
                    UpdateCpuNewCycleSend(frame);
                    break;
                case MonitorDataType.MDT_CPU_NEW_CYCLE_R_BEGIN:
                case MonitorDataType.MDT_CPU_NEW_CYCLE_R_END:
                    UpdateCpuNewCycleRecv(frame);
                    break;
                case MonitorDataType.MDT_CPU_INPUT_T_BEGIN:
                case MonitorDataType.MDT_CPU_INPUT_T_END:
                    UpdateCpuInputSend(frame);
                    break;
                case MonitorDataType.MDT_CPU_INPUT_R_BEGIN:
                case MonitorDataType.MDT_CPU_INPUT_R_END:
                    UpdateCpuInputRecv(frame);
                    break;
                case MonitorDataType.MDT_CPU_RESULT_T_BEGIN:
                case MonitorDataType.MDT_CPU_RESULT_T_END:
                    UpdateCpuResultSend(frame);
                    break;
                case MonitorDataType.MDT_CPU_RESULT_R_BEGIN:
                case MonitorDataType.MDT_CPU_RESULT_R_END:
                    UpdateCpuResultRecv(frame);
                    break;
                case MonitorDataType.MDT_CPU_HEART_BEAT_T_BEGIN:
                case MonitorDataType.MDT_CPU_HEART_BEAT_T_END:
                    UpdateCpuHeartBeatSend(frame);
                    break;
                case MonitorDataType.MDT_CPU_HEART_BEAT_R_BEGIN:
                case MonitorDataType.MDT_CPU_HEART_BEAT_R_END:
                    UpdateCpuHeartBeatRecv(frame);
                    break;
                /*series*/
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_T_BEGIN:
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_T_END:
                    UpdateSeriesNewCycleSend(frame);
                    break;
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_R_BEGIN:
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_R_END:
                    UpdateSeriesNewCycleRecv(frame);
                    break;
                case MonitorDataType.MDT_SERIES_INPUT_T_BEGIN:
                case MonitorDataType.MDT_SERIES_INPUT_T_END:
                    UpdateSeriesInputSend(frame);
                    break;
                case MonitorDataType.MDT_SERIES_INPUT_R_BEGIN:
                case MonitorDataType.MDT_SERIES_INPUT_R_END:
                    UpdateSeriesInputRecv(frame);
                    break;
                case MonitorDataType.MDT_SERIES_RESULT_T_BEGIN:
                case MonitorDataType.MDT_SERIES_RESULT_T_END:
                    UpdateSeriesResultSend(frame);
                    break;
                case MonitorDataType.MDT_SERIES_RESULT_R_BEGIN:
                case MonitorDataType.MDT_SERIES_RESULT_R_END:
                    UpdateSeriesResultRecv(frame);
                    break;
                case MonitorDataType.MDT_SERIES_HEART_BEAT_T_BEGIN:
                case MonitorDataType.MDT_SERIES_HEART_BEAT_T_END:
                    UpdateSeriesHeartBeatSend(frame);
                    break;
                case MonitorDataType.MDT_SERIES_HEART_BEAT_R_BEGIN:
                case MonitorDataType.MDT_SERIES_HEART_BEAT_R_END:
                    UpdateSeriesHeartBeatRecv(frame);
                    break;

                case MonitorDataType.MDT_CI_CYCLE_BEGIN:
                case MonitorDataType.MDT_CI_CYCLE_END:
                    UpdateCiCycle(frame);
                    break;

                case MonitorDataType.MDT_EEU_T_BEGIN:
                case MonitorDataType.MDT_EEU_T_END:
                    UpdateEeuSend(frame);
                    break;

                case MonitorDataType.MDT_EEU_R_BEGIN:
                case MonitorDataType.MDT_EEU_R_END:
                    UpdateEeuRecv(frame);
                    break;
            }
            return;
        }
        protected virtual void UpdateAxisYMin(MonitorFrame frame)
        {
            return;
        }

        private static UInt16 interrupt_last_sn = 0;
        private static UInt32 interrupt_last_time = 0;
        private static UInt32 interrupt_count = 0;

        protected void UpdateInterrupt(MonitorFrame frame)
        {
            if (frame.data_type != MonitorDataType.MDT_INTRRUPT_BEGIN)
            {
                return;
            }
            /*丢帧*/
            if (frame.frame_sn - interrupt_last_sn != 1)
            {
                interrupt_last_sn = frame.frame_sn;
                interrupt_last_time = 0;
            }
            UInt32 this_time = frame.time_stamp;
            if (interrupt_last_time == 0)
            {
                interrupt_last_time = this_time;
                return;
            }
            UInt32 time_interval = this_time - interrupt_last_time;
            if (interrupt_count++ > 3)
            {
                UpdatePointsData1(InterruptDataPoints,interrupt_count,(time_interval * 1.0) / 1000);
            }

            interrupt_last_time = this_time;

            return;
        }

        private static UInt16 cpu_new_cycle_send_last_sn = 0;
        private static UInt32 cpu_new_cycle_send_start_time = 0;
        private static UInt32 cpu_new_cycle_send_count = 0;

        protected void UpdateCpuNewCycleSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_NEW_CYCLE_T_BEGIN:
                    cpu_new_cycle_send_start_time = frame.time_stamp;
                    cpu_new_cycle_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CPU_NEW_CYCLE_T_END:
                    if (cpu_new_cycle_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_new_cycle_send_last_sn != 1)
                    {
                        cpu_new_cycle_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_new_cycle_send_start_time;
                    if (cpu_new_cycle_send_count++ > 3)
                    {
                        UpdatePointsData2(CpuNewCycleSendDataPoints,cpu_new_cycle_send_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 cpu_new_cycle_recv_last_sn = 0;
        private static UInt32 cpu_new_cycle_recv_start_time = 0;
        private static UInt32 cpu_new_cycle_recv_count = 0;

        protected void UpdateCpuNewCycleRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_NEW_CYCLE_R_BEGIN:
                    cpu_new_cycle_recv_start_time = frame.time_stamp;
                    cpu_new_cycle_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CPU_NEW_CYCLE_R_END:
                    if (cpu_new_cycle_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_new_cycle_recv_last_sn != 1)
                    {
                        cpu_new_cycle_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_new_cycle_recv_start_time;
                    if (cpu_new_cycle_recv_count++ > 3)
                    {
                        UpdatePointsData2(CpuNewCycleRecvDataPoints,cpu_new_cycle_recv_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 cpu_input_send_last_sn = 0;
        private static UInt32 cpu_input_send_start_time = 0;
        private static UInt32 cpu_input_send_count = 0;

        protected void UpdateCpuInputSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_INPUT_T_BEGIN:
                    cpu_input_send_start_time = frame.time_stamp;
                    cpu_input_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CPU_INPUT_T_END:
                    if (cpu_input_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_input_send_last_sn != 1)
                    {
                        cpu_input_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_input_send_start_time;
                    if (cpu_input_send_count++ > 3)
                    {
                        UpdatePointsData2(CpuInputSendDataPoints,cpu_input_send_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 cpu_input_recv_last_sn = 0;
        private static UInt32 cpu_input_recv_start_time = 0;
        private static UInt32 cpu_input_recv_count = 0;

        protected void UpdateCpuInputRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_INPUT_R_BEGIN:
                    cpu_input_recv_start_time = frame.time_stamp;
                    cpu_input_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CPU_INPUT_R_END:
                    if (cpu_input_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_input_recv_last_sn != 1)
                    {
                        cpu_input_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_input_recv_start_time;
                    if (cpu_input_recv_count++ > 3)
                    {
                        UpdatePointsData2(CpuInputRecvDataPoints,cpu_input_recv_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 cpu_result_send_last_sn = 0;
        private static UInt32 cpu_result_send_start_time = 0;
        private static UInt32 cpu_result_send_count = 0;

        protected void UpdateCpuResultSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_RESULT_T_BEGIN:
                    cpu_result_send_start_time = frame.time_stamp;
                    cpu_result_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CPU_RESULT_T_END:
                    if (cpu_result_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_result_send_last_sn != 1)
                    {
                        cpu_result_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_result_send_start_time;
                    if (cpu_result_send_count++ > 3)
                    {
                        UpdatePointsData2(CpuResultSendDataPoints,cpu_result_send_count,(time_interval * 1.0) / 1000);
                    }
                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 cpu_result_recv_last_sn = 0;
        private static UInt32 cpu_result_recv_start_time = 0;
        private static UInt32 cpu_result_recv_count = 0;

        protected void UpdateCpuResultRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_RESULT_R_BEGIN:
                    cpu_result_recv_start_time = frame.time_stamp;
                    cpu_result_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CPU_RESULT_R_END:
                    if (cpu_result_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_result_recv_last_sn != 1)
                    {
                        cpu_result_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_result_recv_start_time;
                    if (cpu_result_recv_count++ > 3)
                    {
                        UpdatePointsData2(CpuResultRecvDataPoints,cpu_result_recv_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }
        private static UInt16 cpu_heart_beat_send_last_sn = 0;
        private static UInt32 cpu_heart_beat_send_start_time = 0;
        private static UInt32 cpu_heart_beat_send_count = 0;

        protected void UpdateCpuHeartBeatSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_HEART_BEAT_T_BEGIN:
                    cpu_heart_beat_send_start_time = frame.time_stamp;
                    cpu_heart_beat_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CPU_HEART_BEAT_T_END:
                    if (cpu_heart_beat_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_heart_beat_send_last_sn != 1)
                    {
                        cpu_heart_beat_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_heart_beat_send_start_time;
                    if (cpu_heart_beat_send_count++ > 3)
                    {
                        
                    }
                    UpdatePointsData2(CpuHeartBeatSendDataPoints,cpu_heart_beat_send_count,(time_interval * 1.0) / 1000);

                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 cpu_heart_beat_recv_last_sn = 0;
        private static UInt32 cpu_heart_beat_recv_start_time = 0;
        private static UInt32 cpu_heart_beat_recv_count = 0;

        protected void UpdateCpuHeartBeatRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CPU_HEART_BEAT_R_BEGIN:
                    cpu_heart_beat_recv_start_time = frame.time_stamp;
                    cpu_heart_beat_recv_last_sn = frame.frame_sn;

                    break;
                case MonitorDataType.MDT_CPU_HEART_BEAT_R_END:
                    if (cpu_heart_beat_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - cpu_heart_beat_recv_last_sn != 1)
                    {
                        cpu_heart_beat_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - cpu_heart_beat_recv_start_time;
                    if (cpu_heart_beat_recv_count++ > 3)
                    {
                        
                    }
                    UpdatePointsData2(CpuHeartBeatRecvDataPoints,cpu_heart_beat_recv_count,(time_interval * 1.0) / 1000);
                    break;
                default:
                    break;
            }
            return;
        }

        protected virtual void UpdateSeriesNewCycleSend(MonitorFrame frame)
        {
            return;
        }
        protected virtual void UpdateSeriesNewCycleRecv(MonitorFrame frame)
        {
            return;
        }

        protected virtual void UpdateSeriesInputSend(MonitorFrame frame)
        {
            return;
        }
        protected virtual void UpdateSeriesInputRecv(MonitorFrame frame)
        {
            return;
        }

        protected virtual void UpdateSeriesResultSend(MonitorFrame frame)
        {
            return;
        }
        protected virtual void UpdateSeriesResultRecv(MonitorFrame frame)
        {
            return;
        }
        protected virtual void UpdateSeriesHeartBeatSend(MonitorFrame frame)
        {
            return;
        }
        protected virtual void UpdateSeriesHeartBeatRecv(MonitorFrame frame)
        {
            return;
        }

        protected virtual void UpdateEeuSend(MonitorFrame frame)
        {
            return;
        }

        protected virtual void UpdateEeuRecv(MonitorFrame frame)
        {
            return;
        }

        private static UInt16 ci_cycle_last_sn = 0;
        private static UInt32 ci_cycle_start_time = 0;
        private static UInt32 ci_cycle_count = 0;

        protected void UpdateCiCycle(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_CI_CYCLE_BEGIN:
                    ci_cycle_start_time = frame.time_stamp;
                    ci_cycle_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_CI_CYCLE_END:
                    if (ci_cycle_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - ci_cycle_last_sn != 1)
                    {
                        ci_cycle_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - ci_cycle_start_time;
                    if (ci_cycle_count++ > 3)
                    {
                        
                    }
                    UpdatePointsData2(CiCycleDataPoints,ci_cycle_count,(time_interval * 1.0) / 1000);

                    break;
                default:
                    break;
            }
            return;
        }
    }
}
