using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Collections;
using System.Web;

namespace Investment.Framework.Utility
{
    public class WebSvcCaller
    {


        /// <summary>
        /// WebRequest 返回 string
        /// </summary>
        /// <param name="URL">地址</param>
        /// <param name="MethodName">方法</param>
        /// <param name="Pars">参数列表</param>
        /// <returns>结果</returns>
        public static string QueryGetWebService(String URL, String MethodName, Hashtable Pars, ref string err)
        {
            string response = string.Empty;

            HttpWebResponse hpwq = null;
            HttpWebRequest request = null;
            try
            {
                string requrl = URL + MethodName + "?" + ParsToString(Pars);
                requrl = requrl.Trim(new char[] { '?' });
                //LogHelper.Send(requrl);
                request = (HttpWebRequest)HttpWebRequest.Create(requrl);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                SetWebRequest(request);
                hpwq = (HttpWebResponse)request.GetResponse();
                response = ReadXmlResponse(hpwq);
                request.Abort();
            }
            catch (Exception ee)
            {
                err = ee.Message;
                LogHelper.Exception(ee.ToString());
            }
            finally
            {
                try
                {
                    if (hpwq != null)
                        hpwq.Close();
                }
                catch
                { }
                try
                {
                    if (request != null)
                        request.Abort();
                }
                catch
                { }


            }
            return response;
        }

        public static string QueryGetWebService(String URL, String MethodName, Hashtable Pars, ref string err, Encoding encode)
        {
            string response = string.Empty;

            HttpWebResponse hpwq = null;
            HttpWebRequest request = null;
            try
            {
                string requrl = URL + MethodName + "?" + ParsToString(Pars);
                requrl = requrl.Trim(new char[] { '?' });
                //LogHelper.Send(requrl);
                request = (HttpWebRequest)HttpWebRequest.Create(requrl);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                SetWebRequest(request);
                hpwq = (HttpWebResponse)request.GetResponse();
                response = ReadXmlResponse(hpwq, encode);
                request.Abort();
            }
            catch (Exception ee)
            {
                err = ee.Message;
                
            }
            finally
            {
                try
                {
                    if (hpwq != null)
                        hpwq.Close();
                }
                catch
                { }
                try
                {
                    if (request != null)
                        request.Abort();
                }
                catch
                { }


            }
            return response;
        }

        public static string QueryGetWebService(String URL, String MethodName, Hashtable Pars, ref string err, Encoding encode, string userAgent)
        {
            string response = string.Empty;

            HttpWebResponse hpwq = null;
            HttpWebRequest request = null;
            try
            {
                string requrl = URL + MethodName + "?" + ParsToString(Pars);
                requrl = requrl.Trim(new char[] { '?' });
                //LogHelper.Send(requrl);
                request = (HttpWebRequest)HttpWebRequest.Create(requrl);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                SetWebRequest(request);
                request.UserAgent = userAgent;
                hpwq = (HttpWebResponse)request.GetResponse();
                response = ReadXmlResponse(hpwq, encode);
                request.Abort();
            }
            catch (Exception ee)
            {
                err = ee.Message;
                 
            }
            finally
            {
                try
                {
                    if (hpwq != null)
                        hpwq.Close();
                }
                catch
                { }
                try
                {
                    if (request != null)
                        request.Abort();
                }
                catch
                { }


            }
            return response;
        }
        /// <summary>
        /// 设置请求参数
        /// </summary>
        /// <param name="request">请求实例</param>
        private static void SetWebRequest(HttpWebRequest request)
        {
            //request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 30000;
            request.UserAgent = "Mozilla/5.0+(Windows+NT+6.1;+WOW64;+Trident/7.0;+rv:11.0)+like+Gecko";
            request.KeepAlive = false;
        }
        /// <summary>
        /// 写入请求内容
        /// </summary>
        /// <param name="request">请求实例</param>
        /// <param name="data">内容</param>
        //private static void WriteRequestData(HttpWebRequest request, byte[] data)
        //{
        //    request.ContentLength = data.Length;
        //    Stream writer = request.GetRequestStream();
        //    writer.Write(data, 0, data.Length);
        //    writer.Close();
        //}
        /// <summary>
        /// 参数Hash内容转字节数组
        /// </summary>
        /// <param name="Pars">参数</param>
        /// <returns>字节数据</returns>
        //private static byte[] EncodePars(Hashtable Pars)
        //{
        //    return Encoding.UTF8.GetBytes(ParsToString(Pars));
        //}
        /// <summary>
        /// 参数Hash内容转字符串
        /// </summary>
        /// <param name="Pars"></param>
        /// <returns></returns>
        private static String ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            if (Pars != null)
            {
                foreach (string k in Pars.Keys)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("&");
                    }
                    sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 读取响应内容
        /// </summary>
        /// <param name="response">响应实例</param>
        /// <returns>内容</returns>
        private static string ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("GBK"));
            String retXml = sr.ReadToEnd();
            sr.Close();
            return retXml;
        }

        private static string ReadXmlResponse(WebResponse response, Encoding encode)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), encode);
            String retXml = sr.ReadToEnd();
            sr.Close();
            return retXml;
        }
    }
}
