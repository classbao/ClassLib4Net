/**
* 命名空间: ClassLib4Net
*
* 功 能： N/A
* 类 名： BookHelper
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2024/7/25 20:54:03 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2024 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLib4Net
{
    /// <summary>
    /// 用于图书/章节等信息处理的类
    /// </summary>
    public class BookHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BookHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region 字符串里提取数字，章节标题提取顺序号(章节序号)
        /// <summary>
        /// 进位制
        /// </summary>
        /// <param name="carrybit">进位制位数</param>
        /// <returns></returns>
        private static int Carrybit(int carrybit)
        {
            switch(carrybit)
            {
                case 2:
                    return 10;
                case 3:
                    return 100;
                case 4:
                    return 1000;
                case 5:
                    return 10000;
                case 6:
                    return 100000;
                case 7:
                    return 1000000;
                case 8:
                    return 10000000;
                case 9:
                    return 100000000;
                case 10:
                    return 1000000000;

                case 0:
                case 1:
                default:
                    return 1;
            }
        }

        /// <summary>
        /// 多组匹配
        /// </summary>
        /// <param name="matches"></param>
        /// <returns></returns>
        private static IList<string> Matches(MatchCollection matches)
        {
            if(null != matches && matches.Count > 0)
            {
                List<string> _matches = new List<string>();
                for(int i = 0; i < matches.Count; i++)
                {
                    if(null != matches[i] && !string.IsNullOrWhiteSpace(matches[i].Value) && !Regex.IsMatch(matches[i].Value, @"^[0〇百千万亿零佰仟萬]*$"))
                    {
                        _matches.Add(matches[i].Value);
                    }
                }

                return _matches;
            }

            return null;
        }

        /// <summary>
        /// 第***部册本卷章节，里面的数字
        /// </summary>
        /// <param name="DigitText">数字文本</param>
        /// <param name="IsArabicNum">是纯阿拉伯数字(默认是)</param>
        /// <returns></returns>
        private static long GetDigitFormTitleBase(string DigitText, bool IsArabicNum = true)
        {
            if(string.IsNullOrWhiteSpace(DigitText))
            {
                return 0;
            }

            string _DigitText = DigitText.Trim();
            long num1 = 0;

            if(!IsArabicNum)
            {
                // 非纯阿拉伯数字，则需要转换成纯阿拉伯数字
                num1 = StringHelper.GetDigitFormUppercase(_DigitText);
                if(num1 > 0)
                {
                    return num1;
                }
                else
                {
                    //string log = $"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName} > {nameof(BookHelper)} > {System.Reflection.MethodBase.GetCurrentMethod().Name} >> +  DigitText={DigitText}，非纯阿拉伯数字，则需要转换成纯阿拉伯数字：num1={num1}";
                    //Console.WriteLine(log);
                    //LogHelper.Save(log, "GetDigitFormTitleBase", LogType.Report, LogTime.month);
                }
            }

            var m = Regex.Match(_DigitText, @"\d*"); // 纯阿拉伯数字，直接用
            if(null != m && !string.IsNullOrWhiteSpace(m.Value))
            {
                num1 = ConvertHelper.GetLong(m.Value);
                if(num1 > 0)
                {
                    return num1;
                }
                else
                {
                    //string log = $"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName} > {nameof(BookHelper)} > {System.Reflection.MethodBase.GetCurrentMethod().Name} >> +  DigitText={DigitText}，非纯阿拉伯数字，则需要转换成纯阿拉伯数字：num1={num1}";
                    //Console.WriteLine(log);
                    //LogHelper.Save(log, "GetDigitFormTitleBase", LogType.Report, LogTime.month);
                }
            }

            return IsArabicNum ? GetDigitFormTitleBase(DigitText, false) : num1;
        }

        /// <summary>
        /// 从标题里提取数字
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static long GetDigitFormTitle(string title)
        {
            long num = 0;
            // 多段序号标题示例：第一部、第三卷 第二十五章，第一百七十四节：五通神
            var matches = Regex.Matches(title, @"(?<=第)[^第部册本卷章节]*(?=[部册本卷章节])");
            IList<string> _matches = Matches(matches);
            if(null != _matches && _matches.Count > 0)
            {
                for(int i = 0; i < _matches.Count; i++)
                {
                    long num1 = GetDigitFormTitleBase(_matches[i]);

                    // 多段序号需要进位
                    if((_matches.Count - 1) == i)
                    {
                        num += num1 * Carrybit(1);
                    }
                    else
                    {
                        num += num1 * Carrybit(_matches.Count - i + 3);
                    }

                }

                return num;
            }


            // 多段序号标题示例：“045节13画26点”，“一万二千三百四十五”，“壹萬贰仟叁佰肆拾伍”，“251000” （即没有“第部册本卷章节”等关键字）
            matches = Regex.Matches(title, @"[\d０１２３４５６７８９〇①②③④⑤⑥⑦⑧⑨〇一二三四五六七八九十百千万亿零壹贰叁肆伍陆柒捌玖拾佰仟萬]*");
            _matches = Matches(matches);
            if(null != _matches && _matches.Count > 0)
            {
                for(int i = 0; i < _matches.Count; i++)
                {
                    long num1 = GetDigitFormTitleBase(_matches[i]);

                    // 多段序号需要进位
                    if((_matches.Count - 1) == i)
                    {
                        num += num1 * Carrybit(1);
                    }
                    else
                    {
                        num += num1 * Carrybit(_matches.Count - i + 3);
                    }

                }

                return num;
            }


            //string log = $"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName} > {nameof(BookHelper)} > {System.Reflection.MethodBase.GetCurrentMethod().Name} >> +  title={title}，标题里找不到序号！！！";
            //Console.WriteLine(log);
            //LogHelper.Save(log, "GetDigitFormTitle", LogType.Report, LogTime.month);

            return num;
        }

        /// <summary>
        /// 是否是最后一章
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool IsLatestChapterSort(string title)
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                return false;
            }

            title = title.Trim();
            if(Regex.IsMatch(title, @"([结結]束语|[终終]\s{0,}章|大[结結]局|尾\s{0,}[声聲]|[结結]\s{1,}束|完本感言|文后说明|写在最[后後])")
                || Regex.IsMatch(title, @"([后後]\s{0,}[记記话話言])"))
            {
                return true;
            }

            return false;
        }

        #endregion


    }
}
