using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;

namespace BBS2018.Bussiness.Utils
{
    public class EDcryptUtil
    {

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] input = Encoding.UTF8.GetBytes(source);
            byte[] output = md5.ComputeHash(input);

            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }
        #endregion

        #region base64加密解密
        public static string Base64Encrypt(string source)
        {

            if (string.IsNullOrEmpty(source)) return "";

            byte[] bytes = Encoding.UTF8.GetBytes(source);

            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decrypt(string base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return "";

            byte[] bytes = Convert.FromBase64String(base64String);

            return Encoding.UTF8.GetString(bytes);
        }
        #endregion
    }
}
