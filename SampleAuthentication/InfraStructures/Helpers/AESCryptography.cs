using SampleAuthentication.Entites.Dtos;
using SampleAuthentication.Entites.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SampleAuthentication.InfraStructures.Helpers
{
    public class AESCryptography
    {
        private int KeySize;
        private PaddingMode PaddingMode;
        private CipherMode CipherMode;
        private string Key;

        public AESCryptography(CryptographyEnums.AESKeySize keysize, PaddingMode paddingmode = PaddingMode.PKCS7, CipherMode ciphermode = CipherMode.ECB)
        {
            this.KeySize = (int)keysize;

            this.PaddingMode = paddingmode;
            this.CipherMode = ciphermode;

            _ = CreateProvider();
        }

        public AESCryptography(string key, CryptographyEnums.AESKeySize keysize, PaddingMode paddingmode = PaddingMode.PKCS7, CipherMode ciphermode = CipherMode.ECB)
        {
            if (key.Length * 8 != (int)keysize)
                throw new Exception("Key Length And KeySize Is Not Compatible");

            this.KeySize = (int)keysize;
            this.PaddingMode = paddingmode;
            this.CipherMode = ciphermode;
            this.Key = key;

            _ = CreateProvider();
        }

        private AesCryptoServiceProvider CreateProvider()
        {
            return new AesCryptoServiceProvider
            {
                KeySize = this.KeySize,
                BlockSize = 128,
                //Key = key,
                Padding = this.PaddingMode,
                Mode = this.CipherMode
            };
        }
        public async Task<Boolean> EncryptFile(string filePath, string key)
        {
            Boolean Result = false;
            if (key.Length * 8 != this.KeySize)
                throw new Exception("Key Length And KeySize Is Not Compatible");

            return await Task.Run(() =>
            {
                byte[] plainContent = File.ReadAllBytes(filePath);
                using (var AES = CreateProvider())
                {
                    AES.IV = Encoding.UTF8.GetBytes(key);
                    AES.Key = Encoding.UTF8.GetBytes(key);


                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateEncryptor(),
                            CryptoStreamMode.Write);

                        cryptoStream.Write(plainContent, 0, plainContent.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(filePath, memStream.ToArray());
                        Result = true;
                    }
                }
                return Result;
            });

        }

        public async Task<Boolean> DecryptFile(string filePath, string key)
        {
            Boolean Result = false;
            if (key.Length * 8 != this.KeySize)
                throw new Exception("Key Length And KeySize Is Not Compatible");

            return await Task.Run(() =>
            {
                byte[] encrypted = File.ReadAllBytes(filePath);
                using (var AES = CreateProvider())
                {
                    AES.IV = Encoding.UTF8.GetBytes(key);
                    AES.Key = Encoding.UTF8.GetBytes(key);


                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateDecryptor(),
                            CryptoStreamMode.Write);

                        cryptoStream.Write(encrypted, 0, encrypted.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(filePath, memStream.ToArray());
                        Result = true;
                    }
                }
                return Result;
            });

        }

        public string Decrypt(string key, string ciphertext)
        {
            if (key.Length * 8 != this.KeySize)
                throw new Exception("Key Length And KeySize Is Not Compatible");
            try
            {
                byte[] keyy = StringHelper.StringToByteArray(key);
                var DecryptArray = StringHelper.Base64ToByteArray(ciphertext);

                var AESObject = CreateAESProvider(keyy);

                var Decryptor = AESObject.CreateDecryptor();
                var DecByte = Decryptor.TransformFinalBlock(DecryptArray, 0, DecryptArray.Length);
                var result = StringHelper.ByteArrayToString(DecByte);
                return result;
            }
            catch
            {
                return null;
            }

        }

        public string Encrypt(string key, string plaintext)
        {
            if (key.Length * 8 != this.KeySize)
                throw new Exception("Key Length And KeySize Is Not Compatible");


            try
            {
                byte[] AESkey = StringHelper.StringToByteArray(key);
                var EncryptArray = StringHelper.StringToByteArray(plaintext);

                var AESObject = CreateAESProvider(AESkey);
                var Encryptor = AESObject.CreateEncryptor();

                var EncByte = Encryptor.TransformFinalBlock(EncryptArray, 0, EncryptArray.Length);
                var result = StringHelper.GetBase64(EncByte);
                return result;
            }
            catch
            {
                return null;
            }

        }

        public string Decrypt(string ciphertext)
        {
            try
            {
                byte[] keyy = StringHelper.StringToByteArray(Key);
                var DecryptArray = StringHelper.Base64ToByteArray(ciphertext);

                var AESObject = CreateAESProvider(keyy);

                var Decryptor = AESObject.CreateDecryptor();
                var DecByte = Decryptor.TransformFinalBlock(DecryptArray, 0, DecryptArray.Length);
                var result = StringHelper.ByteArrayToString(DecByte);
                return result;
            }
            catch
            {
                return null;
            }

        }

        public string Encrypt(string plaintext)
        {
            try
            {
                byte[] AESkey = StringHelper.StringToByteArray(Key);
                var EncryptArray = StringHelper.StringToByteArray(plaintext);

                var AESObject = CreateAESProvider(AESkey);
                var Encryptor = AESObject.CreateEncryptor();

                var EncByte = Encryptor.TransformFinalBlock(EncryptArray, 0, EncryptArray.Length);
                var result = StringHelper.GetBase64(EncByte);
                return result;
            }
            catch
            {
                return null;
            }

        }

        public async Task<SymmetricCryptographyResult> GenerateKey()
        {
            return await Task.Run(() => {
                Random Rnd = new Random();
                var RndInt = Rnd.Next(10000, 150000000);
                var datestring = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString() + " " + RndInt.ToString() + " " + DateTime.Now.Date.ToString();
                var result = StringHelper.GetBase64(datestring);
                if (result.Length > 32)
                    result = result.Substring(0, 32);
                return new AESKeyData() { Key = result };
            });
        }

        private AesCryptoServiceProvider CreateAESProvider(byte[] key)
        {
            return new AesCryptoServiceProvider
            {
                KeySize = KeySize,
                BlockSize = 128,
                Key = key,
                Padding = PaddingMode,
                Mode = CipherMode
            };
        }


    }
}
