using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Northwind
{
    public class ProductsController : Controller
    {
        private readonly string baseUrl;
        private readonly string appJson;

        public ProductsController(IConfiguration config)
        {
            baseUrl = config.GetValue<string>("BaseUrl");
            appJson = config.GetValue<string>("AppJson");
        }

        private void ConfigClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));
            client.BaseAddress = new Uri(baseUrl);
        }

        private async Task<List<Category>> GetCategoryAsync()
        {
            var Category = new List<Category>();

            try
            {
                using (var client = new HttpClient())
                {
                    ConfigClient(client);
                    var response = await client.GetAsync("/api/Categories");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Category = JsonSerializer.Deserialize<List<Category>>(json);
                    }
                }
            }
            catch
            {

            } return Category;

        }

        // GET: ProductsController
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? CategoryId)
        {
            CategoryId = (CategoryId != null) ? CategoryId : 1;

            var categories = await GetCategoryAsync();

            ViewBag.CategoryNames = new SelectList(
                    categories,
                    nameof(Category.categoryId),
                    "categoryName"
                );

            try
            {
                var products = new List<Product>();

                using (var client = new HttpClient())
                {
                    ConfigClient(client);
                    var response = await client.GetAsync($"/api/Products/ByCategory?CategoryId={CategoryId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        products = JsonSerializer.Deserialize<List<Product>>(json);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = response.StatusCode.ToString();
                        return View("Error", new ErrorViewModel());
                    }
                }
                return View(products);
            } catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error", new ErrorViewModel());
            }
            
        }



        // GET: ProductsController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            var item = new Product();
            var categories = await GetCategoryAsync();


            using (var client = new HttpClient()){
                try
                {
                    ConfigClient(client);
                    var target = await client.GetAsync($"/api/Products/{id}");

                    if (target.IsSuccessStatusCode)
                    {
                        var json = await target.Content.ReadAsStringAsync();
                        item = JsonSerializer.Deserialize<Product>(json);
                        var itemCategory = from c in categories.Where(c => c.categoryId == item.categoryId) select c;
                        var stringCat = itemCategory.ToArray()[0].categoryName;
                        ViewBag.displayCategory = stringCat;

                    }
                    else
                    {
                        ViewBag.ErrorMessage = target.StatusCode.ToString();
                        return View("Error", new ErrorViewModel());
                    }
                    return View(item);
                } catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View("Error", new ErrorViewModel());
                }
            }
        }
    }
}
