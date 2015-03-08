using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Visifire.Charts;

namespace CiPerformanceAnalyzer
{
    class DataReciever :UdpClient
    {
        private MainWindow context;

        public DataReciever(MainWindow t_context,Int32 port)
            :base(port)
        {
            context = t_context;
        }

        /// <summary>
        /// 接收数据并将数据分发下去
        /// </summary>
        public void RecvAndDispatch()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(new IPAddress(0), 0);
            byte[] data;

            try
            {
                if (this.Available == 0)
                {
                    Thread.Sleep(1);
                    return;
                }

                data = this.Receive(ref ipEndPoint);
            }
            catch (Exception)
            {
                return;
            }
            for (int i = 0; i < data.Length / MonitorFrame.frame_len;i++)
            {
                byte[] one_frame = new byte[MonitorFrame.frame_len];
                for (int j = 0; j < MonitorFrame.frame_len; j++)
                {
                    /*parse this frame*/
                    one_frame[j] = data[i * MonitorFrame.frame_len + j];
                }
                MonitorFrame frame = new MonitorFrame(one_frame);

                if (frame.Check() == false)
                {
                    continue;
                }
                try
                {
                    if (frame.series_state == SeriesState.SERIES_PRIMARY)
                    {
                        if (frame.cpu_state == CpuState.CPU_STATE_PRIMARY && context.active_tab == 0)
                        {
                            PrimarySeriesPrimaryCpu chart = (PrimarySeriesPrimaryCpu)context.Shower.Children[0];
                            chart.Update(frame);
                            context.TabPrimarySeriesPrimaryCpu.Header = "主系主CPU" + ipEndPoint.Address.ToString();
                        }
                        else if (frame.cpu_state == CpuState.CPU_STATE_SLAVE && context.active_tab == 1)
                        {
                            PrimarySeriesSlaveCpu chart = (PrimarySeriesSlaveCpu)context.Shower.Children[0];
                            chart.Update(frame);
                            context.TabPrimarySeriesSlaveCpu.Header = "主系从CPU" + ipEndPoint.Address.ToString();
                        }
                        else
                        {
                            /*some thing be wrong*/
                            continue;
                        }
                    }
                    else if (frame.series_state == SeriesState.SERIES_SPARE)
                    {
                        if (frame.cpu_state == CpuState.CPU_STATE_PRIMARY && context.active_tab == 2)
                        {
                            SpareSeriesPrimaryCpu chart = (SpareSeriesPrimaryCpu)context.Shower.Children[0];
                            chart.Update(frame);
                            context.TabSpareSeriesPrimaryCpu.Header = "备系主CPU" + ipEndPoint.Address.ToString();
                        }
                        else if (frame.cpu_state == CpuState.CPU_STATE_SLAVE && context.active_tab == 3)
                        {
                            SpareSeriesSlaveCpu chart = (SpareSeriesSlaveCpu)context.Shower.Children[0];
                            chart.Update(frame);
                            context.TabSpareSeriesSlaveCpu.Header = "备系从CPU" + ipEndPoint.Address.ToString();
                        }
                        else
                        {
                            /*some thing be wrong*/
                            continue;
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return;
        }
    }
}
