using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace OemCrawler
{


    public partial class Form1 : Form
    {
        private  Models _model;       
        private string _htmlCode = "";
        private HtmlAgilityPack.HtmlDocument _htmlAgilityDoc;
        public int CategoryId;
        private int _totalPartCounter;
        private int _brandCategoryId;


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
                { 5, "אופנועים -> סוזוקי" },
                { 1024, "אופנועים -> הוסקוורנה" }
            };

            cmbCategory.DataSource = new BindingSource(comboSource, null);
            cmbCategory.DisplayMember = "Value";
            cmbCategory.ValueMember = "Key";
        }

        private void btnGo_Click(object sender, EventArgs e)
        {

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

            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                try
                {
                    _htmlCode = client.DownloadString(txtUrl.Text.Trim());
                }
                catch (WebException)
                {
                    System.Threading.Thread.Sleep(3000);
                    _htmlCode = client.DownloadString(txtUrl.Text.Trim());
                }

            }

            ScrapModelPage(); // scrap model page
            GetMichlolNameAndUrl(); // from model page
            ScrapMichlolPages(); // get the parts
            InsertToDb();



            btnGo.Text = @"Go";
        }

        private void InsertToDb()
        {            

            var service = new Services(txtToken.Text);

            int parentId = 0;

            // model name:             
            var category = service.IsCategoryExists(_model.Name, _brandCategoryId); // this func return checks if this cat exists and return it's parentId
            if (!category.IsCategoryExists)  
            {
                // insert & get CatId
                category.Id = service.CategoryInsert(_model.Name, _brandCategoryId,""); // the model takes the catId from combobox
            }
            parentId = category.Id;

         



            // model year           
            category = service.IsCategoryExists(_model.Year, category.ParentCategoryId); // this func return checks if this cat exists and return it's parentId
            if (!category.IsCategoryExists)
            {
                category.Id = service.CategoryInsert(_model.Year, category.ParentCategoryId, ""); 
            }
            category.ParentCategoryId = category.Id;



            // each michlol
            foreach (var michlol in _model.MichlolList)
            {
                category = service.IsCategoryExists(michlol.Name, category.ParentCategoryId);
                if (!category.IsCategoryExists)
                {
                    category.Id = service.CategoryInsert(_model.Year, category.ParentCategoryId, michlol.ImageUrl);
                }
                category.ParentCategoryId = category.Id;

                // each part in michlol
                foreach (var part in michlol.PartList)
                {
                    var productId = service.ProductInsert(part.Name, part.Sku, part.Price, part.IdInDiagram);
                    service.ProductCategoryMapping(productId, category.Id);
                }
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

            _model.Year = LbModelYear.Text = str.Substring(0, 4); // get the year. 4:year length
             

            str = str.Replace(_model.Year, ""); // " Suzuki GSXR600"
            str = str.Trim();  // "Suzuki GSXR600"

            var arrStr = str.Split(' ');
            _model.Brand = arrStr[0]; //Suzuki
            _model.Name = LbModelName.Text = arrStr[1]; //GSXR600

        }
        private void GetMichlolNameAndUrl()
        {

            var stringToFind = "https://www.motosport.com/motorcycle/oem-parts";
            var nextPosLoc = _htmlCode.IndexOf(stringToFind, StringComparison.Ordinal);


            var counter = 1; // just for testing - to make sure it loops only once 
            while (nextPosLoc > 0 && counter <= 1)

            {
                counter++;

                var michlol = new Michlol();
                _htmlCode = _htmlCode.Remove(0, nextPosLoc); // remove to the start

                var nextPosLocEnd = _htmlCode.IndexOf("\">", StringComparison.Ordinal);  // the end of the url  ">
                michlol.PageUrl = _htmlCode.Substring(0, nextPosLocEnd); // get the url                

                _htmlCode = _htmlCode.Remove(0, nextPosLocEnd + ("\">".Length)); // remove all upto the end of url (before the name)

                nextPosLocEnd = _htmlCode.IndexOf("</a>", 0, StringComparison.Ordinal);
                michlol.Name = LbMichlolName.Text = _htmlCode.Substring(0, nextPosLocEnd); // get the name

                var countSlash = michlol.PageUrl.Count(f => f == '/');
                if (countSlash >= 8)
                {
                    _model.MichlolList.Add(michlol);
                }

                nextPosLoc = _htmlCode.IndexOf(stringToFind, StringComparison.Ordinal); // find the next part of the URL (first time before the While() )
            }
        }

        private void AddMichlolParts(Michlol michlol, string htmlCode)
        {
            _htmlAgilityDoc.LoadHtml(htmlCode);
            htmlCode = _htmlAgilityDoc.DocumentNode.SelectSingleNode("//ul[@id='SKUDataTable']").InnerHtml;


            var nextPosLoc = htmlCode.IndexOf(" sku=\"", StringComparison.Ordinal); // keep the " " before sku !

            var partCounter = 0;
            while (nextPosLoc > 0)
            {
                _totalPartCounter++;
                LbTotalPartCounter.Text = _totalPartCounter.ToString();

                partCounter++;
                LbPartCounter.Text = partCounter.ToString();


                var part = new Part();

                //sku
                htmlCode = htmlCode.Remove(0, (nextPosLoc + " sku=\"".Length)); // move to sku, remove before
                var nextPosLocEnd = htmlCode.IndexOf("\"", StringComparison.Ordinal);  // the end of the sku ended with  quote:  "
                part.Sku = htmlCode.Substring(0, nextPosLocEnd);


                //Id in diagram:
                nextPosLoc = htmlCode.IndexOf("class=\"oem-count\">", StringComparison.Ordinal); // just to move cursor
                htmlCode = htmlCode.Remove(0, nextPosLoc + "class=\"oem-count\">".Length); // remove up to this point
                nextPosLocEnd = htmlCode.IndexOf("</span>", StringComparison.Ordinal); //Price: </strong><i class="fa fa-ils"></i>36.63</span>
                part.IdInDiagram = int.Parse(htmlCode.Substring(0, nextPosLocEnd));


                //name
                nextPosLoc = htmlCode.IndexOf("oem-description", StringComparison.Ordinal); // just to move cursor to remove duplicated strings
                htmlCode = htmlCode.Remove(0, nextPosLoc + "oem-description".Length); //  remove up to this point
                nextPosLoc = htmlCode.IndexOf("class=\"trackevent\">", StringComparison.Ordinal); // just to cut before
                htmlCode = htmlCode.Remove(0, nextPosLoc + ("class=\"trackevent\">".Length)); // remove up to "oem-description"
                nextPosLocEnd = htmlCode.IndexOf("</a>", StringComparison.Ordinal); //class="trackevent">VALVE ASSY, 2ND AIR REED</a>
                part.Name = LbPartName.Text = htmlCode.Substring(0, nextPosLocEnd);

                //price
                nextPosLoc = htmlCode.IndexOf("Price: </strong><i class=\"fa fa-ils\"></i>", StringComparison.Ordinal); // just to move cursor
                htmlCode = htmlCode.Remove(0, nextPosLoc + ("Price: </strong><i class=\"fa fa-ils\"></i>".Length));
                nextPosLocEnd = htmlCode.IndexOf("</span>", StringComparison.Ordinal); //Price: </strong><i class="fa fa-ils"></i>36.63</span>
                part.Price = double.Parse(htmlCode.Substring(0, nextPosLocEnd));

                //-----ADD-----            
                michlol.PartList.Add(part);


                nextPosLoc = htmlCode.IndexOf(" sku=\"", StringComparison.Ordinal); // for the next sku loop

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
                        System.Threading.Thread.Sleep(3000);
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

            _htmlAgilityDoc = new HtmlAgilityPack.HtmlDocument();

            _htmlAgilityDoc.LoadHtml(htmlCode);

            var imageElem = _htmlAgilityDoc.DocumentNode.SelectSingleNode("//*[contains(@class,'PGImage active')]");
            var imageHref = imageElem.Attributes["data-src"].Value;
            imageHref = imageHref.Replace("zoom=3", "zoom=5"); // I need zoom=5 (bigger)
            michlol.ImageUrl = "https://www.motosport.com" + imageHref;
        }


       






        public class Models
        {
            public Models()
            {
                MichlolList = new List<Michlol>();
            }

            public string Name { get; set; }
            public string Brand { get; set; }
            public string Year { get; set; }

            public List<Michlol> MichlolList { get; set; }


        }

        public class Michlol
        {
            public Michlol()
            {
                PartList = new List<Part>();
            }

            public string Name { get; set; }
            public string PageUrl { get; set; }

            public string ImageUrl { get; set; }

            public List<Part> PartList { get; set; }

        }

        public class Part
        {
            public string Name { get; set; }
            public string Sku { get; set; }

            public double Price { get; set; }

            public int IdInDiagram { get; set; }

        }


        public class Category
        {
            public int Id { get; set; }
            public int ParentCategoryId { get; set; }

            public bool IsCategoryExists { get; set; }
        }

      

        
    }




}
