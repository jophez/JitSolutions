using System;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Microsoft.Win32;
using TEntS.Types.Exception;

namespace TEntS.BLL.Utilities
{
    internal class SecurityToolSet
    {
        // Change these keys 
        private byte[] Key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
        private byte[] Vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 1, 112, 79, 32, 114, 156 };
        static private string ConpanyName = "TheEnterpriseSolutions";

        private ICryptoTransform EncryptorTransform, DecryptorTransform;
        private System.Text.UTF8Encoding UTFEncoder;

        public SecurityToolSet()
        {
            //This is our encryption method 
            RijndaelManaged rm = new RijndaelManaged();

            //Create an encryptor and a decryptor using our encryption method, key, and vector. 
            EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
            DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);

            //Used to translate bytes to text and vice versa 
            UTFEncoder = new System.Text.UTF8Encoding();
        }

        /// -------------- Two Utility Methods (not used but may be useful) ----------- 
        /// Generates an encryption key. 
        static public byte[] GenerateEncryptionKey()
        {
            //Generate a Key. 
            RijndaelManaged rm = new RijndaelManaged();
            rm.GenerateKey();
            return rm.Key;
        }

        /// Generates a unique encryption vector 
        static public byte[] GenerateEncryptionVector()
        {
            //Generate a Vector 
            RijndaelManaged rm = new RijndaelManaged();
            rm.GenerateIV();
            return rm.IV;
        }


        /// ----------- The commonly used methods ------------------------------     
        /// Encrypt some text and return a string suitable for passing in a URL. 
        private string EncryptString(String dataString)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(dataString);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        static public string DecryptString(string passWord)
        {
            System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
            System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string decrypted = "";
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(ConpanyName));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = System.Security.Cryptography.CipherMode.ECB;
                System.Security.Cryptography.ICryptoTransform DESDecrypter = AES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(passWord);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return decrypted;

            }
            catch (Exception ex)
            {
                return ex + "Error";
            }
        }

        //02-12-2014
        static public string EncrypUser(string passWord)
        {
            try
            {
                System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
                System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
                string encrypted = "";
                try
                {
                    byte[] hash = new byte[32];
                    byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(ConpanyName));
                    Array.Copy(temp, 0, hash, 0, 16);
                    Array.Copy(temp, 0, hash, 15, 16);
                    AES.Key = hash;
                    AES.Mode = System.Security.Cryptography.CipherMode.ECB;
                    System.Security.Cryptography.ICryptoTransform DESEncrypter = AES.CreateEncryptor();
                    byte[] Buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(passWord);
                    encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                    return encrypted;
                }
                catch (Exception ex)
                {
                    return ex + "error";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<string> ShowNetworkInterfaces()
        {
            try
            {
                List<string> macAddressList = new List<string>();
                string macAddress = "";
                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                //Console.WriteLine("Interface information for {0}.{1}     ",
                       // computerProperties.HostName, computerProperties.DomainName);
                if (nics == null || nics.Length < 1)
                {
                    //Console.WriteLine("  No network interfaces found.");
                    //return;
                    throw new TEntSInternalException("NO_NETWORK_INTERFACE_FOUND");
                }

                //Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();
                    //Console.WriteLine();
                    //Console.WriteLine(adapter.Description);
                    //Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                    //Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                    //Console.Write("  Physical address ........................ : ");
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        // Display the physical address in hexadecimal.
                        //Console.Write("{0}", bytes[i].ToString("X2"));
                        macAddress += bytes[i].ToString("X2");
                        // Insert a hyphen after each byte, unless we are at the end of the 
                        // address.
                        if (i != bytes.Length - 1)
                        {
                            macAddress += "-";
                            //Console.Write("-");
                        }                       
                    }
                    macAddressList.Add(macAddress);
                    macAddress = "";
                }
                return macAddressList;
            }
            catch (ApplicationException ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }        

        //06-08-10
        public bool RetrieveRegistryValue(string validityDate, string currentDate)
        {
            bool returnvalue = true;
            try
            {
                RegistryKey regKey = CreateRegistryEntry("SOFTWARE\\BusinessSolutions");//Registry.LocalMachine.OpenSubKey("SOFTWARE\\BusinessSolutions", true);

                if (DateTime.Parse(validityDate).Subtract(DateTime.Parse(currentDate)).Days < 0)
                {
                    regKey.OpenSubKey("SOFTWARE\\BusinessSolutions", true);
                    regKey.SetValue("ISVALID", false);
                }

                if (regKey.GetValue("ISVALID") != null)
                    returnvalue = bool.Parse(regKey.GetValue("ISVALID").ToString());
                regKey.Close();
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
            return returnvalue;
        }

        private RegistryKey CreateRegistryEntry(string Path)
        {
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(Path, true);
            try
            {
                if (regKey == null) //Create the Key
                {
                    regKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\BusinessSolutions");
                }
                if (regKey.GetValue("ISVALID") == null)
                    regKey.SetValue("ISVALID", true);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
            return regKey;
        }
    }
}