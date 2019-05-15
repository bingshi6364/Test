using PLMSide.Data.Dto;
using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface ISOApprovalRepository
    {
        Task<List<CustomerGroup>> GetCustomerBranch();

        Task<List<Province>> GetProvinces();

        Task<List<string>> GetChannels();

        Task<Tuple<List<POS_Master>, int>> GetPageByProcList(string viewName, string fieldName = "*", string wherestring = " 1=1"
            , string orderString = "ID"
            , int page = 1, int pageSize = 10);

        Task<List<POS_Master>> GetExportList(SOApproval Model,string RoleName,string userid,string username,string roleid);

        Task<string> CheckPosCode(string year,string season,string dummycode, string poscode);
    }
}
