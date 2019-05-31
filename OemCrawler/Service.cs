using System;
using System.Collections.Generic;
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
        private readonly string _jsonsFolder = @"D:\www\Oem\wwwroot\images\ScrappedDiagrams\Json\";
        private readonly string _SiteImagesFolder = @"D:\www\Oem\wwwroot\images\ScrappedDiagrams\";
        readonly string _diagramImageSaveAt;
        readonly string _diagramImageWmSaveAt;
        private readonly string _categoryInsertApiUrl = "http://localhost/api/categories/";
        private readonly string _productInsertApiUrl = "http://localhost/api/products/";
        private readonly string _productCategoryMappingInsertApiUrl = "http://localhost/api/product_category_mappings/";
        private readonly string _imageSrcWm;
        private readonly string _imageSrcColorReplaced;
        private readonly string finalImageName = "image_final.jpeg";


        public Services(string token)
        {
            _token = token;
            _diagramImageSaveAt = _SiteImagesFolder + "image.jpeg";
            _diagramImageWmSaveAt = _SiteImagesFolder + "image_wm.jpeg";
            _imageSrcColorReplaced = _SiteImagesFolder + "image_color.jpeg";
            _imageSrcWm = _SiteImagesFolder + finalImageName;
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

            if (File.Exists(_diagramImageSaveAt))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(_diagramImageSaveAt);
            }

            var category = new Category();

            var addWatermark = true;
            // download the images into local storage
            if (michlolImageUrl != "")
            {
                using (var client = new WebClient { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        client.DownloadFile(michlolImageUrl, _diagramImageSaveAt);
                    }
                    catch (WebException)
                    {
                        addWatermark = false;
                        // download: No Image avilable
                        michlolImageUrl = "http://localhost/images/ScrappedDiagrams/no-image-avilable.png";
                        System.Threading.Thread.Sleep(3000);
                        client.DownloadFile(michlolImageUrl, _diagramImageSaveAt);
                    }
                }
                //replace watermark
                SaveDiagramInFolder();

                if (addWatermark)
                {
                    AddWaterMark();
                }
              
            }

            var jsonFileName = (michlolImageUrl == "") ? "CategoryInsertNoImage.txt" : "CategoryInsert.txt";
            var json = File.ReadAllText(_jsonsFolder + jsonFileName);

            name = name.Replace("\"", "'");
            json = json.Replace("{category-name}", name);
            json = json.Replace("{parent-category-id}", parentId.ToString()); // parent
            if (michlolImageUrl != null)
            {
                json = json.Replace("{image-src}", "http://localhost/images/ScrappedDiagrams/" + finalImageName);
            }


            var stringJson = PostToApi(_categoryInsertApiUrl, json, _token);

            Console.WriteLine(@"insert category: " + name);
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

            if (name=="")
            {
                name = sku;
            }

            name = name.Replace("\"", "'");
            json = json.Replace("{part-name}", name.Replace("\\", ""));
            json = json.Replace("{sku}", sku);
            json = json.Replace("{id-in-diagram}", idInDiagram);
            json = json.Replace("{price}", price.ToString(CultureInfo.InvariantCulture));

            var stringJson = PostToApi(_productInsertApiUrl, json, _token);

            Console.WriteLine(@"insert product: " + name);
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


        public string TestApi(string apiUrl)
        {

            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add("Authorization", "Bearer " + _token);
                try
                {
                    return client.DownloadString(new Uri(apiUrl));
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }


        public void SaveDiagramInFolder()
        {
            Bitmap img;
            try
            {
                img = (Bitmap)Image.FromFile(_diagramImageSaveAt);
            }
            catch (Exception e)
            {
                img = (Bitmap)Image.FromFile(_diagramImageSaveAt);
                Console.WriteLine(e);
            }

            var colorsToReplace = new List<string>();

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    var color1 = img.GetPixel(x, y).Name;

                    if (color1 == "ffffffff")
                    {
                        continue;
                    }
                    
                    


                    if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#E4E4E4")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#E7E7E7")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fffefefe")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fffefefe")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fffdfdfd")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fffcfcfc")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fffafafa")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff8f8f8")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff6f6f6")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff5f5f5")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff2f2f2")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff0f0f0")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffefefef")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffeeeeee")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffececec")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffebebeb")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffeaeaea")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe8e8e8")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe7e7e7")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe6e6e6")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe9e9e9")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff3f3f3")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff4f4f4")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff9f9f9")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fffbfbfb")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe4e4e4")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe5e5e5")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe3e3e3")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe2e2e2")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffe1e1e1")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff1f1f1")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#ffededed")) { img.SetPixel(x, y, Color.White); }
                    else if (img.GetPixel(x, y) == ColorTranslator.FromHtml("#fff7f7f7")) { img.SetPixel(x, y, Color.White); }



                }

            }



            img.Save(_imageSrcColorReplaced);
            img.Dispose();

            Console.WriteLine(@"Image  saved in folder:" + _imageSrcColorReplaced);
        }

        private void AddWaterMark()
        {
           // if you need OPACITY use color like this:
            var color = Color.FromArgb(50, 0, 0, 0); //Adds a black watermark with a low alpha value (almost transparent).


            var font = new Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel);
            
            var brushForText = new SolidBrush(color);
            //var brushForBg = new SolidBrush(Color.FromArgb(245, 255, 255, 255)); // first one is opacity
            var brushForBg = new SolidBrush(Color.White);

            var bitImg = new Bitmap(Image.FromFile(_imageSrcColorReplaced));

            var bitImgHight = bitImg.Height;
            var bitImgWidth = bitImg.Width;

            var bgWidthTop = 0;
            var bgWidthBottom = 600;
            var bgHeight = 50;

            var x = (bitImgWidth / 2) - (bgWidthBottom / 2);
            var y = (bitImg.Height / 2) - (bgHeight / 2);


            var textPoint1 = new Point(x + 25, y);   // 25 is the text padding from bg    
            var textPoint2 = new Point(x, bitImgHight - bgHeight);


            var temp = bitImg;
            bitImg = new Bitmap(bitImgWidth, bitImgHight);
            var graphics = Graphics.FromImage(bitImg);
            graphics.DrawImage(temp, new Rectangle(0, 0, bitImgWidth, bitImgHight), 0, 0, bitImgWidth, bitImgHight, GraphicsUnit.Pixel);

            var bg1 = new Rectangle(x, y, bgWidthTop, bgHeight); //center of page
            var bg2 = new Rectangle(x, (bitImg.Height - bgHeight), bgWidthBottom, bgHeight); //bottom of page

            graphics.FillRectangle(brushForBg, bg1);
            graphics.FillRectangle(brushForBg, bg2);

            temp.Dispose();


            graphics.DrawString("OferAvnir", font, brushForText, textPoint1);
            graphics.DrawString("OferAvnir", font, brushForText, textPoint2);


            graphics.Dispose();
            bitImg.Save(_imageSrcWm);
            bitImg.Dispose();

            Console.WriteLine(@"Image add watermark saved in folder:" + _imageSrcWm);


        }








    }
}
