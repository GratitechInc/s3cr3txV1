using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace s3cr3txMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        //[HttpPost, DisableRequestSizeLimit]
        //public IActionResult Upload()
        //{
        //    try
        //    {
        //        var file = Request.Form.Files[0];
        //        var folderName = Path.Combine("Resources", "ClientCerts");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        string strGUID = Guid.NewGuid().ToString();
        //        string strAuth = @"";
        //        if (Request.Headers.TryGetValue(@"AuthCode", out Microsoft.Extensions.Primitives.StringValues svAuth))
        //        {
        //            strAuth = svAuth[0];
        //        }
        //        string strToken = @"";
        //        if (Request.Headers.TryGetValue(@"APIToken", out Microsoft.Extensions.Primitives.StringValues svToken))
        //        {
        //            strToken = svToken[0];
        //        }
        //        string strEmail = @"";
        //        if (Request.Headers.TryGetValue(@"Email", out Microsoft.Extensions.Primitives.StringValues svEmail))
        //        {
        //            strEmail = svEmail[0];
        //        }
        //        string strUpType = @"";
        //        if (Request.Headers.TryGetValue(@"UpType", out Microsoft.Extensions.Primitives.StringValues svInput))
        //        {
        //            strUpType = svInput[0];
        //        }
        //        if (isAuthorized(strAuth, strToken, strEmail))
        //        {
        //            if (file.Length > 0)
        //            {
        //                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').Trim('.').Trim('.').Trim('/').Trim('\\').Trim('.').Trim('.').Trim('\\').Trim('/');
        //                Match m = Regex.Match(Path.GetFileNameWithoutExtension(fileName), @"\w*");
        //                if (!Path.GetExtension(fileName).ToLower().Equals(@".config"))
        //                {
        //                    var fullPath = Path.Combine(pathToSave, strGUID + @"_" + m.Value + Path.GetExtension(fileName));
        //                    var dbPath = Path.Combine(folderName, strGUID + @"_" + m.Value + Path.GetExtension(fileName));

        //                    using (var stream = new FileStream(fullPath, FileMode.Create))
        //                    {
        //                        file.CopyTo(stream);
        //                    }
        //                    StoreFileLocation(strEmail, fileName, fullPath, dbPath, @"ClientCertificate:" + strGUID, @"Client Certificate");
        //                    string Success = @"File Uploaded Successfully";
        //                    return Ok(new { Success });
        //                }
        //                return BadRequest();
        //            }
        //            else
        //            {
        //                return BadRequest();
        //            }
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogIt(ex.GetBaseException().ToString(), @"s3cr3tx.UploadController.Upload");
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}
        private static void StoreFileLocation(string strOwner, string strOrigName, string strDirPath, string strDbPath, string strVirtualPath, string strDescription)
        { //@"s3cr3tx.api.ValuesController"
            string strConnection = @"Data Source=.;Integrated Security=SSPI;Initial Catalog=s3cr3tx";
            SqlConnection sql = new SqlConnection(strConnection);
            SqlCommand command = new SqlCommand();
            command.CommandText = @"dbo.insertFile";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter p3 = new SqlParameter(@"fileOwner", strOwner);
            SqlParameter p4 = new SqlParameter(@"fileOrigName", strOrigName);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            SqlParameter p5 = new SqlParameter(@"fileDirPath", strDirPath);
            SqlParameter p6 = new SqlParameter(@"fileDbPath", strDbPath);
            SqlParameter p7 = new SqlParameter(@"fileVirtualPath", strVirtualPath);
            SqlParameter p8 = new SqlParameter(@"fileDescription", strDescription);
            command.Parameters.Add(p5);
            command.Parameters.Add(p6);
            command.Parameters.Add(p7);
            command.Parameters.Add(p8);
            using (sql)
            {
                sql.Open();
                command.Connection = sql;
                int result = command.ExecuteNonQuery();
            }
        }
        private static void LogIt(string strMessage, string strSource)
        { //@"s3cr3tx.api.ValuesController"
            string strConnection = @"Data Source=.;Integrated Security=SSPI;Initial Catalog=s3cr3tx";
            SqlConnection sql = new SqlConnection(strConnection);
            SqlCommand command = new SqlCommand();
            command.CommandText = @"dbo.insertLog";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter p3 = new SqlParameter(@"Source", strSource);
            SqlParameter p4 = new SqlParameter(@"logMessage", strMessage);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            using (sql)
            {
                sql.Open();
                command.Connection = sql;
                int result = command.ExecuteNonQuery();
            }
        }
        private static bool isAuthorized(string strAuth, string strApiKey, string strEmail)
        { //@"s3cr3tx.api.ValuesController"
            try
            {
                string strConnection = @"Data Source=.;Integrated Security=SSPI;Initial Catalog=s3cr3tx";
                SqlConnection sql = new SqlConnection(strConnection);
                SqlCommand command = new SqlCommand();
                command.CommandText = @"dbo.checkAuth";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter p3 = new SqlParameter(@"APIKey", strApiKey);
                SqlParameter p4 = new SqlParameter(@"Authorization", strAuth);
                SqlParameter p5 = new SqlParameter(@"email", strEmail);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                bool blnResult = false;
                using (sql)
                {
                    sql.Open();
                    command.Connection = sql;
                    int intResult = (int)command.ExecuteScalar();
                    //int intResult = dataReader.GetFieldValue<int>(0);
                    //LogIt(@"intResult is: " + intResult.ToString(), @"isAuthorized.ExecuteScalar result of dbo.checkAuth");
                    if (intResult > 0)
                    {
                        blnResult = true;
                    }
                }
                return blnResult;
            }
            catch (Exception ex)
            {
                LogIt(ex.GetBaseException().ToString(), @"s3cr3tx.UploadController.isAuthorized");
                return false;
            }
        }
    }
}
