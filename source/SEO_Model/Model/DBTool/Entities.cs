using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model.DBTool
{
    public partial class Entities
    {
        public static Dictionary<string, string> CnnStr = new Dictionary<string,string>();

        //public Entities()
        //    : base(GetConnString())
        //{
            
        //    //this.Database.ExecuteSqlCommand("ALTER SESSION SET NLS_COMP = 'LINGUISTIC'");
        //    //this.Database.ExecuteSqlCommand("ALTER SESSION SET NLS_SORT = 'vietnamese'");
        //}

        public Entities(DbConnection conn)
            : base(conn, true)
        {
        }

        public Entities(string EntitiesName) : base(GetConnString(EntitiesName))
        {

        }

        public static string GetConnString(string entitiesName = "Entities")
        {
            if (CnnStr.ContainsKey(entitiesName) && CnnStr[entitiesName] != null) return CnnStr[entitiesName];

            string isEncript = ConfigurationManager.AppSettings["EncryptConnectionString"];
            if (isEncript == "true")
            {
                CnnStr[entitiesName] = Decrypt(ConfigurationManager.ConnectionStrings[entitiesName].ConnectionString);
            }
            else
            {
                CnnStr[entitiesName] = "name=" + entitiesName;
            }

            return CnnStr[entitiesName];
        }

        public static string Decrypt(string TextToBeDecrypted)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            string Password = "disro tnbq37904n t npiesourg";
            byte[] EncryptedData = Convert.FromBase64String(TextToBeDecrypted);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            //Making of the key for decryption
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            //Creates a symmetric Rijndael decryptor object.
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(EncryptedData);
            //Defines the cryptographics stream for decryption.THe stream contains decrpted data
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] PlainText = new byte[EncryptedData.Length];
            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
            memoryStream.Close();
            cryptoStream.Close();


            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            return DecryptedData;
        }
    }
}
