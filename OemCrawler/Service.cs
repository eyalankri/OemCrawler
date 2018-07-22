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
        private readonly string _jsonsFolder = @"D:\Projects\Nop-Oem-Parts\OemCrawler\OemCrawler\json\";
        private readonly string _SiteImagesFolder = @"D:/Projects/Nop-Oem-Parts/Site/Presentation/Nop.Web/wwwroot/images/ScrappedDiagrams/";
        readonly string _diagramImageSaveAt;
        readonly string _diagramImageWmSaveAt;       
        private readonly string _categoryInsertApiUrl = "http://localhost:15536/api/categories/";
        private readonly string _productInsertApiUrl = "http://localhost:15536/api/products/";
        private readonly string _productCategoryMappingInsertApiUrl = "http://localhost:15536/api/product_category_mappings/";
     


        public Services(string token)
        {
            _token = token;
            _diagramImageSaveAt = _SiteImagesFolder + "image.jpeg";
            _diagramImageWmSaveAt = _SiteImagesFolder + "image_wm.jpeg";
        }

        public string PostToApi(string apiUrl, string jsonString, string bearerToken)
        {

            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add("Authorization", "Bearer " + bearerToken);
                return client.UploadString(new Uri(apiUrl), "POST", jsonString);
            }
        }


        public Category IsCategoryExists(string categoryName, int parentCategoryId)
        {
            var category = new Category
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


        public Product IsProductExists(string sku)
        {
            var product = new Product
            {
                Id = -1,
                IsProductExists = false,
            };

            using (var conn = new SqlConnection(DbConn.Conn))
            {
                using (var cmd = new SqlCommand("[OemCrawler_Select_Product_Sku]", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Sku", sku);


                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            product.IsProductExists = true;
                            product.Id = (int)reader["Id"];
                        }
                        reader.Close();
                    }
                }
            }
            return product;
        }


        public bool IsProductCategoryMappingExists(int productId, int categoryId)
        {

            var isMappingExists = false;

            using (var conn = new SqlConnection(DbConn.Conn))
            {
                using (var cmd = new SqlCommand("[OemCrawler_Select_Product_Category_Mapping]", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);


                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            isMappingExists = true;
                        }
                        reader.Close();
                    }
                }
            }
            return isMappingExists;
        }

        public Category CategoryInsert(string name, int parentId, string michlolImageUrl)
        {


            var category = new Category();

            // download the images into local storage
            if (michlolImageUrl != "")
            {
                using (var client = new WebClient { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        client.DownloadFile(michlolImageUrl, michlolImageUrl);
                    }
                    catch (WebException)
                    {
                        System.Threading.Thread.Sleep(3000);
                        client.DownloadFile(michlolImageUrl, _diagramImageSaveAt);
                    }
                }
                //replace watermark
                AddWaterMark();
            }

            var jsonFileName = (michlolImageUrl == "") ? "CategoryInsertNoImage.txt" : "CategoryInsert.txt";
            var json = File.ReadAllText(_jsonsFolder + jsonFileName);

            name = name.Replace("\"", "'");
            json = json.Replace("{category-name}", name);
            json = json.Replace("{parent-category-id}", parentId.ToString()); // parent
            // json = json.Replace("{category-image-url}", _diagramImageWmSaveAt); already in the json file

            var stringJson = PostToApi(_categoryInsertApiUrl, json, _token);
            var jObjext = JObject.Parse(stringJson);

            category.Id = (int)jObjext["categories"][0]["id"];
            category.ParentCategoryId = (int)jObjext["categories"][0]["parent_category_id"];

            return category;
        }



        public int ProductInsert(string name, string sku, double price, string idInDiagram)
        {
            Console.WriteLine(@"Insert product: " + name);
            var jsonFileName = "ProductInsert.txt";
            var json = File.ReadAllText(_jsonsFolder + jsonFileName);

            name = name.Replace("\"", "'");
            json = json.Replace("{part-name}", name.Replace("\\",""));
            json = json.Replace("{sku}", sku);
            json = json.Replace("{id-in-diagram}", idInDiagram);
            json = json.Replace("{price}", price.ToString(CultureInfo.InvariantCulture));

            var stringJson = PostToApi(_productInsertApiUrl, json, _token);
            var jObjext = JObject.Parse(stringJson);

            var produtId = (string)jObjext["products"][0]["id"];

            return int.Parse(produtId);
        }

        public void ProductCategoryMappingInsert(int productId, int categoryId)
        {
            Console.WriteLine(@"Insert mapping: " + categoryId + @"," + productId);

            var jsonFileName = "ProductCategoryMappingInsert.txt";
            var json = File.ReadAllText(_jsonsFolder + jsonFileName);

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

            Bitmap partImg;
            try
            {
                partImg = (Bitmap)Image.FromFile(_diagramImageSaveAt);
            }
            catch (Exception e)
            {
                partImg = (Bitmap)Image.FromFile(_diagramImageSaveAt);
                Console.WriteLine(e);

            }



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

            partImg.Save(_diagramImageWmSaveAt);

            Console.WriteLine(@"Image saved in folder");
        }



 




    }
}
