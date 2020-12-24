using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JwtAuthDemo.Models;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LicenseController : ControllerBase
    {
        private readonly ILogger<LicenseController> _logger;
       // private Data.Candidate cd = new Data.Candidate();
        License lic = new License();
        private readonly IDapper _dapper;
        private IHttpContextAccessor _context;
        public LicenseController(ILoggerFactory logFactory, IDapper dapper, IHttpContextAccessor context)
        {
            _logger = logFactory.CreateLogger<LicenseController>();
            _dapper = dapper;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("license")]
        public IActionResult CheckLicense()
        {
            var result = Unauthorized();
            if (System.IO.File.Exists(Path.Combine(System.IO.Directory.GetCurrentDirectory() + "\\License\\License.lic")) == false)
            {
                _logger.LogInformation("License File Not Found...");
                return NotFound(new { message = "License file not found" });
            }
            string keyDataValue = lic.readLicenseFile(Path.Combine(System.IO.Directory.GetCurrentDirectory() + "\\License\\License.lic"));
            if (keyDataValue == "")
            {
                _logger.LogInformation(":- License File Key Datas Not Found...");
                return Ok(new { message = ":- Invalid License file" });
            }
            string licenseKey = lic.getKeyValue(keyDataValue, "[LICENSE_KEY]", "LicenseKey");
            if (lic.getKeyValue(keyDataValue, "[LICENSE_KEY]", "LicenseKey") == "")
            {
                _logger.LogInformation(":- LicenseKey is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }
            if (lic.getKeyValue(keyDataValue, "[INSTITUTION]", "INST_VALUE") == "")
            {
                _logger.LogInformation(":- Institution Value is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }
            if (lic.getKeyValue(keyDataValue, "[INSTITUTION]", "INST_NAME") == "")
            {
                _logger.LogInformation(":- Institution Name is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }
            if (lic.ValidateServerKey(licenseKey) != true)
            {
                _logger.LogInformation(":- Server Key Not Matched with LicenseKey...");
                return Ok(new { message = "Invalid Server Key" });
            }
            if (lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_SENDING_DATE") == "")
            {
                _logger.LogInformation(":- Setup Sending Date is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }
            if (lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_VERSION_ID") == "")
            {
                _logger.LogInformation(":- Setup Version ID is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }
            if (lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_VERSION_FLAG") == "")
            {
                _logger.LogInformation(":- Setup Version Details is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }
            if (lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_VERSION") == "")
            {
                _logger.LogInformation(":- Setup Version is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }
            string SetupExpiryFlag = lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_CHK_EXPIRY_FLAG");
            if (SetupExpiryFlag == "")
            {
                _logger.LogInformation(":- Setup Expiry Flag is missing...");
                return Ok(new { message = ":- Invalid License file" });
            }


            if (_context.HttpContext.User != null)
            {
                if (lic.getKeyValue(keyDataValue, "[INST_F2]", "INST_DB") != "")
                {
                    string DB = lic.getKeyValue(keyDataValue, "[INST_F2]", "INST_DB");               
                    _context.HttpContext.User.Identities.FirstOrDefault().AddClaim(new Claim("DBConnection", DB));
                }

            }

            if (SetupExpiryFlag == "Y")
            {
                if (lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_EXPIRY_DAYS") == "")
                {
                    _logger.LogInformation(":- Setup Expiry Days is missing...");
                    return Ok(new { message = ":- Setup Expiry Days is missing..." });
                }
                if (lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_SENDING_DATE") == "")
                {
                    _logger.LogInformation(":- Setup SETUP_SENDING_DATE Flag is missing...");
                    return Ok(new { message = ":- Setup SETUP_SENDING_DATE Flag is missing..." });
                }
                if (lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_EXPIRY_DATE") == "")
                {
                    _logger.LogInformation(":- Setup SETUP_EXPIRY_DATE Flag is missing...");
                    return Ok(new { message = ":- Setup SETUP_EXPIRY_DATE Flag is missing..." });
                }

                /////
                string GetDBdate = string.Empty;// cd.GetDBDate();
                var result1 = _dapper.Get<string>($"SELECT SYSDATE FROM DUAL", null, commandType: CommandType.Text);
                GetDBdate = result1.ToString();
                //if (Session["General_DB"].ToString().Trim() == "1")
                //{
                //    OracleDAL DAL = new OracleDAL(HttpContext.Current.Session);
                //    Query = " SELECT TO_CHAR(SYSDATE) FROM DUAL";
                //    GetDBdate = DAL.ReturnScalar(Query, CommandType.Text);
                //}
                //else if (Session["General_DB"].ToString().Trim() == "2")
                //{
                //    SqlDAL DAL = new SqlDAL(HttpContext.Current.Session);
                //    Query = "SELECT DISTINCT CONVERT(DATE,GETDATE(),103) FROM SYS.OBJECTS";
                //    GetDBdate = DAL.ReturnScalar(Query, CommandType.Text);
                //}
                DateTime ExpDate = Convert.ToDateTime(lic.getKeyValue(keyDataValue, "[SETUP_EXPIRY]", "SETUP_EXPIRY_DATE"));
                DateTime DBDate = Convert.ToDateTime(GetDBdate);
                DateTime AppDate = Convert.ToDateTime(DateTime.Today);
                //Session["SETUPEXPIRYALERT"] = "N";
                if (DBDate == AppDate)
                {
                    int NoOfDays = Convert.ToInt32((ExpDate - DBDate).TotalDays);
                    //int AlertDays = Convert.ToInt32(Session["SETUPALERTDAYS"].ToString());
                    //if (Session["SETUPCHKEXPIRY"].ToString() == "Y" && NoOfDays <= AlertDays)
                    //{
                    //    Session["SETUPEXPIRYALERT"] = "Y";
                    //}
                }
                else
                {
                    // Write(PageName + "DP:- DATE TIME Checking ..! : Application Date : " + AppDate + ", Database Date : " + DBDate, LogFilepath);
                    //return msgConfig.GetXMLValues("L_MSG_25");
                }
                // Write("DP:- DATE TIME Checking ..! : SR : " + AppDate + ", DB : " + DBDate + ", AMC : " + ExpDate, LogFilepath);
                if (ExpDate <= AppDate)
                {
                    _logger.LogInformation(":- DP:- Setup Verification Checking with ExpDate <= AppDate..!...");
                    return Ok(new { message = "License Expired" });
                }
                else if (ExpDate <= DBDate)
                {
                    _logger.LogInformation(":- DP:- Setup Verification Checking with ExpDate <= AppDate..!...");
                    return Ok(new { message = "License Expired" });

                }
                else
                {
                    _logger.LogInformation(":- DP:- Setup Verification Checking is Success..!");

                }
                /////
            }


            return Ok(new { message = "Success" }); ;
        }










    }
}
