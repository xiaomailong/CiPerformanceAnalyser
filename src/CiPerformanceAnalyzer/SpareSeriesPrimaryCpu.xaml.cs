using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire.Charts;

namespace CiPerformanceAnalyzer
{
    /// <summary>
    /// SpareSeriesPrimaryCpu.xaml 的交互逻辑
    /// </summary>
    public partial class SpareSeriesPrimaryCpu : ClientControlBase
    {
        public SpareSeriesPrimaryCpu()
        {
            InitializeComponent();

            SeriesNewCycleRecvDataPoints = new DataPointCollection();
            SeriesInputRecvDataPoints = new DataPointCollection();
            SeriesResultRecvDataPoints = new DataPointCollection();
            SeriesHeartBeatRecvDataPoints = new DataPointCollection();
            EeuRecvDataPoints = new DataPointCollection();

            SeriesNewCycle.DataPoints = SeriesNewCycleRecvDataPoints;
            SeriesInput.DataPoints = SeriesInputRecvDataPoints;
            SeriesResult.DataPoints = SeriesResultRecvDataPoints;
            SeriesHeartBeat.DataPoints = SeriesHeartBeatRecvDataPoints;

            CiCycle.DataPoints = CiCycleDataPoints;

            Interrupt.DataPoints = InterruptDataPoints;

            CpuNewCycle.DataPointsSend = CpuNewCycleSendDataPoints;
            CpuNewCycle.DataPointsRecv = CpuNewCycleRecvDataPoints;

            CpuInput.DataPointsSend = CpuInputSendDataPoints;
            CpuInput.DataPointsRecv = CpuInputRecvDataPoints;

            CpuResult.DataPointsSend = CpuResultSendDataPoints;
            CpuResult.DataPointsRecv = CpuResultRecvDataPoints;

            CpuHeartBeat.DataPointsSend = CpuHeartBeatSendDataPoints;
            CpuHeartBeat.DataPointsRecv = CpuHeartBeatRecvDataPoints;

            EeuRecv.DataPoints = EeuRecvDataPoints;
        }
        /*
         * 备系的主CPU负责接收双系之间的数据
         */
        public DataPointCollection SeriesNewCycleRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection SeriesInputRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection SeriesResultRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection SeriesHeartBeatRecvDataPoints
        {
            get;
            set;
        }
        public DataPointCollection EeuRecvDataPoints
        {
            get;
            set;
        }

        protected override void UpdateAxisYMin(MonitorFrame frame)
        {
            double m = 0;

            /*发现下面代码去掉注释后会引发异常，暂时不知道什么原因*/
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_INTRRUPT_BEGIN:
                    m = DataPointsCollectionMin(InterruptDataPoints);
                    Interrupt.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_R_BEGIN:
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_R_END:
                    m = DataPointsCollectionMin(SeriesNewCycleRecvDataPoints);
                    //SeriesNewCycle.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_SERIES_INPUT_R_BEGIN:
                case MonitorDataType.MDT_SERIES_INPUT_R_END:
                    m = DataPointsCollectionMin(SeriesInputRecvDataPoints);
                    //SeriesInput.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_SERIES_RESULT_R_BEGIN:
                case MonitorDataType.MDT_SERIES_RESULT_R_END:
                    m = DataPointsCollectionMin(SeriesResultRecvDataPoints);
                    //SeriesResult.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_SERIES_HEART_BEAT_R_BEGIN:
                case MonitorDataType.MDT_SERIES_HEART_BEAT_R_END:
                    m = DataPointsCollectionMin(SeriesHeartBeatRecvDataPoints);
                    //SeriesHeartBeat.AxisYMininum = m;
                    break;

                case MonitorDataType.MDT_CI_CYCLE_BEGIN:
                case MonitorDataType.MDT_CI_CYCLE_END:
                    m = DataPointsCollectionMin(CiCycleDataPoints);
                    //CiCycle.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_EEU_R_BEGIN:
                case MonitorDataType.MDT_EEU_R_END:
                    m = DataPointsCollectionMin(EeuRecvDataPoints);
                    //EeuRecv.AxisYMininum = m;
                    break;
            }
            return;
        }
        private static UInt16 series_new_cycle_recv_last_sn = 0;
        private static UInt32 series_new_cycle_recv_start_time = 0;
        private static UInt32 series_new_cycle_recv_count = 0;

        protected override void UpdateSeriesNewCycleRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_R_BEGIN:
                    series_new_cycle_recv_start_time = frame.time_stamp;
                    series_new_cycle_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_R_END:
                    if (series_new_cycle_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_new_cycle_recv_last_sn != 1)
                    {
                        series_new_cycle_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_new_cycle_recv_start_time;
                    if (series_new_cycle_recv_count++ > 3)
                    {
                        UpdatePointsData2(SeriesNewCycleRecvDataPoints,series_new_cycle_recv_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 series_input_recv_last_sn = 0;
        private static UInt32 series_input_recv_start_time = 0;
        private static UInt32 series_input_recv_count = 0;

        protected override void UpdateSeriesInputRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_INPUT_R_BEGIN:
                    series_input_recv_start_time = frame.time_stamp;
                    series_input_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_SERIES_INPUT_R_END:
                    if (series_input_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_input_recv_last_sn != 1)
                    {
                        series_input_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_input_recv_start_time;
                    if (series_input_recv_count++ > 3)
                    {
                        UpdatePointsData2(SeriesInputRecvDataPoints,series_input_recv_count,(time_interval * 1.0) / 1000);
                    }
                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 series_result_recv_last_sn = 0;
        private static UInt32 series_result_recv_start_time = 0;
        private static UInt32 series_result_recv_count = 0;

        protected override void UpdateSeriesResultRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_RESULT_R_BEGIN:
                    series_result_recv_start_time = frame.time_stamp;
                    series_result_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_SERIES_RESULT_R_END:
                    if (series_result_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_result_recv_last_sn != 1)
                    {
                        series_result_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_result_recv_start_time;
                    if (series_result_recv_count++ > 3)
                    {
                        UpdatePointsData2(SeriesResultRecvDataPoints,series_result_recv_count,(time_interval * 1.0) / 1000);
                    }
                    break;
                default:
                    break;
            }
            return;
        }
        private static UInt16 series_heart_beat_recv_last_sn = 0;
        private static UInt32 series_heart_beat_recv_start_time = 0;
        private static UInt32 series_heart_beat_recv_count = 0;

        protected override void UpdateSeriesHeartBeatRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_HEART_BEAT_R_BEGIN:
                    series_heart_beat_recv_start_time = frame.time_stamp;
                    series_heart_beat_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_SERIES_HEART_BEAT_R_END:
                    if (series_heart_beat_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_heart_beat_recv_last_sn != 1)
                    {
                        series_heart_beat_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_heart_beat_recv_start_time;
                    if (series_heart_beat_recv_count++ > 3)
                    {
                        UpdatePointsData2(SeriesHeartBeatRecvDataPoints,series_heart_beat_recv_count,(time_interval * 1.0) / 1000);
                    }
                    break;
                default:
                    break;
            }
            return;
        }
        private static UInt16 eeu_recv_last_sn = 0;
        private static UInt32 eeu_recv_start_time = 0;
        private static UInt32 eeu_recv_count = 0;

        protected override void UpdateEeuRecv(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_EEU_R_BEGIN:
                    eeu_recv_start_time = frame.time_stamp;
                    eeu_recv_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_EEU_R_END:
                    if (eeu_recv_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - eeu_recv_last_sn != 1)
                    {
                        eeu_recv_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }

                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - eeu_recv_start_time;
                    if (eeu_recv_count++ > 3)
                    {
                        UpdatePointsData2(EeuRecvDataPoints,eeu_recv_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }
    }
}
