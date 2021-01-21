
using InsentiveCalculation.DataManager;
using InsentiveCalculation.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SQIndustryThree.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;

namespace InsentiveCalculation.DAL
{
    public class HomeDAL
    {
        private DataAccessManager accessManager = new DataAccessManager();

        public int ReadQrCode(String dateValue)
        {
            DirectoryInfo dir = new DirectoryInfo(@"E:\NPT\9, 10 Jan Scan Pdf\9-01-2020");
            FileInfo[] imageFiles = dir.GetFiles("*.pdf");
            foreach (FileInfo f in imageFiles)
            {
                //string plainText = "";
                //plainText += f.Name + "\n" + f.Length + "\n" + f.DirectoryName + "\n" + f.Attributes + "\n";
                string inputFilename = f.DirectoryName + '/' + f.Name;
                PdfReader pdfReader = new PdfReader(inputFilename);
                int numberOfPages = pdfReader.NumberOfPages;
                for (int i = 1; i < numberOfPages + 1; i++)
                {
                    ReadBarcode(inputFilename, i, dateValue);
                }
            }
            return imageFiles.Length;
        }
        public void ReadBarcode(string fileName, int page, string workingDate)
        {
            List<BarcodeModel> barcodemodelList = new List<BarcodeModel>();
            string workerId = "";
            try
            {
               // BarcodeReader reader = new BarcodeReader();
               // reader.Pdf417 = true;
                // reader.DataMatrix = true;
                // reader.QR = true;
                //Inlite.ClearImageNet.Barcode[] barcodes = reader.Read(fileName, page);
                // Process results

                //foreach (Inlite.ClearImageNet.Barcode bc in barcodes)
                //{
                //    string[] smv = bc.Text.ToString().Split(',');
                //    if (smv.Length < 2)
                //    {
                //        workerId = smv[0];
                //        if (workerId.Length == 5)
                //        {
                //            workerId = '0' + workerId;
                //        }
                //        else if (workerId.Length == 4)
                //        {
                //            workerId = "00" + workerId;
                //        }
                //    }
                //    else
                //    {
                //        BarcodeModel barcode = new BarcodeModel();
                //        barcode.BarcodeNumber = System.Int32.Parse(smv[0]);
                //        barcode.StandardMinuteValue = float.Parse(smv[1]);
                //        barcodemodelList.Add(barcode);
                //    }
                //}   // do other processing
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            DataSaveToDatabase(barcodemodelList, workerId, workingDate);

        }
        private void DataSaveToDatabase(List<BarcodeModel> barcodeModels, string workerId, string workingDate)
        {
            DataAccessManager accessManager = new DataAccessManager();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                ResultResponse result = new ResultResponse();
                result.isSuccess = true;
                foreach (BarcodeModel qrread in barcodeModels)
                {
                    List<SqlParameter> aParameters = new List<SqlParameter>();
                    aParameters.Add(new SqlParameter("@barcodeNumber", qrread.BarcodeNumber));
                    aParameters.Add(new SqlParameter("@StandardMinuteValue", qrread.StandardMinuteValue));
                    aParameters.Add(new SqlParameter("@WorkerId", workerId));
                    aParameters.Add(new SqlParameter("@WorkingDate", workingDate));
                    result.isSuccess = accessManager.SaveData("sp_SaveBarCodeValue", aParameters);
                }
            }
            catch (Exception e)
            {
                accessManager.SqlConnectionClose(true);
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
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
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public List<CommonModel> GetBuyer(int UserId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<CommonModel> buyerlist = new List<CommonModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@UserId", UserId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_getAllBuyer",aParameters);
                while (dr.Read())
                {
                    CommonModel buyer = new CommonModel();
                    buyer.BuyerId = (int)dr["BuyerId"];
                    buyer.BuyerName = dr["BuyerName"].ToString();
                    buyerlist.Add(buyer);
                }
                return buyerlist;
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
        public List<CommonModel> GetStyle(int BuyerId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<CommonModel> stylelist = new List<CommonModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@buyerId", BuyerId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_StyleName",aParameters);
                while (dr.Read())
                {
                    CommonModel style = new CommonModel();
                    style.StyleId = (int)dr["StyleId"];
                    style.StyleName = dr["StyleName"].ToString();
                    stylelist.Add(style);
                }
                return stylelist;
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
        public List<CommonModel> GetOperations()
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<CommonModel> operationList = new List<CommonModel>();
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_LoadAllOperations");
                while (dr.Read())
                {
                    CommonModel operation = new CommonModel();
                    operation.OperationId = (int)dr["OperationId"];
                    operation.OperationName = dr["OperationName"].ToString();
                    operation.OperationSection = dr["OperationSection"].ToString();
                    operationList.Add(operation);
                }
                return operationList;
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
        public List<CommonModel> GetBusinessUnit(int UserId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SQQeye);
                List<CommonModel> bulist = new List<CommonModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@userId", UserId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_UserWiseBusinessUnit",aParameters);
                while (dr.Read())
                {
                    CommonModel bul = new CommonModel();
                    bul.BusinessUnitId = (int)dr["BusinessUnitId"];
                    bul.BusinessUnitName = dr["BusinessUnitName"].ToString();
                    bulist.Add(bul);
                }
                return bulist;
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
        public List<CommonModel> LoadStyleWiseOperation(String styleId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<CommonModel> operationList = new List<CommonModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@styleId", styleId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetStyleWiseOperation", aParameters);
                while (dr.Read())
                {
                    CommonModel operation = new CommonModel();
                    operation.OperationId = (int)dr["OperationId"];
                    operation.OperationNumber = (int)dr["OperationNumber"];
                    operation.OperationName = dr["OperationName"].ToString();
                    operation.OperationSection = dr["OperationSection"].ToString();
                    operation.OperationSMV = float.Parse(dr["OperationSMV"].ToString());
                    operationList.Add(operation);
                }
                return operationList;
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
        public ResultResponse SaveOperaTionToStyle(CommonModel commonModel, int userId)
        {
            ResultResponse resultResponse = new ResultResponse();
            try
            {

                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                int count = 0;
                foreach (CommonModel common in commonModel.OperationList)
                {
                    count++;
                    List<SqlParameter> aParameters = new List<SqlParameter>();
                    aParameters.Add(new SqlParameter("@StyleId", commonModel.StyleId));
                    aParameters.Add(new SqlParameter("@operationId", common.OperationId));
                    aParameters.Add(new SqlParameter("@operationSMV", common.OperationSMV));
                    aParameters.Add(new SqlParameter("@NoOfOperation", count));
                    accessManager.SaveData("sp_SaveOperationToStyle", aParameters);
                }
                resultResponse.isSuccess = true;
                return resultResponse;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public bool SaveStyleToDataBAse(String StyleName,int BuyerID)
        {
            bool result = false;
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@StyleName",StyleName));
                aParameters.Add(new SqlParameter("@buyerId", BuyerID));
                result=accessManager.SaveData("sp_SaveStyleTodatabase", aParameters);
                return result;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public bool SavePotoDatabase(string PoNumber, int StyleId, int numberProduction, int maxNumber,int userId)
        {
            bool result = false;
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@StyleId", StyleId));
                aParameters.Add(new SqlParameter("@PoNumber", PoNumber));
                aParameters.Add(new SqlParameter("@numberProduction", numberProduction));
                aParameters.Add(new SqlParameter("@maxNumber", maxNumber));
                aParameters.Add(new SqlParameter("@UserId", userId));
                result = accessManager.SaveData("sp_SavePoToDatabaseByStyle", aParameters);
                return result;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public List<PurchaseOrderModel> GetPoFromDatabaseByStyle(int StyleId,int userId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<PurchaseOrderModel> purchasorderlist = new List<PurchaseOrderModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@StyleId", StyleId));
                aParameters.Add(new SqlParameter("@UserId", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetALLPoAndInformation", aParameters);
                while (dr.Read())
                {
                    PurchaseOrderModel purchaseOrder = new PurchaseOrderModel();
                    purchaseOrder.PoID= (int)dr["PuchaseOrderId"];
                    purchaseOrder.PONumeber= dr["PurchaseOrderNumber"].ToString();
                    purchaseOrder.POQuantity= (int)dr["ProductionQuantity"];
                    purchaseOrder.PoPercentage= (int)dr["GenateParcentage"];
                    int total = (int)dr["ProductionQuantity"] + (((int)dr["GenateParcentage"] * (int)dr["ProductionQuantity"]) / 100);
                    purchaseOrder.TotalQuantity = total;
                    purchaseOrder.RemainingQuantity = total - (int)dr["GenarateBarcode"];
                    purchasorderlist.Add(purchaseOrder);
                }
                return purchasorderlist;
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
        public PurchaseOrderModel GetPOInformationById(int PoID, int userId)
        {
            PurchaseOrderModel purchaseOrder = new PurchaseOrderModel();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@PoId", PoID));
                aParameters.Add(new SqlParameter("@UserId", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_LoadPODetails", aParameters);
                while (dr.Read())
                {
                    purchaseOrder.PoID = (int)dr["PuchaseOrderId"];
                    purchaseOrder.PONumeber = dr["PurchaseOrderNumber"].ToString();
                    purchaseOrder.POQuantity = (int)dr["ProductionQuantity"];
                    purchaseOrder.PoPercentage = (int)dr["GenateParcentage"];
                    int total = (int)dr["ProductionQuantity"] + (((int)dr["GenateParcentage"] * (int)dr["ProductionQuantity"]) / 100);
                    purchaseOrder.TotalQuantity = total;
                    purchaseOrder.RemainingQuantity = total - (int)dr["GenarateBarcode"];
                }
                return purchaseOrder;
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
        public int SaveDataEntryTable(DataEntryTest datentry,int userid)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@requestId", datentry.RequestId));
                aParameters.Add(new SqlParameter("@requestFor", datentry.RequestFor));
                aParameters.Add(new SqlParameter("@businessunit", datentry.BusinessUnitId));
                aParameters.Add(new SqlParameter("@buyer", datentry.BuyerId));
                aParameters.Add(new SqlParameter("@userId", userid));
                aParameters.Add(new SqlParameter("@instructions", datentry.Instructions));
                aParameters.Add(new SqlParameter("@priority", datentry.Priority));
                int primarykey = accessManager.SaveDataReturnPrimaryKey("sp_TestTableDataEntry", aParameters);
                return primarykey;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public ResultResponse DeleteOperationFromStyle(CommonModel commonModel, int userId)
        {
            ResultResponse resultResponse = new ResultResponse();
            try
            {

                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                foreach (CommonModel common in commonModel.OperationList)
                {
                    List<SqlParameter> aParameters = new List<SqlParameter>();
                    aParameters.Add(new SqlParameter("@StyleId", commonModel.StyleId));
                    aParameters.Add(new SqlParameter("@OperationId", common.OperationId));
                    accessManager.DeleteData("sp_DeleteOperationFromStyle", aParameters);
                }
                resultResponse.isSuccess = true;
                return resultResponse;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public bool DeleteStyle(int StyleId, int userId)
        {
            bool result = false;
            try
            {

                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@StyleId",StyleId));
                aParameters.Add(new SqlParameter("@UserId",userId));
                result=accessManager.DeleteData("sp_DeleteStyle", aParameters);
                return result;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public ResultResponse SaveBarcodes(BarcodeGenarateModal barcodeGenarateModal,int userId)
        {
            ResultResponse resultResponse = new ResultResponse();
            int masterPrimaryKey = 0;
                try
                {
                    accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                    List<SqlParameter> aParameters = new List<SqlParameter>();
                    aParameters.Add(new SqlParameter("@StyleNo", barcodeGenarateModal.StyleID));
                    aParameters.Add(new SqlParameter("@GenarateDate", barcodeGenarateModal.SelectDate));
                    aParameters.Add(new SqlParameter("@BundleQuantity", barcodeGenarateModal.Quantity));
                    aParameters.Add(new SqlParameter("@Size", barcodeGenarateModal.BundleSize));
                    aParameters.Add(new SqlParameter("@Color", barcodeGenarateModal.Color));
                    aParameters.Add(new SqlParameter("@CreateBy", userId));
                    aParameters.Add(new SqlParameter("@CutNo", barcodeGenarateModal.CutNo));
                    aParameters.Add(new SqlParameter("@ShadeNo", barcodeGenarateModal.ShadeNO));
                    aParameters.Add(new SqlParameter("@BuyerId", barcodeGenarateModal.BuyerID));
                    aParameters.Add(new SqlParameter("@PONumber", barcodeGenarateModal.PONumber));
                    aParameters.Add(new SqlParameter("@QuantityGenarate", (barcodeGenarateModal.Quantity*barcodeGenarateModal.NoOfBundle)));
                    aParameters.Add(new SqlParameter("@BusinessUnitId", barcodeGenarateModal.BusinessUnitId));
                    aParameters.Add(new SqlParameter("@MachineId", barcodeGenarateModal.MachineId));
                    aParameters.Add(new SqlParameter("@LotNo", barcodeGenarateModal.LotNo));
                    masterPrimaryKey = accessManager.SaveDataReturnPrimaryKey("sp_SaveGenaratedMasterTable", aParameters);
                }
                catch (Exception exception)
                {
                    accessManager.SqlConnectionClose(true);
                    throw exception;
                }
                finally
                {
                    accessManager.SqlConnectionClose();
                }
                var pgSize = new iTextSharp.text.Rectangle(225, 75); //225,75
                iTextSharp.text.Document doc = new iTextSharp.text.Document(pgSize, 0, 0, 2, 2);//0,0,0,0
                string path = HttpContext.Current.Server.MapPath("~/GenaratePDF/");
                DateTime dt = DateTime.Now;
                string fileName = dt.Minute.ToString()+dt.Second.ToString()+dt.Millisecond.ToString();
                PdfWriter.GetInstance(doc, new FileStream(path +fileName+".pdf" , FileMode.Create));
                //open the document for writing
                doc.Open();
               // doc.SetPageSize(new iTextSharp.text.Rectangle(100,65));
                PdfPTable pdftable = new PdfPTable(1);
                for (int i = 0; i < barcodeGenarateModal.NoOfBundle; i++)
                {
                    int count = 0;
                    iTextSharp.text.Font font = FontFactory.GetFont("Calibri", 6f, BaseColor.BLACK); // from 5.0f 
                    iTextSharp.text.Font fontlast = FontFactory.GetFont("Calibri", 6f, BaseColor.BLACK); // from 5.0f 
                    //String header = "Buyer Name : " + barcodeGenarateModal.BuyerName+"\nStyle : " + barcodeGenarateModal.StyleName + "\nBundle Quantity : " + barcodeGenarateModal.Quantity +
                    //        "\nBundle No : " + (i+1) + "\nColor : " + barcodeGenarateModal.Color + ", Size : " + barcodeGenarateModal.BundleSize+ "\nPO Number : " + barcodeGenarateModal.PONumber;
                    //iTextSharp.text.Paragraph paraheader = new iTextSharp.text.Paragraph(header, font);
                   //PdfPCell cell1 = new PdfPCell { PaddingLeft = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                   // cell1.AddElement(paraheader);
                   // cell1.Border = 0;
                    //pdftable.AddCell(cell1);
                    foreach (CommonModel barcodeGenarate in barcodeGenarateModal.OprationList)
                    {
                        count++;
                        int barcodeNo = BarcodePrimaryKey(masterPrimaryKey,i+1, barcodeGenarate,barcodeGenarateModal.BusinessUnitId);
                        var bw = new ZXing.BarcodeWriter()
                        {
                            Format = ZXing.BarcodeFormat.PDF_417
                        };
                        bw.Options = new ZXing.Common.EncodingOptions() { Margin = 1 };

                    // var encOptions = new ZXing.Common.EncodingOptions() { Margin = 0}; // margin 0 to 1 

                    //bw.Format = ZXing.BarcodeFormat.CODE_128;
                    var bitmap = bw.Write(barcodeNo + "," + barcodeGenarate.OperationSMV);
                        Bitmap result = new Bitmap(bitmap,new Size(210,55));//100,60 // 210,60
                        //var result = new Bitmap();
                        result.Save(path+barcodeGenarate.OperationName, System.Drawing.Imaging.ImageFormat.Png);
                        //barcodeNo + "-" +
                        string first = barcodeNo+" , "+barcodeGenarateModal.StyleName + ", " + barcodeGenarate.OperationName+","+ barcodeGenarateModal.BundleSize;
                        string last =barcodeGenarate.OperationSMV+
                            ", " + barcodeGenarateModal.SelectDate+","
                        + barcodeGenarateModal.Quantity +
                            ", " + (i+1) + "-" + count + ", " + barcodeGenarateModal.Color+","+barcodeGenarateModal.PONumber;

                        iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(first, font);
                        iTextSharp.text.Paragraph lastparagraph = new iTextSharp.text.Paragraph(last, fontlast);
                        paragraph.SetLeading(0.8f, 0.8f);
                        paragraph.SpacingAfter = 1;//2
                        lastparagraph.SetLeading(0.8f, 0.8f);
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(path + barcodeGenarate.OperationName);
                        //image.SpacingAfter = 5;// for code 128
                        //image.ScaleAbsolute(85,50);
                       // image.PaddingTop = 3.0f;
                        PdfPCell cell = new PdfPCell { PaddingLeft = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                        // cell.FixedHeight = 60;
                        cell.AddElement(paragraph);
                        cell.AddElement(image);
                        cell.AddElement(lastparagraph);
                        cell.Border = 0;
                        pdftable.AddCell(cell);
                        File.Delete(path + barcodeGenarate.OperationName);
                    }
                }
                doc.Add(pdftable);
                doc.Close();
                resultResponse.isSuccess = true;
                resultResponse.data = path+ fileName + ".pdf";
                return resultResponse; 
        }
        private int BarcodePrimaryKey(int masterkey, int bundlekey,CommonModel commonModel,int BusinessUnit)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@MasterGenarationId", masterkey));
                aParameters.Add(new SqlParameter("@BundleNO", bundlekey));
                aParameters.Add(new SqlParameter("@BusinessUnit", BusinessUnit));
                aParameters.Add(new SqlParameter("@OperationNo", commonModel.OperationId));
                aParameters.Add(new SqlParameter("@OperationSMV", commonModel.OperationSMV));
                int barcodePrimaryKey= accessManager.SaveDataReturnPrimaryKey("sp_SaveBarcodeGenarationTable", aParameters);
                return barcodePrimaryKey;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public List<UserInformation> GetWorkersSearch(string BusinessUnitId, string Department, string Section, string Subsection, string Line)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<UserInformation> userList = new List<UserInformation>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@buid", BusinessUnitId));
                aParameters.Add(new SqlParameter("@Department", Department));
                aParameters.Add(new SqlParameter("@Section", Section));
                aParameters.Add(new SqlParameter("@SubSection", Subsection));
                aParameters.Add(new SqlParameter("@Line", Line));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetWorkerBySearch", aParameters);
                while (dr.Read())
                {
                    UserInformation user = new UserInformation();
                    user.UserInformationId = (int)dr["WorkerID"];
                    user.UserInformationPhoneNumber = dr["EmployeeCode"].ToString();
                    user.UserInformationName = dr["EmployeeName"].ToString();
                    user.DesignationName = dr["Designation"].ToString();
                    userList.Add(user);
                }
                return userList;
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
        public List<UserInformation> GetWorkerByCode(string EmployeeCode, int userID,int BusinessUnit)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<UserInformation> userList = new List<UserInformation>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@EmpoyeeCode", EmployeeCode));
                aParameters.Add(new SqlParameter("@BusinessUnitId", BusinessUnit));
                aParameters.Add(new SqlParameter("@UserID", userID));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_SearchByWorkerCode", aParameters);
                while (dr.Read())
                {
                    UserInformation user = new UserInformation();
                    user.UserInformationId = (int)dr["WorkerID"];
                    user.UserInformationPhoneNumber = dr["EmployeeCode"].ToString();
                    user.UserInformationName = dr["EmployeeName"].ToString();
                    user.DesignationName = dr["Designation"].ToString();
                    userList.Add(user);
                }
                return userList;
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
        public ResultResponse GenerateWorkerBarcode(List<UserInformation> users)
        {
            ResultResponse resultResponse = new ResultResponse();

            var pgSize = new iTextSharp.text.Rectangle(225,75);//100,65 //200,65
            iTextSharp.text.Document doc = new iTextSharp.text.Document(pgSize, 0, 0, 2, 2);//0,0,0,0
            string path = HttpContext.Current.Server.MapPath("~/GenaratePDF/");
            DateTime dt = DateTime.Now;
            string fileName = dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();
            PdfWriter.GetInstance(doc, new FileStream(path + fileName + ".pdf", FileMode.Create));
            //open the document for writing
            doc.Open();
            // doc.SetPageSize(new iTextSharp.text.Rectangle(100,65));
            PdfPTable pdftable = new PdfPTable(1);
            for (int i = 0; i < users.Count; i++)
            {
                    int count = 0;
                    iTextSharp.text.Font font = FontFactory.GetFont("Calibri", 6.0f, BaseColor.BLACK); // from 5.0f 
                    count++;
                   // var bw = new ZXing.BarcodeWriter();
                    //var encOptions = new ZXing.Common.EncodingOptions() { Margin = 0 }; // margin 0 to 1 
                    //bw.Options = encOptions;
                    //bw.Format = ZXing.BarcodeFormat.PDF_417;
                    var bw = new ZXing.BarcodeWriter()
                    {
                        Format = ZXing.BarcodeFormat.PDF_417
                    };
                    bw.Options = new ZXing.Common.EncodingOptions() { Margin = 1 };
                    var result = new Bitmap(bw.Write(users[i].UserSQNumber),new Size(210,55)); //200,50
                    result.Save(path + users[i].UserSQNumber, System.Drawing.Imaging.ImageFormat.Png);
                    string first = "Employee Code : "+users[i].UserSQNumber;
                    string last = users[i].UserInformationName.ToString() + " - " + users[i].DesignationName.ToString();
                    iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(first, font);
                    iTextSharp.text.Paragraph lastparagraph = new iTextSharp.text.Paragraph(last, font);
                    paragraph.SetLeading(0.8f, 0.8f);
                    paragraph.SpacingAfter = 1;
                    lastparagraph.SetLeading(0.8f, 0.8f);
                    lastparagraph.SpacingBefore = 1;
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(path + users[i].UserSQNumber);
                   // image.ScaleToFit(new iTextSharp.text.Rectangle(85, 100));
                    //image.SetAbsolutePosition(0.0f,0.0f);
                  //  image.PaddingTop = 5;
                    PdfPCell cell = new PdfPCell { PaddingLeft = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                    // cell.FixedHeight = 60;
                    cell.AddElement(paragraph);
                    cell.AddElement(image);
                    cell.AddElement(lastparagraph);
                    cell.Border = 0;
                    pdftable.AddCell(cell);
                    File.Delete(path + users[i].UserSQNumber);
            }
            doc.Add(pdftable);
            doc.Close();
            resultResponse.isSuccess = true;
            resultResponse.data = path + fileName + ".pdf";
            return resultResponse;
        }
        public List<ReportModel> SearchReportByDate(string seletedDate,int BusinessUnitId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<ReportModel> reportModelList = new List<ReportModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@SelectedDate", seletedDate));
                aParameters.Add(new SqlParameter("@BusinessUnitId", BusinessUnitId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetReportFromIncen", aParameters);
                while (dr.Read())
                {
                    ReportModel report = new ReportModel();
                    report.EmployeeId =dr["EmpCode"].ToString();
                    report.NoOFBarcode =dr["Barcode"].ToString();
                    report.TotalSMV =dr["SMV"].ToString();
                    report.FileNameId =Convert.ToInt32(dr["FileNameId"]);
                    report.DirectoryName =dr["DirectoryName"].ToString();
                    report.FileName =dr["FileName"].ToString();
                    reportModelList.Add(report);
                }
                return reportModelList;
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
        public  bool AddEmployeeByFileID(int FileId,string EmployeeCode)
        {
            bool result = false;
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@employeeCode", EmployeeCode));
                aParameters.Add(new SqlParameter("@fileNameId", FileId));
                result = accessManager.UpdateData("sp_AddNewEmployeeByFile", aParameters);
                return result;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
    }
}