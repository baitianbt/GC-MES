using System;
using System.IO;
using System.Xml.Serialization;

namespace GC_MES.Upgrade
{
    [Serializable]
    public class UpdateInfo
    {
        public Version Version { get; set; }
        public string DownloadUrl { get; set; }
        public string FileName { get; set; }
        public string ReleaseNotes { get; set; }
        public bool IsForceUpdate { get; set; }
        public DateTime ReleaseDate { get; set; }

        public static UpdateInfo FromFile(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UpdateInfo));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    return (UpdateInfo)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"无法解析更新信息文件: {ex.Message}");
            }
        }

        public void SaveToFile(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UpdateInfo));
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, this);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"无法保存更新信息文件: {ex.Message}");
            }
        }
    }
} 