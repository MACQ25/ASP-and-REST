using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Models
{

    public class Product
    {
        [Display(Name = "Product Id")]
        public int productId { get; set; }

        [Display(Name = "Product Name")]
        public string productName { get; set; }
        
        [Display(Name = "Supplier Id")]
        public int supplierId { get; set; }
        
        [Display(Name = "Category Id")]
        public int categoryId { get; set; }
        
        [Display(Name = "Quantity Per Unit")]
        public string quantityPerUnit { get; set; }
        
        [Display(Name = "Unit Price")]
        public decimal unitPrice { get; set; }
        
        [Display(Name = "In Stock")]
        public int unitsInStock { get; set; }

        [Display(Name = "Units on Order")]
        public int unitsOnOrder { get; set; }

        [Display(Name = "Reorder Level")]
        public int reorderLevel { get; set; }

        [Display(Name = "Discontinued")]
        public bool discontinued { get; set; }

        [Display(Name = "Category")]
        public Category category { get; set; }
    }

}
