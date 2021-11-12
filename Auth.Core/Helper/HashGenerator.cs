using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
    public interface IHashGenerator
    {
        string CreateSha256Hash<T>(T data, string salt);
    }
    public class HashGenerator: IHashGenerator
    {
        public string CreateSha256Hash<T>(T data, string salt)
        {
            var tokenDetailsJsonString = JsonSerializer.Serialize(data) + salt;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(tokenDetailsJsonString));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
