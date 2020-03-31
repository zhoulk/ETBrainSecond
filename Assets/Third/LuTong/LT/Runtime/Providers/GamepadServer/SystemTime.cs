using System;

namespace LT
{
    /// <summary>
    /// 时间
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// Utc时间
        /// </summary>
        private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        /// <summary>
        /// 客户端时间 ms
        /// </summary>
        /// <returns></returns>
        public static long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000;
        }

        /// <summary>
        /// 获取低16位时间戳，实际需要32位表示时间戳，但因初期设计漏洞，只能临时使用原先包头无用的16位空间。
        /// </summary>
        /// <returns></returns>
        public static ushort LowClientNow()
        {
            return (ushort)(ClientNow());
        }
    }
}