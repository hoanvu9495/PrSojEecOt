using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.CommonBusiness;
using Business.Business;
using Web.Common;
using Web.Custom;
using Web.FwCore;
using Model.DBTool;
using Web.Areas.BaiVietArea.Models;


namespace Web.Areas.BaiVietArea.Controllers
{
    public class BaiVietController : BaseController
    {
        //
        // GET: /BaiVietArea/BaiViet/

        #region KhaiBao
        private SpinBaiVietBusiness SpinBaiVietBusiness;
        private SpinBaiVietGroupBusiness SpinBaiVietGroupBusiness;
        private SpinGroupWordBusiness SpinGroupWordBusiness;
        private SpinWordsBusiness SpinWordsBusiness;

        #endregion

        public ActionResult Index()
        {
            AssignUserInfo();
            var modelresult = new IndexBaiVietVM();
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            SessionManager.SetValue("SearchBaiViet", null);
            modelresult.ListBaiViet = GetData();
            return View(modelresult);
        }



        [HttpPost]
        public JsonResult reloadPage(int page)
        {
            return Json(GetData(page));
        }

        public PageListResultBO<BaiVietBO> GetData(int pageindex = 1)
        {
            AssignUserInfo();
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var searchModel = SessionManager.GetValue("SearchBaiViet") as SearchBaiVietBO;

            var ListBaiViet = SpinBaiVietBusiness.GetByUser(searchModel, currentUserId, pageindex);
            return ListBaiViet;
        }

        public PartialViewResult Create()
        {
            return PartialView("_ThemMoiBaiVietPartial");
        }

        public PartialViewResult CauHinhSpin(int id)
        {
            SpinGroupWordBusiness = Get<SpinGroupWordBusiness>();
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var model = new ImportSpinVM();
            model.LstTuSpin = SpinGroupWordBusiness.GetTuDienSys();
            model.BaiViet = SpinBaiVietBusiness.GetByID(id);
            return PartialView("_QuanLySpinPartial", model);
        }


        public PartialViewResult KeThuaCauHinhSpin(int id)
        {
            SpinGroupWordBusiness = Get<SpinGroupWordBusiness>();
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            AssignUserInfo();
            var model = new ImportSpinVM();
            model.lstBaiViet = SpinBaiVietBusiness.GetListBaiVietUser(currentUserId).Where(x => x.ID != id).ToList();
            model.BaiViet = SpinBaiVietBusiness.GetByID(id);
            return PartialView("_KeThuaPartial", model);
        }

        [HttpPost]
        public JsonResult SaveCauHinh(int idbai, List<int> lstGroup)
        {
            AssignUserInfo();
            var model = new JsonResultBO(true);
            SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
            SpinGroupWordBusiness = Get<SpinGroupWordBusiness>();
            model = SpinGroupWordBusiness.SaveListGroupForUser(currentUserId, idbai, lstGroup);


            return Json(model);
        }

        public JsonResult DeleteGroup(int idgroup, int idbaiviet)
        {
            var model = new JsonResultBO(true);
            try
            {

                SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
                SpinGroupWordBusiness = Get<SpinGroupWordBusiness>();
                SpinWordsBusiness = Get<SpinWordsBusiness>();
                var listWord = this.Context.SPIN_WORDS.Where(x => x.NHOMTU_ID == idgroup).ToList();
                if (listWord.Any())
                {
                    SpinWordsBusiness.DeleteAll(listWord);
                    SpinWordsBusiness.Save();
                }

                SpinGroupWordBusiness.Delete(idgroup);
                SpinGroupWordBusiness.Save();
                var groubai = this.Context.SPIN_BAIVIET_GROUP.Where(x => x.ID_BAI == idbaiviet && x.ID_GROUP_TU == idgroup).FirstOrDefault();
                SpinBaiVietGroupBusiness.Delete(groubai.ID);
                SpinBaiVietGroupBusiness.Save();

            }
            catch
            {

                model.Status = false;
                model.Message = "Không xóa được nhóm từ";
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult saveGroup(int idgroup, string name, string words)
        {
            AssignUserInfo();
            var model = new JsonResultBO(true);
            SpinWordsBusiness = Get<SpinWordsBusiness>();
            SpinGroupWordBusiness = Get<SpinGroupWordBusiness>();
            var lstWord = SpinWordsBusiness.getListByGroup(idgroup);
            var groupp = SpinGroupWordBusiness.Find(idgroup);
            groupp.NAME = name;
            groupp.NGAYSUA = DateTime.Now;
            groupp.NGUOISUA = currentUserId;
            SpinGroupWordBusiness.Save(groupp);
            if (lstWord.Any())
            {
                SpinWordsBusiness.DeleteAll(lstWord);
                SpinWordsBusiness.Save();
            }
            var lstWordNew = words.Split(',');
            foreach (var item in lstWordNew)
            {
                var w = new SPIN_WORDS();
                w.ID_USER = currentUserId;
                w.NGAYTAO = DateTime.Now;
                w.NGUOITAO = currentUserId;
                w.NHOMTU_ID = idgroup;
                w.TU_CUMTU = item;
                w.CHUDE = 2;
                SpinWordsBusiness.Save(w);
            }
            return Json(model);
        }

        public PartialViewResult TaoBai(int id)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var bv = SpinBaiVietBusiness.Find(id);
            SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
            var modelResult = new TaoBaiVM();
            modelResult.BaiViet = bv;
            var lstTuDien = SpinBaiVietGroupBusiness.GetTuDienSpin(id);
            modelResult.ContentEdit = SpinProvider.GenContentEdit(lstTuDien, bv.NOIDUNG);
            return PartialView("_TaoBaiSpinPartial", modelResult);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveBaiViet(int id, string content)
        {
            AssignUserInfo();
            var model = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();

            var bvorigin = SpinBaiVietBusiness.Find(id);
            var bv = new SPIN_BAIVIET();
            bv.NOIDUNG = content;
            bv.NGAYTAO = DateTime.Now;
            bv.NGAYSUA = DateTime.Now;
            bv.NGUOISUA = currentUserId;
            bv.NGUOITAO = currentUserId;
            bv.IS_ORIGIN = false;
            bv.USER_ID = currentUserId;
            bv.EXTEND_OF = id;
            bv.TIEUDE = bvorigin.TIEUDE;
            SpinBaiVietBusiness.Save(bv);
            bv.TIEUDE += "_CLONE_" + bv.ID.ToString();
            SpinBaiVietBusiness.Save(bv);
            return Json(model);
        }


        [HttpPost]
        public JsonResult GetContentResult(int idBai, List<ParamReplaceSpinBO> param)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
            var bv = SpinBaiVietBusiness.Find(idBai);
            var lstTuDien = SpinBaiVietGroupBusiness.GetTuDienSpin(bv.ID);
            var marks = SpinProvider.GenMark(lstTuDien, bv.NOIDUNG);
            return Json(SpinProvider.GetResultContent(param, marks.Content));
        }
        public PartialViewResult Edit(int id)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var model = SpinBaiVietBusiness.Find(id);
            return PartialView("_CapNhatBaiVietPartial", model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Insert(FormCollection form)
        {
            var resultModel = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            AssignUserInfo();
            var bv = new SPIN_BAIVIET();
            bv.TIEUDE = form["TIEUDEBaiViet"];
            bv.NOIDUNG = form["NOIDUNGBaiViet"];
            bv.NGAYTAO = DateTime.Now;
            bv.NGUOITAO = currentUserId;
            bv.NGAYSUA = DateTime.Now;
            bv.NGUOISUA = currentUserId;
            bv.IS_ORIGIN = true;
            bv.USER_ID = currentUserId;
            SpinBaiVietBusiness.Save(bv);
            return Json(resultModel);
        }

        public ActionResult QuanLySpinBaiViet(int id)
        {
            SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var model = new DataSpinVM();
            model.DataSpin = SpinBaiVietGroupBusiness.GetTuDienSpin(id);
            model.BaiViet = SpinBaiVietBusiness.GetByID(id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditBaiViet(FormCollection form)
        {
            var resultModel = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            AssignUserInfo();
            SPIN_BAIVIET bv = null;
            var id = form["editID"].ToIntOrZero();
            if (id > 0)
            {
                bv = SpinBaiVietBusiness.Find(id);
            }
            if (bv == null)
            {
                bv = new SPIN_BAIVIET();
                bv.NGAYTAO = DateTime.Now;
                bv.NGUOITAO = currentUserId;
            }

            bv.TIEUDE = form["TIEUDEBaiViet"];
            bv.NOIDUNG = form["NOIDUNGBaiViet"];

            bv.NGAYSUA = DateTime.Now;
            bv.NGUOISUA = currentUserId;
            bv.USER_ID = currentUserId;
            SpinBaiVietBusiness.Save(bv);
            return Json(resultModel);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var bv = SpinBaiVietBusiness.Find(id);
            if (bv != null)
            {
                var listExtend = SpinBaiVietBusiness.All.Where(x => x.IS_ORIGIN == false && x.EXTEND_OF == id).ToList();
                if (listExtend.Any())
                {
                    SpinBaiVietBusiness.DeleteAll(listExtend);
                    SpinBaiVietBusiness.Save();
                }
                SpinBaiVietBusiness.Delete(id);
                SpinBaiVietBusiness.Save();
            }
            else
            {
                model.Status = false;
                model.Message = "Không tìm thấy bài viết";
            }
            return Json(model);
        }

        public ActionResult Detail(int id)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var bv = SpinBaiVietBusiness.Find(id);
            var model = new DetailBaiVietVM();
            SessionManager.SetValue("SearchBVExitend", null);
            model.BaiViet = SpinBaiVietBusiness.GetByID(id);
            model.LstBaiVietExtend = GetDataExtendBaiViet(id);
            return View(model);

        }
        [HttpPost]
        public JsonResult SearchDetail(int id, string key)
        {
            var searchModel = new SearchBaiVietBO();
            searchModel.TieuDe = key;
            SessionManager.SetValue("SearchBVExitend", searchModel);
            return Json(GetDataExtendBaiViet(id));
        }

        public PartialViewResult TaoBaiAuto(int id)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var bv = SpinBaiVietBusiness.Find(id);
            return PartialView("_TaoBaiAutoPartial", bv);
        }

        public bool GenIsDone(List<bool> lstDone)
        {
            var result = true;
            foreach (var item in lstDone)
            {
                if (!item)
                {
                    result = false;
                    break;
                }

            }
            return result;
        }

        public void InsetBaiMoiAuto(int idBaiOri, List<string> lstKey, List<int> lstCount, List<List<SPIN_WORDS>> param, string contentMask)
        {
            var lstParam = new List<ParamReplaceSpinBO>();
            for (int i = 0; i < lstKey.Count; i++)
            {
                ParamReplaceSpinBO SpinBO = new ParamReplaceSpinBO();
                SpinBO.key = lstKey[i].Replace("[[", "").Replace("]]", "");
                SpinBO.value = param[i][lstCount[i]].TU_CUMTU;
                lstParam.Add(SpinBO);
            }
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
            var bvori = SpinBaiVietBusiness.Find(idBaiOri);
            var content = SpinProvider.GetResultContent(lstParam, contentMask);
            var bv = new SPIN_BAIVIET();
            bv.NOIDUNG = content;
            bv.NGAYTAO = DateTime.Now;
            bv.NGAYSUA = DateTime.Now;
            bv.NGUOISUA = currentUserId;
            bv.NGUOITAO = currentUserId;
            bv.IS_ORIGIN = false;
            bv.USER_ID = currentUserId;
            bv.EXTEND_OF = bvori.ID;
            bv.TIEUDE = bvori.TIEUDE;
            SpinBaiVietBusiness.Save(bv);
            bv.TIEUDE += "_CLONE_" + bv.ID.ToString();
            SpinBaiVietBusiness.Save(bv);
        }

        [HttpPost]
        public JsonResult GenBaiAuto(int id, int sl)
        {
            var model = new JsonResultBO(true);
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            SpinBaiVietGroupBusiness = Get<SpinBaiVietGroupBusiness>();
            var bv = SpinBaiVietBusiness.Find(id);
            var lstTuDien = SpinBaiVietGroupBusiness.GetTuDienSpin(bv.ID);
            var lstmarks = SpinProvider.GenMark(lstTuDien, bv.NOIDUNG);

            var listDone = new List<bool>();
            var listCount = new List<int>();
            for (int i = 0; i < lstmarks.Marks.Count; i++)
            {
                listDone.Add(false);
                listCount.Add(0);
            }
            var lstData = new List<List<SPIN_WORDS>>();
            var lstKey = new List<string>();
            foreach (var item in lstmarks.Marks)
            {
                lstData.Add(item.Value);
                lstKey.Add(item.Key);
            }
            var index = 0;
            var current = 0;
            var curRun = 0;
            try
            {

                while (index < sl)
                {
                    if (!GenIsDone(listDone))
                    {
                        while (current > curRun)
                        {
                            for (int i = 0; i < lstData[curRun].Count; i++)
                            {
                                listCount[curRun] = i;
                                InsetBaiMoiAuto(id, lstKey, listCount, lstData, lstmarks.Content);
                            }
                            curRun++;
                            index++;
                            if (index > sl)
                            {
                                break;
                            }

                        }
                        InsetBaiMoiAuto(id, lstKey, listCount, lstData, lstmarks.Content);
                        index++;
                        if (index == sl)
                        {
                            break;
                        }
                        curRun = 0;

                        if (listCount[current] + 1 == lstData[current].Count)
                        {

                            listDone[current] = true;
                            current++;
                            if (current > lstData.Count)
                            {
                                index = sl;
                                break;
                            }
                        }
                        else
                        {
                            listCount[current]++;
                        }

                    }
                    else
                    {
                        index = sl;
                        break;
                    }
                }
            }
            catch
            {

                model.Status = false;
                model.Message = "Không hoàn thành được thao tác";

            }

            return Json(model);

        }

        [HttpPost]
        public JsonResult ReloadExtendBV(int idBV, int page)
        {
            return Json(GetDataExtendBaiViet(idBV, page));
        }

        public PageListResultBO<SPIN_BAIVIET> GetDataExtendBaiViet(int id, int pageIndex = 1)
        {
            SpinBaiVietBusiness = Get<SpinBaiVietBusiness>();
            var searchModel = SessionManager.GetValue("SearchBVExitend") as SearchBaiVietBO;
            return SpinBaiVietBusiness.GetListExtend(searchModel, id, pageIndex);
        }
        [HttpPost]
        public JsonResult SearchBaiViet(FormCollection form)
        {
            var searchModel = new SearchBaiVietBO();
            searchModel.TieuDe = form["TIEUDE_SEARCH"];
            searchModel.StartNgayTao = form["StartDate_Search"].ToDataTime();
            searchModel.EndNgayTao = form["EndtDate_Search"].ToDataTime();
            SessionManager.SetValue("SearchBaiViet", searchModel);
            return Json(GetData());
        }
    }
}
