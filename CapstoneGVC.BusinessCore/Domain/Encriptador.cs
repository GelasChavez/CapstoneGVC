using CapstoneGVC.Contracts.Constant;
using CapstoneGVC.Contracts.DataTransferObjects;
using CapstoneGVC.Contracts.DomainServices;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CapstoneGVC.BusinessCore.Domain
{
    public class Encriptador : IEncriptador
    {
        /// <summary>
        /// Metodo que cifra en AES256 la cadena que se pasa como parametro
        /// </summary>
        /// <param name="cadena">Objeto que contiene la cadena la cual se va a encriptar</param>
        /// <returns>Regresa un ojeto EncriptadorDto con la cadena sin encriptar y su respectivo cifrado</returns>
        public EncriptadorDto EncriptarCadena(EncriptadorDto cadena)
        {
            string original = cadena.CadenaSinEncriptar;

            using (Aes myAes = Aes.Create())
            {
                myAes.Mode = CipherMode.CBC;
                myAes.KeySize = 256;
                myAes.Padding = PaddingMode.PKCS7;
                myAes.BlockSize = 128;
                myAes.FeedbackSize = 128;

                string str = AesConstants.PASS_ENCRYPT_AES;
                byte[] key = StringToByteArray(str);
                myAes.Key = key;

                byte[] encrypted = EncriptarCadenaToBytesAES(original, myAes.Key, myAes.IV);

                byte[] resultado = new byte[encrypted.Length + myAes.IV.Length];
                Array.Copy(myAes.IV, 0, resultado, 0, myAes.IV.Length);
                Array.Copy(encrypted, 0, resultado, myAes.IV.Length, encrypted.Length);

                EncriptadorDto cadenaEncriptada = new EncriptadorDto()
                {
                    CadenaSinEncriptar = cadena.CadenaSinEncriptar,
                    CadenaEncriptada = encrypted,
                    CadenaEncriptadaString = Convert.ToBase64String(resultado)
                };
                
                return cadenaEncriptada;
            }
        }

        /// <summary>
        /// Metodo que descifra una cadena de bytes en formato AES256
        /// </summary>
        /// <param name="cadena">Objeto que contiene la cadena encriptada en bytes AES256</param>
        /// <returns>Regresa un ojeto EncriptadorDto con la cadena encriptada y su respectivo valor sin encriptar</returns>
        public EncriptadorDto DesencriptarCadena(EncriptadorDto cadena)
        {
            using (Aes myAes = Aes.Create())
            {
                myAes.Mode = CipherMode.CBC;
                myAes.KeySize = 256;
                myAes.Padding = PaddingMode.PKCS7;
                myAes.BlockSize = 128;
                myAes.FeedbackSize = 128;
                String result = "";

                string str = AesConstants.PASS_ENCRYPT_AES;
                byte[] key = StringToByteArray(str);

                var base64EncodedBytes = System.Convert.FromBase64String(cadena.CadenaEncriptadaString);
                byte[] IVAES128 = new byte[16];
                Array.Copy(base64EncodedBytes, 0, IVAES128, 0, 16);
                myAes.IV = IVAES128;

                base64EncodedBytes = System.Convert.FromBase64String(cadena.CadenaEncriptadaString);
                byte[] xmlByte = new byte[base64EncodedBytes.Length - 16];
                Array.Copy(base64EncodedBytes, 16, xmlByte, 0, base64EncodedBytes.Length - 16);
                myAes.Key = key;

                result = DesencriptarCadenaDesdeBytesAES(xmlByte, myAes.Key, myAes.IV);

                EncriptadorDto cadenaSinEncriptar = new EncriptadorDto()
                {
                    CadenaEncriptadaString = cadena.CadenaEncriptadaString,
                    CadenaEncriptada = cadena.CadenaEncriptada,
                    CadenaSinEncriptar = result
                };

                return cadenaSinEncriptar;
            }
        }

        /// <summary>
        /// Metodo que obtiene el hash en SHA256 de la cadena que se pasa como parametro
        /// </summary>
        /// <param name="cadena">Objeto que contiene la cadena de la cual se obtendra el hash</param>
        /// <returns>Regresa un ojeto EncriptadorDto con la cadena de la que se obtuvo el hash y su respectivo hash</returns>
        public EncriptadorDto ObtenerHash(EncriptadorDto cadena)
        {
            EncriptadorDto cadenaConHash = new EncriptadorDto();

            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(cadena.CadenaSinEncriptar);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            cadenaConHash.CadenaSinEncriptar = cadena.CadenaSinEncriptar;
            cadenaConHash.Hash = output.ToString();

            return cadenaConHash;
        }


        private byte[] EncriptarCadenaToBytesAES(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 256;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.BlockSize = 128;
                aesAlg.FeedbackSize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        private string DesencriptarCadenaDesdeBytesAES(byte[] cipherText, byte[] Key, byte[] IV)
        {

            string plaintext = "";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 256;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.BlockSize = 128;
                aesAlg.FeedbackSize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }

        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }
}
