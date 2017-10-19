/**
 * The HiNet License
 *
 * Copyright 2015 Hinet JSC. All rights reserved.
 * HINET PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

/** 
* @author  NAMDV
*/

using DAL.Repository;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.CommonBusiness;
using System.Web.Mvc;

namespace Business.Business
{
    public class DmVaitroBusiness : GenericBussiness<DM_VAITRO>
    {
        public DmVaitroBusiness(Entities context = null)
            : base()
        {
            repository = new DmVaitroRepository(context);
        }
        public void Save(DM_VAITRO vaitro)
        {
            try
            {
                if (vaitro.DM_VAITRO_ID == 0)
                {
                    this.repository.Insert(vaitro);
                }
                else
                    this.repository.Update(vaitro);
                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TreeNodeBO GetTree(int id, int coso)
        {
            var Root = new TreeNodeBO();
            return null;
            //var vaitro = this.context.DM_VAITRO.Where(x => x.DM_VAITRO_ID == id).FirstOrDefault();
            //if (vaitro != null)
            //{
            //    Root.Name = vaitro.TEN_VAITRO;
            //    Root.ID = vaitro.DM_VAITRO_ID;
            //    Root.Type = 1;
            //    var chucnang1 = this.context.VAITRO_CHUCNANG.Where(x => x.DM_VAITRO_ID == id && x.COSO_ID == coso).Select(x => new TreeNodeBO()
            //    {
            //        Name = x.DM_CHUCNANG.TEN_CHUCNANG,
            //        ID_DM = (int)x.DM_CHUCNANG_ID,
            //        ID = x.VAITRO_CHUCNANG_ID,
            //        Type = 2
            //    }).ToList();
            //    Root.Child = new List<TreeNodeBO>();
            //    var listNodeChucNang1 = new List<TreeNodeBO>();
            //    foreach (var chucnang1Item in chucnang1)
            //    {
            //        var chucnang2 = this.context.VAITRO_CHUCNANG.Where(x => x.DM_VAITRO_ID == id && x.COSO_ID == coso && x.DM_CHUCNANG.CHUCNANG_CHA == chucnang1Item.ID).Select(x => new TreeNodeBO()
            //        {
            //            Name = x.DM_CHUCNANG.TEN_CHUCNANG,
            //            ID = x.VAITRO_CHUCNANG_ID,
            //            ID_DM = (int)x.DM_CHUCNANG_ID,
            //            Type = 2
            //        }).ToList();
            //        var thaotac = this.context.VAITRO_THAOTAC.Where(x => x.VAITRO_CHUCNANG_ID == chucnang1Item.ID).Select(x => new TreeNodeBO()
            //        {
            //            Name = x.DM_THAOTAC.TEN_THAOTAC,
            //            ID = x.VAITRO_THAOTAC_ID,
            //            ID_DM = (int)x.DM_THAOTAC_ID,
            //            Type = 3
            //        }).ToList();
            //        chucnang1Item.Child = new List<TreeNodeBO>();
            //        foreach (var chucnang2item in chucnang2)
            //        {
            //            var thaotac2 = this.context.VAITRO_THAOTAC.Where(x => x.VAITRO_CHUCNANG_ID == chucnang2item.ID).Select(x => new TreeNodeBO()
            //           {
            //               Name = x.DM_THAOTAC.TEN_THAOTAC,
            //               ID = x.VAITRO_THAOTAC_ID,
            //               ID_DM = (int)x.DM_THAOTAC_ID,
            //               Type = 3
            //           }).ToList();
            //            chucnang2item.Child = new List<TreeNodeBO>();
            //            chucnang2item.Child.AddRange(thaotac2);
            //        }
            //        chucnang1Item.Child.AddRange(thaotac);
            //        chucnang1Item.Child.AddRange(chucnang2);

            //    }
            //    Root.Child.AddRange(chucnang1);
            //    return Root;
            //}
            //else
            //{
            //    return null;
            //}

        }
       
        //public DmVaiTroBO GetById(int id)
        //{
        //    DmVaiTroBO result = new DmVaiTroBO();
        //    result = (from vaitro in this.context.DM_VAITRO
        //              where vaitro.DM_VAITRO_ID == id
        //              join coso in this.context.COSO on vaitro.COSO_ID equals coso.COSO_ID
        //                  into group1
        //              from gCoSo in group1.DefaultIfEmpty()
        //              join capcs in this.context.DM_CAPCOSO on vaitro.CAPCOSO_ID equals capcs.ID
        //              into g2
        //              from gCapCoSo in g2.DefaultIfEmpty()

        //              orderby vaitro.DM_VAITRO_ID descending
        //              select new DmVaiTroBO
        //              {
        //                  COSO_ID = vaitro.COSO_ID,
        //                  DM_VAITRO_ID = vaitro.DM_VAITRO_ID,
        //                  MA_VAITRO = vaitro.MA_VAITRO,
        //                  TRANGTHAI = vaitro.TRANGTHAI,
        //                  TEN_VAITRO = vaitro.TEN_VAITRO,
        //                  TEN_COSO = gCoSo.TENCOSO,
        //                  CAPCOSO_ID = vaitro.CAPCOSO_ID,
        //                  CAPCOSO = gCapCoSo.CAPCOSO,
        //                  CAPCOSO_PARENT = gCapCoSo.PARENT_CAPCOSO,
        //                  IS_DELETE = vaitro.COSO_ID.HasValue ? true : false

        //              }).FirstOrDefault();
        //    return result;
        //}

        /// <summary>
        /// TRẢ VỀ DANH SÁCH VAI TRÒ BY PHÂN CẤP ĐƠN VỊ
        /// </summary>
        /// <param name="lstCapCoSo"></param>
        /// <returns></returns>
        public List<DmVaiTroBO> GetListByParam()
        {
            List<DmVaiTroBO> result = new List<DmVaiTroBO>();
            result = (from vaitro in this.context.DM_VAITRO
                      //join coso in this.context.COSO on vaitro.COSO_ID equals coso.COSO_ID
                      //    into group1
                      //from gCoSo in group1.DefaultIfEmpty()
                      //join capcs in this.context.DM_CAPCOSO on vaitro.CAPCOSO_ID equals capcs.ID
                      //into g2
                      //from gCapCoSo in g2.DefaultIfEmpty()
                      //where vaitro.COSO_ID == null && vaitro.IS_DELETE == false
                      where vaitro.IS_DELETE == false
                      orderby vaitro.DM_VAITRO_ID descending
                      select new DmVaiTroBO
                      {
                          //COSO_ID = vaitro.COSO_ID,
                          DM_VAITRO_ID = vaitro.DM_VAITRO_ID,
                          MA_VAITRO = vaitro.MA_VAITRO,
                          TRANGTHAI = vaitro.TRANGTHAI,
                          TEN_VAITRO = vaitro.TEN_VAITRO,
                          //TEN_COSO = gCoSo.TENCOSO,
                          //CAPCOSO_ID = vaitro.CAPCOSO_ID,
                          //CAPCOSO = gCapCoSo.CAPCOSO,
                          //CAPCOSO_PARENT = gCapCoSo.PARENT_CAPCOSO,
                          IS_DELETE = true
                      }

                        ).ToList();
            //if (lstCapCoSo != null && lstCapCoSo.Count > 0)
            //{
            //    result = result.Where(x => x.CAPCOSO_ID.HasValue ? lstCapCoSo.Contains(x.CAPCOSO_ID.Value) : false || x.CAPCOSO_PARENT.HasValue ? lstCapCoSo.Contains(x.CAPCOSO_PARENT.Value) : false).ToList();
            //}
            //List<DM_VAITRO> result = this.All.Where(x => !string.IsNullOrEmpty(TENVAITRO) ? x.TEN_VAITRO == TENVAITRO : true).ToList();
            return result;
        }
        public List<DM_VAITRO> ListVaiTro(int? CAPCOSO_ID = 0, int? COSO_ID = 0)
        {
            var result = new List<DM_VAITRO>();
            if (CAPCOSO_ID > 0 && COSO_ID > 0)
            {
                result = this.All.Where(x => x.IS_DELETE == false).ToList();
                result = result.Where(x => x.CAPCOSO_ID == CAPCOSO_ID && x.COSO_ID == COSO_ID).ToList();
            }
            return result;
        }
        public List<DM_VAITRO> GetListVaiTro()
        {
            return this.All.Where(x => x.TRANGTHAI == 1 && x.IS_DELETE == false).ToList();
        }
        public List<DM_VAITRO> GetListByCoSo(int? CoSoId)
        {
            var result = this.All.ToList();
            if (CoSoId.HasValue)
            {
                result = result.Where(x => x.COSO_ID == CoSoId && x.IS_DELETE == false).ToList();
            }
            return result;
        }
        public List<DM_VAITRO> GetListByCoSoID(int? CoSoId)
        {
            var result = new List<DM_VAITRO>();
            if (CoSoId.HasValue)
            {
                result = this.All.Where(x => x.COSO_ID == CoSoId && x.IS_DELETE == false).ToList();
            }
            return result;
        }
        /// <summary>
        /// GET LIST VAI TRO BY CapCoSoID And CoSoID
        /// </summary>
        /// <param name="CapCoSo"></param>
        /// <param name="CoSoID"></param>
        /// <returns></returns>
        public List<DM_VAITRO> GetListByCapCoSoID(int? CapCoSo = 0, int? CoSoID = 0, bool? Is_MaxLever = false)
        {
            var result = new List<DM_VAITRO>();
            // nếu là cấp cao nhất
            if (Is_MaxLever.Value)
            {
                result = this.All.Where(x => x.CAPCOSO_ID == CapCoSo && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).ToList();
            }
            else
            {
                if (CapCoSo > 0 && CoSoID > 0)
                {
                    result = this.All.Where(x => (x.CAPCOSO_ID == CapCoSo && x.COSO_ID == CoSoID) || (x.CAPCOSO_ID == CapCoSo && x.COSO_ID == null) && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).ToList();
                }
                else if (CapCoSo > 0 && CoSoID == 0)
                {
                    result = this.All.Where(x => x.CAPCOSO_ID == CapCoSo && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).ToList();
                }
            }
            return result;
        }
        public List<SelectListItem> DSVaiTroByCapCoSoID(int? CapCoSo = 0, int? CoSoID = 0, bool? Is_MaxLever = false)
        {
            var result = new List<SelectListItem>();
            // nếu là cấp cao nhất
            if (Is_MaxLever.Value)
            {
                result = this.All.Where(x => x.CAPCOSO_ID == CapCoSo && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                {
                    Text = x.TEN_VAITRO,
                    Value = x.DM_VAITRO_ID.ToString(),
                }).ToList();
            }
            else
            {
                if (CapCoSo > 0 && CoSoID > 0)
                {
                    result = this.All.Where(x => (x.CAPCOSO_ID == CapCoSo && x.COSO_ID == CoSoID) || (x.CAPCOSO_ID == CapCoSo && x.COSO_ID == null) && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                    {
                        Text = x.TEN_VAITRO,
                        Value = x.DM_VAITRO_ID.ToString(),
                    }).ToList();
                }
                else if (CapCoSo > 0 && CoSoID == 0)
                {
                    result = this.All.Where(x => x.CAPCOSO_ID == CapCoSo && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                    {
                        Text = x.TEN_VAITRO,
                        Value = x.DM_VAITRO_ID.ToString(),
                    }).ToList();
                }
            }
            return result;
        }

        public List<SelectListItem> DSVaiTro(List<int> selectedList=null)
        {
            var result = new List<SelectListItem>();
            // nếu là cấp cao nhất
            if (selectedList!=null && selectedList.Count>0)
            {
                result = this.All.Where(x => x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                {
                    Text = x.TEN_VAITRO,
                    Value = x.DM_VAITRO_ID.ToString(),
                    Selected = selectedList.Contains(x.DM_VAITRO_ID)
                }).ToList();
            }
            else
            {
                result = this.All.Where(x => x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                {
                    Text = x.TEN_VAITRO,
                    Value = x.DM_VAITRO_ID.ToString(),
                }).ToList();
            }
               
            return result;
        }
        public List<SelectListItem> DSVaiTroUpdateByCapCoSoID(List<int> lstItem, int? CapCoSo = 0, int? CoSoID = 0, bool? Is_MaxLever = false)
        {
            var result = new List<SelectListItem>();
            // nếu là cấp cao nhất
            if (Is_MaxLever.Value)
            {
                result = this.All.Where(x => x.CAPCOSO_ID == CapCoSo && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                {
                    Text = x.TEN_VAITRO,
                    Value = x.DM_VAITRO_ID.ToString(),
                    Selected = lstItem.Contains(x.DM_VAITRO_ID)

                }).ToList();
            }
            else
            {
                if (CapCoSo > 0 && CoSoID > 0)
                {
                    result = this.All.Where(x => (x.CAPCOSO_ID == CapCoSo && x.COSO_ID == CoSoID) || (x.CAPCOSO_ID == CapCoSo && x.COSO_ID == null) && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                    {
                        Text = x.TEN_VAITRO,
                        Value = x.DM_VAITRO_ID.ToString(),
                        Selected = lstItem.Contains(x.DM_VAITRO_ID)

                    }).ToList();
                }
                else if (CapCoSo > 0 && CoSoID == 0)
                {
                    result = this.All.Where(x => x.CAPCOSO_ID == CapCoSo && x.TRANGTHAI.HasValue && x.TRANGTHAI == 1 && x.IS_DELETE == false).Select(x => new SelectListItem()
                    {
                        Text = x.TEN_VAITRO,
                        Value = x.DM_VAITRO_ID.ToString(),
                        Selected = lstItem.Contains(x.DM_VAITRO_ID)

                    }).ToList();
                }
            }
            return result;
        }

        /// <summary>
        /// author duynn
        /// created on 4/6/2017
        /// </summary>
        /// <param name="selected">id tuy chon</param>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList(int selected = 0)
        {
            List<SelectListItem> listVaiTros = this.All.Where(x => x.IS_DELETE != true && x.TRANGTHAI == 1)
                .OrderBy(x => x.TEN_VAITRO)
                .Select(x => new SelectListItem()
                {
                    Value = x.DM_VAITRO_ID.ToString(),
                    Text = x.TEN_VAITRO,
                    Selected = (selected == x.DM_VAITRO_ID)
                }).ToList();
            return listVaiTros;
        }
    }
}
