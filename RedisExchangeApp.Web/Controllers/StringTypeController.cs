using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisExchangeApp.Web.Models;
using RedisExchangeApp.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApp.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(0);
            var msnd = new ErrorViewModel()
            {
                RequestId = "5"
            };
            var json = JsonConvert.SerializeObject(msnd);
            _database.Execute("JSON.SET", "transaction", json);
        }

        public IActionResult Index()
        {
            _database.StringSet("name", "yavuz selim altun");
            _database.StringSet("visitor", 100);
            return View();
        }

        public IActionResult Show()
        {
            //var nameValue = _database.StringGet("name");
            var nameValue = _database.StringGetRange("name", 1, 5);
            if (nameValue.HasValue)
                ViewBag.Name = nameValue.ToString();

            var nameLength = _database.StringLength("name");
            ViewBag.NameLength = nameLength;

            //var visitorValue = _database.StringIncrement("visitor");
            var visitorValue = _database.StringDecrement("visitor", 5);
            ViewBag.Visitor = visitorValue.ToString();

            return View();
        }
    }
}
