/**
* 命名空间: ClassLib4Net.Encrypt
*
* 功 能： N/A
* 类 名： RSAHelper
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2022/7/24 12:20:22 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2022 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.IO;
using System.Text;

namespace ClassLib4Net.Encrypt
{
    /// <summary>
    /// RSA非对称加密/解密。
    /// C＃BouncyCastle - 使用公钥/私钥进行RSA加密。
    /// 注意：RSA加密明文最大长度117字节，解密要求密文最大长度为128字节，所以在加密和解密的过程中需要分块进行。RSA加密对明文的长度是有限制的，如果加密数据过大会抛出异常。
    /// </summary>
    public class RSAHelper
    {

        #region Pkcs1Encoding RSA加密/解密（默认标准有字节长度限制）

        /// <summary>
        /// Pkcs1格式 Rsa 使用公钥加密（常见的）
        /// </summary>
        /// <param name="clearText">明文(待加密文本)，不能超过117字节</param>
        /// <param name="publicKey">公钥格式：-----BEGIN PUBLIC KEY-----  Base64 string omitted  -----END PUBLIC KEY-----（注意保留换行符） </param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>加密结果</returns>
        public static string RsaEncryptWithPublic(string clearText, string publicKey, string charset = "utf-8")
        {
            var bytesToEncrypt = Encoding.GetEncoding(charset).GetBytes(clearText);
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
                encryptEngine.Init(true, keyParameter);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;
        }

        /// <summary>
        /// Pkcs1格式 Rsa 使用私钥加密（不常见的）
        /// </summary>
        /// <param name="clearText">明文(待加密文本)，不能超过117字节</param>
        /// <param name="privateKey">私钥格式：-----BEGIN PRIVATE KEY-----  Base64 string omitted -----END PRIVATE KEY-----（注意保留换行符）</param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>加密结果</returns>
        public static string RsaEncryptWithPrivate(string clearText, string privateKey, string charset = "utf-8")
        {
            var bytesToEncrypt = Encoding.GetEncoding(charset).GetBytes(clearText);
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(privateKey))
            {
                //var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();
                var keyPair = (RsaPrivateCrtKeyParameters)new PemReader(txtreader).ReadObject();
                //encryptEngine.Init(true,  keyPair.Private);
                encryptEngine.Init(true, keyPair);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;
        }


        // Decryption:

        /// <summary>
        /// Pkcs1格式 Rsa 使用私钥解密（常见的）
        /// </summary>
        /// <param name="base64Input">待解密内容（经过公钥加密的）（常见的），不能超过128字节</param>
        /// <param name="privateKey">私钥格式：-----BEGIN PRIVATE KEY-----  Base64 string omitted -----END PRIVATE KEY-----（注意保留换行符）</param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>解密结果</returns>
        public static string RsaDecryptWithPrivate(string base64Input, string privateKey, string charset = "utf-8")
        {
            var bytesToDecrypt = Convert.FromBase64String(base64Input);
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(privateKey))
            {
                //var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();
                var keyPair = (RsaPrivateCrtKeyParameters)new PemReader(txtreader).ReadObject();

                //decryptEngine.Init(false, keyPair.Private);
                decryptEngine.Init(false, keyPair);
            }

            var decrypted = Encoding.GetEncoding(charset).GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
            return decrypted;
        }

        /// <summary>
        /// Pkcs1格式 Rsa 使用公钥解密（不常见的）
        /// </summary>
        /// <param name="base64Input">待解密内容（经过私钥加密的）（不常见的），不能超过128字节</param>
        /// <param name="publicKey">公钥格式：-----BEGIN PUBLIC KEY-----  Base64 string omitted  -----END PUBLIC KEY-----（注意保留换行符） </param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>解密结果</returns>
        public static string RsaDecryptWithPublic(string base64Input, string publicKey, string charset = "utf-8")
        {
            var bytesToDecrypt = Convert.FromBase64String(base64Input);
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, keyParameter);
            }

            var decrypted = Encoding.GetEncoding(charset).GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
            return decrypted;
        }

        #endregion


        #region Pkcs1Encoding RSA加密/解密（支持超大长度分段加密，不限制字节长度）

        /* 注意：
RSA加密明文最大长度117字节，解密要求密文最大长度为128字节，所以在加密和解密的过程中需要分块进行。
RSA加密对明文的长度是有限制的，如果加密数据过大会抛出异常。 */
        /// <summary>
        /// 最大加密长度
        /// </summary>
        private const int MAX_ENCRYPT_BLOCK = 117;
        /// <summary>
        /// 最大解密长度
        /// </summary>
        private const int MAX_DECRYPT_BLOCK = 128;

        /// <summary>
        /// Pkcs1格式 Rsa 使用公钥加密（常见的）
        /// </summary>
        /// <param name="clearText">明文(待加密文本)，分段加密理论上不限字节长度</param>
        /// <param name="publicKey">公钥格式：-----BEGIN PUBLIC KEY-----  Base64 string omitted  -----END PUBLIC KEY-----（注意保留换行符） </param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>加密结果</returns>
        public static string RsaEncryptWithPublicForLongText(string clearText, string publicKey, string charset = "utf-8")
        {
            var bytesToEncrypt = Encoding.GetEncoding(charset).GetBytes(clearText);
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
                encryptEngine.Init(true, keyParameter);
            }

            //var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length)); // Data must not be longer than 117 bytes
            //return encrypted;

            byte[] cache;
            int time = 0;//次数
            int inputLen = bytesToEncrypt.Length;
            int offSet = 0;

            using(MemoryStream outStream = new MemoryStream())
            {
                while(inputLen - offSet > 0)
                {
                    if(inputLen - offSet > MAX_ENCRYPT_BLOCK)
                    {
                        cache = encryptEngine.ProcessBlock(bytesToEncrypt, offSet, MAX_ENCRYPT_BLOCK);
                    }
                    else
                    {
                        cache = encryptEngine.ProcessBlock(bytesToEncrypt, offSet, inputLen - offSet);
                    }
                    //写入
                    outStream.Write(cache, 0, cache.Length);

                    time++;
                    offSet = time * MAX_ENCRYPT_BLOCK;
                }

                byte[] resData = outStream.ToArray();

                string strBase64 = Convert.ToBase64String(resData);
                return strBase64;
            }

        }

        /// <summary>
        /// Pkcs1格式 Rsa 使用私钥加密（不常见的）
        /// </summary>
        /// <param name="clearText">明文(待加密文本)，分段加密理论上不限字节长度</param>
        /// <param name="privateKey">私钥格式：-----BEGIN PRIVATE KEY-----  Base64 string omitted -----END PRIVATE KEY-----（注意保留换行符）</param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>加密结果</returns>
        public static string RsaEncryptWithPrivateForLongText(string clearText, string privateKey, string charset = "utf-8")
        {
            var bytesToEncrypt = Encoding.GetEncoding(charset).GetBytes(clearText);
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(privateKey))
            {
                //var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();
                var keyPair = (RsaPrivateCrtKeyParameters)new PemReader(txtreader).ReadObject();
                //encryptEngine.Init(true,  keyPair.Private);
                encryptEngine.Init(true, keyPair);
            }

            //var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length)); // Data must not be longer than 117 bytes
            //return encrypted;

            byte[] cache;
            int time = 0;//次数
            int inputLen = bytesToEncrypt.Length;
            int offSet = 0;

            using(MemoryStream outStream = new MemoryStream())
            {
                while(inputLen - offSet > 0)
                {
                    if(inputLen - offSet > MAX_ENCRYPT_BLOCK)
                    {
                        cache = encryptEngine.ProcessBlock(bytesToEncrypt, offSet, MAX_ENCRYPT_BLOCK);
                    }
                    else
                    {
                        cache = encryptEngine.ProcessBlock(bytesToEncrypt, offSet, inputLen - offSet);
                    }
                    //写入
                    outStream.Write(cache, 0, cache.Length);

                    time++;
                    offSet = time * MAX_ENCRYPT_BLOCK;
                }

                byte[] resData = outStream.ToArray();

                string strBase64 = Convert.ToBase64String(resData);
                return strBase64;
            }

        }


        // Decryption:

        /// <summary>
        /// Pkcs1格式 Rsa 使用私钥解密（常见的）
        /// </summary>
        /// <param name="base64Input">待解密内容（经过公钥加密的）（常见的），分段加密理论上不限字节长度</param>
        /// <param name="privateKey">私钥格式：-----BEGIN PRIVATE KEY-----  Base64 string omitted -----END PRIVATE KEY-----（注意保留换行符）</param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>解密结果</returns>
        public static string RsaDecryptWithPrivateForLongText(string base64Input, string privateKey, string charset = "utf-8")
        {
            var bytesToDecrypt = Convert.FromBase64String(base64Input);
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(privateKey))
            {
                //var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();
                var keyPair = (RsaPrivateCrtKeyParameters)new PemReader(txtreader).ReadObject();

                //decryptEngine.Init(false, keyPair.Private);
                decryptEngine.Init(false, keyPair);
            }

            //var decrypted = Encoding.GetEncoding(charset).GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));  // Data must not be longer than 128 bytes
            //return decrypted;

            byte[] cache;
            int time = 0;//次数
            int inputLen = bytesToDecrypt.Length;
            int offSet = 0;
            using(MemoryStream outStream = new MemoryStream())
            {
                while(inputLen - offSet > 0)
                {
                    if(inputLen - offSet > MAX_DECRYPT_BLOCK)
                    {
                        cache = decryptEngine.ProcessBlock(bytesToDecrypt, offSet, MAX_DECRYPT_BLOCK);
                    }
                    else
                    {
                        cache = decryptEngine.ProcessBlock(bytesToDecrypt, offSet, inputLen - offSet);
                    }
                    //写入
                    outStream.Write(cache, 0, cache.Length);

                    time++;
                    offSet = time * MAX_DECRYPT_BLOCK;
                }
                byte[] resData = outStream.ToArray();

                string strDec = Encoding.GetEncoding(charset).GetString(resData);
                return strDec;
            }
        }

        /// <summary>
        /// Pkcs1格式 Rsa 使用公钥解密（不常见的）
        /// </summary>
        /// <param name="base64Input">待解密内容（经过私钥加密的）（不常见的），分段加密理论上不限字节长度</param>
        /// <param name="publicKey">公钥格式：-----BEGIN PUBLIC KEY-----  Base64 string omitted  -----END PUBLIC KEY-----（注意保留换行符） </param>
        /// <param name="charset">编码字符集(默认utf-8)</param>
        /// <returns>解密结果</returns>
        public static string RsaDecryptWithPublicForLongText(string base64Input, string publicKey, string charset = "utf-8")
        {
            var bytesToDecrypt = Convert.FromBase64String(base64Input);
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            using(var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, keyParameter);
            }

            //var decrypted = Encoding.GetEncoding(charset).GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));  // Data must not be longer than 128 bytes
            //return decrypted;

            byte[] cache;
            int time = 0;//次数
            int inputLen = bytesToDecrypt.Length;
            int offSet = 0;
            using(MemoryStream outStream = new MemoryStream())
            {
                while(inputLen - offSet > 0)
                {
                    if(inputLen - offSet > MAX_DECRYPT_BLOCK)
                    {
                        cache = decryptEngine.ProcessBlock(bytesToDecrypt, offSet, MAX_DECRYPT_BLOCK);
                    }
                    else
                    {
                        cache = decryptEngine.ProcessBlock(bytesToDecrypt, offSet, inputLen - offSet);
                    }
                    //写入
                    outStream.Write(cache, 0, cache.Length);

                    time++;
                    offSet = time * MAX_DECRYPT_BLOCK;
                }
                byte[] resData = outStream.ToArray();

                string strDec = Encoding.GetEncoding(charset).GetString(resData);
                return strDec;
            }
        }

        #endregion


    }
}
