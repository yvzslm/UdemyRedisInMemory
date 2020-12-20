using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RedisExchangeApp.Web.Services;

namespace RedisExchangeApp.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        private string hashKey = "hashnames";

        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            var dict = new Dictionary<string, string>();

            if (_database.KeyExists(hashKey))
            {
                _database.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    dict.Add(x.Name, x.Value);
                });
            }

            return View(dict);
        }

        public IActionResult Add(string key, string name)
        {
            if (key != null && name != null)
                _database.HashSet(hashKey, key, name);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(string key)
        {
            if (key != null)
                _database.HashDelete(hashKey, key);

            return RedirectToAction("Index");
        }
    }
}
