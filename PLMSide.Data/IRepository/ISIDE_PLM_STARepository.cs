using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface ISIDE_PLM_STARepository: IRepositoryBase<SIDE_PLM_STA>
    {
        Task<Tuple<List<SIDE_PLM_STA>, int>> GetEntitiesByPaging(string whereStr, int currentPageIndex);
        Task<List<SIDE_PLM_STA>> GetEntities(string whereStr);
    }
}
