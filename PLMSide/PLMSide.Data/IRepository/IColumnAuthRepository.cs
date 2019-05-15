using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface IColumnAuthRepository
    {
        Task<List<ColumnAuth>> GetColumnAuths(string roleid,string type);


        Task<List<ColumnAuth>> GetColumnAuthsByName(string roleName, string type);
    }
}
