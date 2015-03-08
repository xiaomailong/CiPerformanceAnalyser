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
    /// </summary>
    public partial class PrimarySeriesPrimaryCpu : ClientControlBase
    {
        public PrimarySeriesPrimaryCpu()
        {
            InitializeComponent();

            SeriesNewCycleSendDataPoints = new DataPointCollection();
            SeriesInputSendDataPoints = new DataPointCollection();
            SeriesResultSendDataPoints = new DataPointCollection();
            SeriesHeartBeatSendDataPoints = new DataPointCollection();

            EeuSendDataPoints = new DataPointCollection();
            EeuRecvDataPoints = new DataPointCollection();

            SeriesNewCycle.DataPoints = SeriesNewCycleSendDataPoints;
            SeriesInput.DataPoints = SeriesInputSendDataPoints;
            SeriesResult.DataPoints = SeriesResultSendDataPoints;
            SeriesHeartBeat.DataPoints = SeriesHeartBeatSendDataPoints;

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

            EeuSend.DataPoints = EeuSendDataPoints;
            EeuRecv.DataPoints = EeuRecvDataPoints;

        }
        /*
         * 主系的主SERIES负责发送双系之间的数据
         */
        public DataPointCollection SeriesNewCycleSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection SeriesInputSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection SeriesResultSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection SeriesHeartBeatSendDataPoints
        {
            get;
            set;
        }
        public DataPointCollection EeuSendDataPoints
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

            switch (frame.data_type)
            {
                case MonitorDataType.MDT_INTRRUPT_BEGIN:
                    m = DataPointsCollectionMin(InterruptDataPoints);
                    Interrupt.AxisYMininum = m;
                    break;
                /*series*/
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_T_BEGIN:
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_T_END:
                    m = DataPointsCollectionMin(SeriesNewCycleSendDataPoints);
                    //SeriesNewCycle.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_SERIES_INPUT_T_BEGIN:
                case MonitorDataType.MDT_SERIES_INPUT_T_END:
                    m = DataPointsCollectionMin(SeriesInputSendDataPoints);
                    //SeriesInput.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_SERIES_RESULT_T_BEGIN:
                case MonitorDataType.MDT_SERIES_RESULT_T_END:
                    m = DataPointsCollectionMin(SeriesResultSendDataPoints);
                    //SeriesResult.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_SERIES_HEART_BEAT_T_BEGIN:
                case MonitorDataType.MDT_SERIES_HEART_BEAT_T_END:
                    m = DataPointsCollectionMin(SeriesHeartBeatSendDataPoints);
                    //SeriesHeartBeat.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_CI_CYCLE_BEGIN:
                case MonitorDataType.MDT_CI_CYCLE_END:
                    m = DataPointsCollectionMin(CiCycleDataPoints);
                    //CiCycle.AxisYMininum = m;
                    break;
                case MonitorDataType.MDT_EEU_T_BEGIN:
                case MonitorDataType.MDT_EEU_T_END:
                    m = DataPointsCollectionMin(EeuSendDataPoints);
                    //EeuSend.AxisYMininum = m;
                    break;

                case MonitorDataType.MDT_EEU_R_BEGIN:
                case MonitorDataType.MDT_EEU_R_END:
                    m = DataPointsCollectionMin(EeuRecvDataPoints);
                    //EeuRecv.AxisYMininum = m;
                    break;
            }
            return;
        }

        private static UInt16 series_new_cycle_send_last_sn = 0;
        private static UInt32 series_new_cycle_send_start_time = 0;
        private static UInt32 series_new_cycle_send_count = 0;

        protected override void UpdateSeriesNewCycleSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_T_BEGIN:
                    series_new_cycle_send_start_time = frame.time_stamp;
                    series_new_cycle_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_SERIES_NEW_CYCLE_T_END:
                    if (series_new_cycle_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_new_cycle_send_last_sn != 1)
                    {
                        series_new_cycle_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_new_cycle_send_start_time;
                    if (series_new_cycle_send_count++ > 3)
                    {
                        UpdatePointsData2(SeriesNewCycleSendDataPoints,series_new_cycle_send_count,(time_interval * 1.0) / 1000);
                    }
                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 series_input_send_last_sn = 0;
        private static UInt32 series_input_send_start_time = 0;
        private static UInt32 series_input_send_count = 0;

        protected override void UpdateSeriesInputSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_INPUT_T_BEGIN:
                    series_input_send_start_time = frame.time_stamp;
                    series_input_send_last_sn = frame.frame_sn;

                    break;
                case MonitorDataType.MDT_SERIES_INPUT_T_END:
                    if (series_input_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_input_send_last_sn != 1)
                    {
                        series_input_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_input_send_start_time;
                    if (series_input_send_count++ > 3)
                    {
                        UpdatePointsData2(SeriesInputSendDataPoints,series_input_send_count,(time_interval * 1.0) / 1000);
                    }
                    break;
                default:
                    break;
            }
            return;
        }
        private static UInt16 series_result_send_last_sn = 0;
        private static UInt32 series_result_send_start_time = 0;
        private static UInt32 series_result_send_count = 0;

        protected override void UpdateSeriesResultSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_RESULT_T_BEGIN:
                    series_result_send_start_time = frame.time_stamp;
                    series_result_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_SERIES_RESULT_T_END:
                    if (series_result_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_result_send_last_sn != 1)
                    {
                        series_result_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }
                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_result_send_start_time;
                    if (series_result_send_count++ > 3)
                    {
                        UpdatePointsData2(SeriesResultSendDataPoints,series_result_send_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }
        private static UInt16 series_heart_beat_send_last_sn = 0;
        private static UInt32 series_heart_beat_send_start_time = 0;
        private static UInt32 series_heart_beat_send_count = 0;

        protected override void UpdateSeriesHeartBeatSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_SERIES_HEART_BEAT_T_BEGIN:
                    series_heart_beat_send_start_time = frame.time_stamp;
                    series_heart_beat_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_SERIES_HEART_BEAT_T_END:
                    if (series_heart_beat_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - series_heart_beat_send_last_sn != 1)
                    {
                        series_heart_beat_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }

                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - series_heart_beat_send_start_time;
                    if (series_heart_beat_send_count++ > 3)
                    {
                        UpdatePointsData2(SeriesHeartBeatSendDataPoints,series_heart_beat_send_count,(time_interval * 1.0) / 1000);
                    }

                    break;
                default:
                    break;
            }
            return;
        }

        private static UInt16 eeu_send_last_sn = 0;
        private static UInt32 eeu_send_start_time = 0;
        private static UInt32 eeu_send_count = 0;

        protected override void UpdateEeuSend(MonitorFrame frame)
        {
            switch (frame.data_type)
            {
                case MonitorDataType.MDT_EEU_T_BEGIN:
                    eeu_send_start_time = frame.time_stamp;
                    eeu_send_last_sn = frame.frame_sn;
                    break;
                case MonitorDataType.MDT_EEU_T_END:
                    if (eeu_send_start_time == 0)
                    {
                        break;
                    }
                    /*丢帧*/
                    if (frame.frame_sn - eeu_send_last_sn != 1)
                    {
                        eeu_send_start_time = 0;
                        /*丢弃本次统计*/
                        break;
                    }

                    UInt32 end_time = frame.time_stamp;
                    UInt32 time_interval = end_time - eeu_send_start_time;
                    if (eeu_send_count++ > 3)
                    {
                        UpdatePointsData2(EeuSendDataPoints,eeu_send_count,(time_interval * 1.0) / 1000);
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
