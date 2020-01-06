using Inlite.ClearImageNet;
using InsentiveCalculation.Models;
using SQIndustryThree.DataManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.DAL
{
    public class HomeDAL
    {
        private DataAccessManager accessManager = new DataAccessManager();

        public int ReadQrCode(String dateValue)
        {
            DirectoryInfo dir = new DirectoryInfo(@"E:\PdfRead");
            FileInfo[] imageFiles = dir.GetFiles("*.pdf");
            int id = 15000;
            foreach (FileInfo f in imageFiles)
            {
                List<QRReaderValue> qrReadList = new List<QRReaderValue>();
                string plainText = "";
                plainText += f.Name + "\n" + f.Length + "\n" + f.DirectoryName + "\n" + f.Attributes + "\n";
                string inputFilename = f.DirectoryName + '/' + f.Name;
                int workerId = 0;
                try
                {
                    BarcodeReader reader = new BarcodeReader();
                    reader.Pdf417 = true;
                    Inlite.ClearImageNet.Barcode[] barcodes = reader.Read(inputFilename, 1);
                    // Process results
                    foreach (Inlite.ClearImageNet.Barcode bc in barcodes)
                    {
                        String[] smv = bc.Text.ToString().Split('.');
                        
                        if (smv.Length == 3)
                        {
                            workerId= Convert.ToInt32(smv[1]);
                        }
                        else
                        {
                            QRReaderValue qRReaderValue = new QRReaderValue();
                            qRReaderValue.QRNo = Convert.ToInt32(smv[0]);
                            qRReaderValue.StandardMinuteValue = Convert.ToDouble(smv[1]);
                            qrReadList.Add(qRReaderValue);
                        }
                    }   
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                id++;
                InsertIntoQRreaderValue(qrReadList,dateValue,id,inputFilename);
            }
            return imageFiles.Length;
        }
        private void InsertIntoQRreaderValue(List<QRReaderValue> qRReaderValues, String datevalue, int workerId, String datasource)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                ResultResponse result = new ResultResponse();
                result.isSuccess = true;
                foreach (QRReaderValue qrread in qRReaderValues)
                {
                    List<SqlParameter> aParameters = new List<SqlParameter>();
                    aParameters.Add(new SqlParameter("@QrNo", qrread.QRNo));
                    aParameters.Add(new SqlParameter("@StandardMinuteValue", qrread.StandardMinuteValue));
                    aParameters.Add(new SqlParameter("@WorkerId", workerId));
                    aParameters.Add(new SqlParameter("@WorkingDate", datevalue));
                    result.isSuccess = accessManager.SaveData("sp_SaveQRCodeValue", aParameters);
                }
                List<SqlParameter> aList = new List<SqlParameter>();
                aList.Add(new SqlParameter("@WorkerId", workerId));
                aList.Add(new SqlParameter("@WorkingDate", datevalue));
                aList.Add(new SqlParameter("@dataSource", datasource));
                result.isSuccess = accessManager.SaveData("sp_InsertIntoSMVTable", aList);
            }
            catch (Exception e)
            {
                accessManager.SqlConnectionClose(true);
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public List<ProductionModel> showStyleList()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<ProductionModel> styleList = new List<ProductionModel>();
                List<SqlParameter> aList = new List<SqlParameter>();
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_ShowStyles");
                while (dr.Read())
                {
                        ProductionModel style = new ProductionModel();
                        style.StyleId = (int)dr["StyleId"];
                        style.StyleName = dr["StyleName"].ToString();
                        styleList.Add(style);
                }
                return styleList;
            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        } 
    }
}