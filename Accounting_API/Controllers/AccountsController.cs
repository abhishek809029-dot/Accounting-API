using ClosedXML.Excel;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebAtithya.Models;

namespace Accounting_API.Controllers
{
    public class AccountsController : Controller
    {
        ConnectionClass cs = new ConnectionClass();
        SqlConnection scon = null; SqlCommand cmd = null; SqlTransaction strans = null;

        public ActionResult APIPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserSignUp(string UserName, string Password)
        {
            try
            {
                string query = $@"SELECT 'UI' + CAST(ISNULL(MAX(CAST(SUBSTRING(Code, 3, LEN(Code)) AS INT)), 0) + 1 AS VARCHAR(10)) AS NextCode FROM UserInfo";
                string Code = cs.execute_scaler_string(query);

                query = $"Insert into UserInfo (Code,UserName,Password) values ('{Code}','{UserName.ToUpper()}','{Password}')";
                int result = cs.execute_nonquery(query);
                return cs.JsonContent(new { success = result > 0, message = result > 0 ? "Saved Successfully ..." : "Somethibg we wrong ..." });
            }
            catch (Exception ex)
            {
                return cs.JsonContent(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UserLogin(string UserName, string Password)
        {
            try
            {
                string query = $"Select * from UserInfo where UserName = '{UserName}' and Password = '{Password}'";
                DataTable UserDT = cs.FillDatatable(query);
                return cs.JsonContent(new { success = UserDT.Rows.Count > 0, message = UserDT.Rows.Count > 0 ? "Login Successfully ..." : "Incorrect username or password ..." });
            }
            catch (Exception ex)
            {
                return cs.JsonContent(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ReasonMaster(string mode, string SearchCode, string Name, string UserName)
        {
            try
            {
                string query = string.Empty;
                if (mode == "save")
                {
                    if (string.IsNullOrEmpty(SearchCode))
                    {
                        query = $"Insert into ReasonMast (Code,Name,UserName) values ('{cs.getMaxCode("RM", "ReasonMast")}','{Name}','{UserName}')";
                        int result = cs.execute_nonquery(query);
                        object response = new
                        {
                            success = result > 0,
                            message = result > 0 ? "Saved Successfully ..." : "Somethibg we wrong ..."
                        };
                        return cs.JsonContent(response);
                    }
                    else
                    {
                        query = $"Update ReasonMast set Name = '{Name}' where Code = '{SearchCode}'";
                        int result = cs.execute_nonquery(query);
                        object response = new
                        {
                            success = result > 0,
                            message = result > 0 ? "Updated Successfully ..." : "Somethibg we wrong ..."
                        };
                        return cs.JsonContent(response);
                    }
                }
                else if (mode == "find")
                {
                    DataTable dt = cs.FillDatatable("Select Code,Name from ReasonMast Where UserName = '" + UserName + "'");
                    int result = dt.Rows.Count;
                    object response = new
                    {
                        success = result > 0,
                        message = result > 0 ? "Fetched Successfully ..." : "Data not found ...",
                        data = result > 0 ? dt : null
                    };
                    return cs.JsonContent(response);
                }
                else if (mode == "delete")
                {
                    query = $"Delete from ReasonMast where Code = '{SearchCode}'";
                    int result = cs.execute_nonquery(query);
                    object response = new
                    {
                        success = result > 0,
                        message = result > 0 ? "Deleted Successfully ..." : "Somethibg we wrong ..."
                    };
                    return cs.JsonContent(response);
                }
                else
                {
                    object response = new
                    {
                        success = false,
                        message = "Mode is not Correct ..."
                    };
                    return cs.JsonContent(response);
                }
            }
            catch (Exception ex)
            {
                object response = new
                {
                    success = false,
                    message = ex.Message
                };
                return cs.JsonContent(response);
            }
        }

        [HttpPost]
        public ActionResult CategoryMaster(string mode, string SearchCode, string Name, string UserName)
        {
            try
            {
                string query = string.Empty;
                if (mode == "save")
                {
                    if (string.IsNullOrEmpty(SearchCode))
                    {
                        query = $"Insert into CategoryMast (Code,Name,UserName) values ('{cs.getMaxCode("CM", "CategoryMast")}','{Name}','{UserName}')";
                        int result = cs.execute_nonquery(query);
                        object response = new
                        {
                            success = result > 0,
                            message = result > 0 ? "Saved Successfully ..." : "Somethibg we wrong ..."
                        };
                        return cs.JsonContent(response);
                    }
                    else
                    {
                        query = $"Update CategoryMast set Name = '{Name}' where Code = '{SearchCode}'";
                        int result = cs.execute_nonquery(query);
                        object response = new
                        {
                            success = result > 0,
                            message = result > 0 ? "Updated Successfully ..." : "Somethibg we wrong ..."
                        };
                        return cs.JsonContent(response);
                    }
                }
                else if (mode == "find")
                {
                    DataTable dt = cs.FillDatatable("Select Code,Name from CategoryMast Where UserName = '" + UserName + "'");
                    int result = dt.Rows.Count;
                    object response = new
                    {
                        success = result > 0,
                        message = result > 0 ? "Fetched Successfully ..." : "Data not found ...",
                        data = result > 0 ? dt : null
                    };
                    return cs.JsonContent(response);
                }
                else if (mode == "delete")
                {
                    query = $"Delete from CategoryMast where Code = '{SearchCode}'";
                    int result = cs.execute_nonquery(query);
                    object response = new
                    {
                        success = result > 0,
                        message = result > 0 ? "Deleted Successfully ..." : "Somethibg we wrong ..."
                    };
                    return cs.JsonContent(response);
                }
                else
                {
                    object response = new
                    {
                        success = false,
                        message = "Mode is not Correct ..."
                    };
                    return cs.JsonContent(response);
                }
            }
            catch (Exception ex)
            {
                object response = new
                {
                    success = false,
                    message = ex.Message
                };
                return cs.JsonContent(response);
            }
        }

        [HttpPost]
        public ActionResult PaymentTypeMaster(string mode, string SearchCode, string Name, string UserName)
        {
            try
            {
                string query = string.Empty;
                if (mode == "save")
                {
                    if (string.IsNullOrEmpty(SearchCode))
                    {
                        query = $"Insert into PaymentTypeMast (Code,Name,UserName) values ('{cs.getMaxCode("PM", "PaymentTypeMast")}','{Name}','{UserName}')";
                        int result = cs.execute_nonquery(query);
                        object response = new
                        {
                            success = result > 0,
                            message = result > 0 ? "Saved Successfully ..." : "Somethibg we wrong ..."
                        };
                        return cs.JsonContent(response);
                    }
                    else
                    {
                        query = $"Update PaymentTypeMast set Name = '{Name}' where Code = '{SearchCode}'";
                        int result = cs.execute_nonquery(query);
                        object response = new
                        {
                            success = result > 0,
                            message = result > 0 ? "Updated Successfully ..." : "Somethibg we wrong ..."
                        };
                        return cs.JsonContent(response);
                    }
                }
                else if (mode == "find")
                {
                    DataTable dt = cs.FillDatatable("Select Code,Name from PaymentTypeMast Where UserName = '" + UserName + "'");
                    int result = dt.Rows.Count;
                    object response = new
                    {
                        success = result > 0,
                        message = result > 0 ? "Fetched Successfully ..." : "Data not found ...",
                        data = result > 0 ? dt : null
                    };
                    return cs.JsonContent(response);
                }
                else if (mode == "delete")
                {
                    query = $"Delete from PaymentTypeMast where Code = '{SearchCode}'";
                    int result = cs.execute_nonquery(query);
                    object response = new
                    {
                        success = result > 0,
                        message = result > 0 ? "Deleted Successfully ..." : "Somethibg we wrong ..."
                    };
                    return cs.JsonContent(response);
                }
                else
                {
                    object response = new
                    {
                        success = false,
                        message = "Mode is not Correct ..."
                    };
                    return cs.JsonContent(response);
                }
            }
            catch (Exception ex)
            {
                object response = new
                {
                    success = false,
                    message = ex.Message
                };
                return cs.JsonContent(response);
            }
        }

        [HttpPost]
        public ActionResult AccountEntry(string mode, string SearchCode, DateTime Date, string UserName, double Debit, double Credit, string Comment, string ReasonCode, string CategoryCode, string PayTypeCode)
        {
            scon = cs.GetConnection();
            strans = scon.BeginTransaction();
            try
            {
                string query = string.Empty;
                if (mode == "add")
                {
                    query = $@"Insert into AccountEntry (Code,Date,Debit,Credit,Comment,ReasonCode,CategoryCode,PaymentTypeCode,UserName,U_EntryDt) 
                                        values ('{cs.getMaxCode("AE", "AccountEntry")}','{Date.ToString("dd/MMM/yyyy")}',{Debit},{Credit},'{Comment}','{ReasonCode}','{CategoryCode}','{PayTypeCode}','{UserName}',getDate())";
                    cmd = new SqlCommand(query, scon)
                    {
                        Transaction = strans
                    };
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    strans.Commit();
                    scon.Close();
                    return cs.JsonContent(new { success = true, message = "Saved Successfully ..." });
                }
                else if (mode == "edit")
                {
                    query = $@"Update AccountEntry set Date = '{Date.ToString("dd/MMM/yyyy")}', Debit = {Debit}, Credit = {Credit}, Comment = '{Comment}', ReasonCode = '{ReasonCode}', CategoryCode = '{CategoryCode}', PaymentTypeCode = '{PayTypeCode}'
                            where Code = '{SearchCode}' and UserName = '{UserName}'";
                    cmd = new SqlCommand(query, scon)
                    {
                        Transaction = strans
                    };
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    strans.Commit();
                    scon.Close();
                    return cs.JsonContent(new { success = true, message = "Updated Successfully ..." });
                }
                else if (mode == "find")
                {
                    DataTable dt = cs.FillDatatable("Select Distinct FORMAT(Date,'dd/MMM/yyyy') as Date from AccountEntry Where UserName = '" + UserName + "' Order By Date Desc");
                    int result = dt.Rows.Count;
                    return cs.JsonContent(new { success = result > 0, message = result > 0 ? "Fetched Successfully ..." : "Data not found ...", data = result > 0 ? dt : null });
                }
                else if (mode == "delete")
                {
                    query = $"Delete from AccountEntry where Code = '{SearchCode}' and UserName = '{UserName}'";
                    int result = cs.execute_nonquery(query);
                    object response = new
                    {
                        success = result > 0,
                        message = result > 0 ? "Deleted Successfully ..." : "Somethibg we wrong ..."
                    };
                    return cs.JsonContent(response);
                }
                else if (mode == "datefind")
                {
                    query = "Select a.Code,FORMAT(a.Date,'dd/MMM/yyyy') as Date,a.Credit,a.Debit,a.Comment,a.ReasonCode,r.Name as ReasonName,a.CategoryCode,c.Name as CategoryName,a.PaymentTypeCode,p.Name as PayTypeName " +
                            "from AccountEntry a join ReasonMast r on a.ReasonCode = r.Code join CategoryMast c on a.CategoryCode = c.Code join PaymentTypeMast p on a.PaymentTypeCode = p.Code " +
                            "where a.Date = '" + Date.ToString("dd/MMM/yyyy") + "' and a.UserName = '" + UserName + "'";
                    DataTable dt = cs.FillDatatable(query);
                    int result = dt.Rows.Count;
                    return cs.JsonContent(new { success = true, message = result > 0 ? "Fetched Successfully ..." : "Data not found ...", data = result > 0 ? dt : null });
                }
                else
                {
                    object response = new
                    {
                        success = false,
                        message = "Mode is not Correct ..."
                    };
                    return cs.JsonContent(response);
                }
            }
            catch (Exception ex)
            {
                strans.Rollback();
                scon.Close();
                return cs.JsonContent(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult FetchAccountEntryData(string UserName)
        {
            try
            {
                DataTable Category = cs.FillDatatable($"Select Code,Name from CategoryMast Where UserName = '{UserName}' Order By Name");
                DataTable PaymentType = cs.FillDatatable($"Select Code,Name from PaymentTypeMast Where UserName = '{UserName}' Order By Name");
                DataTable Reason = cs.FillDatatable($"Select Code,Name from ReasonMast Where UserName = '{UserName}' Order By Name");
                return cs.JsonContent(new { Category = Category, PaymentType = PaymentType, Reason = Reason });
            }
            catch (Exception ex)
            {
                return cs.JsonContent(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetDashboardData(int Month, int Year, string ReasonCode, string CategoryCode, string PayTypeCode, string UserName)
        {
            try
            {
                string Condition = string.Empty;
                if (!string.IsNullOrEmpty(ReasonCode))
                    Condition += $" and a.ReasonCode = '{ReasonCode}'";
                if (!string.IsNullOrEmpty(CategoryCode))
                    Condition += $" and a.CategoryCode = '{CategoryCode}'";
                if (!string.IsNullOrEmpty(PayTypeCode))
                    Condition += $" and a.PaymentTypeCode = '{PayTypeCode}'";
                string query = $@"Select a.Code,FORMAT(a.Date,'dd/MMM/yyyy') as Date,a.Credit,a.Debit,a.Comment,r.Name as ReasonName,c.Name as CategoryName,p.Name as PayTypeName
                                from AccountEntry a left join ReasonMast r on a.ReasonCode = r.Code join CategoryMast c on a.CategoryCode = c.Code join PaymentTypeMast p on a.PaymentTypeCode = p.Code 
                                where Month(Date) = {Month} and Year(Date) = {Year} {Condition} and a.UserName = '{UserName}' Order By Date";
                DataTable dt = cs.FillDatatable(query);
                int result = dt.Rows.Count;
                return cs.JsonContent(new { success = result > 0, message = result > 0 ? "Fetched Successfully ..." : "Data not found ...", data = result > 0 ? dt : null });
            }
            catch (Exception ex)
            {
                return cs.JsonContent(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult InsertFromExcel(HttpPostedFileBase ExcelFile, string UserName)
        {
            string query = string.Empty, LineNo = string.Empty;
            try
            {
                if (ExcelFile == null || ExcelFile.ContentLength == 0)
                    return cs.JsonContent(new { success = false, message = "No file uploaded ..." });

                ExcelPackage.License.SetNonCommercialPersonal("Accounts");

                using (var package = new ExcelPackage(ExcelFile.InputStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    if (worksheet.Dimension == null)
                        return cs.JsonContent(new { success = false, message = "Worksheet is empty ..." });

                    scon = cs.GetConnection();
                    strans = scon.BeginTransaction();

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        LineNo = worksheet.Cells[row, 1].Text;
                        DateTime date = Convert.ToDateTime(worksheet.Cells[row, 2].Text);
                        double debit = string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text) ? 0 : Convert.ToDouble(worksheet.Cells[row, 3].Text);
                        double credit = string.IsNullOrWhiteSpace(worksheet.Cells[row, 4].Text) ? 0 : Convert.ToDouble(worksheet.Cells[row, 4].Text);
                        string reason = worksheet.Cells[row, 5].Text;
                        string comment = worksheet.Cells[row, 5].Comment?.Text ?? "";
                        string category = worksheet.Cells[row, 6].Text;
                        string paytype = worksheet.Cells[row, 7].Text;

                        string ReasonCode = cs.execute_scaler_string("Select Code from ReasonMast Where Name = '" + reason + "' and UserName = '" + UserName + "'");
                        if (string.IsNullOrEmpty(ReasonCode) && !string.IsNullOrEmpty(reason))
                        {
                            ReasonMaster("save", "", reason, UserName);
                            ReasonCode = cs.execute_scaler_string("Select Code from ReasonMast Where Name = '" + reason + "' and UserName = '" + UserName + "'");
                        }

                        string CategoryCode = cs.execute_scaler_string("Select Code from CategoryMast Where Name = '" + category + "' and UserName = '" + UserName + "'");
                        if (string.IsNullOrEmpty(CategoryCode))
                        {
                            CategoryMaster("save", "", category, UserName);
                            CategoryCode = cs.execute_scaler_string("Select Code from CategoryMast Where Name = '" + category + "' and UserName = '" + UserName + "'");
                        }

                        string PayTypeCode = cs.execute_scaler_string("Select Code from PaymentTypeMast Where Name = '" + paytype + "' and UserName = '" + UserName + "'");
                        if (string.IsNullOrEmpty(PayTypeCode))
                        {
                            PaymentTypeMaster("save", "", paytype, UserName);
                            PayTypeCode = cs.execute_scaler_string("Select Code from PaymentTypeMast Where Name = '" + paytype + "' and UserName = '" + UserName + "'");
                        }

                        query = $@"SELECT 'AE' + CAST(ISNULL(MAX(CAST(SUBSTRING(Code, 3, LEN(Code)) AS INT)), 0) + 1 AS VARCHAR(10)) AS NextCode FROM AccountEntry";
                        cmd = new SqlCommand(query, scon)
                        {
                            Transaction = strans
                        };
                        var result = cmd.ExecuteScalar();
                        string code = (result == null || result == DBNull.Value) ? "" : result.ToString();

                        query = $@"Insert into AccountEntry (Code,Date,Debit,Credit,Comment,ReasonCode,CategoryCode,PaymentTypeCode,UserName,U_EntryDt) 
                                        values ('{code}','{date.ToString("dd/MMM/yyyy")}',{debit},{credit},'{comment}','{ReasonCode}','{CategoryCode}','{PayTypeCode}','{UserName}',getDate())";
                        cmd = new SqlCommand(query, scon)
                        {
                            Transaction = strans
                        };
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    strans.Commit();
                    scon.Close();
                    return cs.JsonContent(new { success = true, message = "Inserted Successfully ..." });
                }
            }
            catch (Exception ex)
            {
                strans.Rollback();
                scon.Close();
                return cs.JsonContent(new { success = false, message = ex.Message + " in Line No. " + LineNo });
            }
        }

        [HttpPost]
        public ActionResult SundryDrExcel(int Month, int Year, string ReasonCode, string CategoryCode, string UserName)
        {
            string Condition = string.Empty;
            if (!string.IsNullOrEmpty(ReasonCode))
                Condition += $" and a.ReasonCode = '{ReasonCode}'";
            if (!string.IsNullOrEmpty(CategoryCode))
                Condition += $" and a.CategoryCode = '{CategoryCode}'";

            string query = $@"Select FORMAT(a.Date,'dd/MMM/yyyy') as Date,a.Debit,a.Credit,a.Comment as Reason from AccountEntry a where Month(Date) = {Month} and Year(Date) = {Year} {Condition} and a.UserName = '{UserName}' Order By Date";
            DataTable dt = cs.FillDatatable(query);

            string CategoryName = cs.execute_scaler_string("select Name from CategoryMast Where Code = '" + CategoryCode + "'");

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Report");

                ws.Range("A1:E1").Merge();
                ws.Cell("A1").Value = CategoryName;
                ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A1").Style.Font.FontSize = 18;

                ws.Range("A1:E1").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell(2, 1).Value = "S.No";
                ws.Cell(2, 2).Value = "Date";
                ws.Cell(2, 3).Value = "Debit";
                ws.Cell(2, 4).Value = "Credit";
                ws.Cell(2, 5).Value = "Reason";

                ws.Range("A2:E2").Style.Font.Bold = true;
                ws.Range("A2:E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 3, SNo = 1;
                int totalDebit = 0;
                int totalCredit = 0;

                foreach (DataRow item in dt.Rows)
                {
                    ws.Cell(row, 1).Value = SNo;
                    ws.Cell(row, 2).Value = item["Date"].ToString();
                    ws.Cell(row, 3).Value = item["Debit"].ToString() ?? "";
                    ws.Cell(row, 4).Value = item["Credit"].ToString() ?? "";
                    ws.Cell(row, 5).Value = item["Reason"].ToString() ?? "";

                    totalDebit += Convert.ToInt32(item["Debit"] ?? 0);
                    totalCredit += Convert.ToInt32(item["Credit"] ?? 0);

                    SNo++;
                    row++;
                }

                ws.Range(row, 1, row, 3).Merge();
                ws.Cell(row, 1).Value = "Total";
                ws.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(row, 1).Style.Font.Bold = true;
                ws.Cell(row, 1).Style.Font.FontSize = 14;

                ws.Cell(row, 4).Value = totalCredit - totalDebit;
                ws.Cell(row, 4).Style.Font.Bold = true;

                ws.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                var range = ws.Range(1, 1, row, 5);
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Demo1.xlsx");
                }
            }
        }

        [HttpPost]
        public ActionResult MonthwiseBalance(int Month, int Year, string UserName)
        {
            string query = $"select p.Name, SUM(a.Credit) - SUM(a.Debit) as Balance from AccountEntry a join PaymentTypeMast p on a.PaymentTypeCode = p.Code where Month(Date) = {Month} and Year(Date) = {Year} and a.UserName = '{UserName}' group by p.Name";
            DataTable dt = cs.FillDatatable(query);
            return cs.JsonContent(new { success = dt.Rows.Count > 0, message = dt.Rows.Count > 0 ? "Fetched Successfully ..." : "Data not Found ...", data = dt.Rows.Count > 0 ? dt : null });
        }

        [HttpPost]
        public ActionResult RemoveMonthData(int Month, int Year, string UserName, string Mode)
        {
            if (Mode == "Find")
            {
                string query = $"select distinct FORMAT(Date,'MMM-yyyy') as MonthDate from AccountEntry where UserName = '" + UserName + "'";
                DataTable dt = cs.FillDatatable(query);
                return cs.JsonContent(new { success = dt.Rows.Count > 0, message = dt.Rows.Count > 0 ? "Fetched Successfully ..." : "Data not Found ...", data = dt.Rows.Count > 0 ? dt : null });
            }
            else
            {
                string query = $"delete from AccountEntry where Month(Date) = {Month} and Year(Date) = {Year} and UserName = '{UserName}'";
                int result = cs.execute_nonquery(query);
                return cs.JsonContent(new { success = result > 0, message = result > 0 ? "Deleted Successfully ..." : "Something went wrong ..." });
            }
        }

    }
}