using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace OemCrawler
{
    public class Services
    {
        private readonly string _token;
        private readonly string _imageUrl = @"D:\Projects\OemCrawler\OemCrawler\Image\image.jpeg";
        private readonly string _imageWmUrl = @"D:\Projects\OemCrawler\OemCrawler\Image\image_wm.jpeg";


        public Services(string token)
        {
            _token = token;
        }

        private string _categoryInsertApiUrl = "http://localhost:15536/api/categories/";
        private string _productInsertApiUrl = "http://localhost:15536/api/products/";
        private string _productCategoryMappingInsertApiUrl = "http://localhost:15536/api/product_category_mappings/";



        public string PostToApi(string apiUrl, string jsonString, string bearerToken)
        {

            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add("Authorization", "Bearer " + bearerToken);
                return client.UploadString(new Uri(apiUrl), "POST", jsonString);
            }
        }






        public Form1.Category IsCategoryExists(string categoryName, int parentCategoryId)
        {


            var category = new Form1.Category
            {
                ParentCategoryId = -1,
                Id = -1,
                IsCategoryExists = false,
            };

            using (var conn = new SqlConnection(DbConn.Conn))
            {
                using (var cmd = new SqlCommand("[OemCrawler_Select_Category_Name-ParentId]", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    cmd.Parameters.AddWithValue("@ParentCategoryId", parentCategoryId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            category.IsCategoryExists = true;
                            category.Id = (int)reader["Id"];
                            category.ParentCategoryId = (int)reader["ParentCategoryId"];
                        }
                        reader.Close();
                    }
                }
            }
            return category;
        }

        public int CategoryInsert(string name, int parentId, string imageUrl)
        {
            // download the images into local storage
            if (imageUrl != "")
            {
                using (var client = new WebClient { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        client.DownloadFile(imageUrl, _imageUrl);
                    }
                    catch (WebException)
                    {
                        System.Threading.Thread.Sleep(3000);
                        client.DownloadFile(imageUrl, _imageUrl);
                    }
                }
                //replace watermark
                AddWaterMark();
            }
            


            var jsonFileName = (imageUrl == "") ? "CategoryCreateNoImage.json" : "CategoryCreate.json";
            var json = File.ReadAllText(@"D:\Projects\OemCrawler\OemCrawler\json\" + jsonFileName);

            json = json.Replace("{category-name}", name);
            json = json.Replace("{parent-category-id}", parentId.ToString()); // parent
            json = json.Replace("{category-image}", _imageWmUrl);

            var stringJson = PostToApi(_categoryInsertApiUrl, json, _token);
            var jObjext = JObject.Parse(stringJson);

            var newCategoryId = (string)jObjext["categories"][0]["id"];

            return int.Parse(newCategoryId);
        }



        public int ProductInsert(string name, string sku, double price, int idInDiagram)
        {

            var jsonFileName = "CategoryCreate.json";
            var json = File.ReadAllText(@"D:\Projects\OemCrawler\OemCrawler\json\" + jsonFileName);

            json = json.Replace("{part-name}", idInDiagram + "." + name);
            json = json.Replace("{sku}", sku);
            json = json.Replace("{price}", price.ToString(CultureInfo.InvariantCulture));

            var stringJson = PostToApi(_productInsertApiUrl, json, _token);
            var jObjext = JObject.Parse(stringJson);

            var produtId = (string)jObjext["products"][0]["id"];

            return int.Parse(produtId);
        }

        public void ProductCategoryMapping(int productId, int categoryId)
        {
            var jsonFileName = "ProductCategoryMapping.json";
            var json = File.ReadAllText(@"D:\Projects\OemCrawler\OemCrawler\json\" + jsonFileName);

            json = json.Replace("{product-id}", productId.ToString());
            json = json.Replace("{category-id}", categoryId.ToString());


            PostToApi(_productCategoryMappingInsertApiUrl, json, _token);
        }




        private void AddWaterMark()
        {

            var font = new Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel);
            var color = Color.FromArgb(50, 0, 0, 0); //Adds a black watermark with a low alpha value (almost transparent).
            var brushForText = new SolidBrush(color);
            var brushForBg = new SolidBrush(Color.FromArgb(245, 255, 255, 255)); // first one is opacity

            var partImg = (Bitmap)Image.FromFile(_imageUrl);


            var bgWidth = 250;
            var bgHeight = 50;

            var x = (partImg.Width / 2) - (bgWidth / 2);
            var y = (partImg.Height / 2) - (bgHeight / 2);


            var textPoint1 = new Point(x + 25, y);   // 25 is the text padding from bg    
            var textPoint2 = new Point(x, partImg.Height - bgHeight);


            Graphics graphics;

            try
            {
                graphics = Graphics.FromImage(partImg);
            }
            catch
            {
                var temp = partImg;
                partImg = new Bitmap(partImg.Width, partImg.Height);
                graphics = Graphics.FromImage(partImg);
                graphics.DrawImage(temp, new Rectangle(0, 0, partImg.Width, partImg.Height), 0, 0, partImg.Width, partImg.Height, GraphicsUnit.Pixel);

                var bg1 = new Rectangle(x, y, bgWidth, bgHeight); //center of page
                var bg2 = new Rectangle(x, (partImg.Height - bgHeight), bgWidth, bgHeight); //bottom of page


                graphics.FillRectangle(brushForBg, bg1);
                graphics.FillRectangle(brushForBg, bg2);


                temp.Dispose();
            }

            graphics.DrawString("OferAvnir", font, brushForText, textPoint1);
            graphics.DrawString("OferAvnir", font, brushForText, textPoint2);

            graphics.Dispose();

            partImg.Save(_imageWmUrl);
        }

    }
}
