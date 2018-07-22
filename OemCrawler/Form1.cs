using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace OemCrawler
{


    public partial class Form1 : Form
    {
        private readonly Models _model;
        private string _htmlCode = "";
        private HtmlAgilityPack.HtmlDocument _htmlAgilityDoc;
        public int CategoryId;
        private int _brandCategoryId;
        private string _lastStoppedMichlolName;
        private string[] _arrUrls;


        public Form1()
        {
            InitializeComponent();
            _model = new Models();
            _htmlAgilityDoc = new HtmlAgilityPack.HtmlDocument();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            
            var comboSource = new Dictionary<int, string>
            {
                { 0, "-- בחר קטגוריה --" },
                { 1, "אופנועים -> סוזוקי" },
                //{ 1242, "אופנועים -> הוסקוורנה" },
                //{ 1024, "אופנועים -> הוסקוורנה" }
            };

            cmbCategory.DataSource = new BindingSource(comboSource, null);
            cmbCategory.DisplayMember = "Value";
            cmbCategory.ValueMember = "Key";
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            _lastStoppedMichlolName = txtLastStoppedMichlolName.Text.Trim().ToLower();

            if (txtUrl.TextLength < 10)
            {
                MessageBox.Show(@"יש להכניס URL");
                return;
            }

            if (!IsApiOnline())
            {
                MessageBox.Show(@"Api is not online!");
                return;
            }

            _brandCategoryId = int.Parse(cmbCategory.SelectedValue.ToString());

            if (_brandCategoryId == 0)
            {
                MessageBox.Show(@"יש לבחור קטגוריה");
                return;
            }

            if (txtToken.TextLength < 10)
            {
                MessageBox.Show(@"יש להכניס טוקן");
                return;
            }

            btnGo.Text = @"working...";


            _arrUrls = txtUrl.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            
            ThreadPool.QueueUserWorkItem(StartScrap);
            
        }

        
        private void StartScrap(object state)
        {
             
            foreach (var url in _arrUrls)
            {
                Console.Clear();
                _model.MichlolList.Clear();

                if (url=="")
                {
                    continue;;
                }
                
                using (var client = new WebClient {Encoding = Encoding.UTF8})
                {
                    try
                    {
                        _htmlCode = client.DownloadString(url.Trim());
                    }
                    catch (WebException)
                    {
                        Thread.Sleep(5000);
                        _htmlCode = client.DownloadString(url.Trim());
                    }

                    ScrapModelPage(); // scrap model page
                    GetMichlolNameAndUrl(); // from model page
                    ScrapMichlolPages(); // get the parts
                    InsertToDb();
                    Console.WriteLine(@"---------> Finish: " + url);

                }
            }

            foreach (var url in _arrUrls)
            {
                Console.WriteLine(@"---------> All Finished: " + url);
            }
        




    }


        private void ScrapModelPage()
        {

            //we need to navigate to the model & year test
            var nextPosLoc = _htmlCode.IndexOf("image logo", StringComparison.Ordinal);
            _htmlCode = _htmlCode.Remove(0, nextPosLoc);

            nextPosLoc = _htmlCode.IndexOf("</div>", StringComparison.Ordinal);
            _htmlCode = _htmlCode.Remove(0, nextPosLoc + "</div>".Length);

            var nextPosLocEnd = _htmlCode.IndexOf("OEM ", 0, StringComparison.Ordinal);

            var lengthOfString = nextPosLocEnd;
            var str = _htmlCode.Substring(0, lengthOfString); // "2017 Suzuki GSXR600  "
            str = str.Trim(); // "2017 Suzuki GSXR600"

            if (str.ToLower().Contains("all"))
            {
                str = str.Replace("All", "").Trim();
            }
            _model.Year = str.Substring(0, 4); // get the year. 4:year length
            Console.WriteLine(@"List Year: " + _model.Year);

            str = str.Replace(_model.Year, ""); // " Suzuki GSXR600"
            str = str.Trim();  // "Suzuki GSXR600"

            var arrStr = str.Split(' ');
            _model.Brand = arrStr[0].Trim(); //Suzuki
            Console.WriteLine(@"List Brand: " + _model.Brand);

            _model.Name = str.Replace(_model.Brand,"").Trim(); //GSXR600
            Console.WriteLine(@"List Model: " + _model.Name);

        }

        private void GetMichlolNameAndUrl()
        {            
            _htmlAgilityDoc.LoadHtml(_htmlCode);
            var oemCols = _htmlAgilityDoc.DocumentNode.SelectNodes("//div[contains(@class,'oem-inner-col')]");
          
            foreach (var col in oemCols)
            {
                var colLinks = col.SelectNodes("a");

                foreach (var a in colLinks)
                {
                    var michlol = new Michlol
                    {
                        PageUrl = a.Attributes["href"].Value,
                        Name = a.InnerText
                    };

                    Console.WriteLine(@"List michlol: " + michlol.Name);

                    // if _lastStoppedMichlolName has value, we need to add to list only from this string
                    bool flagFromNowAddToMichlolList = !(_lastStoppedMichlolName.Length > 0); 
                    // we will add the michlol only when we reach this string (to continiue from that position)
                    if (michlol.Name.Trim().ToLower() == _lastStoppedMichlolName)
                    {
                        flagFromNowAddToMichlolList = true;
                    }
                    if (flagFromNowAddToMichlolList)
                    {
                        // we dont need to find the last name
                        _model.MichlolList.Add(michlol);
                    }
                }
            }

            
        }

        private void AddMichlolParts(Michlol michlol, string htmlCode)
        {
            _htmlAgilityDoc.LoadHtml(htmlCode);
            var nodes = _htmlAgilityDoc.DocumentNode.SelectNodes("//li[contains(@id,'skuID')]");

            if (nodes == null)
            {
                return;
            }
            foreach (HtmlNode node in nodes)
            {
                var part = new Part();


                //part.Sku = p.Attributes["sku"].Value.Trim();

                var htmlTag = node.SelectSingleNode(".//span[@class='oem-sku']");
                part.Sku = htmlTag.InnerText.Trim();

                // load again the current node


                htmlTag = node.SelectSingleNode(".//span[@class='oem-count']");
                part.IdInDiagram = htmlTag.InnerText.Trim();

                htmlTag = node.SelectSingleNode(".//span[@class='oem-description']");
                part.Name = htmlTag.InnerText.Trim();
                part.Name = Regex.Replace(part.Name, @"\s+", " ");

                Console.WriteLine(@"List part: " + part.Name);

                htmlTag = node.SelectSingleNode(".//span[contains(@id,'price')]");

                part.Price = 0;
                if (htmlTag == null) continue;

                var strPrice = htmlTag.InnerText.Trim();
                if (double.TryParse(strPrice, out var price))
                {
                    part.Price = price;
                }
                michlol.PartList.Add(part);
            }

        }


        private void ScrapMichlolPages()
        {

            // loop מיכלולים
            foreach (var michlol in _model.MichlolList)
            {
                var htmlCode = "";
                using (var client = new WebClient { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        htmlCode = client.DownloadString(michlol.PageUrl);
                    }
                    catch (WebException)
                    {
                        Thread.Sleep(3000);
                        htmlCode = client.DownloadString(michlol.PageUrl);
                    }
                }
                
                // add the 
                AddDiagramImageUrl(michlol, htmlCode);
                AddMichlolParts(michlol, htmlCode);

            }



        }

        private void AddDiagramImageUrl(Michlol michlol, string htmlCode)
        {
            try
            {
                _htmlAgilityDoc = new HtmlAgilityPack.HtmlDocument();

                _htmlAgilityDoc.LoadHtml(htmlCode);

                var imageHref = "";
                var imageElem = (((_htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage7']") ??
                                   _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage6']")) ??
                                  _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage5']")) ??
                                 _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage4']")) ??
                                _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage3']");


                // still cannot find the diagram image
                if (imageElem == null)
                {
                    Thread.Sleep(10000);
                    using (var client = new WebClient())
                    {
                        htmlCode = client.DownloadString(michlol.PageUrl);                       
                    }
                    _htmlAgilityDoc.LoadHtml(htmlCode);
                    imageElem = (((_htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage7']") ??
                                       _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage6']")) ??
                                      _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage5']")) ??
                                     _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage4']")) ??
                                    _htmlAgilityDoc.DocumentNode.SelectSingleNode(".//img[@id='PartGroupImage3']");
                }

                imageHref = imageElem.Attributes["data-src"].Value;

                // incase one of the zoom=1-4
                imageHref = imageHref.Replace("zoom=1", "zoom=5"); // I need zoom=5 (bigger)
                imageHref = imageHref.Replace("zoom=2", "zoom=5");  
                imageHref = imageHref.Replace("zoom=3", "zoom=5");  
                imageHref = imageHref.Replace("zoom=4", "zoom=5");  
             


                michlol.ImageUrl = "https://www.motosport.com" + imageHref;
                Console.Write(@"Add michlol image: " + michlol.ImageUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }


        }


        private void InsertToDb()
        {

            var service = new Services(txtToken.Text);

            int prevCatId = _brandCategoryId;

            // model name:             
            var category = service.IsCategoryExists(_model.Name, prevCatId); // this func return checks if this cat exists and return it's parentId
            if (!category.IsCategoryExists)
            {
                // insert & get CatId
                category = service.CategoryInsert(_model.Name, prevCatId, ""); // the model takes the catId from combobox
            }

            //gsx=122
            prevCatId = category.Id;





            // model year           
            category = service.IsCategoryExists(_model.Year, prevCatId); // this func return checks if this cat exists and return it's parentId
            if (!category.IsCategoryExists)
            {
                category = service.CategoryInsert(_model.Year, prevCatId, "");
            }

            // year = 123
            var yearParentId = category.Id;


            var michlolLoopCount = 0;
            var michlolTotalCount = _model.MichlolList.Count;
            // each michlol
            foreach (var michlol in _model.MichlolList)
            {
                michlolLoopCount++;

                category = service.IsCategoryExists(michlol.Name, yearParentId);
                if (!category.IsCategoryExists)
                {                    
                    category = service.CategoryInsert(michlol.Name, yearParentId, michlol.ImageUrl);
                    Console.WriteLine(@"Insert category: " + michlol.Name + $@" ,({michlolLoopCount}/{michlolTotalCount})"  );
                }
                else
                {
                    Console.WriteLine(@"Category exists: " + michlol.Name + $@" ,({michlolLoopCount}/{michlolTotalCount})");
                }

                prevCatId = category.Id;

                // each part in michlol
                foreach (var part in michlol.PartList)
                {
                    var product = service.IsProductExists(part.Sku);

                    if (!product.IsProductExists)
                    {
                        product.Id = service.ProductInsert(part.Name, part.Sku, part.Price, part.IdInDiagram);
                        // it's a new product: map to category
                        service.ProductCategoryMappingInsert(product.Id, prevCatId);
                    }
                    else
                    {
                        // it's not a new product. check if it mapped this category
                        var isMappingExists = service.IsProductCategoryMappingExists(product.Id, prevCatId);
                        if (!isMappingExists)
                        {
                            service.ProductCategoryMappingInsert(product.Id, prevCatId);
                        }
                    }

                }
            }
        }







        public bool IsApiOnline()
        {
            bool isOnline;
            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                try
                {
                    var str = client.DownloadString(
                        "http://localhost:15536/images/ScrappedDiagrams/test-file-dont-delete.txt");

                    isOnline = str.Contains("eyalankri");
                }
                catch (Exception)
                {
                    isOnline = false;
                }
            }
            return isOnline;




        }

        private void btnRemoveSpaces_Click(object sender, EventArgs e)
        {
            // remove multiple white-spaces

            var list = new List<Part>();
            using (var conn = new SqlConnection(DbConn.Conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("[OemCrawler_Product_Select]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Part()
                            {
                                Id = (int)reader["Id"],
                                Name = Regex.Replace((string)reader["Name"], @"\s+", " "),
                            });

                        }
                    }
                }
                foreach (var p in list)
                {
                    var cmd = new SqlCommand("[OemCrawler_Product_Update-Name]", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Id", p.Id);
                    cmd.Parameters.AddWithValue("@Name", p.Name);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine(p.Name);
                }
                conn.Close();
            }

            Console.WriteLine(@"--------> Done!");
        }

        private void btnFixDiagramId_Click(object sender, EventArgs e)
        {
            var list = new List<Part>();
            using (var conn = new SqlConnection(DbConn.Conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("[OemCrawler_Product_Select]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new Part();

                            item.Id = (int)reader["Id"];
                           item.Name = Regex.Replace((string)reader["Name"], @"\s+", " ");

                            var name = item.Name;

                            var index = name.IndexOf('.');
                            var length = name.Length;

                            if (index < 0) { continue; }

                            name = name.Substring(index + 1, length - (index + 1)).Trim();
                            var diagramId = item.Name.Substring(0, index).Replace(".", string.Empty);

                            item.IdInDiagram = diagramId;

                            if (name.IndexOf('.',0) == 0 )
                            {
                                name = name.Substring(1, name.Length - 1);
                            }
                            item.Name = name;                            
                            list.Add(item);
                        }
                    }
                }
                foreach (var p in list)
                {
                    var cmd = new SqlCommand("[OemCrawler_Product_Fix-Name]", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Id", p.Id);
                    cmd.Parameters.AddWithValue("@Name", p.Name);
                    cmd.Parameters.AddWithValue("@IdInDiagram", p.IdInDiagram);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine(p.Name);
                }
                conn.Close();
            }

            Console.WriteLine(@"--------> Done!");
        }
    }




}
