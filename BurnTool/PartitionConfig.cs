using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BurnTool
{
    public class PartitionConfig
    {
        class DataPartition
        {
            public string name;
            public int addr;
            public int size;
            public string path;
        };
        //json
        public int flashSize;
        public int appSize;
        public string appPath;
        ArrayList dataPartitions;

        //attr
        public byte[] wholebinBuf;
        public string wholebinFname;

        public PartitionConfig()
        {
            dataPartitions = new ArrayList();
            wholebinFname = "flash_wholebin.bin";
        }
        public int loadFile(string fpath)
        {
            try
            {
                using (System.IO.StreamReader file = System.IO.File.OpenText(fpath))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject rootObj = (JObject)JToken.ReadFrom(reader);

                        flashSize = SizeString2int(rootObj["flashSize"].ToString());
                        appSize= SizeString2int(rootObj["appSize"].ToString());
                        appPath = rootObj["appPath"].ToString();//

                        JArray dataPartitionsObj = (JArray)rootObj["dataPartitions"];
                        dataPartitions.Clear();
                        for (int i = 0; i < dataPartitionsObj.Count; i++)
                        {
                            JObject dataPartitionObj = (JObject)dataPartitionsObj[i];
                            DataPartition dataPartition = new DataPartition();
                            dataPartition.name = dataPartitionObj["name"].ToString();
                            dataPartition.addr = Convert.ToInt32(dataPartitionObj["addr"].ToString(),16);
                            dataPartition.size = SizeString2int(dataPartitionObj["size"].ToString());
                            dataPartition.path = dataPartitionObj["path"].ToString();
                            dataPartitions.Add(dataPartition);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }
        public string checkContent() {
            //totalSize>flashSize?
            int totalSize = appSize;
            for (int i = 0; i < dataPartitions.Count; i++) {
                DataPartition dataPartition = (DataPartition)dataPartitions[i];
                totalSize += dataPartition.size;
            }
            if (totalSize > flashSize) {
                return "分区总大小超出flash大小";
            }
            //address没有按256对齐？
            for (int i = 0; i < dataPartitions.Count; i++)
            {
                DataPartition dataPartition = (DataPartition)dataPartitions[i];
                if (dataPartition.addr % 256 > 0) {
                    return dataPartition.name+ "分区地址没有按256对齐";
                }
            }
            //文件不存在？
            if (false==File.Exists(appPath)) {
                return "appPath:" + appPath + "不存在";
            }
            for (int i = 0; i < dataPartitions.Count; i++)
            {
                DataPartition dataPartition = (DataPartition)dataPartitions[i];
                if (false == File.Exists(dataPartition.path))
                {
                    return dataPartition.name+" path:" + dataPartition.path + "不存在";
                }
            }
            return "ok";
        }
        public string wholebinCreate() {            
            wholebinBuf = new byte[flashSize];
            for (int i=0; i<wholebinBuf.Length; i++) {
                wholebinBuf[i] = 0xff;
            }
            

            try
            {
                FileStream appFile= new FileStream(appPath, FileMode.Open, FileAccess.Read);
                appFile.Read(wholebinBuf,0, (int)appFile.Length);
                appFile.Close();
                for (int i = 0; i < dataPartitions.Count; i++) {
                    DataPartition dataPartition = (DataPartition)dataPartitions[i];
                    FileStream dataFile = new FileStream(dataPartition.path, FileMode.Open, FileAccess.Read);
                    dataFile.Read(wholebinBuf, dataPartition.addr, (int)dataFile.Length);
                    dataFile.Close();
                }

                FileStream wholebinFile = new FileStream(wholebinFname, FileMode.Create, FileAccess.Write);
                wholebinFile.Write(wholebinBuf,0, flashSize);
                wholebinFile.Close();
            }
            catch (Exception e) {
                return "wholebin生成失败"+e.Message;
            }

            return "ok";
        }

        private int SizeString2int(string str)
        {
            int num = -1;
            char lastch = str[str.Length - 1];
            str = str.Remove(str.Length - 1, 1);
            try
            {
                num = Convert.ToInt32(str, 10);
                if ('K' == lastch)
                {
                    num *= 1024;
                }
                else if ('M' == lastch)
                {
                    num *= 1024 * 1024;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return num;
        }
    }
}
