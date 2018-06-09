using System.IO;
using System.Web;

namespace OemCrawler
{

    public static class FileReader
    {
       

        public static string ReadFile(string filePath)
        {

            //Open a file for reading
            var filename = HttpContext.Current.Server.MapPath(filePath);
            var objStreamReader = File.OpenText(filename);

            //Now, read the entire file into a string
            var content = objStreamReader.ReadToEnd();
            objStreamReader.Close();

            return content;

        }

    }
}