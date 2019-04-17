using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using TEntS.BLL.Session;

namespace TEntS.BLL.Utilities
{
    public static class ConvertToString
    {

        public static string path = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\') + 1);
        public static string modifiedPath = path + "App.config";

        public static string GetSection(this string fullFilePath, string section, string key)
        {
            string filePath = fullFilePath;
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            NameValueCollection dataCol = new NameValueCollection();
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = filePath;
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                string xmlString = config.GetSection(section).SectionInformation.GetRawXml();
                xmlDoc.LoadXml(xmlString);

                System.Xml.XmlNode nodeList = xmlDoc.ChildNodes[0];
                foreach (System.Xml.XmlNode node in nodeList)
                    if (node.Attributes[0].Value.ToLower() == key.ToLower())
                        return node.Attributes[1].Value;
            }
            catch { }

            return "";
        }

        public static bool VerifyFileInSection(this string fullFilePath, string section, string key)
        {
            return CheckXmlFile(GetSection(fullFilePath, section, key));
        }

        public static bool CheckXmlFile(string path)
        {
            System.IO.FileInfo fInfo = new System.IO.FileInfo(path);
            return fInfo.Exists ? true : false;
        }

        public static string SpellDecimal(decimal number)
        {
            string[] digit =
      {
            "", "one", "two", "three", "four", "five", "six", 
            "seven", "eight", "nine", "ten", "eleven", "twelve", 
            "thirteen", "fourteen", "fifteen", "sixteen", 
            "seventeen", "eighteen", "nineteen" 
      };

            string[] baseten = 
      {
            "", "", "twenty", "thirty", "fourty", "fifty", 
            "sixty", "seventy", "eighty", "ninety" 
      };

            string[] expo = 
      { 
            "", "thousand", "million", "billion", "trillion",
            "quadrillion", "quintillion"
      };

            if (number == Decimal.Zero)
                return "zero";

            decimal n = Decimal.Truncate(number);
            decimal cents = Decimal.Truncate((number - n) * 100);

            StringBuilder sb = new StringBuilder();
            int thousands = 0;
            decimal power = 1;

            if (n < 0)
            {
                sb.Append("minus ");
                n = -n;
            }

            for (decimal i = n; i >= 1000; i /= 1000)
            {
                power *= 1000;
                thousands++;
            }

            bool sep = false;
            for (decimal i = n; thousands >= 0; i %= power, thousands--, power /= 1000)
            {
                int j = (int)(i / power);
                int k = j % 100;
                int hundreds = j / 100;
                int tens = j % 100 / 10;
                int ones = j % 10;

                if (j == 0)
                    continue;

                if (hundreds > 0)
                {
                    if (sep)
                        sb.Append(", ");

                    sb.Append(digit[hundreds]);
                    sb.Append(" hundred");
                    sep = true;
                }

                if (k != 0)
                {
                    if (sep)
                    {
                        sb.Append(" and ");
                        sep = false;
                    }

                    if (k < 20)
                        sb.Append(digit[k]);
                    else
                    {
                        sb.Append(baseten[tens]);
                        if (ones > 0)
                        {
                            sb.Append("-");
                            sb.Append(digit[ones]);
                        }
                    }
                }

                if (thousands > 0)
                {
                    sb.Append(" ");
                    sb.Append(expo[thousands]);
                    sep = true;
                }
            }

            sb.Append(" and ");
            if (cents < 10) sb.Append("0");
            sb.Append(cents);
            sb.Append("/100");

            return sb.ToString().ToUpper();
        }

    }

    public class Utility
    {
        private static Hashtable m_userHashSession = new Hashtable();
        private static Hashtable m_sessionHashData = new Hashtable();
        private static Dictionary<ulong, TEntS.Types.SessionDetails> m_sessionData = new Dictionary<ulong, Types.SessionDetails>();

        public Utility()
        { }

        public static List<T> Paginate<T>(List<T> source, ref int pageNumber, out int totalPages)
        {
            if (pageNumber == 0)
                pageNumber = 1;

            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            int remainder;
            totalPages = Math.DivRem(source.Count, pageSize, out remainder);
            if (remainder > 0)
                totalPages += 1;

            if (pageNumber > totalPages)
                pageNumber = totalPages;

            return new List<T>(source.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        public static bool ValidateInput(string input)
        {
            bool result = true;
            Regex exp = new Regex("^[a-zA-Z0-9]*$");
            if (input.Trim().Length > 0)
                result = exp.Match(input).Success ? true : false;
            return result;
        }

        //generate a unique session ID
        public static ulong GenerateSessionId()
        {
            string lockString = "";
            lock (lockString)
            {
                ulong sId = 0;
                //generate a random number
                do
                {
                    Random tryNum = new Random(unchecked((int)DateTime.UtcNow.Ticks));
                    sId = (ulong)tryNum.Next();
                } while (sId < 1);
                return sId;
            }
        }

        public static string ConStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public static int LockDuration = int.Parse(ConfigurationManager.AppSettings["LockDuration"].ToString());
        public static int FailedLogCount = int.Parse(ConfigurationManager.AppSettings["MaxFailedLogCount"].ToString());
        public static int TimeOut = int.Parse(ConfigurationManager.AppSettings["TimeOut"].ToString());

        public Dictionary<ulong, TEntS.Types.SessionDetails> SessionData
        {
            get { return m_sessionData; }
            set { m_sessionData = value; }
        }

        public Hashtable _UserHashSessions
        {
            get { return m_userHashSession; }
            set { m_userHashSession = value; }
        }

        public Hashtable _SessionHashData
        {
            get { return m_sessionHashData; }
            set { m_sessionHashData = value; }
        }
    }
}
