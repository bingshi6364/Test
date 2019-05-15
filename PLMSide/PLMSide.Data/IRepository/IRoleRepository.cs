using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface IRoleRepository: IRepositoryBase<Roles>
    {


        Task<List<Roles>> GetRoles();

        Task PostRole(Roles entity);

        Task PutRole(Roles entity);

        Task DeleteRole(int Id);

        Task<Roles> GetRoleDetail(int Id);
    }
}
