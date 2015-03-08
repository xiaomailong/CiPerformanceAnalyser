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
    /// PrimarySeriesSlaveCpu.xaml 的交互逻辑
    /// </summary>
    public partial class PrimarySeriesSlaveCpu : ClientControlBase
    {
        public PrimarySeriesSlaveCpu()
        {
            InitializeComponent();

            EeuRecvDataPoints = new DataPointCollection();

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
