using System;
using System.Web.Mvc;
using Business.Business;
using Model.eAita;
using Web.FwCore;
using Web.Custom;
using Web.Common;
using System.Linq;
using System.IO;
using System.Net;
using Web.Areas.TDKTDanhHieuThiDuaArea.Models;
using System.Text;
using Elasticsearch.Net;

namespace Web.Areas.TDKTDanhHieuThiDuaArea.Controllers
{
    public class QuanLyDanhHieuCaNhanController : BaseController
    {
        TdktDanhhieucanhanBusiness TdktDanhhieucanhanBusiness;
        TdktDieukiendanhhieucanhanBusiness TdktDieukiendanhhieucanhanBusiness;
        private TdktDanhHieuCaNhanConditionBusiness TdktDanhHieuCaNhanConditionBusiness;

        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Index()
        {
            //string data_str="";
            //var local = new Uri("http://192.168.224.131:9200");
            //var settings = new ConnectionConfiguration(local);
            //var elastic = new ElasticLowLevelClient(settings);

            //string template_path = Server.MapPath("~/Content/Template/NangLuong/danh sách.doc");
            //byte[] bytes = System.IO.File.ReadAllBytes(template_path);
            //string content = System.IO.File.ReadAllText(template_path);            
            //FileElastic fileelas = new FileElastic
            //{
            //    tile_file = "duynt",
            //    file = System.Convert.ToBase64String(bytes)
            //};
            //var index = elastic.Index<FileElastic>("test", "document", fileelas);

            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            var DanhHieus = TdktDanhhieucanhanBusiness.All.ToList();
            ViewData["Search"] = "0";
            SessionManager.SetValue("DanhHieus", DanhHieus);
            return View();
        }
        public ActionResult Create()
        {
            DanhHieuCaNhanViewModel model = new DanhHieuCaNhanViewModel();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            model.ListDanhHieu = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DANHHIEUTHIDUA,
                    Value = x.ID.ToString()
                }
                ).ToList();
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult CreateDanhHieuCaNhan(FormCollection DataPost)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();

            var data = DataPost;
            int COUNT = DataPost["COUNTMAXCONDITION"].ToIntOrZero();
            string DANHHIEUTHIDUA = DataPost["DANHHIEUTHIDUA"];
            string MOTA = DataPost["MOTA"];

            TDKT_DANHHIEUCANHAN danhhieu = new TDKT_DANHHIEUCANHAN();
            danhhieu.DANHHIEUTHIDUA = DANHHIEUTHIDUA;
            danhhieu.MOTA = MOTA;
            danhhieu.YEAR = DataPost["YEAR"].ToIntOrZero();
            danhhieu.TYLE = DataPost["TYLE"].ToFloatOrZero();
            danhhieu.TYLE_DANHHIEU_ID = DataPost["TYLE_DANHHIEU_ID"].ToIntOrZero();
            danhhieu.TONG_SO_XET_CHON = DataPost["TONG_SO_XET_CHON"].ToIntOrZero();
            TdktDanhhieucanhanBusiness.Save(danhhieu);

            for (int i = 0; i <= COUNT; i++)
            {
                string tmp_key = "SONAMCONGTAC_" + i.ToString();
                if (!string.IsNullOrEmpty(DataPost[tmp_key]))
                {
                    int SONAMCONGTAC = DataPost[tmp_key].ToIntOrZero();
                    string tmp_key_snn = "SONGAYNGHI_" + i.ToString();
                    string tmp_key_skyt = "SOSANGKIEN_" + i.ToString();                    
                    if (!string.IsNullOrEmpty(DataPost[tmp_key_snn]))
                    {
                        int SONGAYNGHI = DataPost[tmp_key_snn].ToIntOrZero();
                        int SOSANGKIEN = DataPost[tmp_key_skyt].ToIntOrZero();
                        TDKT_DIEUKIENDANHHIEUCANHAN model = new TDKT_DIEUKIENDANHHIEUCANHAN();
                        model.DANHHIEUCANHAN_ID = danhhieu.ID;
                        model.SONAMCONGTAC = SONAMCONGTAC;
                        model.SONGAYNGHI = SONGAYNGHI;
                        model.SOSANGKIEN = SOSANGKIEN;
                        TdktDieukiendanhhieucanhanBusiness.Save(model);

                        string danhhieu_ids = DataPost["DANHHIEU_IDS_" + i.ToString()];
                        if (!string.IsNullOrEmpty(danhhieu_ids))
                        {
                            var LstDanhHieuId = danhhieu_ids.ToListInt(',');
                            if (!string.IsNullOrEmpty(DataPost["SOLUONGDANHHIEU_" + i.ToString()]))
                            {
                                var LstSoLuong = DataPost["SOLUONGDANHHIEU_" + i.ToString()].ToListInt(',');
                                var numberOfDh = LstDanhHieuId.Count();
                                for (var index = 0; index < numberOfDh; index++)
                                {
                                    if (LstDanhHieuId[index] > 0 && LstSoLuong[index] > 0)
                                    {
                                        string tmp_key_is_lientuc = "DATDANHHIEULIENTUC_" + index.ToString();
                                        string tmp_key_is_lienke = "THOIDIEM_LIENKE_" + index.ToString();                                        
                                        TDKT_DANH_HIEU_CA_NHAN_CONDITION tmpObj = new TDKT_DANH_HIEU_CA_NHAN_CONDITION();
                                        if (!string.IsNullOrEmpty(DataPost[tmp_key_is_lientuc]))
                                        {
                                            tmpObj.IS_LIEN_TUC = true;
                                        }
                                        if (!string.IsNullOrEmpty(DataPost[tmp_key_is_lienke]))
                                        {
                                            tmpObj.IS_LIEN_KE = true;
                                        }
                                        tmpObj.DANH_HIEU_ID = danhhieu.ID;
                                        tmpObj.COND_DANH_HIEU_ID = LstDanhHieuId[index];
                                        tmpObj.COND_SO_LUONG = LstSoLuong[index];
                                        tmpObj.DIEUKIEN_ID = model.ID;
                                        TdktDanhHieuCaNhanConditionBusiness.Save(tmpObj);
                                    }
                                }
                            }
                        }
                    //}
                }
            }
            return RedirectToAction("Index");
        }
        [ValidateInput(false)]
        public ActionResult UpdateDanhHieuCaNhan(FormCollection DataPost)
        {
            var test = DataPost;
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();
            TDKT_DANHHIEUCANHAN danhhieuObj = TdktDanhhieucanhanBusiness.Find(DataPost["ID"].ToIntOrZero());
            danhhieuObj.DANHHIEUTHIDUA = DataPost["DANHHIEUTHIDUA"];
            danhhieuObj.MOTA = DataPost["MOTA"];
            TdktDanhhieucanhanBusiness.Save(danhhieuObj);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Function view detail danh hieu thi dua khen thuong ca nhan
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ViewDetail(int ID)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();

            DanhHieuCaNhanViewModel model = new DanhHieuCaNhanViewModel();
            TDKT_DANHHIEUCANHAN danhhieucanhan = TdktDanhhieucanhanBusiness.Find(ID);
            model.DanhHieuCaNhan = danhhieucanhan;
            model.LstCondDateTime = TdktDieukiendanhhieucanhanBusiness.All.Where(x => x.DANHHIEUCANHAN_ID == ID).ToList();
            model.LstCondDanhHieu = TdktDanhHieuCaNhanConditionBusiness.getConditionDanhHieuCaNhan(ID);
            return View(model);
        }
        /// <summary>
        /// Function edit danh hieu thi dua khen thuong ca nhan
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Edit(int ID)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDieukiendanhhieucanhanBusiness = Get<TdktDieukiendanhhieucanhanBusiness>();
            TdktDanhHieuCaNhanConditionBusiness = Get<TdktDanhHieuCaNhanConditionBusiness>();
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            
            DanhHieuCaNhanViewModel model = new DanhHieuCaNhanViewModel();
            TDKT_DANHHIEUCANHAN danhhieucanhan = TdktDanhhieucanhanBusiness.Find(ID);
            model.DanhHieuCaNhan = danhhieucanhan;
            model.LstCondDateTime = TdktDieukiendanhhieucanhanBusiness.All.Where(x => x.DANHHIEUCANHAN_ID == ID).ToList();
            model.LstCondDanhHieu = TdktDanhHieuCaNhanConditionBusiness.getConditionDanhHieuCaNhan(ID);
            model.ListDanhHieu = TdktDanhhieucanhanBusiness.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DANHHIEUTHIDUA,
                    Value = x.ID.ToString()
                }
                ).ToList();
            return View(model);
        }

        public JsonResult Delete(int ID)
        {
            TdktDanhhieucanhanBusiness = Get<TdktDanhhieucanhanBusiness>();
            TdktDanhhieucanhanBusiness.Delete(ID);
            TdktDanhhieucanhanBusiness.Save();
            return Json(new { Type = "SUCCESS", Message = "ok" });
        }
    }
}
