using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeApp.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApp.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string sortedSetKey = "sortedsetnames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(2);
        }

        public IActionResult Index()
        {
            if (_database.KeyExists(sortedSetKey))
            {
                //ViewBag.Names = _database.SortedSetScan(sortedSetKey);
                ViewBag.Names = _database.SortedSetRangeByRank(sortedSetKey, order: Order.Descending); //score değerleri gelmiyor.
                ViewBag.Names = _database.SortedSetRangeByRankWithScores(sortedSetKey, order: Order.Descending);
            }

            return View();
        }

        public IActionResult Add(string name, int score)
        {
            if (name != null)
                _database.SortedSetAdd(sortedSetKey, name, score);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(string name)
        {
            if (name != null)
                _database.SortedSetRemove(sortedSetKey, name);

            return RedirectToAction("Index");
        }
    }
}
