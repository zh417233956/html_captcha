using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using WebAPI.App_Code;

namespace WebAPI.Controllers
{
    public class APIController : ApiController
    {
        // GET: api/API
        [HttpGet]
        public IHttpActionResult demo()
        {
            var result = ClientResult.Error("获取数据失败");
            try
            {
                var myapi = new MyAPI();
                result = ClientResult.Success(myapi.GetCaptcha("123456"));
            }
            catch (Exception ex)
            {
                result = ClientResult.Error("获取数据失败");
            }
            return Json(result);
        }        
    }   
    public class MyAPI
    {
        private String responseStr = "";
        private String captchaID = "123456";
        private String privateKey = "123456";

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="tokenID">用户标识，若担心用户信息风险，可作预处理(如哈希处理)再提供</param>   
        /// <returns></returns>
        public string GetCaptcha(string tokenID = "")
        {
            string result = "";

            //status=1表示初始化成功，status=0表示宕机状态
            Byte gtServerStatus = preProcess(tokenID);

            result = getResponseStr();
            return result;
        }
        /// <summary>
        /// 验证初始化预处理
        /// </summary>
        /// <returns>初始化结果</returns>
        public Byte preProcess(string userID = "")
        {
            if (this.captchaID == null)
            {
                Console.WriteLine("publicKey is null!");
            }
            else
            {
                this.getFailPreProcessRes();
                Console.WriteLine("Server regist challenge failed!");
            }

            return 0;

        }
        public String getResponseStr()
        {
            return this.responseStr;
        }
        /// <summary>
        /// 预处理失败后的返回格式串
        /// </summary>
        private void getFailPreProcessRes()
        {
            int rand1 = this.getRandomNum();
            int rand2 = this.getRandomNum();
            String md5Str1 = this.md5Encode(rand1 + "");
            String md5Str2 = this.md5Encode(rand2 + "");
            String challenge = md5Str1 + md5Str2.Substring(0, 2);
            this.responseStr = "{" + string.Format(
                 "\"success\":{0},\"gt\":\"{1}\",\"challenge\":\"{2}\",\"new_captcha\":{3}", 0,
                this.captchaID, challenge, "true") + "}";
        }
        private String md5Encode(String plainText)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(plainText)));
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }
        private int getRandomNum()
        {
            Random rand = new Random();
            int randRes = rand.Next(100);
            return randRes;
        }
    }
}
