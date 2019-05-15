using Dapper;
using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Data.Repository
{
    public class SIDE_PLM_BLR_BuyWindowRepository : RepositoryBase<SIDE_PLM_BLR_BuyWindow>, ISIDE_PLM_BLR_BuyWindowRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<SIDE_PLM_BLR_BuyWindow> GetModel(int ID)
        {
            string selectSql = @"SELECT * FROM [dbo].[SIDE_PLM_BLR_BuyWindow] where ID=@ID";
            return await Select(ID, selectSql);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<SIDE_PLM_BLR_BuyWindow> GetModel(string Year, string Season)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                string selectSql = @"SELECT top 1 * FROM [dbo].[SIDE_PLM_BLR_BuyWindow] where Year=@Year and  Season=@Season ";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Year", Year, DbType.String, ParameterDirection.Input);
                parameters.Add("@Season", Season, DbType.String, ParameterDirection.Input);
                return await Task.Run(() => conn.Query<SIDE_PLM_BLR_BuyWindow>(selectSql, parameters).ToList().FirstOrDefault());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SIDE_PLM_BLR_BuyWindow>> GetSIDE_PLM_BLR_BuyWindowList()
        {
            string selectSql = @"SELECT * FROM [dbo].[SIDE_PLM_BLR_BuyWindow]";
            return await Select(selectSql);
        }


        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<List<SIDE_PLM_BLR_BuyWindow>, int>> GetPageModelList(string whereStr)
        {
            Tuple<List<SIDE_PLM_BLR_BuyWindow>, int> tp = await GetPageByProcList("SIDE_PLM_BLR_BuyWindow", "*", whereStr, "ID", 1, 10);
            return tp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SIDE_PLM_BLR_BuyWindow>> GetSIDE_PLM_BLR_BuyWindowListByState(int State)
        {
            string selectSql = @"SELECT * FROM [dbo].[SIDE_PLM_BLR_BuyWindow] where State=@State order by ID desc";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@State", State, DbType.String, ParameterDirection.Input);
            return await SelectPara(selectSql, parameters);
        }


        /// <summary>
        /// 添加买货窗口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddBuyWindowSP(SIDE_PLM_BLR_BuyWindow model)
        {
            ResultModel result = new ResultModel();
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Year", model.Year, DbType.String, ParameterDirection.Input);
                parameters.Add("@Name", model.WindowContent, DbType.String, ParameterDirection.Input);
                parameters.Add("@Season", model.Season, DbType.String, ParameterDirection.Input);
                parameters.Add("@CreatePeople", model.CreatePeople, DbType.String, ParameterDirection.Input);

                parameters.Add("@ReturnMsg", "", DbType.String, ParameterDirection.Output, 100);
                parameters.Add("@ReturnCode", "0", DbType.String, ParameterDirection.Output, 100);
                conn.Execute("SIDE_AddBuyWindow", parameters, null, null, CommandType.StoredProcedure);
                string ReturnCode = parameters.Get<string>("@ReturnCode");
                result.StatusCode = ReturnCode.ToString();
                result.Message = parameters.Get<string>("@ReturnMsg");
                if (ReturnCode == "1")
                {
                    result.IsSuccess = true;

                }
                return result;
            }
        }
        /// <summary>
        /// 修改买货窗口 部分信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateEntity(SIDE_PLM_BLR_BuyWindow model)
        {
            string updateSql = @"update SIDE_PLM_BLR_BuyWindow set    POSConfirmationStartDate=@POSConfirmationStartDate, POSConfirmationEndDate=@POSConfirmationEndDate, TargetPOSConfirmationStartDate=@TargetPOSConfirmationStartDate, TargetPOSConfirmationEndDate=@TargetPOSConfirmationEndDate, TradingMeetingStartDate=@TradingMeetingStartDate, TradingMeetingEndDate=@TradingMeetingEndDate, STC_MandatoryStartDate=@STC_MandatoryStartDate, STC_MandatoryEndDate=@STC_MandatoryEndDate, Segment_POSTypeStartDate=@Segment_POSTypeStartDate, Segment_POSTypeEndDate=@Segment_POSTypeEndDate, Year=@Year, V1StartDate=@V1StartDate, V1EndDate=@V1EndDate, V2StartDate=@V2StartDate, V2EndDate=@V2EndDate, Season=@Season, WindowContent=@WindowContent, PosState=@PosState, TargetState=@TargetState,  CreatePeople=@CreatePeople, State=@State  FROM [dbo].SIDE_PLM_BLR_BuyWindow WHERE ID=@ID";
            await Update(model, updateSql);
        }

        /// <summary>
        /// 修改买货窗口 部分信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateNewBuyWindowSP(SIDE_PLM_BLR_BuyWindow model)
        {
            string updateSql = @"update SIDE_MD_OCSGrading_Type set   POSConfirmationStartDate=@POSConfirmationStartDate    WHERE ID=@ID";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@POSConfirmationStartDate", model.POSConfirmationStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@POSConfirmationEndDate", model.POSConfirmationEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TargetPOSConfirmationStartDate", model.TargetPOSConfirmationStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TargetPOSConfirmationEndDate", model.TargetPOSConfirmationEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TradingMeetingStartDate", model.TradingMeetingStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TradingMeetingEndDate", model.TradingMeetingEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@STC_MandatoryStartDate", model.STC_MandatoryStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@STC_MandatoryEndDate", model.STC_MandatoryEndDate, DbType.String, ParameterDirection.Input);

                parameters.Add("@Segment_POSTypeStartDate", model.Segment_POSTypeStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@Segment_POSTypeEndDate", model.Segment_POSTypeEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V1StartDate", model.V1StartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V1EndDate", model.V1EndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V2StartDate", model.V2StartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V2EndDate", model.V2EndDate, DbType.String, ParameterDirection.Input);

                parameters.Add("@Year", model.Year, DbType.String, ParameterDirection.Input);
                parameters.Add("@Name", model.WindowContent, DbType.String, ParameterDirection.Input);
                parameters.Add("@Season", model.Season, DbType.String, ParameterDirection.Input);
                return conn.Execute(updateSql);
            }

        }

        /// <summary>
        /// 添加买货窗口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel NewBuyWindowSP(SIDE_PLM_BLR_BuyWindow model)
        {
            ResultModel result = new ResultModel();
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@POSConfirmationStartDate", model.POSConfirmationStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@POSConfirmationEndDate", model.POSConfirmationEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TargetPOSConfirmationStartDate", model.TargetPOSConfirmationStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TargetPOSConfirmationEndDate", model.TargetPOSConfirmationEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TradingMeetingStartDate", model.TradingMeetingStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@TradingMeetingEndDate", model.TradingMeetingEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@STC_MandatoryStartDate", model.STC_MandatoryStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@STC_MandatoryEndDate", model.STC_MandatoryEndDate, DbType.String, ParameterDirection.Input);

                parameters.Add("@Segment_POSTypeStartDate", model.Segment_POSTypeStartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@Segment_POSTypeEndDate", model.Segment_POSTypeEndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V1StartDate", model.V1StartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V1EndDate", model.V1EndDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V2StartDate", model.V2StartDate, DbType.String, ParameterDirection.Input);
                parameters.Add("@V2EndDate", model.V2EndDate, DbType.String, ParameterDirection.Input);

                parameters.Add("@Year", model.Year, DbType.String, ParameterDirection.Input);
                parameters.Add("@Name", model.WindowContent, DbType.String, ParameterDirection.Input);
                parameters.Add("@Season", model.Season, DbType.String, ParameterDirection.Input);
                parameters.Add("@CreatePeople", model.CreatePeople, DbType.String, ParameterDirection.Input);
                parameters.Add("@ReturnMsg", "", DbType.String, ParameterDirection.Output, 100);
                parameters.Add("@ReturnCode", "0", DbType.String, ParameterDirection.Output, 100);
                conn.Execute("SIDE_NewBuyingWindow", parameters, null, null, CommandType.StoredProcedure);
                string ReturnCode = parameters.Get<string>("@ReturnCode");
                result.StatusCode = ReturnCode.ToString();
                result.Message = parameters.Get<string>("@ReturnMsg");
                if (ReturnCode == "1")
                {
                    result.IsSuccess = true;

                }
                return result;
            }
        }

        /// <summary>
        /// 查询没有新建买货窗口的季度列表
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public async Task<List<string>> GetSeasonListByYear(string Year)
        {
            // List<string> listSeason = new List<string>();
            // ResultModel result = new ResultModel();
            string sqlstr = " select YS.Season from(select 'Q1' as Season,@Year as [Year] union select 'Q2',@Year union select 'Q3',@Year union select 'Q4',@Year) as YS  left join  SIDE_PLM_BLR_BuyWindow as W on w.[Year]=YS.[Year] and W.Season=YS.Season where ID is null ";
            //using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            //{
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Year", Year, DbType.String, ParameterDirection.Input);
            //    IDataReader read=  conn.ExecuteReader(sqlstr, parameters, null, null, CommandType.Text);
            //    while (read.Read())
            //    {                   
            //        string Season = read["Season"].ToString();
            //        listSeason.Add(Season);
            //    }
            //    result.IsSuccess = true;
            //    result.Data = listSeason;
            //}
            //return result;
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //string selectSql = @"SELECT Id, UserName, Password, Gender, Birthday, CreateDate, IsDelete FROM dbo.Users";
                return await Task.Run(() => conn.Query<string>(sqlstr, parameters).ToList());
            }

        }
        public async Task<SIDE_PLM_BLR_BuyWindow> GetNewestBuyWindow()
        {
            string sql = @"select *  from SIDE_PLM_BLR_BuyWindow where [YEAR]=
              (SELECT MAX([Year]) from SIDE_PLM_BLR_BuyWindow
              ) AND [Season]=(SELECT MAX(Season) from SIDE_PLM_BLR_BuyWindow WHERE [YEAR]=(SELECT MAX([Year]) from SIDE_PLM_BLR_BuyWindow))";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<SIDE_PLM_BLR_BuyWindow>(sql).FirstOrDefault());
            }
        }
    }
}

