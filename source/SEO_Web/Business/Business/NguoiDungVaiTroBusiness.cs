using DAL.Repository;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.CommonBusiness;
namespace Business.Business
{
    public class NguoiDungVaiTroBusiness : GenericBussiness<NGUOIDUNG_VAITRO>
    {
        public NguoiDungVaiTroBusiness(Entities context = null)
            : base()
        {
            repository = new NguoiDungVaiTroRepository(context);
        }
        public void Save(NGUOIDUNG_VAITRO nguoidung_vaitro)
        {
            try
            {
                if (nguoidung_vaitro.ID == 0)
                {
                    this.repository.Insert(nguoidung_vaitro);
                }
                else
                    this.repository.Update(nguoidung_vaitro);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }
        public List<NGUOIDUNG_VAITRO> GetListByNguoiDung(int? DM_NGUOIDUNG_ID = 0)
        {
            return this.All.Where(x => x.NGUOIDUNG_ID == DM_NGUOIDUNG_ID).ToList();
        }
        public List<DM_VAITRO> getListVaiTroByUserID(long userid)
        {
            var listVaitro = (from vaitro in this.context.NGUOIDUNG_VAITRO.Where(x => x.NGUOIDUNG_ID == userid)
                              join dmvaitro in this.context.DM_VAITRO on vaitro.VAITRO_ID equals dmvaitro.DM_VAITRO_ID
                              select dmvaitro).ToList();
            return listVaitro;
        }
        public List<int> GetListRole(int? DM_NGUOIDUNG_ID = 0)
        {
            var result = new List<int>();
            if (DM_NGUOIDUNG_ID > 0)
            {
                result = this.All.Where(x => x.NGUOIDUNG_ID == DM_NGUOIDUNG_ID).Select(x => x.VAITRO_ID.Value).ToList();
            }
            return result;
        }
        /// <summary>
        /// trả về người dùng vai trò
        /// </summary>
        /// <param name="NGUOIDUNG_ID"></param>
        /// <param name="VAITRO"></param>
        /// <returns></returns>
        public NGUOIDUNG_VAITRO GetRole(int? NGUOIDUNG_ID = 0, int? VAITRO = 0)
        {
            if (NGUOIDUNG_ID > 0 && VAITRO > 0)
            {
                return this.All.Where(x => x.NGUOIDUNG_ID == NGUOIDUNG_ID && x.VAITRO_ID == VAITRO).FirstOrDefault();
            }
            return new NGUOIDUNG_VAITRO();
        }

        public void ResetRoleDefault(decimal? NGUOIDUNG_ID = 0)
        {
            if (NGUOIDUNG_ID > 0)
            {
                var listRole = this.All.Where(x => x.NGUOIDUNG_ID == NGUOIDUNG_ID).ToList();
                if (listRole != null)
                {
                    foreach (var item in listRole)
                    {
                        item.ROLE_DEFAULT = false;
                        this.Save(item);
                    }
                }
            }
        }


        public List<NguoiDungBO> ListUserIDByRoleID(int RoleID)
        {
            var result = (
                from vt in this.context.NGUOIDUNG_VAITRO
                join nd in this.context.DM_NGUOIDUNG
                on vt.NGUOIDUNG_ID equals nd.DM_NGUOIDUNG_ID
                //into group1
                //from g1 in group1.DefaultIfEmpty()
                where vt.VAITRO_ID == RoleID
                orderby nd.HOTEN
                select new NguoiDungBO()
                {
                    DM_NGUOIDUNG_ID = (int)vt.NGUOIDUNG_ID,
                    HOTEN = nd.HOTEN,
                    TENDANGNHAP = nd.TENDANGNHAP
                }
                );
            //ListUserID = this.context.NGUOIDUNG_VAITRO.Where(o => o.VAITRO_ID == RoleID).Select(o => (int)o.NGUOIDUNG_ID).ToList();
            //return ListUserID;
            return result.ToList();
        }
        public bool CheckHasRole(long userID, int roleID)
        {
            var user_role = this.All.Where(x => x.NGUOIDUNG_ID == userID && x.VAITRO_ID == roleID).FirstOrDefault();
            if (user_role != null)
            {
                return true;
            }
            return false;
        }
        public bool CheckHasRole(long userID, string roleCode)
        {
            var listRoles = (from user_role in this.context.NGUOIDUNG_VAITRO.Where(x => x.NGUOIDUNG_ID == userID)
                             join role in this.context.DM_VAITRO.Where(x => x.IS_DELETE != true && x.TRANGTHAI == 1 && !string.IsNullOrEmpty(x.MA_VAITRO))
                             on user_role.VAITRO_ID equals role.DM_VAITRO_ID
                             select new NguoiDungVaiTroBO()
                             {
                                 MA_VAITRO = role.MA_VAITRO,
                                 DM_NGUOIDUNG_ID = user_role.NGUOIDUNG_ID
                             }).ToList();
            bool result = listRoles.Any(x => x.MA_VAITRO.Contains(roleCode));
            return result;
        }

        public List<long> GetListNguoiDungByMaVaiTro(List<string> listRoleCodes)
        {
            List<long> listNguoiDungIDs = new List<long>();
            var result = (from user_role in this.context.NGUOIDUNG_VAITRO.Where(x => x.NGUOIDUNG_ID.HasValue)
                          join role in this.context.DM_VAITRO.Where(x => x.IS_DELETE != true
                              && x.TRANGTHAI == 1 && !string.IsNullOrEmpty(x.MA_VAITRO))
                          on user_role.VAITRO_ID equals role.DM_VAITRO_ID
                          //into group1
                          //from g1 in group1.DefaultIfEmpty()
                          select new NguoiDungVaiTroBO()
                          {
                              MA_VAITRO = role.MA_VAITRO,
                              DM_NGUOIDUNG_ID = user_role.NGUOIDUNG_ID
                          }).ToList();
            if (result != null && result.Count > 0)
            {
                foreach (string role in listRoleCodes)
                {
                    var listRole = result.Where(x => x.MA_VAITRO.Equals(role)).ToList();
                    listNguoiDungIDs.AddRange(listRole.Select(x => x.DM_NGUOIDUNG_ID.Value).ToList());
                }
            }
            return listNguoiDungIDs;
        }

        public List<DMNguoiDungBO> GetListNguoiDungVaiTro(int coSoId, int donViId = 0)
        {
            List<DMNguoiDungBO> listUsers = (from user in this.context.DM_NGUOIDUNG
                                             join user_role in this.context.NGUOIDUNG_VAITRO
                                             on user.DM_NGUOIDUNG_ID equals user_role.NGUOIDUNG_ID
                                             into group0
                                             from g0 in group0.DefaultIfEmpty()

                                             join role in this.context.DM_VAITRO.Where(x => x.IS_DELETE != true && x.TRANGTHAI == 1)
                                             on g0.VAITRO_ID equals role.DM_VAITRO_ID
                                             into group1
                                             from g1 in group1.DefaultIfEmpty()

                                        
                                             select new DMNguoiDungBO()
                                             {
                                                 DM_NGUOIDUNG_ID = user.DM_NGUOIDUNG_ID,
                                                 HOTEN = user.HOTEN,
                                            
                                                 vaiTroId = g1.DM_VAITRO_ID,
                                                 tenVaiTro = g1.TEN_VAITRO
                                             }).ToList();
            if (coSoId > 0)
            {
                listUsers = listUsers.Where(x => x.coSoId == coSoId).ToList();
            }
            if (donViId > 0)
            {
                listUsers = listUsers.Where(x => x.dmDonViId == donViId).ToList();
            }
            return listUsers;
        }

        /// <summary>
        /// author duynn
        /// created date : 14/6/2017
        /// modify date : 14/06/2017
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<DMNguoiDungBO> GetListUserByRoleCode(string code, int phongBanId = 0)
        {
            IQueryable<DMNguoiDungBO> listIQueryUsers = (from user in this.context.DM_NGUOIDUNG
                                                         join user_role in this.context.NGUOIDUNG_VAITRO
                                                         on user.DM_NGUOIDUNG_ID equals user_role.NGUOIDUNG_ID
                                                         into group0
                                                         from g0 in group0.DefaultIfEmpty()

                                                         join role in this.context.DM_VAITRO.Where(x => x.IS_DELETE != true && x.TRANGTHAI == 1)
                                                         on g0.VAITRO_ID equals role.DM_VAITRO_ID
                                                         into group1
                                                         from g1 in group1.DefaultIfEmpty()

                                                         join phongBan in this.context.CCTC_THANHPHAN.Where(x => x.IS_DELETE != true)
                                                         on user.DM_PHONGBAN_ID equals phongBan.ID

                                                         into group2
                                                         from g2 in group2.DefaultIfEmpty()
                                                         orderby user.HOTEN
                                                         select new DMNguoiDungBO()
                                                         {
                                                             DM_NGUOIDUNG_ID = user.DM_NGUOIDUNG_ID,
                                                             HOTEN = user.HOTEN,
                                                             dmPhongBanId = g2 == null ? 0 : g2.ID,
                                                             tenPhongBan = g2.NAME,
                                                             vaiTroId = g1.DM_VAITRO_ID,
                                                             tenVaiTro = g1.TEN_VAITRO,
                                                             maVaiTro = g1.MA_VAITRO
                                                         });
            if (!string.IsNullOrEmpty(code))
            {
                listIQueryUsers = listIQueryUsers.Where(x => !string.IsNullOrEmpty(x.maVaiTro) && x.maVaiTro.Equals(code));
            }
            if (phongBanId > 0)
            {
                listIQueryUsers = listIQueryUsers.Where(x => x.dmPhongBanId == phongBanId);
            }
            List<DMNguoiDungBO> listUsers = listIQueryUsers.ToList();
            return listUsers;
        }

    }
}