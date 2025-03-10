using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace MyDMS.Application
{
    public static class DMSConfig
    {
        public static byte[] EncryptionKey;
        public static byte[] EncryptionIV;
        public static string Profile;
        public static void Initialize(IConfiguration configuration)
        {
            EncryptionKey = Encoding.UTF8.GetBytes(configuration["EncryptionKey"]);
            EncryptionIV = Encoding.UTF8.GetBytes(configuration["EncryptionIV"]);
            Profile = Encoding.UTF8.GetBytes(configuration.GetSection("OCI")["Profile"]).ToString();
        }
    }
}
