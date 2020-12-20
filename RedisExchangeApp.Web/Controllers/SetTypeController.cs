using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeApp.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApp.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string setKey = "setnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(1);
            _database.KeyExpire(setKey, DateTime.Now.AddMinutes(5)); //absolute time
        }

        public IActionResult Index()
        {
            if (_database.KeyExists(setKey))
                ViewBag.Names = _database.SetMembers(setKey);

            return View();
        }

        public IActionResult Add(string name)
        {
            if (name != null)
                _database.SetAdd(setKey, name);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(string name)
        {
            if (name != null)
                _database.SetRemove(setKey, name);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveRandom()
        {
            _database.SetPop(setKey);

            return RedirectToAction("Index");
        }
    }
}
