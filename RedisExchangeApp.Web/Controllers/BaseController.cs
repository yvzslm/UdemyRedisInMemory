using Microsoft.AspNetCore.Mvc;
using RedisExchangeApp.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApp.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redisService;
        protected readonly IDatabase _database;

        public BaseController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(2);
        }
    }
}
