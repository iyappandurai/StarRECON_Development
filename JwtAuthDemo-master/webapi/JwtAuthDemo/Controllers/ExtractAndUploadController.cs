//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using JwtAuthDemo.Models;
//using RECONFunctionDB;


//namespace JwtAuthDemo.Controllers
//{
//   // RECONFunctionsDB  = new RECONFunctionsDB();
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ExtractAndUploadController : Controller
//    {
//        RECONFunctionsDB db = new RECONFunctionsDB();
//        private readonly IDapper _dapper;
     
//        public ExtractAndUploadController(IDapper dapper)
//        {
//            _dapper = dapper;
//        }
       
//        [HttpGet("GetExtractUploadDetails")]
//        public List<ExtractAndUpload> GetExtractUploadDetails(string OpType)
//        {
//            string Fin = "F2";
//            List<ExtractAndUpload> extrct = new List<ExtractAndUpload>();
//            try
//            {
//                string selectquery = "SELECT FNAME AS FILENAME,TOTAL_REC_COUNT AS TOTALCOUNT,EXTRACT_REC_COUNT AS SUCCESSCOUNT,REJECT_REC_COUNT AS REJECTEDCOUNT,STATUS AS STATUS," + db.SystemDate("FDATE", "dd-mm-yyyy", 2) + " FILEDATE FROM RECON_EXTRACTINFO WHERE " + db.SystemDate("EXTRACTDATE", "dd-mm-yyyy", 2) + "=" + db.SystemDate("dd-mm-yyyy", 2) + " AND FIN_ID='"+Fin+"' ORDER BY FILEDATE DESC";
//                return extrct=_dapper.GetAll<ExtractAndUpload>(selectquery, null, commandType: CommandType.Text);  
                
//            }
//            catch(Exception ex)
//            {
//                return null;
//            }
//            //try
//            //{
//            //    string Status = string.Empty;
//            //    DataTable dt = new DataTable();
//            //    if (HttpContext.Current.Session["General_DB"].ToString() == "1")
//            //    {
//            //        if (OpType == "Extract")
//            //            Qry = "Select FNAME as FileName,TOTAL_REC_COUNT as TotalCount,EXTRACT_REC_COUNT as SuccessCount,REJECT_REC_COUNT as RejectedCount,STATUS as Status,to_char(FDATE,'dd-mm-yyyy') as FileDate,'' as \"Size\" From RECON_EXTRACTINFO where to_char(EXTRACTDATE,'dd-mm-yyyy')=to_char(sysdate,'dd-mm-yyyy') and fin_id='" + HttpContext.Current.Session["FinId"].ToString() + "'";
//            //        else if (OpType == "Upload")
//            //            Qry = "Select FNAME as FileName,TOTAL_REC_COUNT as  TotalCount,UPLOAD_REC_COUNT as  SuccessCount,REJECT_REC_COUNT as  RejectedCount,STATUS,to_char(FDATE,'dd-mm-yyyy') as FileDate,'' as \"Size\" From RECON_UPLOADINFO where to_char(UPLOADDATE,'dd-mm-yyyy')=to_char(sysdate,'dd-mm-yyyy') and fin_id='" + HttpContext.Current.Session["FinId"].ToString() + "'";
//            //        OracleDAL DAL = new OracleDAL(HttpContext.Current.Session);
//            //        DAL.ReturnDataTable(Qry, CommandType.Text, dt);
//            //    }
//            //    else if (HttpContext.Current.Session["General_DB"].ToString() == "2")
//            //    {
//            //        if (OpType == "Extract")
//            //            Qry = "SELECT FNAME FILENAME,TOTAL_REC_COUNT TOTALCOUNT,EXTRACT_REC_COUNT SUCCESSCOUNT,REJECT_REC_COUNT REJECTEDCOUNT,STATUS STATUS,CONVERT(VARCHAR,FDATE,103) FILEDATE,'' SIZE FROM RECON_EXTRACTINFO WHERE CONVERT(VARCHAR,EXTRACTDATE,112)=CONVERT(VARCHAR,GETDATE(),112) AND FIN_ID='" + HttpContext.Current.Session["FinId"].ToString() + "' ORDER BY FILEDATE DESC";
//            //        else
//            //            Qry = "SELECT FNAME FILENAME,TOTAL_REC_COUNT TOTALCOUNT,UPLOAD_REC_COUNT SUCCESSCOUNT,REJECT_REC_COUNT REJECTEDCOUNT,STATUS,CONVERT(VARCHAR,FDATE,103) FILEDATE,'' SIZE FROM RECON_UPLOADINFO WHERE CONVERT(VARCHAR,UPLOADDATE,112)=CONVERT(VARCHAR,GETDATE(),112) AND FIN_ID='" + HttpContext.Current.Session["FinId"].ToString() + "' ORDER BY FILEDATE DESC";
//            //        SqlDAL DAL = new SqlDAL(HttpContext.Current.Session);
//            //        DAL.ReturnDataTable(Qry, CommandType.Text, dt);
//            //    }
//            //    if (Directory.Exists((OpType == "Extract" ? ExtractFilePath : UploadFilePath)))
//            //    {
//            //        DirectoryInfo dir = new DirectoryInfo((OpType == "Extract" ? ExtractFilePath : UploadFilePath));
//            //        foreach (FileInfo fi in dir.GetFiles())
//            //        {
//            //            System.Data.DataRow drFile = dt.NewRow();
//            //            drFile["FileName"] = fi.Name;
//            //            drFile["Status"] = (OpType == "Extract" ? "Not Extracted" : "Not Uploaded");
//            //            drFile["Size"] = CalculateFileSize(fi.Length);
//            //            dt.Rows.Add(drFile);
//            //        }
//            //        //dt.DefaultView.Sort = "[Size] DESC";
//            //        gv.DataSource = dt;
//            //        if (dt.Rows.Count > 0)
//            //            gv.DataBind();
//            //    }
//            //}
//            //catch (Exception Ex)
//            //{
//            //    Write(Ex.Message.ToString());
//            //}
//        }
//    }
//}
