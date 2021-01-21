using InsentiveCalculation.DataManager;
using InsentiveCalculation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace InsentiveCalculation.DAL
{
    public class QmsAppDAL
    {
        private DataAccessManager accessManager = new DataAccessManager();
        public ResultResponse UploadSilhoutte(string SilhouteeName,int CatagoryId,List<StyleUploadModal> fileModelList)
        {

            try
            {
                int masterId = 0;
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse result = new ResultResponse();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                var fileslist = new JavaScriptSerializer().Serialize(fileModelList);
                aParameters.Add(new SqlParameter("@SilhoutteName", SilhouteeName));
                aParameters.Add(new SqlParameter("@CategoryId", CatagoryId));
                aParameters.Add(new SqlParameter("@FileUploadJSon",fileslist));

                masterId = accessManager.SaveDataReturnPrimaryKey("sp_UploadSilhoutte", aParameters);
                
                if (masterId == 0)
                {
                    result.isSuccess = false;
                }
                else
                {
                    result.pk = masterId;
                    result.isSuccess = true;
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public List<ImageGalleryModel> GetImageForgalary(int CategoryType)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                List<ImageGalleryModel> imageGalaryList = new List<ImageGalleryModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@Status", "0"));
                aParameters.Add(new SqlParameter("@CategoryType", CategoryType));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetAllImageForGalary", aParameters);
                while (dr.Read())
                {
                    //imageGalaryList= JsonConvert.DeserializeObject<List<ImageGalleryModel>>(dr.ToString());
                    ImageGalleryModel imageGallery = new ImageGalleryModel();
                    imageGallery.SilhouetteId = (int)dr["SilhouetteId"];
                    imageGallery.SilhouetteName = dr["SilhouetteName"].ToString();
                    imageGallery.ImageList = JsonConvert.DeserializeObject<List<StyleUploadModal>>(dr["ImageList"].ToString());
                    imageGalaryList.Add(imageGallery);
                }
                return imageGalaryList;
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
        public ResultResponse RegisterSilhoutte(int BusinessUnitId, int BuyerId, int StyleId, int SilhoutteId,int CategoryID, int UserId)
        {
            try
            {
                int masterId = 0;
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse result = new ResultResponse();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@BusinessUnitId", BusinessUnitId));
                aParameters.Add(new SqlParameter("@BuyerId", BuyerId));
                aParameters.Add(new SqlParameter("@StyleId", StyleId));
                aParameters.Add(new SqlParameter("@ShilhoutteId", SilhoutteId));
                aParameters.Add(new SqlParameter("@CategoryID", CategoryID));
                aParameters.Add(new SqlParameter("@UserId", UserId));

                masterId = accessManager.SaveDataReturnPrimaryKey("sp_SaveMasterSilhoteeTable", aParameters);

                if (masterId == 0)
                {
                    result.isSuccess = false;
                    result.msg = "Already A Silhoutte Resigtered In This Style";
                }
                else
                {
                    result.pk = masterId;
                    result.isSuccess = true;
                    result.msg = "Style Registered Succssfully";
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public List<CommonModel> GetCatagoryType(int UserId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                List<CommonModel> buyerlist = new List<CommonModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@UserId", UserId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_getCatagory", aParameters);
                while (dr.Read())
                {
                    CommonModel buyer = new CommonModel();
                    buyer.CatagoryId = (int)dr["CategoryId"];
                    buyer.CatagoryName = dr["CategoryName"].ToString();
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
        public List<DefectPostionModel> GetAllDefectPosition(int Status,int CategoryId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                List<DefectPostionModel> DefectPositionList = new List<DefectPostionModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@Status", Status));
                aParameters.Add(new SqlParameter("@categoryId", CategoryId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetallDefectPositionName", aParameters);
                while (dr.Read())
                {
                    DefectPostionModel defect = new DefectPostionModel();
                    defect.DefectPoistionId= (int)dr["DefectPoistionId"];
                    defect.DefectPositionName = dr["DefectPositionName"].ToString();
                    defect.DefectColor = dr["DefectColor"].ToString();
                    DefectPositionList.Add(defect);
                }
                return DefectPositionList;
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
        public List<DefectPostionModel> GetDefectByPosition(string PositionId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                List<DefectPostionModel> DefectPositionList = new List<DefectPostionModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@DefectPositionId", PositionId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetDefectByPositionId", aParameters);
                while (dr.Read())
                {
                    DefectPostionModel defect = new DefectPostionModel();
                    defect.DefectPoistionId = (int)dr["DefectPoistionId"];
                    defect.DefectPositionName = dr["DefectPositionName"].ToString();
                    DefectPositionList.Add(defect);
                }
                return DefectPositionList;
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
        public List<CommonModel> GetUnitsfromDatabase(int userId)
        {
            List<CommonModel> commonModelList = new List<CommonModel>();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.DataEntryTracker);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@userId", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_LoadUNits", aParameters);
                while (dr.Read())
                {
                    CommonModel common = new CommonModel();
                    common.BusinessUnitId = (int)dr["UnitId"];
                    common.BusinessUnitName = dr["UnitName"].ToString();
                    commonModelList.Add(common);
                }

                return commonModelList;
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
        public MasterSilhoteeTable GetMasterSilhoutteInformation(int MasterId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                MasterSilhoteeTable masterInformation = new MasterSilhoteeTable();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@MasterSilhoutteId", MasterId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_getSilhoutteInformation", aParameters);
                while (dr.Read())
                {
                    masterInformation.MasterSilhouteeId = (int)dr["MasterSilhouteeId"];
                    masterInformation.StyleId = (int)dr["StyleId"];
                    masterInformation.CatagoryId = (int)dr["CatagoryId"];
                    masterInformation.StyleName = dr["StyleName"].ToString();
                    masterInformation.BuyerName = dr["BuyerName"].ToString();
                    masterInformation.UnitName = dr["UnitName"].ToString();
                    masterInformation.SilhouetteName = dr["SilhouetteName"].ToString();
                    masterInformation.CreateDate = dr["CreateDate"].ToString();
                    
                    masterInformation.Imagelist = JsonConvert.DeserializeObject<List<StyleUploadModal>>(dr["ImageList"].ToString());
                    //masterInformation.SilhoutteGridList = JsonConvert.DeserializeObject<List<ShilhouteeGridTable>>(dr["ShilhoutteeGridList"].ToString());
                    masterInformation.FrontPositionList = JsonConvert.DeserializeObject<List<DefectPostionModel>>(dr["FrontPositionList"].ToString());
                    masterInformation.BackPostionList = JsonConvert.DeserializeObject<List<DefectPostionModel>>(dr["BackPostionList"].ToString());
                }
                return masterInformation;
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
        public List<MasterSilhoteeTable> GetSilhoutteTableList(int UserId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                List<MasterSilhoteeTable> masterSilhoutteList = new List<MasterSilhoteeTable>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@UserId", UserId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetAllMasterSilhoutteList", aParameters);
                while (dr.Read())
                {
                    MasterSilhoteeTable masterInformation = new MasterSilhoteeTable();
                    masterInformation.MasterSilhouteeId = (int)dr["MasterSilhouteeId"];
                    masterInformation.StyleId = (int)dr["StyleId"];
                    masterInformation.StyleName = dr["StyleName"].ToString();
                    masterInformation.BuyerName = dr["BuyerName"].ToString();
                    masterInformation.UnitName = dr["UnitName"].ToString();
                    masterInformation.SilhouetteName = dr["SilhouetteName"].ToString();
                    masterInformation.UserId = (int)dr["UserId"];
                    masterInformation.CreateDate = dr["CreateDate"].ToString();
                    masterSilhoutteList.Add(masterInformation);
                }
                return masterSilhoutteList;
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
        public ResultResponse AssignDefectPosition(List<ShilhouteeGridTable> positionGridlist,int UserId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse result = new ResultResponse();
                var positionGridList = new JavaScriptSerializer().Serialize(positionGridlist);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@PositionGridList", positionGridList));
                aParameters.Add(new SqlParameter("@UserId", UserId));
                result.isSuccess = accessManager.SaveData("sp_AssignedDefectPosition", aParameters);
                result.msg = "Defect Position Assigned";
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public ResultResponse AddDefectToPosition(DefectPostionModel DefectPostionModel,int UserId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse result = new ResultResponse();
                var DefectList = new JavaScriptSerializer().Serialize(DefectPostionModel.DefectList);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@PositionName", DefectPostionModel.DefectPositionName));
                aParameters.Add(new SqlParameter("@DefectList", DefectList));
                aParameters.Add(new SqlParameter("@UserId", UserId));
                aParameters.Add(new SqlParameter("@PositionId", DefectPostionModel.DefectPoistionId));
                aParameters.Add(new SqlParameter("@CategoryId", DefectPostionModel.CategoryId));
                result.isSuccess = accessManager.SaveData("sp_AddDefectToPosition", aParameters);
                result.msg = "Defect Added To Position";
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public ResultResponse AddOrDeleteDefect(int DefectKey, string DefectName,int CategoryId,int UserID)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse result = new ResultResponse();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@DefectKey",DefectKey));
                aParameters.Add(new SqlParameter("@DefectName", DefectName));
                aParameters.Add(new SqlParameter("@UserId", UserID));
                aParameters.Add(new SqlParameter("@CategoryId", CategoryId));
                result.isSuccess = accessManager.SaveData("sp_AddOrDeleteDefect", aParameters);
                result.msg = "Successful";
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public List<QmsUserModel> AllQmsUserInformation(int userId)
        {
            List<QmsUserModel> qmsUsersList = new List<QmsUserModel>();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@Status", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetAllQmsUserInfo", aParameters);
                while (dr.Read())
                {
                    QmsUserModel users = new QmsUserModel();
                    users.QmsUserId = (int)dr["QmsUserId"];
                    users.BusinessUnitName = dr["BusinessUnitName"].ToString();
                    users.PunitName = dr["PunitName"].ToString();
                    users.FloorModuleName = dr["FloorModuleName"].ToString();
                    users.LineName = dr["LineName"].ToString();
                    users.UserName = dr["UserName"].ToString();
                    qmsUsersList.Add(users);
                }

                return qmsUsersList;
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

        public List<DefectPostionModel> ShowGridByDefectPosition(int DefectPositionId,int MasterSilhouteeId,int FrontOrBack)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                List<DefectPostionModel> DefectPositionList = new List<DefectPostionModel>();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@DefectPositionId", DefectPositionId));
                aParameters.Add(new SqlParameter("@MasterSilhoutteId", MasterSilhouteeId));
                aParameters.Add(new SqlParameter("@FrontOrBack", FrontOrBack));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_ShowGridByPosition", aParameters);
                while (dr.Read())
                {
                    DefectPostionModel defect = new DefectPostionModel();
                    defect.DefectPoistionId = (int)dr["DefectPoistionId"];
                    defect.GridNo = (int)dr["GridNo"];
                    defect.DefectPositionName = dr["DefectPositionName"].ToString();
                    defect.DefectColor = dr["DefectColor"].ToString();
                    DefectPositionList.Add(defect);
                }
                return DefectPositionList;
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
        public ResultResponse DeletePositionFromGrid(int DefectPositionId, int MasterSilhouteeId, int FrontOrBack)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse resultResponse = new ResultResponse();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@DefectPositionId", DefectPositionId));
                aParameters.Add(new SqlParameter("@MasterSilhoutteId", MasterSilhouteeId));
                aParameters.Add(new SqlParameter("@FrontOrBack", FrontOrBack));
                resultResponse.isSuccess= accessManager.DeleteData("sp_DeletePositionFormGrid", aParameters);
                return resultResponse;
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

        public ResultResponse DeleteRegisteredStyle(int MasterSilhouteeId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse resultResponse = new ResultResponse();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@MasterSilhoutteId", MasterSilhouteeId));
                resultResponse.isSuccess = accessManager.DeleteData("sp_DeleteRegisteredStyle", aParameters);
                return resultResponse;
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

        public ResultResponse CopySilhoutteToStyle(int StyleId, int SilhoutteId,int UserId)
        {
            try
            {
                int masterId = 0;
                accessManager.SqlConnectionOpen(DataBase.QMSDatabase);
                ResultResponse result = new ResultResponse();
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@StyleId", StyleId));
                aParameters.Add(new SqlParameter("@MasteSilhoutteId", SilhoutteId));
                aParameters.Add(new SqlParameter("@UserId", UserId));

                masterId = accessManager.SaveDataReturnPrimaryKey("sp_CopyExistingStyle", aParameters);

                if (masterId == 0)
                {
                    result.isSuccess = false;
                    result.msg = "Already A Silhoutte Resigtered In This Style";
                }
                else
                {
                    result.pk = masterId;
                    result.isSuccess = true;
                    result.msg = "Style Copied Succssfully";
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

    }
}