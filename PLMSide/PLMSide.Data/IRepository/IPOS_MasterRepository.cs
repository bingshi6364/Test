using Dapper;
using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface IPOS_MasterRepository : IRepositoryBase<POS_Master>
    {
        Task<Tuple<List<POS_Master>, int>> GetEntities(string whereStr);
        Task<POS_Master> GetSingelEntity(string TM_YEAR, string TM_SEASON, string POS_CODE);
        Task PostEntity(POS_Master entity);
        ResultModel InsertPos_MasterInfo(POS_Master entity);
        /// <summary>
        /// 获取最大的编号
        /// </summary>
        /// <param name="paraname"></param>
        /// <returns></returns>
        string GetMaxPOSCode(string paraname);
        /// <summary>
        /// count(1) 返回记录数
        /// </summary>
        /// <param name="shipcode"></param>
        /// <param name="shipcodeindex"></param>
        /// <returns></returns>
        int ExistshipCode(string shipcode, int shipcodeindex);
        /// <summary>
        /// 返回审批记录数 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        int CheckPOSReviewBLRStatus(int UserId, string year, string season);
        string GetProductchannel(string shipcode);
   

        Task PutEntity(POS_Master entity);
        Task DeleteEntity(int Id);

        Task<List<POS_Master>> GetAllList();
        /// <summary>
        /// 根据YEAR SEASON AS获取AccountSales 待提交数据
        /// </summary>
        /// <param name="YEAR"></param>
        /// <param name="SEASON"></param>
        /// <param name="AS_ID"></param>
        /// <returns></returns>
        Task<List<POS_Master>> GetApprovalPOS(string YEAR, string SEASON, string AS_ID);
        /// <summary>
        /// 获取ASM审批数据
        /// </summary>
        /// <param name="YEAR"></param>
        /// <param name="SEASON"></param>
        /// <param name="AS_ID"></param>
        /// <param name="ASM_ID"></param>
        /// <returns></returns>
        Task<List<ASApprovalModel>> GetASMApproval(string YEAR, string SEASON, int ASM_ID,bool isSO,string userName);
        /// <summary>
        /// 通过用户角色查看不同的内容
        /// </summary>
        /// <returns></returns>
        DataTable GetListByUserRole(string sqlstr, DynamicParameters parameters);
        /// <summary>
        /// 暂时不用
        /// </summary>
        /// <returns></returns>
        DataTable GetListByUser(int UserId, string Year, string Season);
        Task<object> StatisBigFormat(string year,string season,string ASM_ID);
        Task<object> GetPreviousSeason(string year, string season, string pos_code);

    }
}
