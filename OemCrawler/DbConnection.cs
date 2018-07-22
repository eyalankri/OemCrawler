namespace OemCrawler
{
    public static class DbConn
    {
        static DbConn()
        {

            Conn = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public static string Conn { get; set; }
        
    }




}
