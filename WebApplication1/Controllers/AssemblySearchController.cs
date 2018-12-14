using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Common;
using WebApplication1.Model;
using WebApplication1.Model.Dto;
using WebApplication1.Models;
using Zhongyu.Data.Extensions;
using static WebApplication1.Models.DapperService;

namespace WebApplication1.Controllers
{
    [Signin]
    public class AssemblySearchController : Controller
    {
        public static List<AssembleSearchDto> list=new List<AssembleSearchDto>();//{ get; set; }
        // 页面初次加载
        public ActionResult Index(ForAssemblyModel model,int? PageIndex, int? PageSize = 30)
        { 
            try
            {
                ViewBag.totalcount = 0;
                ViewBag.Line = CommonHelp.list;
                if (PageIndex == null && model.StartTime < model.EndTime && !string.IsNullOrEmpty(model.Area))
                {
                    model.EndTime = Convert.ToDateTime(model.EndTime);
                    model.StartTime = Convert.ToDateTime(model.StartTime);
                    model.Area = model.Area;
                    if (model.FigureNo.IsNullOrEmpty())
                    model.FigureNo="";
                    list = GetAssemblyList(model).OrderBy(p=>p.end_time).ToList();
                    foreach (var item in list)
                    {
                        item.area = model.Area; 
                    }
                    ViewBag.totalcount = list.Count;
                    ViewBag.list = list.Take(30).ToList();
                }
                if (PageIndex != null)
                {
                    var model1 = new PageInfoModel<AssembleSearchDto>();
                    model1.List=list; ;
                    model1.PageSize = PageSize;
                    model1.PageIndex = PageIndex;
                    var res = CommonHelp.PageList(model1);
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetNum(DateTime start,DateTime end,string line)
        {
            List<string> list=new List<string>(){""};
             AssemblySearch.GetNum(start, end, line).ForEach(
                 p =>
                 {
                     list.Add(p);
                 });
           
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     模糊查询
        /// </summary>
        /// <param name="Area"></param>
        /// <returns></returns>
        public ActionResult DarkSearch(string Area,string Code)
        {
                ViewBag.count = 0;
                ViewBag.Line = CommonHelp.list;
                var Successed = true;
                var Message = "";
            List<AssembleSearchDto> listDark=new List<AssembleSearchDto>();
                if (!string.IsNullOrEmpty(Area)&& !string.IsNullOrEmpty(Code))
                {
                    try
                    {
                        var code = Code.Trim();
                        listDark = DapperService.SqlHelp.DarkSearch(code, Area);
                        if (listDark.FirstOrDefault().COUNT>0)
                        {
                             Successed = false;
                             Message = "查询量较大，请输入条码后16位！";
                        }
                        foreach (var item in listDark)
                        {
                            item.area = Area;
                        }
                    }
                    catch (Exception e)
                    {
                        Successed = false;
                        Message = "数据查询有误！";
                    }
                  
                    return Json(new
                    {
                        list1 = listDark,
                        totalcount = listDark.Count,
                        Success= Successed,
                        Mess=Message
                    }, JsonRequestBehavior.AllowGet);
               
                }
            return View();
        }

        //精确查询
        public ActionResult DataSource(string Area, string Code)
        {
                var Successed = true;
                var Message = "";
                ViewBag.count = 0;
                ViewBag.Line = CommonHelp.list;
                List<AssembleSearchDto> listDataSource=new List<AssembleSearchDto>();
                if (!string.IsNullOrEmpty(Area) && !string.IsNullOrEmpty(Code))
                {
                    try
                    {
                        var code = Code.Trim();
                        listDataSource = DapperService.SqlHelp.DataSourceSearch(code, Area);
                        foreach (var item in listDataSource)
                        {
                            item.area = Area;
                        }
                        Message = "数据查询有误！";
                     }
                    catch (Exception e)
                    {
                        Successed = false;
                        Message = "成功";
                     }
                   
                    return Json(new
                    {
                        list1 = listDataSource,
                        totalcount = listDataSource.Count,
                        mess=Message,
                        success= Successed
                    }, JsonRequestBehavior.AllowGet);
                    
                }
            return View();
        }
        //查询方法
        public List<AssembleSearchDto> GetAssemblyList(ForAssemblyModel model)
        {
            var list = new List<AssembleSearchDto>();
            ViewBag.table = null;
            var forassemlymodel = new AssembleSearchModel();
            forassemlymodel.Area = model.Area;
            forassemlymodel.StarTime = model.StartTime;
            forassemlymodel.EndTime = model.EndTime;
            forassemlymodel.FigureNo = model.FigureNo;
            if (model.StartTime < model.EndTime)
            {
              
                list = SqlHelp.AssemblySearch(forassemlymodel).OrderByDescending(p => p.end_time)
                    .ToList();
                ViewBag.list = list.Take(30).ToList();
                ViewBag.count = SqlHelp.AssemblySearch(forassemlymodel).Count;
            }
            else
            {
                ViewBag.count = 0;
            }
            return list;
        }
    }
}