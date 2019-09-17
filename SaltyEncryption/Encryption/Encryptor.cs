using SaltyEncryption.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SaltyEncryption.Encryption
{
    public class Encryptor
    {
        public string Encrypt(string data, string pass)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string data, string pass)
        {
            throw new NotImplementedException();
        }

        private byte[] Encrypt(byte[] toEncrypt, byte[] pwd, KeySize keySize, BlockSize blockSize)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged rm = new RijndaelManaged())
                {
                    rm.KeySize = (int)keySize;
                    rm.BlockSize = (int)blockSize;

                    var key = new Rfc2898DeriveBytes(pwd, GetRandomBytes(), 1000);
                    rm.Key = key.GetBytes(rm.KeySize / 8);
                    rm.IV = key.GetBytes(rm.BlockSize / 8);
                }
            }

            throw new NotImplementedException();
        }

        private byte[] Decrypt(byte[] toDecrypt, byte[] pwd)
        {
            throw new NotImplementedException();
        }

        private byte[] Salting(string data, int length = 8)
        {
            byte[] bData = Encoding.UTF8.GetBytes(data);
            byte[] saltArray = GetRandomBytes(length);
            byte[] bEncrypted = new byte[bData.Length + saltArray.Length];
            SaltBytes(bData, saltArray, ref bEncrypted);
            return bEncrypted;
        }

        private byte[] GetRandomBytes(int length = 8)
        {
            byte[] saltArray = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(saltArray);
            return saltArray;
        }

        private void SaltBytes(byte[] data, byte[] salt, ref byte[] salted)
        {
            for (int i = 0; i < salt.Length; i++)
            {
                salted[i] = salt[i];
            }
            for (int i = 0; i < data.Length; i++)
            {
                salted[i + salt.Length] = data[i];
            }
        }
    }
}
