using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

namespace JzSayGen
{
    /// <summary>
    /// 字符串编码扩展
    /// </summary>
    public static class StringCodingExten
    {
        /// <summary>
        /// html编码
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return System.Web.HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <returns></returns>
        public static string HtmlDecode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return System.Web.HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return System.Web.HttpUtility.UrlEncode(s);
        }

        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlDecode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return System.Web.HttpUtility.UrlDecode(s);
        }

        /// <summary>
        /// MD5 不可逆Hash加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string MD5(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";

            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = x.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        /// <summary>
        /// SHA256 不可逆Hash加密
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <returns>SHA256结果,返回长度为44字节的字符串</returns>
        public static string SHA256(this string s)
        {
            return s.SHA256Swap(false);
        }

        /// <summary>
        /// SHA256 不可逆Hash加密 与php、java等交互
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <param name="isHex">是否十六进制编码 true返回64字节 false返回44字节 默认true</param>
        /// <returns>返回长度为44或64字节的字符串</returns>
        public static string SHA256Swap(this string s, bool isHex = true)
        {
            if (string.IsNullOrEmpty(s)) return "";

            byte[] SHA256Data = Encoding.UTF8.GetBytes(s);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return isHex ? BitConverter.ToString(Result).Replace("-", "") : Convert.ToBase64String(Result);
        }

        /// <summary>
        /// AES256 ECB PKCS7 加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key32">加密密钥，有效长度最短1位最长只截取32位</param>
        /// <param name="isHex">是否十六进制编码 默认false</param>
        /// <returns></returns>
        public static string AESEncrypt(this string s, string key32, bool isHex = false)
        {
            if (s.IsNullOrEmpty()) return "";
            if (key32.IsNullOrEmpty()) return "";

            RijndaelManaged rm = new RijndaelManaged();
            rm.Key = UTF8Encoding.UTF8.GetBytes(key32.PadRight(32, '*').Substring(0, 32));
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;

            byte[] dat = UTF8Encoding.UTF8.GetBytes(s);
            using (ICryptoTransform ct = rm.CreateEncryptor())
            {
                byte[] resultArray = ct.TransformFinalBlock(dat, 0, dat.Length);
                return isHex == true ? StringCodingExten.ByteToHexStr(resultArray) : Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        /// <summary>
        /// AES256 ECB PKCS7 解密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key32">解密密钥，有效长度最短1位最长只截取32位</param>
        /// <param name="isHex">是否十六进制编码 默认false</param>
        /// <returns></returns>
        public static string AESDecrypt(this string s, string key32, bool isHex = false)
        {
            if (s.IsNullOrEmpty()) return "";
            if (key32.IsNullOrEmpty()) return "";

            RijndaelManaged rm = new RijndaelManaged();
            rm.Key = UTF8Encoding.UTF8.GetBytes(key32.PadRight(32, '*').Substring(0, 32));
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;

            byte[] dat = isHex == true ? StringCodingExten.StrToHexByte(s) : Convert.FromBase64String(s);
            using (ICryptoTransform ct = rm.CreateDecryptor())
            {
                byte[] resultArray = ct.TransformFinalBlock(dat, 0, dat.Length);
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }

        /// <summary>
        /// AES256 CBC PKCS7 加密 与php、java等交互
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key32">32位密钥</param>
        /// <returns></returns>
        public static string AESEncryptSwap(this string s, string key32)
        {
            if (s.IsNullOrEmpty()) return "";
            if (key32.IsNullOrEmpty() || key32.Length != 32) return "";

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key32);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using (ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV))
            {                
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(s);
                        cs.Write(xXml, 0, xXml.Length);
                    }                
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// AES256 CBC PKCS7 解密 与php、java等交互
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key32">32位密钥</param>
        /// <returns></returns>
        public static string AESDecryptSwap(this string s, string key32)
        {
            if (s.IsNullOrEmpty()) return "";
            if (key32.IsNullOrEmpty() || key32.Length != 32) return "";

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key32);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {                
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(s);
                        cs.Write(xXml, 0, xXml.Length);
                    }                    
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }




        /// <summary> 
        /// 字符串转16进制字节数组 
        /// </summary> 
        /// <param name="hexStr"></param> 
        /// <returns></returns> 
        private static byte[] StrToHexByte(string hexStr)
        {
            if (hexStr.IsNullOrEmpty()) return null;
            if ((hexStr.Length % 2) != 0) hexStr += " ";
            byte[] returnBytes = new byte[hexStr.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string ByteToHexStr(byte[] bytes)
        {
            if (bytes == null) return "";
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                s.Append(bytes[i].ToString("X2"));
            }
            return s.ToString();
        }

    }
}
