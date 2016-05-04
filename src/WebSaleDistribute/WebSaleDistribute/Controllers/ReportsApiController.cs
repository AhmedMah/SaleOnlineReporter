﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Dapper;
using System.Globalization;
using WebSaleDistribute.Core;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace WebSaleDistribute.Controllers
{
    public class ReportsApiController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: api/Reports
        [Route("Reports/GetInvoiceRemainChart")]
        public IEnumerable<dynamic> GetInvoiceRemainChart()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());                       

            var sqlConn = AdoManager.ConnectionManager.Find(Properties.Settings.Default.SaleTabriz).SqlConn;
            var result = sqlConn.Query("sp_GetInvoiceRemainChart",
                new { EmployeeID = currentUser.UserName, EmployeeTypeid = currentUser.EmployeeType, RunDate = DateTime.Now.GetPersianDate() },
                commandType: System.Data.CommandType.StoredProcedure);

            return result;
        }


        // GET: api/Reports/GetOfficerOrderStatisticsChart
        [Route("Reports/GetOfficerOrderStatisticsChart")]
        public async Task<IHttpActionResult> GetOfficerOrderStatisticsChart()
        {
            var routParams = Request.GetQueryStrings();
            var fromDate = "1395/02/13"; // routParams.ContainsKey("fromDate") ? routParams["fromDate"] : DateTime.Now.GetPersianDate();
            var toDate = routParams.ContainsKey("toDate") ? routParams["toDate"] : fromDate;

            var sqlConn = AdoManager.ConnectionManager.Find(Properties.Settings.Default.SaleTabriz).SqlConn;
            var result = await sqlConn.QueryAsync("sp_GetOfficerOrderStatisticsChart",
                new { FromDate = fromDate, ToDate = toDate },
                commandType: System.Data.CommandType.StoredProcedure);

            return Ok(result);
        }


        // GET: api/Reports/GetOrderStatisticsChart/{officerEmployeeId}
        [Route("Reports/GetOrderStatisticsChart/{officerEmployeeId}")]
        public async Task<IHttpActionResult> GetOrderStatisticsChart(int officerEmployeeId)
        {
            var routParams = Request.GetQueryStrings();
            var fromDate = "1395/02/13"; //routParams.ContainsKey("fromDate") ? routParams["fromDate"] : DateTime.Now.GetPersianDate();
            var toDate = routParams.ContainsKey("toDate") ? routParams["toDate"] : fromDate;

            var sqlConn = AdoManager.ConnectionManager.Find(Properties.Settings.Default.SaleTabriz).SqlConn;
            var result = await sqlConn.QueryAsync("sp_GetOrderStatisticsChart",
                new { FromDate = fromDate, ToDate = toDate, OfficerEmployeeID = officerEmployeeId },
                commandType: System.Data.CommandType.StoredProcedure);

            return Ok(result);
        }
    }
}
