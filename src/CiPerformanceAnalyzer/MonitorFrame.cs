using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CiPerformanceAnalyzer
{
    public class MonitorFrame
    {
        public UInt16 frame_head_tag;
        public UInt16 frame_sn;
        public UInt32 time_stamp;
        public MonitorDataType data_type;
        public SeriesState series_state;
        public CpuState cpu_state;
        public UInt16 data_length;
        public UInt16 crc;

        public static Int16 frame_len = 16;
        private bool is_ok = false;

        public MonitorFrame(byte[] buf)
        {
            if (buf.Length < frame_len)
            {
                return;
            }
            frame_head_tag = BitConverter.ToUInt16(buf, 0);
            if (frame_head_tag != 0xAA50)
            {
                return;
            }

            frame_sn = BitConverter.ToUInt16(buf, 2);
            time_stamp = BitConverter.ToUInt32(buf, 4);
            data_type = (MonitorDataType)BitConverter.ToUInt16(buf, 8);
            series_state = (SeriesState)buf[10];
            cpu_state = (CpuState)buf[11];
            data_length = BitConverter.ToUInt16(buf, 12);

            crc = BitConverter.ToUInt16(buf,14);

            UInt16 new_crc = Crc16.GetCrc(buf, 14);
            if (new_crc != crc)
            {
                return;
            }
            is_ok = true;
            return;
        }
        public bool Check()
        {
            return is_ok;
        }
    }
}
