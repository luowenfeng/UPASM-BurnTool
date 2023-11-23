using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BurnTool
{
	internal class IICConnection
    {
        public static bool InitDLL()
        {
            int res = 0;
            try
			{
                res = IICConnection_Init();
                if (res == 0)
				{
                    throw new Exception("IICConnection_Init失败, 请确认驱动被正确安装!");
				}
            }
            catch(Exception ex)
			{
                MessageBox.Show(ex.Message);
            }
            return res > 0;
        }

        public static void DeinitDLL()
        {
            IICConnection_Deinit();
        }

        protected IntPtr iic = (IntPtr)0;
        protected bool bigEndian = false;
        public IICConnection(bool _bigEndian, byte i2c_addr)
        {
            bigEndian = _bigEndian;
            iic = IICConnection_Create(bigEndian ? 0 : 1, bigEndian ? 2 : 4, i2c_addr);
            if (iic == (IntPtr)0)
            {
                throw new Exception("IICConnection_Create failed...");
            }
        }
        ~IICConnection()
		{
            if (iic != (IntPtr)0)
            {
                IICConnection_Destroy(iic);
            }
        }



        public Int32 ReadValue(UInt32 addr)
        {
            byte[] data = this.Read(addr, 4);
            if (bigEndian)
            {
                return (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
            }
            else
            {
                return (data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0];
            }
        }
        public void WriteValue(UInt32 addr, Int32 value)
        {
            byte[] data = new byte[4];
            if (bigEndian)
            {
                data[0] = (byte)((value >> 24) & 0xff);
                data[1] = (byte)((value >> 16) & 0xff);
                data[2] = (byte)((value >> 8) & 0xff);
                data[3] = (byte)((value) & 0xff);
            }
            else
            {
                data[3] = (byte)((value >> 24) & 0xff);
                data[2] = (byte)((value >> 16) & 0xff);
                data[1] = (byte)((value >> 8) & 0xff);
                data[0] = (byte)((value) & 0xff);
            }

            this.Write(addr, data, 4);
        }

        public byte[] Read(UInt32 addr, Int32 length)
		{
            byte[] data = new byte[length];
            Int32 res = IICConnection_Read(iic, addr, length, ref data[0]);
            if (res != length)
            {
                throw new Exception("CH341IIC_Read +0x"+ String.Format("{0:x8}", addr) + "+failed...res="+ res);
            }
            return data;
        }

        public void Write(UInt32 addr, byte[] data, int length)
        {
            Int32 res = IICConnection_Write(iic, addr, length, ref data[0]);
            if (res != length)
            {
                throw new Exception("CH341IIC_Write failed...");
            }
        }

        [DllImport("IICConnection.dll", EntryPoint = "IICConnection_Init", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 IICConnection_Init();

        [DllImport("IICConnection.dll", EntryPoint = "IICConnection_Deinit", CallingConvention = CallingConvention.Cdecl)]
        private static extern void IICConnection_Deinit();

        [DllImport("IICConnection.dll", EntryPoint = "IICConnection_Create", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IICConnection_Create(Int32 endian, Int32 addrLen, byte i2c_addr);

        [DllImport("IICConnection.dll", EntryPoint = "IICConnection_Destroy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void IICConnection_Destroy(IntPtr iic);

        [DllImport("IICConnection.dll", EntryPoint = "IICConnection_Read", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 IICConnection_Read(IntPtr iic, UInt32 addr, Int32 length, ref byte data);

        [DllImport("IICConnection.dll", EntryPoint = "IICConnection_Write", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 IICConnection_Write(IntPtr iic, UInt32 addr, Int32 length, ref byte data);
    }
}
