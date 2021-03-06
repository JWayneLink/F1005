﻿using F1005.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace F1005.Areas.Overview.Controllers
{
    public class MyAssetController : Controller
    {
        private MyInvestEntities db = new MyInvestEntities();

        // GET: Overview/MyAsset
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToRoute("Default", new { Controller = "Home", Action = "Index" });
            }

            return View();
        }

        // GET: Overview/MyAsset/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Overview/MyAsset/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Overview/MyAsset/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Overview/MyAsset/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Overview/MyAsset/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Overview/MyAsset/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Overview/MyAsset/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //====================================

        //Get Asset Percentage
        public ActionResult GetPie()
        {
            string username = Convert.ToString(Session["User"]);
            var query = db.UsersData.ToList().Where(c => c.UserName == username).Select(c => new PercentageViewModel
            {
                CashValueP = ((decimal)c.CashValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue)*100).ToString("f2"),
                StockValueP = ((decimal)c.StockValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),
                FXValueP = ((decimal)c.FXValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),
                InsuranceValueP = ((decimal)c.InsuranceValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),
                FundValueP = ((decimal)c.FundValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),

                CashValue = ((decimal)c.CashValue),
                StockValue = ((decimal)c.StockValue),
                FXValue = ((decimal)c.FXValue),
                InsuranceValue = ((decimal)c.InsuranceValue),
                FundValue = ((decimal)c.FundValue)
            });

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //匯出Excel
        public ActionResult ExportToExcel()
        {
            string username = Convert.ToString(Session["User"]);
            var gv = new GridView();
            gv.DataSource = db.UsersData.ToList().Where(c => c.UserName == username).Select(c => new PercentageViewModel
            {
                CashValueP = ((decimal)c.CashValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),
                StockValueP = ((decimal)c.StockValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),
                FXValueP = ((decimal)c.FXValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),
                InsuranceValueP = ((decimal)c.InsuranceValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),
                FundValueP = ((decimal)c.FundValue / (decimal)(c.CashValue + c.StockValue + c.FXValue + c.InsuranceValue + c.FundValue) * 100).ToString("f2"),

                CashValue = ((decimal)c.CashValue),
                StockValue = ((decimal)c.StockValue),
                FXValue = ((decimal)c.FXValue),
                InsuranceValue = ((decimal)c.InsuranceValue),
                FundValue = ((decimal)c.FundValue)

            });
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyInvest_report.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        //發送Email
        public ActionResult SendEmail()
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new NetworkCredential("msit119.finalterm@gmail.com", "msit1199");
            client.EnableSsl = true;
            MailMessage msg = new MailMessage("msit119.finalterm@gmail.com", "msit119.one@gmail.com");
            msg.Subject = "我的財務報表";
            msg.Body = "我的財務報表，請查收附件檔";
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(@"C:\Users\III\Downloads\MyInvest_report.xls");
            msg.Attachments.Add(attachment);

            client.Send(msg);
            return View("Index");
        }


    }
}
