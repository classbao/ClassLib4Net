/**
* 命名空间: TestConsoleApplication
*
* 功 能： N/A
* 类 名： EncryptDemo
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2022/7/24 14:07:31 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2022 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    public class EncryptDemo
    {
        //公钥（用于加密）
        private static string RSA_PUBLIC_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDws/6HnJz/fUF+5zLpj2QFg1Js+JcCwZZS2/4lMTUFsiP/tg0Mo+Z0S2/Xs62Aoj8l177K69FDFQqWVRNrZ6ZFk2vSEhhvi8hOW34z3HFLd/GJu4lLxzXe4ZwCnuNzgGTr/Kn3D06y0wQuTOlnyU5est0bo8cUoodbbSBwhovufQIDAQAB";
        //私钥（用于解密）
        private static string RSA_PRIVATE_KEY = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAPCz/oecnP99QX7nMumPZAWDUmz4lwLBllLb/iUxNQWyI/+2DQyj5nRLb9ezrYCiPyXXvsrr0UMVCpZVE2tnpkWTa9ISGG+LyE5bfjPccUt38Ym7iUvHNd7hnAKe43OAZOv8qfcPTrLTBC5M6WfJTl6y3RujxxSih1ttIHCGi+59AgMBAAECgYAs+hU3leYoP2l7owv6ZJbWCOHgBtuA4leKiB0HKhi0RcNARu5NTFsFpAr0DVkATlDqa1P1avchR89ApAkK6sVAoi88NmZ+b72QFR801BCl0BBKdhSZuRWbctj523mq1tdP54QVcKxdUP3e28TNTUWjVF1JzPj/V25dd4LQLdKN6QJBAP14IzZkfzj+/X+uinbrjIsCU8BWQ7QQAdR66Bm/IReGGhyw3ayKWF90eZpHN9mkG7tHnHTnSZ9wqCRZnsmP0YsCQQDzGzoCEb3wHaZC4vqQbhWKQvx8PkhNBr77Dwbxmxio9heYkAMAeg7U09EYEhbOnaoRySDQPhEo2U4bkxt6HbEXAkACtBRUETBWMEoN4ZOyfwKpvOWdyI3CTuVmGcV9+M7mjFSc97n1jCgzJG/jmFzdImX1Umc1y/26CJ6SfK434iPPAkEAtHBZ7t1CUC2BkRWtinPa0wODKaiEu38tU2TE76EBfa8itl71i24NAhSxlp8ehH7rk0XocAgRbnNUEQ1wEIRnQwJASvRWMY7FMqp7XlxnN89TKO7vpeKiNLXlrsVPFY+UGrKyuerhZP9FvtAWbgM1Ih0LzfSuBGirwwjuGtWceY7n+g==";

        private static string plaintext = "钱庄王员外这个人怎么样？Xxh1991还是在二次战役的时候，有一支志愿军的部队向敌后猛插，去切断军隅里敌人的逃路。当他们赶到书堂站时，逃敌也恰恰赶到那里，眼看就要从汽车路上开过去。这支部队的先头连（三连）就匆匆占领了汽车路边一个很低的光光的小山岗，阻住敌人，一场壮烈的搏斗就开始了。敌人为了逃命，用三十二架飞机，十多辆坦克和集团冲锋向这个连的阵地汹涌卷来。整个山顶都被打翻了。汽油弹的火焰把这个阵地烧红了。但勇士们在这烟与火的山岗上，高喊着口号，一次又一次把敌人打死在阵地前面。Abc123";
        private static string ciphertext = "iCFrFWxnPB1J0qee70mXZvYoocs5zLdQvLY1DOikx3i+UDqHfPfRFaXUyjMDHG4iiZSoasYJIYq0wnd8+GlBN3APxB92uMSSBbP+/O8ivcf+UaEGPcbB9zqvjsEMaYb2F1p5VnJwvfOZOosX26mk+QXcGPNymE6Q9PHV6RLclBo=";


        // "-----BEGIN PUBLIC KEY----- // Base64 string omitted // -----END PUBLIC KEY-----"
        private static string publicKey = $"-----BEGIN PUBLIC KEY-----\n{RSA_PUBLIC_KEY}\n-----END PUBLIC KEY-----\n";
        // "-----BEGIN PRIVATE KEY----- // Base64 string omitted// -----END PRIVATE KEY-----"
        private static string privateKey = $"-----BEGIN PRIVATE KEY-----\n{RSA_PRIVATE_KEY}\n-----END PRIVATE KEY-----\n";



        private static string cipheredWithPublic = "", cipheredWithPrivate = "";
        private static string encryptedWithPublic = "", encryptedWithPrivate = "";
        public static void rsaEncrypt()
        {
            // Set up 

            if(ClassLib4Net.StringHelper.GetRealLength(plaintext) < 117)
            {
                // Encrypt it
                encryptedWithPublic = ClassLib4Net.Encrypt.RSAHelper.RsaEncryptWithPublic(plaintext, publicKey);

                // 不常见的
                encryptedWithPrivate = ClassLib4Net.Encrypt.RSAHelper.RsaEncryptWithPrivate(plaintext, privateKey);

                Console.WriteLine($" encryptedWithPublic={encryptedWithPublic}\r\n encryptedWithPrivate={encryptedWithPrivate}");
            }
            else
            {
                // Encrypt it
                encryptedWithPublic = ClassLib4Net.Encrypt.RSAHelper.RsaEncryptWithPublicForLongText(plaintext, publicKey);

                // 不常见的
                encryptedWithPrivate = ClassLib4Net.Encrypt.RSAHelper.RsaEncryptWithPrivateForLongText(plaintext, privateKey);

                Console.WriteLine($" encryptedWithPublic={encryptedWithPublic}\r\n encryptedWithPrivate={encryptedWithPrivate}");
            }

            Console.Read();
        }


        public static void rsaDecrypt()
        {
            if(ClassLib4Net.StringHelper.GetRealLength(ciphertext) < 128)
            {
                // Decrypt
                cipheredWithPrivate = ClassLib4Net.Encrypt.RSAHelper.RsaDecryptWithPrivate(encryptedWithPublic, privateKey);

                // 不常见的
                cipheredWithPublic = ClassLib4Net.Encrypt.RSAHelper.RsaDecryptWithPublic(encryptedWithPrivate, publicKey);

                Console.WriteLine($" cipheredWithPublic={cipheredWithPublic}\r\n cipheredWithPrivate={cipheredWithPrivate}");
            }
            else
            {
                // Decrypt
                cipheredWithPrivate = ClassLib4Net.Encrypt.RSAHelper.RsaDecryptWithPrivateForLongText(encryptedWithPublic, privateKey);

                // 不常见的
                cipheredWithPublic = ClassLib4Net.Encrypt.RSAHelper.RsaDecryptWithPublicForLongText(encryptedWithPrivate, publicKey);

                Console.WriteLine($" cipheredWithPublic={cipheredWithPublic}\r\n cipheredWithPrivate={cipheredWithPrivate}");
            }

            Console.Read();
        }

        public static void verifyEqual()
        {
            // Encrypt it
            if(string.Compare(plaintext, cipheredWithPublic) == 0)
                Console.WriteLine($" plaintext == cipheredWithPublic > Yes，解密成功");
            else
                Console.WriteLine($" plaintext == cipheredWithPublic > No，解密失败");

            if(string.Compare(plaintext, cipheredWithPrivate) == 0)
                Console.WriteLine($" plaintext == cipheredWithPrivate > Yes，解密成功");
            else
                Console.WriteLine($" plaintext == cipheredWithPrivate > No，解密失败");


            // Decrypt
            if(string.Compare(ciphertext, encryptedWithPublic) == 0)
                Console.WriteLine($" ciphertext == encryptedWithPublic > Yes，加密匹配");
            else
                Console.WriteLine($" ciphertext == encryptedWithPublic > No，加密不匹配，长字节时可忽略");

            if(string.Compare(ciphertext, encryptedWithPrivate) == 0)
                Console.WriteLine($" ciphertext == encryptedWithPrivate > Yes，加密匹配");
            else
                Console.WriteLine($" ciphertext == encryptedWithPrivate > No，加密不匹配，长字节时可忽略");
                      
            
            Console.Read();
        }


    }
}
