using System;
using System.Text;

namespace ClassLib4Net
{
    /// <summary>
    /// ����ת��
    /// </summary>
    public class EncodingTransfer
    {
        /// <summary>
        /// ���ַ�����һ�ֱ���ת������һ�ֱ���
        /// </summary>
        /// <param name="fromText">��Ҫת�����ַ���</param>
        /// <param name="fromEncoding">ԭʼ����</param>
        /// <param name="toEncoding">ת����ı���</param>
        /// <returns>ת�����</returns>
        public static string TransferEncoding(string fromText, Encoding fromEncoding, Encoding toEncoding)
        {
            string rst = string.Empty;
            byte[] fromTextBytes = fromEncoding.GetBytes(fromText);
            byte[] rstAsciiBytes = Encoding.Convert(fromEncoding, toEncoding, fromTextBytes);
            char[] rstAsciiChars = new char[toEncoding.GetCharCount(rstAsciiBytes, 0, rstAsciiBytes.Length)];
            toEncoding.GetChars(rstAsciiBytes, 0, rstAsciiBytes.Length, rstAsciiChars, 0);
            rst = new string(rstAsciiChars);
            return rst;
        }

        /// <summary>
        /// ����base64������ַ���ת��Ϊbyte[]����
        /// </summary>
        /// <param name="base64String">��base64������ַ���</param>
        /// <returns></returns>
        public static byte[] FromBase64String(string base64String)
        {
            string[] strs = new string[] { "", "=", "==", "===", "" };
            base64String = base64String + strs[4 - (base64String.Length % 4)];
            return Convert.FromBase64String(base64String);
        }
    }
}
