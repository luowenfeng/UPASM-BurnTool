using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BurnTool
{
    public class UIConfig
    {
        public class UIJson
        {
            public string chipType { get; set; }
            public string partitionFile { get; set; }
        };
        public UIConfig()
        {
            uiJson = new UIJson();
            savePath = "uiconfig.json";
        }
        public UIJson uiJson;
        public string savePath;
        public string load() {
            UIJson json;
            try
            {                
                FileStream saveFile = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                byte[] buf = new byte[saveFile.Length];
                saveFile.Read(buf, 0, (int)saveFile.Length);
                saveFile.Close();
                string jstr = System.Text.Encoding.Default.GetString(buf);
                json = JsonConvert.DeserializeObject<UIJson>(jstr);
                if (null != json)
                {
                    uiJson = json;
                }
            }
            catch (Exception e)
            {                
                return "ui配置文件打开失败，" + e.Message+"\r\n";
            }            
            return "ok";
        }
        public int save() {
            string jstr = JsonConvert.SerializeObject(uiJson, Formatting.Indented);
            byte[] buf = System.Text.Encoding.Default.GetBytes(jstr);
            FileStream saveFile = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write);
            saveFile.Write(buf, 0, buf.Length);
            saveFile.Close();
            return 0;
        }
    };
}
