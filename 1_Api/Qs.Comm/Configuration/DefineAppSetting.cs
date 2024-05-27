namespace Qs.Comm
{
    /// <summary>
    /// 配置文件格式化
    /// </summary>
    public static class DefineAppSetting
    {
        /// <summary>
        /// 记录日志级别(20:消息 30:警告 40:错误 50:严重错误)
        /// </summary>
        public static readonly int LogLevel = xConv.ToInt(AppSettingsHelper.GetContent("LogSetting", "LogLevel"));
    }
}
