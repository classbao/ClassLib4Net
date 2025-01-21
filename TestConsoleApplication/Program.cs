using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ConvertHelperTest.timestamp();
            /*
            #region 密码加、解密
            
            const string s = "123456";
            Console.WriteLine("密码：" + s);
            string md5 = ClassLib4Net.Encrypt.MD5Helper.MD5Encrypt(s);
            Console.WriteLine("Md5：" + md5);
            Console.WriteLine("长度：" + md5.Length);
            string sha1 = ClassLib4Net.Encrypt.SHA1Helper.SHA1Encrypt(s);
            Console.WriteLine("Sha1：" + sha1);
            Console.WriteLine("长度：" + sha1.Length);
            string des = ClassLib4Net.Encrypt.DESHelper.Encrypt(s);
            Console.WriteLine("DES：" + des);
            Console.WriteLine("长度：" + des.Length);
            
            #endregion
            */

            // 图片加水印
            //Image.ImageTest.WaterMarkByText();

            //Cache.RedisTest.String();
            //Cache.RedisTest.Sort();

            //EncryptDemo.rsaEncrypt();
            //EncryptDemo.rsaDecrypt();
            //EncryptDemo.verifyEqual();

            Console.ReadKey();
        }
    }
}
