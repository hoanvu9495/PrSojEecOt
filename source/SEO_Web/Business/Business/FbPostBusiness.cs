using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DBTool;
using DAL.Repository;
using Business.CommonBusiness;
using PagedList;

namespace Business.Business
{
    public class FbPostBusiness : GenericBussiness<FB_POST>
    {
        public FbPostBusiness(Entities context)
            : base(context)
        {
            repository = new FbPostRepository(context);
        }

        public void Save(FB_POST item)
        {
            try
            {
                if (item.ID == 0)
                {
                    this.repository.Insert(item);
                }
                else
                    this.repository.Update(item);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PageListResultBO<FB_POST> GetListByUser(SearchBaiVietBO searchModel, int userid, int pageindex = 1, int pageSize = 20)
        {
            var query = this.context.FB_POST.Where(x => x.NGUOITAO == userid);
            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.TieuDe))
                {
                    query = query.Where(x => x.TIEUDE.Contains(searchModel.TieuDe));
                }

                if (searchModel.StartNgayTao.HasValue)
                {
                    query = query.Where(x => x.NGAYTAO >= searchModel.StartNgayTao.Value);

                }

                if (searchModel.EndNgayTao.HasValue)
                {
                    query = query.Where(x => x.NGAYTAO <= searchModel.EndNgayTao.Value);

                }
            }
            var pagelist = query.OrderByDescending(x => x.ID).ToPagedList(pageindex, pageSize);
            var model = new PageListResultBO<FB_POST>();
            model.TotalPage = pagelist.PageCount;
            model.Count = pagelist.TotalItemCount;
            model.ListItem = pagelist.ToList();

            return model;
        }
    }
}
