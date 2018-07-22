
using System.Collections.Generic;

namespace OemCrawler
{




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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }

        public double Price { get; set; }

        public string IdInDiagram { get; set; }

    }


    public class Category
    {
        public int Id { get; set; }
        public int ParentCategoryId { get; set; }

        public bool IsCategoryExists { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public bool IsProductExists { get; set; }
    }
}
