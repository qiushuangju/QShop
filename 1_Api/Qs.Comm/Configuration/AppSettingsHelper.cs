using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Qs.Comm
{
    /// <summary>
    /// ��ȡAppsettings������Ϣ
    /// </summary>
    public class AppSettingsHelper
    {
        static IConfiguration Configuration { get; set; }

        /// <summary>
        ///   ��ȡAppsettings������Ϣ
        /// </summary>
        /// <param name="contentPath"></param>
        public AppSettingsHelper(string contentPath)
        {
            string Path = "appsettings.json";
            Configuration = new ConfigurationBuilder().SetBasePath(contentPath).Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true }).Build();
        }

        /// <summary>
        /// ��װҪ�������ַ�
        /// AppSettingsHelper.GetContent(new string[] { "JwtConfig", "SecretKey" });
        /// </summary>
        /// <param name="sections">�ڵ�����</param>
        /// <returns></returns>
        public static string GetContent(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration.GetSection(string.Join(":", sections)).Value;
                }
            }
            catch (Exception) { }
            return "";
        }

    }
}