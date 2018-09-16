
using System.IO;

namespace localserver
{
    public class StaticContentOptions
    {
        public string FilePath { get; set; }
        public string UrlRoutePath { get; set; }

        public string LocalFilePath => Path.Combine(Directory.GetCurrentDirectory(), FilePath);
    }
}