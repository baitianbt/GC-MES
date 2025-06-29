using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace GC_MES.Upgrade
{
    [Serializable]
    public class UpdateInfo
    {
        // 基本版本信息
        public Version Version { get; set; }
        public string DownloadUrl { get; set; }
        public string FileName { get; set; }
        public string ReleaseNotes { get; set; }
        public bool IsForceUpdate { get; set; }
        public DateTime ReleaseDate { get; set; }
        
        // 新增属性
        public string MD5Checksum { get; set; }
        public long FileSize { get; set; }
        public string MinRequiredVersion { get; set; }
        public string[] UpdatedComponents { get; set; }
        public string[] Prerequisites { get; set; }

        // 从XML文件加载更新信息
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
                throw new Exception($"无法解析更新信息文件: {ex.Message}", ex);
            }
        }

        // 保存更新信息到XML文件
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
                throw new Exception($"无法保存更新信息文件: {ex.Message}", ex);
            }
        }

        // 校验文件MD5
        public bool VerifyFileChecksum(string filePath)
        {
            if (string.IsNullOrEmpty(MD5Checksum))
                return true; // 如果未提供校验码，则默认通过

            try
            {
                using (var md5 = MD5.Create())
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    return hashString.Equals(MD5Checksum, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"校验文件失败: {ex.Message}", ex);
            }
        }
        
        // 判断当前版本是否满足最低版本要求
        public bool CheckMinVersionRequirement(Version currentVersion)
        {
            if (string.IsNullOrEmpty(MinRequiredVersion))
                return true;
                
            Version minVersion = new Version(MinRequiredVersion);
            return currentVersion >= minVersion;
        }
    }
} 