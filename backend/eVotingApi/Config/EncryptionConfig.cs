using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace eVotingApi.Config
{
    public class EncryptionConfig<T>
    {

        public async Task<string> EncryptPayload(string data, string voterId)
        {
            string folderName = Path.Combine("wwwroot", "certs");
            string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), $"{folderName}/{voterId}_publickey.pem");

            using(var rsa = RSA.Create())
            {
                using (StreamReader sr = new StreamReader(pathToFile))
                {
                    var certText = await sr.ReadToEndAsync();
                    rsa.ImportFromPem(certText);
                    var bytes = Encoding.UTF8.GetBytes(data);
                    //Debug.Write("These are the bytes: ------------------------------------ " + bytes.ToString());
                    var encrypted = Convert.ToBase64String(rsa.Encrypt(bytes, RSAEncryptionPadding.OaepSHA1));
                    return encrypted;
                }
            }
        }

        public async Task<T> DecryptPayload(string payload)
        {
            var cert = await GetCertificateAsync();
            var rsa = cert.GetRSAPrivateKey();
            var decryptedMessageJson = Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(payload), RSAEncryptionPadding.OaepSHA1));
            return JsonConvert.DeserializeObject<T>(decryptedMessageJson);
        }

        private async Task<X509Certificate2> GetCertificateAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var secret = await keyVaultClient.GetSecretAsync("https://evoting-keys.vault.azure.net/secrets/certificate-base64/d471eebb69b94aae967136e6b2d37b82").ConfigureAwait(false);
            var pfxBase64 = secret.Value;
            var bytes = Convert.FromBase64String(pfxBase64);
            var coll = new X509Certificate2Collection();
            coll.Import(bytes, "QEk5$s2PrRcZrj4xHb", X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            return coll[0];
        }
    }
}
