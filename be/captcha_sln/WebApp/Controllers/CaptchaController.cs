using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApp.Code;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private IMemoryCache _cache;
        private string CACHE_KEY = "captcha_";

        public CaptchaController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        // GET: api/captcha/create?guid=
        [HttpGet("Create")]
        public IActionResult Create(string guid)
        {
            List<int> resultIndex = new List<int>();
            var img = CaptchaHelper.testLocal(guid, out resultIndex);
            DateTimeOffset dtSet = new DateTimeOffset(DateTime.Now.AddMinutes(10));
            _cache.Set(CACHE_KEY + guid, resultIndex, dtSet);
            byte[] imgByte;
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                imgByte = ms.ToArray();
            }
            return File(imgByte, @"image/jpeg");
        }

        // GET: api/captcha/check?guid=&select=1,2
        [HttpGet("Check")]
        public ClientResult Get(string guid, string select)
        {
            var result = ClientResult.Error("获取数据失败");
            try
            {
                List<int> cache_selectIndex = new List<int>();
                if (_cache.TryGetValue(CACHE_KEY + guid, out cache_selectIndex)) {
                    var selectIndex = select.Split(',');
                    if (cache_selectIndex.Count == selectIndex.Length)
                    {
                        foreach (var item in selectIndex)
                        {
                            if (cache_selectIndex.IndexOf(int.Parse(item)) < 0)
                            {
                                throw new Exception("请选择正确的图案");
                            }
                        }
                        result = ClientResult.Success("恭喜,选择正确");
                    }
                    else
                    {
                        result = ClientResult.Error("请选择正确的图案");
                    }
                }
            }
            catch (Exception)
            {
                result = ClientResult.Error("请选择正确的图案");
            }
            return result;
        }
    }
}
