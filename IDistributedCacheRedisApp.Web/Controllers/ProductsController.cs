using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(60);

            //_distributedCache.SetString("name", "yavuz", options);

            var product = new Product() { Id = 1, Name = "Kalem", Price = 4.25 };
            var jsonProduct = JsonConvert.SerializeObject(product);
           // _distributedCache.SetString("product:1", jsonProduct, options);

            var byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:1", byteProduct, options);

            return View();
        }

        public IActionResult Show()
        {
            //ViewBag.name = _distributedCache.GetString("name");
            //var jsonProduct = _distributedCache.GetString("product:1");

            var byteProduct = _distributedCache.Get("product:1");
            var jsonProduct = Encoding.UTF8.GetString(byteProduct);

            var product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            return View(product);
        }

        public IActionResult Remove()
        {
            //_distributedCache.Remove("name");
            _distributedCache.Remove("product:1");
            return View("Show");
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/image1.jpg");
            var byteImage = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image:1", byteImage);

            return View();
        }

        public IActionResult ImageShow()
        {
            var byteImage = _distributedCache.Get("image:1");

            return File(byteImage, "image/jpg");
        }
    }
}
