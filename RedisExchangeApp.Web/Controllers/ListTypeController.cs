using Microsoft.AspNetCore.Mvc;
using RedisExchangeApp.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApp.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        {            
            _redisService = redisService;
            _database = _redisService.GetDatabase(1);
        }

        public IActionResult Index()
        {
            if (_database.KeyExists(listKey))
                ViewBag.Names = _database.ListRange(listKey);

            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            if(name!= null)
                _database.ListRightPush(listKey, name);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(string name)
        {
            if (name != null)
                _database.ListRemove(listKey, name);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFirst()
        {
            _database.ListLeftPop(listKey);

            return RedirectToAction("Index");
        }
    }
}
