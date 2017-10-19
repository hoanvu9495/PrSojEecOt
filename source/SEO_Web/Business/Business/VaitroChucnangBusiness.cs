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

namespace Business.Business
{
    public class VaitroChucnangBusiness : GenericBussiness<VAITRO_CHUCNANG>
    {
        public VaitroChucnangBusiness(Entities context = null)
            : base()
        {
            repository = new VaitroChucnangRepository(context);
        }
        public void Save(VAITRO_CHUCNANG vaitro_chucnang)
        {
            try
            {
                if (vaitro_chucnang.VAITRO_CHUCNANG_ID == 0)
                {
                    this.repository.Insert(vaitro_chucnang);
                }
                else
                    this.repository.Update(vaitro_chucnang);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }
        public List<int?> GetCoSoID(int VAITRO_ID)
        {
            return this.All.Where(x => x.DM_VAITRO_ID == VAITRO_ID && x.TRANGTHAI == 1).Select(x => x.COSO_ID).ToList();
        }

        public VAITRO_CHUCNANG getVaiTroChucNang(int vaitro, int coso, int chucnang)
        {
            var obj = this.context.VAITRO_CHUCNANG.Where(x => x.DM_VAITRO_ID == vaitro && x.COSO_ID == coso && x.DM_CHUCNANG_ID == chucnang).FirstOrDefault();
            return obj;
        }
        public void SaveChucNangDefault(int VAITRO_ID, int CHUCNANG_DEFAULT)
        {
            try
            {
                if (VAITRO_ID > 0 && CHUCNANG_DEFAULT > 0)
                {
                    VAITRO_CHUCNANG vaitroResult = this.All.Where(x => x.DM_VAITRO_ID == VAITRO_ID && x.DM_CHUCNANG_ID == CHUCNANG_DEFAULT).FirstOrDefault();
                    foreach (var item in this.All.Where(x => x.DM_VAITRO_ID == VAITRO_ID))
                    {
                        if (item.MAC_DINH == 1)
                        {
                            VAITRO_CHUCNANG vt = this.repository.Find(item.VAITRO_CHUCNANG_ID);
                            vt.MAC_DINH = null;
                            this.repository.Update(vt);
                        }
                    }
                    if (vaitroResult != null)
                    {
                        vaitroResult.MAC_DINH = 1;
                        this.repository.Update(vaitroResult);
                    }
                    else
                    {
                        VAITRO_CHUCNANG vaitro = new VAITRO_CHUCNANG();
                        vaitro.DM_CHUCNANG_ID = (int)CHUCNANG_DEFAULT;
                        vaitro.DM_VAITRO_ID = (int)VAITRO_ID;
                        vaitro.MAC_DINH = 1;
                        vaitro.TRANGTHAI = 1;
                        this.repository.Insert(vaitro);
                    }
                    this.repository.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
