using Business.CommonBusiness;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SyncLDAP
{
    public class LdapHelper
    {
        private static readonly ILog logger =
           LogManager.GetLogger(typeof(LdapHelper));
        private string domain;
        private string username;
        private string password;
        public LdapHelper()
        {
            string LdapConnection = ConfigurationManager.AppSettings["LdapConnection"];
            string str = Decrypt(LdapConnection);
            string[] arr = str.Split('|');

            domain = arr[0];
            username = arr[1];
            password = arr[2];
        }

        public string Decrypt(string TextToBeDecrypted)
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

        public DirectoryEntry createDirectoryEntry()
        {
            DirectoryEntry ldapConnection = new DirectoryEntry("LDAP://" + domain, username, password);

            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
            return ldapConnection;
        }


        public bool CreateUser(string username, string password, DirectoryEntry myLdapConnection, bool enabled)
        {
            DirectoryEntry user;

            DirectorySearcher search = new DirectorySearcher(myLdapConnection);
            search.Filter = "(cn=" + username + ")";
            SearchResult result = search.FindOne();

            if (result != null)
            {
                throw new BusinessException("Tạo tài khoản không thành công. Tài khoản " + username + " đã tồn tại.");
            }

            try
            {
                logger.Info("Create user for: " + username);
                String[] groups = { };
                // create new user object and write into AD  

                user = myLdapConnection.Children.Add(
                                      "CN=" + username + ",OU=QUANLY-VPN", "user");
                logger.Info("Added directory " + username);

                // User name (domain based)   
                user.Properties["userprincipalname"].Add(username + "@" + domain);

                // User name (older systems)  
                user.Properties["samaccountname"].Add(username);

                // Forename  
                user.Properties["givenname"].Add(username);

                // Display name  
                user.Properties["displayname"].Add(username);

                // Description  
                user.Properties["description"].Add("Auto sync");
                user.Properties["msNPAllowDialin"].Value = true;

                // E-mail  
                user.Properties["mail"].Add(username + "@" + domain);

                logger.Info("Added properties " + username);
                user.CommitChanges();
            }
            catch(Exception ex)
            {
                logger.Error("Exists - " + ex.Message);
                throw new BusinessException("Không tạo được tài khoản VPN. Vui lòng liên hệ theo số hotline để được hỗ trợ");
            }
            try
            {
                logger.Info("Commited " + username);

                // set user's password  

                user.Invoke("SetPassword", password);
                logger.Info("Set password " + username);

                if (enabled)
                {
                    user.Invoke("Put", new object[] { "userAccountControl", "512" });
                    logger.Info("Enabled account " + username);
                }


                user.CommitChanges();
                logger.Info("Commited");

                return true;
            }
            catch (Exception ex)
            {
                DirectorySearcher search2 = new DirectorySearcher(myLdapConnection);
                search2.Filter = "(cn=" + username + ")";
                SearchResult result2 = search2.FindOne();
                if (result2 != null)
                {
                    result2.GetDirectoryEntry().DeleteTree();
                }

                logger.Error(ex.Message);
                throw new BusinessException("Tạo tài khoản không thành công. Mật khẩu không đủ mạnh. Mật khẩu phải chứa cả chữ, số, ký tự đặc biệt và không chứa tên đăng nhập");
            }
        }

        public bool UpdateUser(string username, string password, bool enabled, DirectoryEntry ldapConnection)
        {
            try
            {
                logger.Info("Update user " + username);
                if (ldapConnection != null)
                {
                    DirectorySearcher search = new DirectorySearcher(ldapConnection);
                    search.Filter = "(cn=" + username + ")";
                    SearchResult result = search.FindOne();

                    if (result != null)
                    {
                        // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  
                        DirectoryEntry user = result.GetDirectoryEntry();

                        // update
                        // set user's password  

                        user.Invoke("SetPassword", password);
                        logger.Info("Set password " + username);

                        // enable account if requested (see http://support.microsoft.com/kb/305144 for other codes)   

                        if (enabled)
                        {
                            user.Invoke("Put", new object[] { "userAccountControl", "512" });
                            logger.Info("Enabled account " + username);
                        }
                        user.CommitChanges();
                        logger.Info("Commited");
                    }
                    else
                    {
                        throw new BusinessException("Không cập nhật được tài khoản VPN");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                
                throw new BusinessException("Tạo tài khoản không thành công. Mật khẩu không đủ mạnh. Mật khẩu phải chứa cả chữ, số, ký tự đặc biệt và không chứa tên đăng nhập");
            }
        }

        

        public bool DeleteUser(string username, DirectoryEntry ldapConnection)
        {
            try
            {
                logger.Info("Delete user " + username);
                if (ldapConnection != null)
                {
                    DirectorySearcher search = new DirectorySearcher(ldapConnection);
                    search.Filter = "(cn=" + username + ")";
                    SearchResult result = search.FindOne();

                    if (result != null)
                    {
                        // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  
                        DirectoryEntry user = result.GetDirectoryEntry();

                        // update
                        // set user's password  
                        user.DeleteTree();
                        user.CommitChanges();
                        logger.Info("Delete " + username + " Commited");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new BusinessException("Không xóa được tài khoản VPN " + username);
            }
        }
    }
}
