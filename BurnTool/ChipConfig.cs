using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BurnTool
{
    public class ChipConfig
    {
        public enum EndianType
        {
            None,
            Big,
            Little,
        };
        private string name;
        public string Name { get { return name; } }
        private EndianType endian = EndianType.None;
        public EndianType Endian { get { return endian; } }

        private UInt32 i2c_addr = 0xffffff00;
        public byte I2CAddr { get { return (byte)(i2c_addr & 0xff); } }
        private UInt32 haltAddr = 0;
        public UInt32 HaltAddr { get { return haltAddr; } }
        
        private UInt32 unhaltAddr = 0;
        public UInt32 UnhaltAddr { get { return unhaltAddr; } }

        private UInt32 dbgAddr = 0;
        public UInt32 DbgAddr { get { return dbgAddr; } }

        private UInt32 stepAddr = 0;
        public UInt32 StepAddr { get { return stepAddr; } }

        private UInt32 contAddr = 0;
        public UInt32 ContAddr { get { return contAddr; } }

        private bool toFlash = false;
        public bool ToFlash { get { return toFlash; } }

        private UInt32 sizeAddr = 0;
        public UInt32 SizeAddr { get { return sizeAddr; } }

        private ChipConfig(string _name)
        {
            this.name = _name;
        }

        public static ChipConfig loadConfig(FileInfo finfo)
        {
            if (!finfo.Exists) throw new Exception(finfo.FullName + " not exists!");
            string[] lines = File.ReadAllText(finfo.FullName).Split('\n');
            ChipConfig cfg = new ChipConfig(finfo.Name.Substring(0, finfo.Name.Length-4));
            foreach(string text in lines)
            {
                string[] parts = text.Split('=');
                if (parts.Length != 2)
                {
                    throw new Exception("Bad text:" + text);
                }
                switch(parts[0].Trim())
                {
                    case "i2c_addr":
                        cfg.i2c_addr = Convert.ToUInt32(parts[1].Trim(), 16);
                        if (cfg.i2c_addr > 0xff)
						{
                            throw new Exception("Bad i2c_addr:" + parts[1]);
                        }
                        break;
                    case "endian":
                        if (parts[1].Trim() == "big")
                        {
                            cfg.endian = EndianType.Big;
                        }
                        else if (parts[1].Trim() == "little")
                        {
                            cfg.endian = EndianType.Little;
                        }
                        else
                        {
                            throw new Exception("Bad endian:" + parts[2]);
                        }
                        break;

                    case "halt":
                        cfg.haltAddr = Convert.ToUInt32(parts[1].Trim(), 16);
                        break;

                    case "unhalt":
                        cfg.unhaltAddr = Convert.ToUInt32(parts[1].Trim(), 16);
                        break;

                    case "dbg":
                        cfg.dbgAddr = Convert.ToUInt32(parts[1].Trim(), 16);
                        break;

                    case "step":
                        cfg.stepAddr = Convert.ToUInt32(parts[1].Trim(), 16);
                        break;

                    case "continue":
                        cfg.contAddr = Convert.ToUInt32(parts[1].Trim(), 16);
                        break;

                    case "size":
                        cfg.sizeAddr = Convert.ToUInt32(parts[1].Trim(), 16);
                        break;
                }
            } // end of foreach

            if (cfg.endian == EndianType.None)
            {
                throw new Exception("Endian not found in file " + finfo.FullName);
            }
            if (cfg.i2c_addr > 0xff)
			{
                throw new Exception("Bad i2c_addr in file " + finfo.FullName);
            }
            if (cfg.haltAddr <= 0)
            {
                throw new Exception("Bad halt addr in file " + finfo.FullName);
            }
            if (cfg.unhaltAddr <= 0)
            {
                throw new Exception("Bad unhalt addr in file " + finfo.FullName);
            }
            if (cfg.dbgAddr <= 0)
            {
                throw new Exception("Bad dbg addr in file " + finfo.FullName);
            }
            if (cfg.stepAddr <= 0)
            {
                throw new Exception("Bad step addr in file " + finfo.FullName);
            }
            if (cfg.contAddr <= 0)
            {
                throw new Exception("Bad continue addr in file " + finfo.FullName);
            }
            if (cfg.sizeAddr > 0)
            {
                cfg.toFlash = true;
            }

            return cfg;
        }
    }
}
