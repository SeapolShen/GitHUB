using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;



namespace CyBLE_MTK_Application
{
    public class StrOperator
    {
        #region AES解密
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion AES加密

        #region AES解密
        public static string Decrypt(string toDecrypt)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception )
            {
                return "";
            }
        }
        #endregion AES解密  
    }
    //public class FstInterface
    //{


    //    //函数定义 int GetTestResult(string barCode);
    //    //输入：string 条码号
    //    /*输出：整数 -1 -- 该条码未测试
    //             0 --测试通过
    //             1...n:错误吗(测试未通过)
    //   */
    //    static public void WriteTestResult(string barCode, int result)
    //    {
    //        SqlConnection Con;
    //        //barCode = "1234567abcdefg";
    //       // result = 1;
    //        string adoCon;
    //        string sql;
    //        string site;

    //        site = ConfigurationManager.AppSettings["Site"];
    //        adoCon = StrOperator.Decrypt(ConfigurationManager.AppSettings["AdoCon"]);
    //        Con = new SqlConnection(adoCon);
    //        Con.Open();
            
    //        sql = "INSERT INTO if_check(if_site,if_barcode,if_result) VALUES (@Site,@BarCode,@Result)";

    //        SqlCommand sqlCmd = new SqlCommand(sql, Con);
    //        sqlCmd.Parameters.AddWithValue("@Site", site);
    //        sqlCmd.Parameters.AddWithValue("@BarCode",barCode);
    //        sqlCmd.Parameters.AddWithValue("@Result", result);
    //        sqlCmd.ExecuteNonQuery();

    //        Con.Close();
    //        return;
    //    }
    //}
}
