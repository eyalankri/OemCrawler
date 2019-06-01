using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace OemCrawler
{


    public partial class Form1 : Form
    {
        private HtmlAgilityPack.HtmlDocument _htmlAgilityDoc;
        private readonly IWebDriver _driver = new FirefoxDriver();

        private readonly Models _model;
        private string _htmlCode = "";
        public int CategoryId;
        private int _brandCategoryId;
        private string _lastStoppedMichlolName;
        private string[] _arrUrls;
        private string[] _listColorsToReplace;


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
                { 15718, "פיאגו - קטנועים" },
                //{ 1242, "אופנועים -> הוסקוורנה" },
                //{ 1024, "אופנועים -> הוסקוורנה" }
            };

            cmbCategory.DataSource = new BindingSource(comboSource, null);
            cmbCategory.DisplayMember = "Value";
            cmbCategory.ValueMember = "Key";
        }

        private void btnGo_Click(object sender, EventArgs e)
        {


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
             _listColorsToReplace =  txtColorsListToReplace.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            ThreadPool.QueueUserWorkItem(StartScrap);

        }


        private void StartScrap(object state)
        {

            foreach (var url in _arrUrls)
            {
                Console.Clear();
                _model.MichlolList.Clear();

                if (url == "")
                {
                    continue; ;
                }



                Thread t = new Thread(ScrapMichlolPageForParts);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                t.Join();



                //InsertToDb();
                Console.WriteLine(@"---------> Finish: " + url);

            }

            foreach (var url in _arrUrls)
            {
                Console.WriteLine(@"---------> All Finished: " + url);
            }

        }

        private void ScrapMichlolPageForParts()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));


            // דף המכלולים
            _driver.Url = txtUrl.Text;
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("body_vue")));

            //   רשימת המכלולים <li>
            IList<IWebElement> listMichlolimLi = _driver.FindElements(By.ClassName("well"));

             
            var michlolList = new List<Michlol>();
            foreach (var li in listMichlolimLi)
            {
                var a = li.FindElement(By.TagName("a"));
                var href = a.GetAttribute("href");
                var michName = a.Text;

                var michlol = new Michlol();
                michlol.Name = michName;
                michlol.PageUrl = href;
                michlolList.Add(michlol);
                //michlol.ImageUrl = diagramUrl;
            }

            var counter = -1;
            foreach (var m in michlolList)
            {
                counter++;
                try
                {
                    _driver.Url = m.PageUrl;
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("assembly")));
                }
                catch (Exception)
                {
                    Thread.Sleep(4000); // wait before trying again

                    try
                    {
                        _driver.Url = m.PageUrl;
                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("assembly")));
                    }
                    catch (Exception)
                    {

                        michlolList.RemoveAt(counter);
                        continue;
                    }

                }



                m.ImageUrl = _driver.FindElement(By.ClassName("image-zoomed")).GetAttribute("src");

                var assemblyTable = _driver.FindElement(By.Id("assembly-table"));
                var tbody = _driver.FindElement(By.TagName("tbody"));

                IList<IWebElement> assemblyTrList = tbody.FindElements(By.TagName("tr"));

                var partList = new List<Part>();
                foreach (var tr in assemblyTrList)
                {
                    var sku = tr.FindElement(By.TagName("a")).GetAttribute("href");

                    var part = new Part();
                    part.Name = tr.FindElement(By.TagName("a")).GetAttribute("title");
                    part.Sku = sku.Replace("https://www.fowlersparts.co.uk/parts/view/", "");
                    part.Price = 0; //
                    part.IdInDiagram = tr.FindElement(By.ClassName("part-ref")).Text;

                    partList.Add(part);
                }
                m.PartList = partList;
            }


            // טען את דף המכלול



            _model.MichlolList = michlolList;
            InsertToDb();




        }

        private By SelectorByAttributeValue(string p_strAttributeName, string p_strAttributeValue)
        {
            var elem = (By.XPath(String.Format("//*[@{0} = '{1}']", p_strAttributeName, p_strAttributeValue)));
            return elem;
        }







        private void InsertToDb()
        {

            var arrModelYears = txtModelYears.Text.Split(new[] { "," }, StringSplitOptions.None);
            _model.Name = txtModelName.Text;

            foreach (var year in arrModelYears)
            {
                _model.Year = year;

                var service = new Services(txtToken.Text);
                int prevCatId = _brandCategoryId; // at this point it's from the DDL



                // model year           
                var category = service.IsCategoryExists(_model.Year, prevCatId); // this func return checks if this cat exists and return it's parentId
                if (!category.IsCategoryExists)
                {
                    category = service.CategoryInsert(_model.Year, prevCatId, "", null);
                }

                // year = 123
                prevCatId = category.Id;

                // model name:             
                category = service.IsCategoryExists(_model.Name, prevCatId); // this func return checks if this cat exists and return it's parentId
                if (!category.IsCategoryExists)
                {
                    // insert & get CatId
                    category = service.CategoryInsert(_model.Name, prevCatId, "", null); // the model takes the catId from combobox
                }

                //gsx=122
                var modelNameCatId = category.Id;




                var michlolLoopCount = 0;
                var michlolTotalCount = _model.MichlolList.Count;
                // each michlol
                foreach (var michlol in _model.MichlolList)
                {
                    michlolLoopCount++;

                    category = service.IsCategoryExists(michlol.Name, modelNameCatId);
                    if (!category.IsCategoryExists)
                    {
                        category = service.CategoryInsert(michlol.Name, modelNameCatId, michlol.ImageUrl, _listColorsToReplace);
                        Console.WriteLine(@"Insert category: " + michlol.Name + $@" ,({michlolLoopCount}/{michlolTotalCount})");
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

        }

        public bool IsApiOnline()
        {
            bool isOnline;
            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                try
                {
                    var str = client.DownloadString(
                        "http://localhost/images/ScrappedDiagrams/test-file-dont-delete.txt");

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

                            if (name.IndexOf('.', 0) == 0)
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
            btnGo.Text = @"Go";
        }

        private void btnTestToken_Click(object sender, EventArgs e)
        {
            var service = new Services(txtToken.Text);
            var str = service.TestApi("http://localhost/api/products/1");
            MessageBox.Show(!string.IsNullOrEmpty(str) ? @"Api & token is ok!" : @"Api Failed!!!");
        }

        private void BtnGetColors_Click(object sender, EventArgs e)
        {


            v.Text = "working...";
            txtColorList.Text = "";

            Bitmap img = (Bitmap)Image.FromFile(txtImagePath.Text);

            var listColors = new List<string>();

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    var colorName = img.GetPixel(x, y).Name;

                    if (colorName != "ffffffff")
                    {
                        // this is just to collect the color from the old diagram - I took a diagram image and kept only the diagram using photoshop
                        // use this to get the colors from the diagram to replace with #fffffff
                        if (!listColors.Contains(colorName))
                        {
                            Console.WriteLine(colorName + " - listed");
                            listColors.Add(colorName);
                            txtColorList.Text += colorName + Environment.NewLine; ;
                        }
                    }
                }

            }
            Console.WriteLine("Finished!");





        }
 

        private void BtnWatermarkRemoveTest_Click(object sender, EventArgs e)
        {
            //var listColor = GetColorList();
            var listColorsToReplace = txtColorListToReplace.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string imagePath = @"D:\www\Oem\wwwroot\images\ScrappedDiagrams\TestImage.jpeg";
            string imagePathRes = @"D:\www\Oem\wwwroot\images\ScrappedDiagrams\TestImageRes.jpeg";


            Bitmap img;
            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                if (txtDiagramTextUrl.Text.Contains("http"))
                {
                    client.DownloadFile(txtDiagramTextUrl.Text, imagePath);
                    img = (Bitmap)Image.FromFile(imagePath);
                }
                else {
                    imagePath = txtDiagramTextUrl.Text;
                    img = (Bitmap)Image.FromFile(imagePath);
                    imagePathRes = txtDiagramTextUrl.Text.Replace(".", "_1."); 
                }

               
                



                var colorsToReplace = new List<string>();

                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        var colorName = img.GetPixel(x, y).Name;

                        if (colorName == "ffffffff") // if white skip
                        {
                            continue;
                        }


                        if (listColorsToReplace.Contains(colorName))
                        {
                            Console.WriteLine(colorName + " replaced");
                            img.SetPixel(x, y, Color.White);
                        }
                    }
                   

                }

                img.Save(imagePathRes);
                img.Dispose();

                Console.WriteLine(@"Image  saved in folder:" + imagePathRes);
            }


        }

        private void BtnToGrayScale_Click(object sender, EventArgs e)
        {
            var imagePath = txtMakeGray.Text;
            var imagePathRes = txtMakeGray.Text.Replace(".", "_gray.");
            
            Bitmap img = (Bitmap)Image.FromFile(imagePath);

            for (int i = 0; i < img.Width; i++)
            {
                for (int x = 0; x < img.Height; x++)
                {
                    Color oc = img.GetPixel(i, x);
                    int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                    img.SetPixel(i, x, nc);
                }
            }

         
            img.Save(imagePathRes);
        }
    }





}
