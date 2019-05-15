using PLMSide.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface IUserRepository : IRepositoryBase<Users> 
    {
        #region 扩展的dapper操作

        //加一个带参数的存储过程
        string ExecExecQueryParamSP(string spName, string name, int Id);

        Task<List<Users>> GetUsers();

        Task<Users> GetUserAndRoles(int Id);

        Task<Users> GetUserAndRolesByAccount(string account);

        Task PostUser(Users entity);

        Task PutUser(Users entity);

        Task DeleteUser(int Id);

        Task<Users> GetUserDetail(int Id);
        Users GetUserByEmail(string email);

        #endregion
    }
}
