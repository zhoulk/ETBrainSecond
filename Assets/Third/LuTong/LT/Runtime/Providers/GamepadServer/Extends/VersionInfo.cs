using System.Collections.Generic;
using System.ComponentModel;

namespace LT
{
    public class FileVersionInfo
    {
        public string File;
        public string MD5;
        public long Size;
    }

    public class VersionConfig : ISupportInitialize
    {
        public int Version;

        public long TotalSize;

        public Dictionary<string, FileVersionInfo> FileInfoDict = new Dictionary<string, FileVersionInfo>();

        public void BeginInit() { }

        public void EndInit()
        {
            foreach (FileVersionInfo fileVersionInfo in this.FileInfoDict.Values)
            {
                this.TotalSize += fileVersionInfo.Size;
            }
        }
    }
}