
using InsentiveCalculation.DataManager;
using InsentiveCalculation.Models;
using SQIndustryThree.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace InsentiveCalculation.DAL
{
    public class AccountDAL
    {
        private DataAccessManager accessManager = new DataAccessManager();

        public UserInformation CheckUserLogin(string UserEmail, string UserPassword)
        {
            UserInformation user = new UserInformation();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SQQeye);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@userName", UserEmail));
                aParameters.Add(new SqlParameter("@userPassword", PasswordManager.Encrypt(UserPassword)));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_CheckUserLogin", aParameters);
                while (dr.Read())
                {
                    user.UserInformationId = (int)dr["UserId"];
                    user.UserInformationName = dr["UserName"].ToString();
                    user.UserInformationEmail = dr["UserEmail"].ToString();
                    user.DesignationId = (int)dr["DesignationID"];
                    //user.UserInformationPhoneNumber = (int)dr["UserPhone"];
                }

                return user;
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


        public UserInformation GetUserInformation(int userId)
        {
            UserInformation user = new UserInformation();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SQQeye);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@userId", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetUserInformation", aParameters);
                while (dr.Read())
                {
                    user.UserInformationName = dr["UserName"].ToString();
                    user.UserInformationEmail = dr["UserEmail"].ToString();
                    user.UserInformationPhoneNumber = dr["UserPhone"].ToString();
                    user.UserSQNumber = dr["SqIDNumber"].ToString();
                    user.DesignationName = dr["DesignationName"].ToString();
                }

                return user;
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
        public bool changePassword(int userId, string newpass)
        {
            bool success = false;
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SQQeye);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@userId", userId));
                aParameters.Add(new SqlParameter("@newPass", PasswordManager.Encrypt(newpass)));
                success = accessManager.UpdateData("sp_changePassword", aParameters);
                return success;
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

        public bool RecoveryPassword(int userId)
        {
            String password = "", name = "", email = "";
            bool success = false;
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SQQeye);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@userId", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_PasswordRecovery", aParameters);
                while (dr.Read())
                {
                    password = dr["UserPassword"].ToString();
                    name = dr["UserName"].ToString();
                    email = dr["UserEmail"].ToString();
                }
                if (name != "" && email != "")
                {
                    try
                    {
                        MailMessage message = new MailMessage();
                        System.Net.Mail.SmtpClient smtp = new SmtpClient();
                        message.From = new MailAddress("noreply@sqgc.com");
                        message.To.Add(new MailAddress(email));
                        message.Subject = "Insentive Password Recovery";
                        message.IsBodyHtml = true; //to make message body as html  
                        message.Body = "Dear Mr." + name + "<br/> You requested for Recover your password <br/> Your Password for the Insentive Calculation system is : " + PasswordManager.Decrypt(password) + " <br/>" +
                            "Thank you For Being with Us <br/>" +
                            "<br/>Thank You<br/> Insentive Calculation System<br/>SQ Group<br/>sqgc.com";
                        smtp.Port = 587;
                        smtp.Host = "smtp.office365.com"; //for gmail host  
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("noreply@sqgc.com", "Sweater@123");
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(message);
                        success = true;

                    }
                    catch (Exception) { }
                }
                return success;
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
        public int ModulePermission(int module, int userId)
        {
            int result = 0;
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SQQeye);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                List<SqlParameter> aList = new List<SqlParameter>();
                aList.Add(new SqlParameter("@moduleId", module));
                aList.Add(new SqlParameter("@userId", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_GetModulePermission", aList);
                while (dr.Read())
                {
                    result = (int)dr["Permission"];
                }
                return result;
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

        public List<ModuleClassModel> GetModuleByuUser(int userId)
        {
            List<ModuleClassModel> module = new List<ModuleClassModel>();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.WorkerInsentive);
                List<SqlParameter> aParameters = new List<SqlParameter>();
                aParameters.Add(new SqlParameter("@userId", userId));
                SqlDataReader dr = accessManager.GetSqlDataReader("sp_ModulePermissionByUser", aParameters);
                while (dr.Read())
                {
                    ModuleClassModel moduleClass = new ModuleClassModel();
                    moduleClass.ModuleKey = (int)dr["Modulekey"];
                    moduleClass.ModuleName = dr["ModuleName"].ToString();
                    moduleClass.ModuleValue = dr["ModuleValue"].ToString();
                    moduleClass.ModuleController = dr["ModuleController"].ToString();
                    module.Add(moduleClass);
                }

                return module;
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
    }
}