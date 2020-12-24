using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models
{
    public class License
    {
        EncryptDecrypt.EncryptDecrypt ed = new EncryptDecrypt.EncryptDecrypt();
        internal string readLicenseFile(string LicenseFile)
        {

            string sLine = "";
            sLine = ed.DecryptText(System.IO.File.ReadAllText(LicenseFile));
            return sLine;
        }

        internal string getKeyValue(string keyDataValue, string SearchKey, string SearchSubKey)
        {
            try
            {
                string sLine = "";
                string keyValue = "";
                string[] dataSplit;
                string[] keySplit;
                int LoopCount;
                Boolean SearchFlag = false;

                if (sLine != null && sLine != "0")
                {
                    dataSplit = keyDataValue.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    for (LoopCount = 0; LoopCount <= dataSplit.Length; LoopCount++)
                    {
                        if (dataSplit[LoopCount].Contains(SearchKey) || SearchFlag == true)
                        {
                            SearchFlag = true;
                            if (dataSplit[LoopCount].Contains(SearchSubKey))
                            {
                                keySplit = dataSplit[LoopCount].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                keyValue = keySplit[1].ToString();
                                SearchFlag = false;
                                break;
                            }
                        }
                    }
                    return keyValue;
                }
                else
                { return ""; }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal Boolean CompareServerKey(string ServerKey, string LicenseKey)
        {
            try
            {
                if (ServerKey.ToString() == LicenseKey.ToString())
                { return true; }
                else
                { return false; }
            }
            catch
            { return false; }
        }
        internal string DecryptServerKey(string ServerKey)
        {
            string DecryptKey = "";
            string TempKeyValue = "";

            Int32 LoopCount = 0;

            for (LoopCount = 0; LoopCount <= ServerKey.Length - 1; LoopCount++)
            {
                switch (ServerKey.Substring(LoopCount, 1))
                {
                    case "9":
                        TempKeyValue = "0";
                        break;

                    case "8":
                        TempKeyValue = "1";
                        break;

                    case "7":
                        TempKeyValue = "2";
                        break;

                    case "6":
                        TempKeyValue = "3";
                        break;

                    case "5":
                        TempKeyValue = "4";
                        break;

                    case "4":
                        TempKeyValue = "5";
                        break;

                    case "3":
                        TempKeyValue = "6";
                        break;

                    case "2":
                        TempKeyValue = "7";
                        break;

                    case "1":
                        TempKeyValue = "8";
                        break;

                    case "0":
                        TempKeyValue = "9";
                        break;

                    case "f":
                        TempKeyValue = "a";
                        break;

                    case "F":
                        TempKeyValue = "A";
                        break;

                    case "e":
                        TempKeyValue = "b";
                        break;

                    case "E":
                        TempKeyValue = "B";
                        break;

                    case "d":
                        TempKeyValue = "c";
                        break;

                    case "D":
                        TempKeyValue = "C";
                        break;

                    case "c":
                        TempKeyValue = "d";
                        break;

                    case "C":
                        TempKeyValue = "D";
                        break;

                    case "b":
                        TempKeyValue = "e";
                        break;

                    case "B":
                        TempKeyValue = "E";
                        break;

                    case "a":
                        TempKeyValue = "f";
                        break;

                    case "A":
                        TempKeyValue = "F";
                        break;

                    default:
                        break;
                }
                DecryptKey = DecryptKey + TempKeyValue;
            }
            return DecryptKey;
        }

        internal Boolean ValidateServerKey(string LicenseKey)
        {
            string getMachineID = getMACID();
            string EncryptMacID = EncryptServerKey(getMachineID.ToString());
            string DecryptServerID = DecryptServerKey(LicenseKey);
            return CompareServerKey(EncryptMacID.ToString(), LicenseKey);
            //return true;
        }
        internal string getMACID()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            PhysicalAddress MachineID = nics[0].GetPhysicalAddress();
            return MachineID.ToString();
        }
        internal string EncryptServerKey(string ServerKey)
        {
            string EncryptKey = "";
            string TempKeyValue = "";
            Int32 LoopCount = 0;

            for (LoopCount = 0; LoopCount <= ServerKey.Length - 1; LoopCount++)
            {
                switch (ServerKey.Substring(LoopCount, 1))
                {
                    case "0":
                        TempKeyValue = "9";
                        break;

                    case "1":
                        TempKeyValue = "8";
                        break;

                    case "2":
                        TempKeyValue = "7";
                        break;

                    case "3":
                        TempKeyValue = "6";
                        break;

                    case "4":
                        TempKeyValue = "5";
                        break;

                    case "5":
                        TempKeyValue = "4";
                        break;

                    case "6":
                        TempKeyValue = "3";
                        break;

                    case "7":
                        TempKeyValue = "2";
                        break;

                    case "8":
                        TempKeyValue = "1";
                        break;

                    case "9":
                        TempKeyValue = "0";
                        break;

                    case "a":
                        TempKeyValue = "f";
                        break;

                    case "A":
                        TempKeyValue = "F";
                        break;

                    case "b":
                        TempKeyValue = "e";
                        break;

                    case "B":
                        TempKeyValue = "E";
                        break;

                    case "c":
                        TempKeyValue = "d";
                        break;

                    case "C":
                        TempKeyValue = "D";
                        break;

                    case "d":
                        TempKeyValue = "c";
                        break;

                    case "D":
                        TempKeyValue = "C";
                        break;

                    case "e":
                        TempKeyValue = "b";
                        break;

                    case "E":
                        TempKeyValue = "B";
                        break;

                    case "f":
                        TempKeyValue = "a";
                        break;

                    case "F":
                        TempKeyValue = "A";
                        break;

                    default:
                        break;
                }
                EncryptKey = EncryptKey + TempKeyValue;
            }
            return EncryptKey;
        }
    }
}
