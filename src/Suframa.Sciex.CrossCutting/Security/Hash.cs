using System;
using System.Security.Cryptography;
using System.Text;

namespace Suframa.Sciex.CrossCutting.Security
{
    public static class Hash
    {
        public static string GerarMD5(string text)
        {
            MD5 md5hasher = MD5.Create();
            StringBuilder geraString = new StringBuilder();
            byte[] vetor = Encoding.Default.GetBytes(text);
            vetor = md5hasher.ComputeHash(vetor);
            foreach (byte item in vetor)
            {
                geraString.Append(item.ToString("x2"));
            }

            return geraString.ToString();
        }
    }
}