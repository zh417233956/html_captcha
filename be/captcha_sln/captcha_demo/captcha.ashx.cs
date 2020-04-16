using Mango.Captcha.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace captcha_demo
{
    /// <summary>
    /// zhh_test 的摘要说明
    /// </summary>
    public class captcha : IHttpHandler
    {
        MangoCaptcha mangoCaptcha;

        public captcha()
        {
            mangoCaptcha = new MangoCaptcha();
        }
        /// <summary>
        /// 上下文链接
        /// </summary>
        HttpContext _context;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            _context = context;
            string action = context.Request["action"];
            switch (action)
            {
                //获取js
                case "getscript":
                    string scriptStr = GetScript();
                    context.Response.Write(scriptStr);
                    break;
                //获取图片
                case "getcaptcha":
                    var captch = GetCaptcha();
                    context.Response.Write(captch);
                    break;
                //校验结果
                case "valcaptcha":
                    //验证码的标识
                    var req_key = context.Request.Form["key"];
                    //用户选中的图片Index
                    var req_select = context.Request.Form["selectValues"];
                    var valcaptch = ValidateCaptcha(Guid.Parse(req_key), req_select);
                    context.Response.Write(valcaptch);
                    break;
            }
        }
        /// <summary>
        /// 获取js
        /// </summary>
        /// <returns></returns>
        private string GetScript()
        {
            //获取
            return mangoCaptcha.GetScript();
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <returns></returns>
        public string GetCaptcha()
        {
            //获取合成后的图片，并返回一个guid
            var image = mangoCaptcha.Generate(out Guid key);

            //进行输出结构化
            byte[] imgByte;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                ms.Position = 0;
                imgByte = ms.ToArray();
            }

            var base64Img = Convert.ToBase64String(imgByte);
            var resultObj = new
            {
                key = key.ToString(),
                base64Img
            };
            return MangoMis.Frame.Helper.Json.JsonHelper.GetJsonString(resultObj);
        }
        /// <summary>
        /// 校验结果
        /// </summary>
        /// <param name="key">验证码的标识</param>
        /// <param name="selectIds">用户选中的图片Index</param>
        /// <returns></returns>
        public string ValidateCaptcha(Guid key, string selectIds)
        {
            var result_obj = new
            {
                success = false,
                message = "校验失败"
            };
            try
            {
                if (string.IsNullOrEmpty(selectIds))
                {
                    throw new Exception("未选择任何图片");
                }

                var result = mangoCaptcha.Validate(key, selectIds.Split(',').Select(m => int.Parse(m)).ToList());
                var message = result ? "success" : "fail";
                result_obj = new
                {
                    success = result,
                    message
                };
            }
            catch (Exception e)
            {
                result_obj = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return MangoMis.Frame.Helper.Json.JsonHelper.GetJsonString(result_obj);

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}