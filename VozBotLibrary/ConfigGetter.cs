using System.Configuration;

namespace VozBotLibrary
{
    /// <summary>
    /// Get API and data files path
    /// </summary>
    public class ConfigGetter
    {
        public static string GetAPI()
        {
            return ConfigurationManager.ConnectionStrings["HTTPAPI"].ConnectionString;
        }

        public static string GetWelcomePath()
        {
            return ConfigurationManager.ConnectionStrings["Welcome"].ConnectionString;
        }

        public static string GetNotePath()
        {
            return ConfigurationManager.ConnectionStrings["Note"].ConnectionString;
        }

        public static string GetFilterPath()
        {
            return ConfigurationManager.ConnectionStrings["Filter"].ConnectionString;
        }

        public static string GetBioPath()
        {
            return ConfigurationManager.ConnectionStrings["Bio"].ConnectionString;
        }

        public static string GetAdminPath()
        {
            return ConfigurationManager.ConnectionStrings["Admin"].ConnectionString;
        }
    }
}
