using PLMSide.Data.Dto;
using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public  interface ICTCApprovalRepository
    {
        //Task<Tuple<List<POS_Master>, int>> GetPageByProcList(string viewName, string fieldName = "*", string wherestring = " 1=1"
        // , string orderString = "ID"
        // , int page = 1, int pageSize = 10);

        Task<List<POS_Master>> GetExportList(CTCApproval Model);

        Task<Tuple<List<POS_Master>, int>> GetPageByProcList(CTCApproval Model);

        Task<List<POS_MasterImport>> ImportData(List<POS_MasterImport> Model, List<string> colString,string ProcName);

    }
}
