﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Xml;
using System.IO;
using System.Globalization;

namespace ClassLib4Net
{
    /// <summary>
    /// 用于把对象型数据转成特定数据类型的类
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        /// 转成基本的类型（支持可空类型）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">可空的对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ToObject<T>(object obj, T defaultValue)
        {
            if(null == obj || System.DBNull.Value == obj)
                return defaultValue;
            return obj.ConvertObject<T>(defaultValue);
        }

        #region 将对象变量转成字符串变量的方法
        /// <summary>
        /// 将对象变量转成字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>字符串变量</returns>
        public static string GetString(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? "" : obj.ToString();
        }
        #endregion

        #region 将对象变量转成32位整数型变量的方法
        /// <summary>
        /// 将对象变量转成32位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>32位整数型变量</returns>
        public static int GetInteger(object obj)
        {
            return ConvertStringToInteger(GetString(obj));
        }
        #endregion

        #region 将对象变量转成短整型变量的方法

        /// <summary>
        /// 将对象变量转成短整型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>短整型变量</returns>
        public static short GetShortInt(object obj)
        {
            return ConvertStringToShortInt(GetString(obj));
        }
        #endregion

        #region 将对象变量转成64位整数型变量的方法
        /// <summary>
        /// 将对象变量转成64位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>64位整数型变量</returns>
        public static long GetLong(object obj)
        {
            return ConvertStringToLong(GetString(obj));
        }
        #endregion

        #region 将对象变量转成双精度浮点型变量的方法
        /// <summary>
        /// 将对象变量转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>双精度浮点型变量</returns>
        public static double GetDouble(object obj)
        {
            return ConvertStringToDouble(GetString(obj));
        }
        #endregion

        #region 将对象变量转成十进制数字变量的方法
        /// <summary>
        /// 将对象变量转成十进制数字变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>十进制数字变量</returns>
        public static decimal GetDecimal(object obj)
        {
            return ConvertStringToDecimal(GetString(obj));
        }
        #endregion

        #region 将对象变量转成布尔型变量的方法
        /// <summary>
        /// 将对象变量转成布尔型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>布尔型变量</returns>
        public static bool GetBoolean(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? false :
                GetString(obj).ToLower() == "true" ? true : false;
        }
        #endregion

        #region 将对象变量转成日期时间型字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期时间型字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <param name="sFormat">时间字符串格式，例：yyyy-MM-dd</param>
        /// <returns>时间型字符串变量</returns>
        public static string GetDateTimeString(object obj, string sFormat)
        {
            return GetDateTime(obj).ToString(sFormat);
        }
        #endregion

        #region 将对象变量转成日期字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期字符串变量</returns>
        public static string GetShortDateString(object obj)
        {
            return GetDateTimeString(obj, "yyyy-MM-dd");
        }
        #endregion

        #region 将对象变量转成日期型变量的方法
        /// <summary>
        /// 将对象变量转成日期型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期型变量</returns>
        public static DateTime GetDateTime(object obj)
        {
            DateTime result;
            DateTime.TryParse(GetString(obj), out result);
            return result;
        }
        #endregion

        #region 将对象变量转成货币字符串的方法
        /// <summary>
        /// 将对象变量转成货币字符串的方法
        /// </summary>
        /// <param name="Currency">货币值</param>
        /// <param name="EndLength">默认格式化小数点后面保留两位小数</param>
        /// <param name="CurrencySymbol">是否有货币符号</param>
        /// <param name="Country">货币符号地区</param>
        /// <returns></returns>
        public static string ConvertCurrency(double Currency, int EndLength = 2, bool CurrencySymbol = false, string Country = "zh-CN")
        {
            string _format = "";
            CultureInfo cul = null; //https://msdn.microsoft.com/zh-cn/library/kx54z3k7(VS.80).aspx
            if(CurrencySymbol)
            {
                if(!string.IsNullOrWhiteSpace(Country))
                {
                    switch(Country.Trim().ToLower())
                    {
                        case "zh-cn":
                            Country = "zh-CN";//中国大陆
                            break;
                        case "zh-hk":
                            Country = "zh-HK";//香港
                            break;
                        case "zh-tw":
                            Country = "zh-TW";//台湾
                            break;
                        case "en-us":
                            Country = "en-US";//美国
                            break;
                        case "en-gb":
                            Country = "en-GB";//英国
                            break;
                        case "ja-jp":
                            Country = "ja-JP";//日本
                            break;
                        default:
                            Country = "zh-CN";//中国大陆
                            break;
                    }
                }
                else
                    Country = "zh-CN";//中国大陆
                cul = new CultureInfo(Country);

                switch(EndLength)
                {
                    case 6:
                        _format = "{0:C6}";
                        break;
                    case 5:
                        _format = "{0:C5}";
                        break;
                    case 4:
                        _format = "{0:C4}";
                        break;
                    case 3:
                        _format = "{0:C3}";
                        break;
                    case 1:
                        _format = "{0:C1}";
                        break;
                    default:
                        _format = "{0:C2}"; //默认格式化小数点后面保留两位小数
                        break;
                }
                return Currency.ToString(_format, cul);//有货币符号
            }
            else
            {
                switch(EndLength)
                {
                    case 6:
                        _format = "{0:N6}";
                        break;
                    case 5:
                        _format = "{0:N5}";
                        break;
                    case 4:
                        _format = "{0:N4}";
                        break;
                    case 3:
                        _format = "{0:N3}";
                        break;
                    case 1:
                        _format = "{0:N1}";
                        break;
                    default:
                        _format = "{0:N2}"; //默认格式化小数点后面保留两位小数
                        break;
                }
                return string.Format(_format, Currency);//没有货币符号 
            }
        }
        #endregion

        #region 私有方法

        #region 将字符串转成32位整数型变量的方法
        /// <summary>
        /// 将字符串转成32位整数型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>32位整数型变量</returns>
        private static int ConvertStringToInteger(string s)
        {
            int result;
            int.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成短整型变量的方法

        /// <summary>
        /// 将字符串转成短整型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>短整型变量</returns>
        private static short ConvertStringToShortInt(string s)
        {
            short result;
            short.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成64位整数型变量的方法
        /// <summary>
        /// 将字符串转成64位整数型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>64位整数型变量</returns>
        private static long ConvertStringToLong(string s)
        {
            long result;
            long.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成双精度浮点型变量的方法
        /// <summary>
        /// 将字符串转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>双精度浮点型变量</returns>
        private static double ConvertStringToDouble(string s)
        {
            double result;
            double.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成十进制数字变量的方法
        /// <summary>
        /// 将字符串转成十进制数字变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>十进制数字变量</returns>
        private static decimal ConvertStringToDecimal(string s)
        {
            decimal result;
            decimal.TryParse(s, out result);
            return result;
        }
        #endregion

        #endregion

        #region 将DataTable转换成xml格式数据
        /// <summary>
        /// 将DataTable转换成xml格式数据
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <returns>xml结果集</returns>
        public static string ConvertDataTableToXml(DataTable dt)
        {
            var returnValue = string.Empty;
            if(dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter xmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt
                    xmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据
                    dt.WriteXml(xmlWt);
                    var count = (int)ms.Length;
                    var temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本
                    var ucode = new UnicodeEncoding();
                    returnValue = ucode.GetString(temp).Trim();
                }
                catch(System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if(xmlWt != null)
                    {
                        xmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            return returnValue;
        }
        #endregion

        #region  将xml字符串转换成DataSet
        /// <summary>
        /// 将xml字符串转换成DataSet
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns>转换后的DataSet</returns>
        public static DataSet ConvertXmlToDataSet(string xmlStr)
        {
            if(!string.IsNullOrEmpty(xmlStr))
            {
                StringReader strStream = null;
                XmlTextReader xmlrdr = null;
                try
                {
                    var ds = new DataSet();
                    //读取字符串中的信息
                    strStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    xmlrdr = new XmlTextReader(strStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(xmlrdr);
                    return ds;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if(xmlrdr != null)
                    {
                        xmlrdr.Close();
                        strStream.Close();
                        strStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region xml转换datatable
        /// <summary>
        /// xml转换datatable
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <param name="tableIndex">表格索引</param>
        /// <returns>转换后的DataTable</returns>
        public static DataTable ConvertXmlToDataTable(string xmlStr, int tableIndex)
        {
            var ds = ConvertXmlToDataSet(xmlStr);
            if(ds != null && ds.Tables.Count > tableIndex)
            {
                return ds.Tables[tableIndex];
            }
            return null;
        }
        #endregion

        #region xml转换datatable
        /// <summary>
        ///  xml转换datatable
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns>转换后的DataTable</returns>
        public static DataTable ConvertXmlToDataTable(string xmlStr)
        {
            return ConvertXmlToDataTable(xmlStr, 0);
        }
        #endregion


        #region 时间戳（方式一）UnixEpoch ToUniversalTime

        /// <summary>
        /// 时间戳原点 / Unix纪元（协调世界时 (UTC)的 1970-1-1 0:0:0 +00 ）
        /// </summary>
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 任意时区的日期时间转换为UTC时间后与时间戳原点的时长（用于计算时间戳）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>时间距离</returns>
        public static TimeSpan UnixElapsedTime(DateTime dateTime)
        {
            DateTime utcTime = dateTime.ToUniversalTime();
            TimeSpan elapsedTime = utcTime - UnixEpoch;
            return elapsedTime;
        }
        /// <summary>
        /// 将任意时区的日期时间转换为Unix时间戳（秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double ToUnixTimeSeconds(DateTime dateTime)
        {
            return UnixElapsedTime(dateTime).TotalSeconds;
        }
        /// <summary>
        /// 将任意时区的日期时间转换为Unix时间戳（秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestampSeconds(DateTime dateTime)
        {
            return (long)UnixElapsedTime(dateTime).TotalSeconds;
        }

        /// <summary>
        /// 将任意时区的日期时间转换为Unix时间戳（毫秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double ToUnixTimeMilliseconds(DateTime dateTime)
        {
            return UnixElapsedTime(dateTime).TotalMilliseconds;
        }
        /// <summary>
        /// 将任意时区的日期时间转换为Unix时间戳（毫秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestampMilliseconds(DateTime dateTime)
        {
            return (long)UnixElapsedTime(dateTime).TotalMilliseconds;
        }

        /// <summary>
        /// Unix时间戳（秒）转换为当地时间（System.DateTimeKind.Local）
        /// </summary>
        /// <param name="unixTimeSeconds"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSeconds(double unixTimeSeconds)
        {
            var utcTime = UnixEpoch.AddSeconds(unixTimeSeconds);
            return utcTime.ToLocalTime();
        }
        /// <summary>
        /// Unix时间戳（秒）转换为当地时间（System.DateTimeKind.Local）
        /// </summary>
        /// <param name="unixTimeSeconds"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSeconds(long unixTimeSeconds)
        {
            var utcTime = UnixEpoch.AddSeconds(unixTimeSeconds);
            return utcTime.ToLocalTime();
        }
        /// <summary>
        /// Unix时间戳（毫秒）转换为当地时间（System.DateTimeKind.Local）
        /// </summary>
        /// <param name="unixTimeMilliseconds"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(double unixTimeMilliseconds)
        {
            var utcTime = UnixEpoch.AddMilliseconds(unixTimeMilliseconds);
            return utcTime.ToLocalTime();
        }
        /// <summary>
        /// Unix时间戳（毫秒）转换为当地时间（System.DateTimeKind.Local）
        /// </summary>
        /// <param name="unixTimeMilliseconds"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(long unixTimeMilliseconds)
        {
            var utcTime = UnixEpoch.AddMilliseconds(unixTimeMilliseconds);
            return utcTime.ToLocalTime();
        }
        
        #endregion

        #region 时间戳（方式二） TimeZone.CurrentTimeZone.ToLocalTime

        /// <summary>
        /// 当前时区的日期时间与unix时间戳原点的时长（用于计算时间戳）
        /// DateTime时间格式转换为Unix时间戳格式（基准时间为"1970-1-1 00:00:00"转换成当前计算机的时区的时间）
        /// </summary>
        /// <param name="time">DateTime时间</param>
        /// <returns>时间距离</returns>
        public static TimeSpan LocalTimeElapsedTime(DateTime dateTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            TimeSpan elapsedTime = dateTime - startTime;
            return elapsedTime;
        }

        /// <summary>
        /// 当前时区的日期时间转换为Unix时间戳（秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Unix时间戳（秒）</returns>
        public static double ToTimeStampSecondsFromLocalTime(DateTime dateTime)
        {
            return LocalTimeElapsedTime(dateTime).TotalSeconds;
        }
        /// <summary>
        /// 当前时区的日期时间转换为Unix时间戳（秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Unix时间戳（秒）</returns>
        public static long ToTimestampSecondsFromLocalTime(DateTime dateTime)
        {
            return (long)LocalTimeElapsedTime(dateTime).TotalSeconds;
        }
        /// <summary>
        /// 当前时区的日期时间转换为Unix时间戳（毫秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Unix时间戳（毫秒）</returns>
        public static double ToTimeStampMillisecondsFromLocalTime(DateTime dateTime)
        {
            return LocalTimeElapsedTime(dateTime).TotalMilliseconds;
        }
        /// <summary>
        /// 当前时区的日期时间转换为Unix时间戳（毫秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Unix时间戳（毫秒）</returns>
        public static long ToTimestampMillisecondsFromLocalTime(DateTime dateTime)
        {
            return (long)LocalTimeElapsedTime(dateTime).TotalMilliseconds;
        }


        /// <summary>
        /// Unix时间戳（秒）转换为当地时间（System.DateTimeKind.Local）
        /// </summary>
        /// <param name="timestampSeconds"></param>
        /// <returns></returns>
        public static DateTime FromTimeStampSecondsToLocalTime(double timestampSeconds)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
            time = startTime.AddSeconds(timestampSeconds);
            return time;
        }

        /// <summary>
        /// Unix时间戳（毫秒）转换为当地时间（System.DateTimeKind.Local）
        /// </summary>
        /// <param name="timestamp">double 型数字（13位）</param>
        /// <returns>DateTime</returns>
        public static DateTime FromTimeStampMillisecondsToLocalTime(double timestampMilliseconds)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
            time = startTime.AddMilliseconds(timestampMilliseconds);
            return time;
        }

        #endregion

        #region 时间戳（方式三） 极简版

        /// <summary>
        /// Unix时间戳（秒）转换为标准时间格式（基准时间为"1970-1-1 08:00:00"北京时间）
        /// </summary>
        /// <param name="TimeStamp">Unix时间戳（10位）</param>
        /// <returns>标准时间格式</returns>
        public static DateTime TimeStamp(long timestamp)
        {
            DateTime baseTime = Convert.ToDateTime("1970-1-1 08:00:00");
            return baseTime.AddSeconds(timestamp);
        }
        /// <summary>
        /// 标准时间格式转换为Unix时间戳（秒）（基准时间为"1970-1-1 08:00:00"北京时间）
        /// </summary>
        /// <param name="time">标准时间格式</param>
        /// <returns>Unix时间戳</returns>
        public static long TimeStamp(DateTime time)
        {
            //基准为"1970-1-1 08:00:00"时间转整数
            DateTime baseTime = Convert.ToDateTime("1970-1-1 08:00:00");
            TimeSpan ts = time - baseTime;
            long TimeStamp = (long)ts.TotalSeconds;

            return TimeStamp;
        }

        #endregion


    }
}
