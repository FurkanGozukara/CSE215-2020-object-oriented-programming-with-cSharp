using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace lecture_6
{
    public static class staticMethods
    {
        public static readonly string srSplitValueSeperator = "split".ComputeSha256Hash();

        public static readonly string srSplitListSeperator = "splitList".ComputeSha256Hash();

        public static readonly string srCarObjectSeperator = "srCarObjectSeperator".ComputeSha256Hash();

        //this below would throw error because const is required to be initialized at the begining different than static
        //public const string srRandomSplitKey;

        //this below one works unlike above, because it doesnt have to be assigned to a value at the first definition
        //public static string srRandomSplitKey;

        public const int irConstNum = 100;

        //this is the only way to change static readonly
        //static staticMethods ()
        //{
        //    srSplitValueSeperator = "";
        //}

        static string ComputeSha256Hash(this string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //when you put this keyword in front of the parameter class type, that method becomes an extension
        //extensions can be defined only as static
        public static int toInt(this string srValue)
        {
            try
            {
                return Convert.ToInt32(srValue);
            }
            catch (FormatException E)
            {
                return -1;
            }
            catch (OverflowException E)
            {
                return Int32.MaxValue;
            }
            catch
            {
                //catch all other exceptions
                return 0;
            }
        }


        public static List<int> toIntList(this string srValue, string srSplitKey = null)
        {
            if (string.IsNullOrEmpty(srSplitKey))
                srSplitKey = srSplitListSeperator;

            List<int> lstReturnList = new List<int>();

            foreach (var vrPerSplit in srValue.Split(srSplitKey))
            {
                lstReturnList.Add(vrPerSplit.toInt());
            }

            return lstReturnList;
        }

        public class cars
        {
            public static string srCarManufacturer_v3 = "Global";

            public const string srCarManufacturer_v2 = "Global";

            public readonly string srCarManufacturer = "Global";

            //field
            public string srCarBrand = "NA";

            public int irCarPrice { get; set; }

            public string srCarColor = "NA";

            private int _irCarWeight;

            //property
            public int irCarWeight
            {
                get { return _irCarWeight; }   // get method

                set
                {
                    _irCarWeight = value;

                    if (_irCarWeight < 500)
                        _irCarWeight = 500;

                }  // set method
            }

            public string srCarModel { get; set; }

            private int _irProductionYear;

            public int irProductionYear
            {
                get
                {
                    if (_irProductionYear < 1850)
                        return 1850;

                    return _irProductionYear;
                }

                set { _irProductionYear = value; }
            }

            List<int> _lstRepairYears = new List<int>();

            public List<int> lstRepairYears
            {
                get
                {
                    for (int i = 0; i < _lstRepairYears.Count; i++)
                    {
                        if (_lstRepairYears[i] < this.irProductionYear)
                            _lstRepairYears[i] = this.irProductionYear;
                    }

                    return _lstRepairYears;
                }

                set
                {
                    _lstRepairYears = value;
                }
            }

            public void saveToAFile(string srFileName)
            {
                File.WriteAllText(srFileName, returnTextFormatOfObject());
            }

            public string returnTextFormatOfObject()
            {
                StringBuilder srBuilder = new StringBuilder();

                srBuilder.AppendLine("srCarBrand" + srSplitValueSeperator + this.srCarBrand);
                srBuilder.AppendLine("irCarPrice" + srSplitValueSeperator + irCarPrice);
                srBuilder.AppendLine("srCarColor" + srSplitValueSeperator + srCarColor);
                srBuilder.AppendLine("irCarWeight" + srSplitValueSeperator + irCarWeight);
                srBuilder.AppendLine("srCarModel" + srSplitValueSeperator + srCarModel);
                srBuilder.AppendLine("irProductionYear" + srSplitValueSeperator + irProductionYear);
                srBuilder.AppendLine("lstRepairYears" + srSplitValueSeperator + string.Join(srSplitListSeperator, this.lstRepairYears));

                return srBuilder.ToString();
            }

            public void loadFromFile(string srFileName, bool blNoFileName = false)
            {
                List<string> lstAllLines = new List<string>();

                if(blNoFileName==false)
                {
                    lstAllLines = File.ReadAllLines(srFileName).ToList();
                }
                else
                {
                    lstAllLines = srFileName.Split("\r\n").ToList();
                }

                foreach (var vrLine in lstAllLines)
                {
                    var vrKey = vrLine.Split(srSplitValueSeperator);

                    switch (vrKey[0])
                    {
                        case "srCarBrand":
                            this.srCarBrand = vrKey[1];
                            break;
                        case "irCarPrice":
                            this.irCarPrice = vrKey[1].toInt();
                            break;
                        case "srCarColor":
                            this.srCarColor = vrKey[1];
                            break;
                        case "irCarWeight":
                            this.irCarWeight = vrKey[1].toInt();
                            break;
                        case "srCarModel":
                            this.srCarModel = vrKey[1];
                            break;
                        case "irProductionYear":
                            this.irProductionYear = vrKey[1].toInt();
                            break;
                        case "lstRepairYears":
                            this.lstRepairYears = vrKey[1].toIntList();
                            break;
                    }
                }
            }
        }

        public static void saveToFileCarsList(this List<cars> lstMyCars, string srFileName)
        {
            var vrFinalText = string.Join(srCarObjectSeperator, lstMyCars.Select(pr => pr.returnTextFormatOfObject()));

            File.WriteAllText(srFileName, vrFinalText);
        }

        public static void loadCarsFromFile(this List<cars> lstMyCars, string srFileName)
        {
            var vrAllFile = File.ReadAllText(srFileName);

            foreach (var vrPerCarObject in vrAllFile.Split(srCarObjectSeperator))
            {
                cars tempCar = new cars();
                tempCar.loadFromFile(vrPerCarObject, true);
                lstMyCars.Add(tempCar);
            }
        }
    }
}
