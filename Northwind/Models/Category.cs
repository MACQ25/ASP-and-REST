using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
    public class Category
    {
        public int categoryId { get; set; }

        [Display(Name = "Category Name")]
        public string categoryName { get; set; }
        public string description { get; set; }
        public string[] products { get; set; }
    }

}
