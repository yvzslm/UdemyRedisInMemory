using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High; //memory dolduğu zaman öncelik sırası düşük olan ram'den silinir.
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set<string>("callback", $"key:{key} value:{value} reason:{reason} state:{state}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            var product = new Product() { Id = 1, Name = "Product 1", Price = 25.1 };
            _memoryCache.Set<Product>("product:1", product);
            return View();
        }

        public IActionResult Show()
        {
            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            ViewBag.callback = _memoryCache.Get<string>("callback");
            ViewBag.product = _memoryCache.Get<Product>("product:1");

            //ViewBag.zaman = _memoryCache.GetOrCreate<string>("zaman", e =>
            //{
            //    return DateTime.Now.ToString();
            //});

            return View();
        }
    }
}
