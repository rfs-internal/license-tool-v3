using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace ICSLicenseMaintV2.Utils
{
    public class LicenseUtil
    {
        private readonly string ServerPrivateKeyXml = "<RSAKeyValue><Modulus>yhYJ8CeBICdbOdEk/Q37cw+iWru0pVMNXDMDy57AgwNuBRDrw7e+Mqh7DAHV+0AbRn8oICcMHOPLXxb9tU+JmyPTZSCQM+kjNmgUq9BQV6jDyCliyeezbcba5Ax2UZus6FZ/U99Gxil6CDBc4tcN7Yhl8ZDRoUCx/j1DKUHWTaU=</Modulus><Exponent>AQAB</Exponent><P>6Hra+pVT5eKzhyji+Kj0HWKzCXwbqz2yHgULN7+bDdlPrIUfE6lRyQX7Sqb7GyoAhPNFZHxJURtn68MtITjO+w==</P><Q>3of+ps3YhAZD8XMmbosDKyxvb3X9vD6OUzcbxmvlwz0DD7LShQDgd1IZHv3LMQ97Dk2b48AKYXWIR0zgbfoz3w==</Q><DP>gTJNYbb9EiOji7iQMoqKZ45DW0EKi2bVBtPcwRWNkOu02HZ+p8mQNvxJA9q6cAUulrQvW0Gq6RUm8qHcAbt1Yw==</DP><DQ>2gFJDIjk8JJixYwVvn4ZYJZrpTpmlaCDNirq3vydXyPKd/qsGvi87qhTS/U+tpV/7IdDjV95y/ikxZUe2R8g6Q==</DQ><InverseQ>g/NkFczB+AyQHkCim5kE0NRJCifYyR1dyozTa0IJIwIBUsd1MCLDEOsHuTgQprvBUD6TLZz5JdVqHX2/bp8XbA==</InverseQ><D>n8R+xOwmjSowWGx+RsaYJmaU4BEIh7A6nssCVChFYQ8EG2M+UjThXSGQbnTBHOuY5MpBCfJ1BB4gOiRuHrssOSCFC9RW4Fd40Cr5f8WEFrnYxanR3ejwNpo7bpJvGdUg6lPX3d2fitzE5/sSshYcUaATVmVcDL2yamwr7dj0en0=</D></RSAKeyValue>";
        private readonly string ClientPublicKeyXml = "<RSAKeyValue><Modulus>2rIlNAcLxeOua6eazWev4VBt1M2mG0u3LBNzZKJNFSNYRW4rxgxtliLqZ/flxeSyOUlEKOt0aDQIxIJKg3u2QHS298myHOQo3nmgyBZ0EFqZMEGb/HARdfnz9a/fR8XXWYAmOzaapT2l80G6SSFomucsghLn9N55Do4KZGuH3kk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private readonly string ClientPrivateKeyXml = "<RSAKeyValue><Modulus>2rIlNAcLxeOua6eazWev4VBt1M2mG0u3LBNzZKJNFSNYRW4rxgxtliLqZ/flxeSyOUlEKOt0aDQIxIJKg3u2QHS298myHOQo3nmgyBZ0EFqZMEGb/HARdfnz9a/fR8XXWYAmOzaapT2l80G6SSFomucsghLn9N55Do4KZGuH3kk=</Modulus><Exponent>AQAB</Exponent><P>70JhN8kCV2Z55xG5OVy6tmyrGcXlAAjzGoohI89u0cWeiAKcQ7iStDnMgkyqFTrN9Gw1ieDFqzsaxqcBsEuPOw==</P><Q>6f9u2Rb98SpeMmbsGdtI4nPuRa6Y0Mp3GzhG/d40JOoIk4sHMlHwjOgVVIlumDMtd6Cx1UgStMFnzIvu+TA4Sw==</Q><DP>1H0DkmO27KBaS1l2QveT60f/fVg/1RQds8hRPliPd0YnUWvgFFTsFZvvgRlNRKWBHD6uHdG+PCC12w+fdE7m2Q==</DP><DQ>Z8EV4nZaZQu3NrwCJjjgKWDkHsubAME1bMFYYQqcrl2DLgCwUTSZ57CkfJZvjlbq6yc6kuphOPqkzsKhVKZ33w==</DQ><InverseQ>FViBfymiGEwmhm6QnWExEDmKIJvkSgaasx49UZsOMM4PcWWmBJpfKnnVDPoFIBPgWqxcQpB8I9npisDsXd59iw==</InverseQ><D>YE4qy+p+aLqGyKmaJfIPJa2BcEDPcR26oBJAsoQ2ZaSW7pxBcoluiLr/dqFX8flv8oItHcfyDyE66y5lGdmUu6VBxihBfo+WNtiJVCWdgBCe9PLKaZw+enf/SlLf0a/Xz1s1Rz5pOk406maj7iJkr25M0JdvBWE9/3aLAlLld/E=</D></RSAKeyValue>";
        private readonly string ServerPublicKeyXml = "<RSAKeyValue><Modulus>yhYJ8CeBICdbOdEk/Q37cw+iWru0pVMNXDMDy57AgwNuBRDrw7e+Mqh7DAHV+0AbRn8oICcMHOPLXxb9tU+JmyPTZSCQM+kjNmgUq9BQV6jDyCliyeezbcba5Ax2UZus6FZ/U99Gxil6CDBc4tcN7Yhl8ZDRoUCx/j1DKUHWTaU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public bool GetCurrentLicense(int licenseID, out string ResponseData, out string ErrorMessage)
        {
            try
            {
                DataSet ds = RetrieveCurrentLicense(licenseID);
                ResponseData = SerializeDataSet(ds);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Caught: " + ex.Message;
                ResponseData = "";
                return false;
            }
            ResponseData = Crypto.EncryptAndSign(ResponseData, ServerPrivateKeyXml, ClientPublicKeyXml);
            ErrorMessage = "Success";
            return true;
        }

        public int GetLicenseIdFromResult(string resultData)
        {
            bool verified = false;
            var temp = LicenseUtil.Crypto.VerifyAndDecrypt(resultData, ServerPublicKeyXml, ClientPrivateKeyXml, ref verified);
            if(verified)
            {
                var ds = DeSerializeDataSet(temp);
                return ds.Tables["license"].Rows[0].Field<int>("licenseid");
            }
            return 0;
        }

        private DataSet RetrieveCurrentLicense(int LicenseId)
        {
            DataSet dataSet = new DataSet();
            using (DataTable dataTable1 = new DataTable("License"))
            {
                using (DataTable dataTable2 = new DataTable("LicensedModules"))
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicenses"].ConnectionString))
                    {
                        using (SqlCommand selectCommand = new SqlCommand("Select licenseid, a.Customerid, CustomerName, a.siteid, SiteName, productid, machineid, installpath, totalusercount, timeout, daysremaining, lastrequestedupdate from Licenses a, customers b, customersites c where Licenseid = @LicenseId and (a.customerid = b.customerid) and (a.siteid = c.siteid and a.customerid = c.customerid)", connection))
                        {
                            SqlParameter sqlParameter = new SqlParameter("@LicenseId", LicenseId);
                            selectCommand.Parameters.Add(sqlParameter);
                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                                sqlDataAdapter.Fill(dataTable1);
                            selectCommand.Parameters.Clear();
                            selectCommand.CommandText = "select a.Moduleid, ModuleName, UserCount, Timeout, DaysRemaining, LastRequestedUpdate from LicensedModules a, ProductModules b where licenseid = @LicenseId and (a.moduleid = b.moduleid and a.productid = b.productid)";
                            selectCommand.Parameters.Add(sqlParameter);
                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                                sqlDataAdapter.Fill(dataTable2);
                        }
                    }
                    dataSet.Tables.Add(dataTable1);
                    dataSet.Tables.Add(dataTable2);
                }
            }
            return dataSet;
        }

        public static string SerializeDataSet(DataSet ds)
        {
            if (ds == null)
                return "";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
            StringBuilder sb = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(sb))
            {
                xmlSerializer.Serialize((TextWriter)stringWriter, (object)ds);
                stringWriter.Close();
            }   
            return sb.ToString();
        }

        public static DataSet DeSerializeDataSet(string xml)
        {
            if (xml.Length <= 0)
                return (DataSet)null;
            DataSet dataSet = null;
            using (StringReader stringReader = new StringReader(xml))
            {
                dataSet = (DataSet)new XmlSerializer(typeof(DataSet)).Deserialize((TextReader)stringReader);
                stringReader.Close();
            }
            return dataSet;
        }

        public class Crypto
        {
            private static byte[] EncryptMessage(string strOriginalMessage, string KeyXml)
            {
                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                try
                {
                    string xmlString = KeyXml;
                    cryptoServiceProvider.FromXmlString(xmlString);
                    char[] chArray = strOriginalMessage.ToCharArray();
                    byte[] rgb = new byte[chArray.Length];
                    for (int index = 0; index < chArray.Length; ++index)
                        rgb[index] = (byte)chArray[index];
                    return cryptoServiceProvider.Encrypt(rgb, false);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (cryptoServiceProvider != null)
                        cryptoServiceProvider.Clear();
                }
                return (byte[])null;
            }

            private static byte[] DecryptMessage(string strEncrypted, string certXml)
            {
                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                try
                {
                    cryptoServiceProvider.FromXmlString(certXml);
                    byte[] rgb = Convert.FromBase64String(strEncrypted);
                    return cryptoServiceProvider.Decrypt(rgb, false);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (cryptoServiceProvider != null)
                        cryptoServiceProvider.Clear();
                }
                return (byte[])null;
            }

            private static byte[] encryptStringToBytes_AES(string plainText, byte[] Key, byte[] IV)
            {
                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("Key");
                MemoryStream memoryStream = (MemoryStream)null;
                CryptoStream cryptoStream = (CryptoStream)null;
                StreamWriter streamWriter = (StreamWriter)null;
                RijndaelManaged rijndaelManaged = (RijndaelManaged)null;
                try
                {
                    rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.Key = Key;
                    rijndaelManaged.IV = IV;
                    ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                    memoryStream = new MemoryStream();
                    cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                    streamWriter = new StreamWriter((Stream)cryptoStream);
                    streamWriter.Write(plainText);
                }
                finally
                {
                    if (streamWriter != null)
                        streamWriter.Close();
                    if (cryptoStream != null)
                        cryptoStream.Close();
                    if (memoryStream != null)
                        memoryStream.Close();
                    if (rijndaelManaged != null)
                        rijndaelManaged.Clear();
                }
                return memoryStream.ToArray();
            }

            private static string decryptStringFromBytes_AES(byte[] cipherText, byte[] Key, byte[] IV)
            {
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("Key");
                MemoryStream memoryStream = (MemoryStream)null;
                CryptoStream cryptoStream = (CryptoStream)null;
                StreamReader streamReader = (StreamReader)null;
                RijndaelManaged rijndaelManaged = (RijndaelManaged)null;
                string str = (string)null;
                try
                {
                    rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.Key = Key;
                    rijndaelManaged.IV = IV;
                    ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                    memoryStream = new MemoryStream(cipherText);
                    cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
                    streamReader = new StreamReader((Stream)cryptoStream);
                    str = streamReader.ReadToEnd();
                }
                finally
                {
                    if (streamReader != null)
                        streamReader.Close();
                    if (cryptoStream != null)
                        cryptoStream.Close();
                    if (memoryStream != null)
                        memoryStream.Close();
                    if (rijndaelManaged != null)
                        rijndaelManaged.Clear();
                }
                return str;
            }

            public static string EncryptAndSign(string strOriginalMessage, string senderXmlKey, string receiverXmlKey)
            {
                try
                {
                    RijndaelManaged rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.GenerateIV();
                    rijndaelManaged.GenerateKey();
                    byte[] key = rijndaelManaged.Key;
                    byte[] iv = rijndaelManaged.IV;
                    byte[] inArray1 = new byte[rijndaelManaged.Key.Length + rijndaelManaged.IV.Length];
                    for (int index = 0; index < rijndaelManaged.Key.Length; ++index)
                        inArray1[index] = key[index];
                    for (int index = 0; index < rijndaelManaged.IV.Length; ++index)
                        inArray1[index + rijndaelManaged.Key.Length] = iv[index];
                    string str1 = string.Format("{0}{1}", (object)Convert.ToBase64String(Crypto.EncryptMessage(Convert.ToBase64String(inArray1), receiverXmlKey)), (object)Convert.ToBase64String(Crypto.encryptStringToBytes_AES(strOriginalMessage, key, iv)));
                    int length1 = str1.Length;
                    RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                    string xmlString = senderXmlKey;
                    cryptoServiceProvider.FromXmlString(xmlString);
                    byte[] hash = new SHA1Managed().ComputeHash(new ASCIIEncoding().GetBytes(strOriginalMessage));
                    byte[] inArray2 = cryptoServiceProvider.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
                    cryptoServiceProvider.Clear();
                    string str2 = Convert.ToBase64String(inArray2);
                    int length2 = str2.Length;
                    return string.Format("{0}{1}", (object)str1, (object)str2);
                }
                catch (Exception ex)
                {
                }
                return (string)null;
            }

            public static string VerifyAndDecrypt(string strOriginalMessage, string senderXml, string receiverXml, ref bool Verified)
            {
                try
                {
                    int length = strOriginalMessage.Length;
                    string s1 = strOriginalMessage.Substring(length - 172);
                    string str = strOriginalMessage.Substring(0, length - 172);
                    string s2 = str.Substring(0, 172);
                    string s3 = str.Substring(172);
                    byte[] numArray1 = Crypto.DecryptMessage(Convert.ToBase64String(Convert.FromBase64String(s2)), receiverXml);
                    char[] inArray = new char[numArray1.Length];
                    for (int index = 0; index < numArray1.Length; ++index)
                        inArray[index] = (char)numArray1[index];
                    byte[] numArray2 = Convert.FromBase64CharArray(inArray, 0, inArray.Length);
                    byte[] Key = new byte[32];
                    byte[] IV = new byte[16];
                    for (int index = 0; index < 32; ++index)
                        Key[index] = numArray2[index];
                    for (int index = 0; index < 16; ++index)
                        IV[index] = numArray2[index + 32];
                    string s4 = Crypto.decryptStringFromBytes_AES(Convert.FromBase64String(s3), Key, IV);
                    RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                    cryptoServiceProvider.FromXmlString(senderXml);
                    byte[] hash = new SHA1Managed().ComputeHash(new ASCIIEncoding().GetBytes(s4));
                    byte[] rgbSignature = Convert.FromBase64String(s1);
                    Verified = cryptoServiceProvider.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), rgbSignature);
                    return s4;
                }
                catch (Exception ex)
                {
                }
                return (string)null;
            }

            public static string EncryptWithNoSignature(string strOriginalMessage)
            {
                try
                {
                    RijndaelManaged rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.GenerateIV();
                    rijndaelManaged.GenerateKey();
                    byte[] key = rijndaelManaged.Key;
                    byte[] iv = rijndaelManaged.IV;
                    byte[] inArray = Crypto.encryptStringToBytes_AES(strOriginalMessage, key, iv);
                    string str = string.Format("{0}{1}{2}", (object)Convert.ToBase64String(iv), (object)Convert.ToBase64String(inArray), (object)Convert.ToBase64String(key));
                    int length = str.Length;
                    return str;
                }
                catch
                {
                }
                return (string)null;
            }

            public static string DecryptWithNoSignature(string strOriginalMessage)
            {
                try
                {
                    int length = strOriginalMessage.Length;
                    return Crypto.decryptStringFromBytes_AES(Convert.FromBase64String(strOriginalMessage.Substring(24, length - 68)), Convert.FromBase64String(strOriginalMessage.Substring(length - 44)), Convert.FromBase64String(strOriginalMessage.Substring(0, 24)));
                }
                catch (Exception ex)
                {
                }
                return (string)null;
            }
        }
    }
}