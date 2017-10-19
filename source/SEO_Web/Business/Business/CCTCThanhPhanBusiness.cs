using Business.CommonBusiness;
using DAL.Repository;
using Model.DBTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Business.Business
{
    public class CCTCThanhPhanBusiness : GenericBussiness<CCTC_THANHPHAN>
    {
        public CCTCThanhPhanBusiness(Entities context = null)
            : base(context)
        {
            this.repository = new CCTCThanhPhanRepository(context);
        }

        private List<CCTCItemTreeBO> ListAll = new List<CCTCItemTreeBO>();

        public bool ExistCode(string code, int id = 0)
        {
            var obj = this.context.CCTC_THANHPHAN.Where(x => x.CODE == code).FirstOrDefault();
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (id > 0 && obj.ID == id)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool ExistChild(int id)
        {
            var lst = this.context.CCTC_THANHPHAN.Where(x => x.PARENT_ID == id).Count();
            if (lst > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Save(CCTC_THANHPHAN thanhphan)
        {
            try
            {
                if (thanhphan.ID == 0)
                {
                    thanhphan.IS_DELETE = false;
                    this.repository.Insert(thanhphan);
                }
                else
                    this.repository.Update(thanhphan);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public List<CCTC_THANHPHAN> GetAllByLeVelUp(int level = 0)
        {
            var lstData = this.context.CCTC_THANHPHAN.Where(x => x.IS_DELETE != true);
            if (level > 0)
            {
                lstData = lstData.Where(x => x.ITEM_LEVEL <= level).OrderBy(x => x.ITEM_LEVEL);
                return lstData.ToList();
            }
            else
            {
                return lstData.OrderBy(x => x.ITEM_LEVEL).ToList();
            }
        }

        public List<CCTC_THANHPHAN> GetDSChild(int id)
        {
            var listItem = new List<CCTC_THANHPHAN>();
            var queue = new Queue();
            queue.Enqueue(id);
            while (queue.Count > 0)
            {
                var parent = (int)queue.Dequeue();
                var item = this.context.CCTC_THANHPHAN.Where(x => x.PARENT_ID == parent).ToList();
                if (item.Count > 0)
                {
                    listItem.AddRange(item);
                    foreach (var cc in item)
                    {
                        queue.Enqueue(cc.ID);
                    }
                }
            }

            return listItem;
        }

        public void GetChild(ref CCTCItemTreeBO Node)
        {
            var id = Node.ID;
            var lstChild = ListAll.Where(x => x.PARENT_ID == id).ToList();
            if (lstChild.Count > 0)
            {
                Node.Child = new List<CCTCItemTreeBO>();
                Node.Child.AddRange(lstChild);
                for (int i = 0; i < lstChild.Count; i++)
                {
                    var item = Node.Child[i];
                    GetChild(ref item);
                    Node.Child[i] = item;
                }
            }
        }

        public List<CCTCItemTreeBO> GetAll()
        {
            ListAll.Clear();
            ListAll = (from item in this.context.CCTC_THANHPHAN
                       where item.IS_DELETE != true
                       select new CCTCItemTreeBO
                       {
                           ID = item.ID,
                           IS_DELETE = item.IS_DELETE.HasValue ? item.IS_DELETE : false,
                           ITEM_LEVEL = item.ITEM_LEVEL,
                           NAME = item.NAME,
                           NGAYSUA = item.NGAYSUA,
                           NGAYTAO = item.NGAYTAO,
                           NGUOISUA = item.NGUOISUA,
                           NGUOITAO = item.NGUOITAO,
                           PARENT_ID = item.PARENT_ID,
                           TYPE = item.TYPE,
                           CODE = item.CODE,
                       }).ToList();
            return ListAll;
        }

        public CCTCItemTreeBO GetTree(int dvID = 0)
        {
            GetAll();
            CCTCItemTreeBO ListRoot = new CCTCItemTreeBO();
            if (dvID > 0)
            {
                ListRoot = ListAll.Where(x => x.ID == dvID).FirstOrDefault();
                if (ListRoot != null)
                {
                    var item = ListRoot;
                    GetChild(ref ListRoot);
                    ListRoot = item;
                }
            }
            else
            {
                ListRoot = ListAll.Where(x => x.PARENT_ID == null).FirstOrDefault();
                if (ListRoot != null)
                {
                    var item = ListRoot;
                    GetChild(ref ListRoot);
                    ListRoot = item;
                }
            }

            return ListRoot;
        }

        public List<SelectListItem> GetDropDownList(int selected = 0)
        {
            List<SelectListItem> listResult = this.All.Where(x => x.IS_DELETE != true)
                .OrderBy(x => x.NAME)
                .Select(x => new SelectListItem()
                {
                    Value = x.ID.ToString(),
                    Text = x.NAME,
                    Selected = (x.ID == selected)
                }).ToList();
            return listResult;
        }
    }
}