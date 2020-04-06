using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.App_Code
{
    /// <summary>
    /// 服务端返回消息体
    /// </summary>    
    public class ClientResult
    {
        public ClientResult()
        {
        }

        private bool iserror = false;

        private bool iswarn = false;
        /// <summary>
        /// 是否产生错误
        /// </summary>
        public bool IsError { set { iserror = value; } get { return iserror; } }

        /// <summary>
        /// 是否产生警告
        /// </summary>
        public bool IsWarn { set { iswarn = value; } get { return iswarn; } }

        private string message;
        /// <summary>
        /// 错误信息，警告信息，或者成功信息
        /// </summary>
        public string Message
        {
            get { return message ?? ""; }
            set { message = value; }
        }

        /// <summary>
        /// 成功可能时返回的数据
        /// </summary>
        public object Data { get; set; }

        #region Error
        public static ClientResult Error()
        {
            return new ClientResult()
            {
                iserror = true,
                iswarn = false
            };
        }
        public static ClientResult Error(string message)
        {
            return new ClientResult()
            {
                iserror = true,
                iswarn = false,
                Message = message
            };
        }
        public static ClientResult Error(string message, object data)
        {
            return new ClientResult()
            {
                iserror = true,
                iswarn = false,
                Data = data,
                Message = message
            };
        }
        #endregion

        #region Warn
        public static ClientResult Warn()
        {
            return new ClientResult()
            {
                iserror = false,
                iswarn = true
            };
        }
        public static ClientResult Warn(string message)
        {
            return new ClientResult()
            {
                iserror = false,
                iswarn = true,
                Message = message
            };
        }
        #endregion


        #region Success
        public static ClientResult Success()
        {
            return new ClientResult()
            {
                iserror = false,
                iswarn = false
            };
        }
        public static ClientResult Success(string message)
        {
            return new ClientResult()
            {
                iserror = false,
                iswarn = false,
                Message = message
            };
        }

        public static ClientResult Success(string message, object data)
        {
            return new ClientResult()
            {
                iserror = false,
                iswarn = false,
                Data = data,
                Message = message
            };
        }
        #endregion
    }
}