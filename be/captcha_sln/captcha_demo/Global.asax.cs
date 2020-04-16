using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace captcha_demo
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //初始化图片库
            //传入图片库，图片库路径
            /*
             * 图片库结构：
             * --images(待传入的图片库,图片至少有10张)
             * ----卡通人(文件夹，单个文件夹多于3张图片)
             * --------小猪佩奇.jpg
             * --------吒儿.jpg
             * --------火娃.jpg
             *  ----建筑物(文件夹，单个文件夹多于3张图片)
             * --------沈阳故宫.jpg
             * --------彩电塔.jpg
             * --------百合塔.jpg
             */
            new Mango.Captcha.Core.MangoCaptcha().Init(@"D:\sln\demo\html_captcha\be\captcha_sln\captcha_demo\images");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}