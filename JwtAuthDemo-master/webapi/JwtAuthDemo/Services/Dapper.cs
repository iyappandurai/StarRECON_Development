using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JwtAuthDemo.Services
{
    public class Dapperr : IDapper
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Dapperr> _logger;
        private string Connectionstring = "DBConnection";
        string db_flag = string.Empty;
        private IHttpContextAccessor _httpContext;

        public Dapperr(IConfiguration config, ILoggerFactory logFactory, IHttpContextAccessor httpContext)
        {
            _config = config;
            _logger = logFactory.CreateLogger<Dapperr>();
            _httpContext = httpContext;
        }
        public void Dispose()
        {

        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            if (GetDb() == "2")
            {
                using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
                return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
            if (GetDb() == "1")
            {
                var claimsIdentity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
                using IDbConnection db = new OracleConnection(_config.GetConnectionString(Connectionstring));
                return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
            return default(T);
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            if (GetDb() == "2")
            {
                using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
                return db.Query<T>(sp, parms, commandType: commandType).ToList();
            }
            if (GetDb() == "1")
            {
                using IDbConnection db = new OracleConnection(_config.GetConnectionString(Connectionstring));
                return db.Query<T>(sp, parms, commandType: commandType).ToList();
            }
            return null;
        }

        public DbConnection GetDbconnection()
        {
            if (GetDb() == "1")
            {
                return new OracleConnection(_config.GetConnectionString(Connectionstring));
            }
            if (GetDb() == "2")
            {
                return new SqlConnection(_config.GetConnectionString(Connectionstring));
            }
            return null;
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = GetDbconnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = GetDbconnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        private string GetDb() {
           var claimsIdentity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
            return  claimsIdentity.FindFirst("DBConnection").Value;
            
           
        }
    }
}
