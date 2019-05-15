using Dapper;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.Repository
{
    public class POS_MasterRepository : RepositoryBase<POS_Master>, IPOS_MasterRepository
    {
        /// <summary>
        /// shipcode 可以是数组 用329093293/32132143324分开 判断Ship Code 是否填写正确
        /// </summary>
        /// <param name="shipcode"></param>
        /// <returns></returns>
        public bool CheckShipcode(string shipcode, string type)
        {
            if (string.IsNullOrEmpty(shipcode))
            {
                return true;
            }
            if (shipcode.ToUpper() == "TBD")
            {
                return true;
            }
            string Productchannel = GetProductchannel(shipcode);
            if (string.IsNullOrEmpty(Productchannel))
            {
                return false;
            }
            else
            {
                if (Productchannel.ToUpper() == type.ToUpper())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        public int CheckPOSReviewBLRStatus(int UserId, string year, string season)
        {
            string selectsql = "";//Submitted By AS、Approved By ASM、Rejected By ASM
            selectsql = " select count(ID)  from SIDE_PLM_BLR_POSReview Where AccountSales=@UserId and Season=@Season and [Year]=@Year and PosState in ('Approved By ASM','Submitted by AS') ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Year", year, DbType.String, ParameterDirection.Input);
            parameters.Add("@Season", season, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = conn.ExecuteScalar<int>(selectsql, parameters);
                return result;
            }
        }

        public string GetProductchannel(string shipcode)
        {
            string selectsql = " select isnull(Product_Channel,'') from  SIDE_PLM_BLR_CustomersInfo where Customer_Code=( select  customer  from SIDE_PLM_STA where id=@id )";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", shipcode, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = conn.ExecuteScalar<string>(selectsql, parameters);
                return result;
            }
        }

        public int ExistshipCode(string shipcode, int shipcodeindex)
        {
            string selectsql = "";
            DynamicParameters parameters = new DynamicParameters();
            if (shipcodeindex == 1)
            {
                selectsql = " select count(1)  from SIDE_PLM_POS_Master Where STC_CORE=@shipcode";

            }
            else if (shipcodeindex == 2)
            {
                selectsql = " select count(1)  from SIDE_PLM_POS_Master Where STC_OCS=@shipcode";

            }
            else if (shipcodeindex == 3)
            {

                selectsql = " select count(1)  from SIDE_PLM_POS_Master Where STC_NEO=@shipcode";
            }
            else if (shipcodeindex == 4)
            {
                selectsql = " select count(1)  from SIDE_PLM_POS_Master Where STC_KIDS=@shipcode";

            }
            parameters.Add("@shipcode", shipcode, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = conn.ExecuteScalar<int>(selectsql, parameters);
                return result;
            }
        }

        public string GetMaxPOSCode(string paraname)
        {
            string selectsql = " select isnull(max(indexnum),0) from (select  REPLACE (POS_CODE,@paraname, '') as indexnum, POS_CODE from SIDE_PLM_POS_Master where POS_CODE like @paraname+'%' and ISNUMERIC(  REPLACE (POS_CODE,@paraname, ''))>0) as T";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@paraname", paraname, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return conn.ExecuteScalar(selectsql, parameters).ToString();

            }
        }


        /// <summary>
        /// 添加 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel InsertPos_MasterInfo(POS_Master model)
        {
            ResultModel result = new ResultModel();
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TM_YEAR", model.TM_YEAR, DbType.String, ParameterDirection.Input);
                parameters.Add("@TM_SEASON", model.TM_SEASON, DbType.String, ParameterDirection.Input);
                parameters.Add("@CUSTOMER_CODE", model.CUSTOMER_CODE, DbType.String, ParameterDirection.Input);

                parameters.Add("@CUSTOMER_GROUP", model.CUSTOMER_GROUP, DbType.String, ParameterDirection.Input);

                parameters.Add("@CUSTOMER_NAME", model.CUSTOMER_NAME, DbType.String, ParameterDirection.Input);
                parameters.Add("@CURRENT_STATUS", model.CURRENT_STATUS, DbType.String, ParameterDirection.Input); //
                                                                                                                  //   parameters.Add("@CURRENT_COMPSTATUS", model.CURRENT_COMPSTATUS, DbType.String , ParameterDirection.Input);
                parameters.Add("@BRANCH_EN", model.BRANCH_EN, DbType.String, ParameterDirection.Input);

                parameters.Add("@BRANCH_CN", model.BRANCH_CN, DbType.String, ParameterDirection.Input);

                parameters.Add("@WHS_CHANNEL", model.WHS_CHANNEL, DbType.String, ParameterDirection.Input);
                parameters.Add("@STC_OCS", model.STC_OCS, DbType.String, ParameterDirection.Input);

                parameters.Add("@STC_CORE", model.STC_CORE, DbType.String, ParameterDirection.Input);
                parameters.Add("@POS_NAME", model.POS_NAME, DbType.String, ParameterDirection.Input);
                //parameters.Add("@Product_Channel_Core", model.Product_Channel_Core, DbType.String, ParameterDirection.Input);
                //parameters.Add("@Product_Channel_OCS", model.Product_Channel_OCS, DbType.String, ParameterDirection.Input);
                //parameters.Add("@Product_Channel_NEO", model.Product_Channel_NEO, DbType.String, ParameterDirection.Input);
                //parameters.Add("@Product_Channel_KIDS", model.Product_Channel_KIDS, DbType.String, ParameterDirection.Input);
                parameters.Add("@STC_NEO", model.STC_NEO, DbType.String, ParameterDirection.Input);
                parameters.Add("@STC_KIDS", model.STC_KIDS, DbType.String, ParameterDirection.Input);

                parameters.Add("@STA_BULK_CORE", model.STA_BULK_CORE, DbType.String, ParameterDirection.Input);
                parameters.Add("@STA_BULK_OCS", model.STA_BULK_OCS, DbType.String, ParameterDirection.Input);
                parameters.Add("@STA_BULK_NEO", model.STA_BULK_NEO, DbType.String, ParameterDirection.Input);
                parameters.Add("@STA_BULK_KIDS", model.STA_BULK_KIDS, DbType.String, ParameterDirection.Input);
                parameters.Add("@Kids_in_Large_Store", model.Kids_in_Large_Store, DbType.String, ParameterDirection.Input);
                parameters.Add("@OCS_Type_in_Large_Store", model.OCS_Type_in_Large_Store, DbType.String, ParameterDirection.Input);
                parameters.Add("@NEO_In_Large_Store", model.NEO_In_Large_Store, DbType.String, ParameterDirection.Input);
                parameters.Add("@Terrex_In_Large_Store", model.Terrex_In_Large_Store, DbType.String, ParameterDirection.Input);
                parameters.Add("@Region", model.Region, DbType.String, ParameterDirection.Input);
                parameters.Add("@PROVINCE_CN", model.PROVINCE_CN, DbType.String, ParameterDirection.Input);

                parameters.Add("@PROVINCE_EN", model.PROVINCE_EN, DbType.String, ParameterDirection.Input);
                parameters.Add("@CITY_CN", model.CITY_CN, DbType.String, ParameterDirection.Input);
                parameters.Add("@CITY_EN", model.CITY_EN, DbType.String, ParameterDirection.Input);
                parameters.Add("@CITY_TIER", model.CITY_TIER, DbType.String, ParameterDirection.Input);
                parameters.Add("@ADDRESS", model.ADDRESS, DbType.String, ParameterDirection.Input);
                parameters.Add("@BLRQ1", model.BLRQ1, DbType.String, ParameterDirection.Input);
                parameters.Add("@BLRQ2", model.BLRQ2, DbType.String, ParameterDirection.Input);
                parameters.Add("@BLRQ3", model.BLRQ3, DbType.String, ParameterDirection.Input);
                parameters.Add("@BLRQ4", model.BLRQ4, DbType.String, ParameterDirection.Input);
                parameters.Add("@COMPQ1", model.COMPQ1, DbType.String, ParameterDirection.Input);
                parameters.Add("@POS_Status", model.POS_Status, DbType.String, ParameterDirection.Input);
                parameters.Add("@COMPQ2", model.COMPQ2, DbType.String, ParameterDirection.Input);
                parameters.Add("@COMPQ3", model.COMPQ3, DbType.String, ParameterDirection.Input);
                parameters.Add("@COMPQ4", model.COMPQ4, DbType.String, ParameterDirection.Input);
                parameters.Add("@AP_POS_BUY", model.AP_POS_BUY, DbType.String, ParameterDirection.Input);
                parameters.Add("@GROSS_SPACE", model.GROSS_SPACE, DbType.String, ParameterDirection.Input);
                parameters.Add("@SELLING_SPACE", model.SELLING_SPACE, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("@STOCK_SPACE", model.STOCK_SPACE, DbType.String, ParameterDirection.Input);
                parameters.Add("@FCST_MPSA_IN_KSR", model.FCST_MPSA_IN_KSR, DbType.String, ParameterDirection.Input);
                parameters.Add("@FCST_MPSA", model.FCST_MPSA, DbType.String, ParameterDirection.Input);
                parameters.Add("@Buying_REGION", model.Buying_REGION, DbType.String, ParameterDirection.Input);
                parameters.Add("@POS_CODE", model.POS_CODE, DbType.String, ParameterDirection.Input);
                parameters.Add("@BIG_FORMAT", model.BIG_FORMAT, DbType.String, ParameterDirection.Input);
                parameters.Add("@Biz_Mode", model.Biz_Mode, DbType.String, ParameterDirection.Input);
                parameters.Add("@Product_Channel", model.Product_Channel, DbType.String, ParameterDirection.Input);
                parameters.Add("@PackagePOS", model.PackagePOS, DbType.String, ParameterDirection.Input);
                parameters.Add("@AssortmentPOS", model.AssortmentPOS, DbType.String, ParameterDirection.Input);
                // parameters.Add("@Venue_Name", model.Venue_Name, DbType.String, ParameterDirection.Input);
                // parameters.Add("@POS_Level_Buy", model.POS_Level_Buy, DbType.String, ParameterDirection.Input);
                parameters.Add("@Comments", model.Comments, DbType.String, ParameterDirection.Input);
                parameters.Add("@Grading_MB", model.Grading_MB, DbType.String, ParameterDirection.Input);
                parameters.Add("@Grading_Kids", model.Grading_Kids, DbType.String, ParameterDirection.Input);
                //  parameters.Add("@OLD_POS_CODE", model.OLD_POS_CODE, DbType.String, ParameterDirection.Input);
                // parameters.Add("@OCS_NS_Target", model.OCS_NS_Target, DbType.String, ParameterDirection.Input);
                parameters.Add("@POS_CHANNEL", model.POS_CHANNEL, DbType.String, ParameterDirection.Input);
                //  parameters.Add("@Seg_Marketplace", model.Seg_Marketplace, DbType.String, ParameterDirection.Input);//
                // parameters.Add("@Seg_Business", model.Seg_Business, DbType.String, ParameterDirection.Input);//
                parameters.Add("@OCS_Grading", model.OCS_Grading, DbType.String, ParameterDirection.Input);
                parameters.Add("@POS_Type", model.POS_Type, DbType.String, ParameterDirection.Input);
                parameters.Add("@Grading_OCS", model.Grading_OCS, DbType.String, ParameterDirection.Input);//
                                                                                                           //  parameters.Add("@POS_Cluster", model.POS_Cluster, DbType.String, ParameterDirection.Input);//            
                parameters.Add("@SUB_CHANNEL", model.SUB_CHANNEL, DbType.String, ParameterDirection.Input);
                parameters.Add("@ReturnMsg", "", DbType.String, ParameterDirection.Output, 100);
                parameters.Add("@ReturnCode", "0", DbType.String, ParameterDirection.Output, 100);
                conn.Execute("SIDE_Proc_AddPOS_MasterInfo", parameters, null, null, CommandType.StoredProcedure);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetListByUserRole(string sqlstr, DynamicParameters parameters)
        {

            DataTable dt = new DataTable();
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                dt.Load(conn.ExecuteReader(sqlstr, parameters));
            }
            return dt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetListByUser(int UserId, string Year, string Season)
        {
            string Typestr = "";// "SO";//写一个函数查询角色权限
            string wheresql = "";//  
            //Account Sales看到其下维护的Customer的店铺信息，Area Sales Manager看到的是Approval Process下维护的所有Account Sales的店铺信息
            if (Typestr == "AS" || Typestr == "HAS")//AS_ID=2 OR ASM_ID=2   OR  SD_ID=2   OR  AH_ID=2  OR  SVP_ID=2
            {
                wheresql = "   and (AS_ID=@UserID OR ASM_ID=@UserID    OR  SD_ID=@UserID   OR  AH_ID=@UserID  OR  SVP_ID=@UserID)";
            }
            else if (Typestr == "SO")//
            {
                wheresql = "   and  BRANCH_EN in (select branch from SIDE_PLM_BLR_Customer_group where ID in (select value from dbo.Side_Split((select Branch from SIDE_PLM_BLR_SalesOperation where ID = @UserID),',')))  ";
            }
            else
            {//BlR 看所有

            }
            string sqlstr = string.Format(@"	  select t.TM_SEASON,t.TM_Year,POS_CHANNEL as Channel ,t.CUSTOMER_GROUP as CUSTOMER,t.BRANCH_EN as Branch,isnull(Account_Sales,'') as ASName
,  SUM(CASE WHEN t.TM_SEASON ='Q1' AND t.BLRQ1 = 'Y' THEN 1 
				                                            WHEN t.TM_SEASON = 'Q2' AND t.BLRQ2='Y' THEN 1
				                                            WHEN t.TM_SEASON = 'Q3' AND t.BLRQ3 = 'Y' THEN 1
				                                            WHEN t.TM_SEASON = 'Q4' AND t.BLRQ4='Y' THEN 1 
				                                             ELSE 0 END) AS POSY,
															SUM(CASE WHEN t.TM_SEASON ='Q1' AND t.BLRQ1 = 'N' THEN 1 
				                                            WHEN t.TM_SEASON = 'Q2' AND t.BLRQ2='N' THEN 1
				                                            WHEN t.TM_SEASON = 'Q3' AND t.BLRQ3 = 'N' THEN 1
				                                            WHEN t.TM_SEASON = 'Q4' AND t.BLRQ4='N' THEN 1 
				                                            ELSE 0 END) AS POSN,
																SUM(CASE WHEN t.TM_SEASON ='Q1' AND t.BLRQ1 is not null THEN 1 
				                                            WHEN t.TM_SEASON = 'Q2' AND t.BLRQ2 is not null THEN 1
				                                            WHEN t.TM_SEASON = 'Q3' AND t.BLRQ3 is not null THEN 1
				                                            WHEN t.TM_SEASON = 'Q4' AND t.BLRQ4 is not null THEN 1 
				                                            ELSE 0 END) AS POS,RePosState as PosState

 from [SIDE_vw_PLM_BLR_POS_Master_Overview] t  where t.TM_Year=@Year and t.TM_SEASON=@Season  and t.POS_Status='Y' {0}
 group by POS_CHANNEL,t.CUSTOMER_GROUP,t.BRANCH_EN ,Account_Sales,RePosState,t.TM_SEASON,t.TM_Year", wheresql);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", UserId, DbType.Int16, ParameterDirection.Input);
            parameters.Add("@Year", Year, DbType.String, ParameterDirection.Input);
            parameters.Add("@Season", Season, DbType.String, ParameterDirection.Input);
            DataTable dt = new DataTable();
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                dt.Load(conn.ExecuteReader(sqlstr, parameters));
            }
            return dt;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<POS_Master>> GetAllList()
        {
            string selectSql = @"SELECT * FROM [dbo].[SIDE_PLM_POS_Master]";
            return await Select(selectSql);
        }


        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<List<POS_Master>, int>> GetEntities(string whereStr)
        {
            Tuple<List<POS_Master>, int> tp = await GetPageByProcList("SIDE_PLM_POS_Master", "*", whereStr, "ID", 1, 10);
            return tp;
        }
        /// <summary>
        /// Get Singel Entity 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<POS_Master> GetSingelEntity(string TM_YEAR, string TM_SEASON, string POS_CODE)
        {
            string selectSql = @"select * from [dbo].[SIDE_PLM_POS_Master] where TM_YEAR=@TM_YEAR and TM_SEASON=@TM_SEASON and POS_CODE=@POS_CODE";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<POS_Master>(selectSql, new { TM_YEAR = TM_YEAR, TM_SEASON = TM_SEASON, POS_CODE = POS_CODE });
            }
        }
        /// <summary>
        /// Post Entity
        /// </summary>
        /// <returns></returns>
        public async Task PostEntity(POS_Master entity)
        {
            string insertSql = @"INSERT INTO [dbo].[SIDE_PLM_POS_Master](
                                [TM_YEAR]
                               ,[TM_SEASON]
                               ,[POS_Status]
                               ,[POS_CODE]
                               ,[OLD_POS_CODE]
                               ,[POS_NAME]
                               ,[POS_CHANNEL]
                               ,[SUB_CHANNEL]
                               ,[CUSTOMER_CODE]
                               ,[CUSTOMER_GROUP]
                               ,[CUSTOMER_NAME]
                               ,[BRANCH_EN]
                               ,[BRANCH_CN]
                               ,[WHS_CHANNEL]
                               ,[STC_CORE]
                               ,[STC_OCS]
                               ,[STC_NEO]
                               ,[STC_KIDS]
                               ,[STA_BULK_CORE]
                               ,[STA_BULK_OCS]
                               ,[STA_BULK_NEO]
                               ,[STA_BULK_KIDS]
                               ,[Kids_in_Large_Store]
                               ,[OCS_Type_in_Large_Store]
                               ,[NEO_In_Large_Store]
                               ,[Terrex_In_Large_Store]
                               ,[Region]
                               ,[PROVINCE_CN]
                               ,[PROVINCE_EN]
                               ,[CITY_CN]
                               ,[CITY_EN]
                               ,[CITY_TIER]
                               ,[ADDRESS]
                               ,[BLRQ1]
                               ,[BLRQ2]
                               ,[BLRQ3]
                               ,[BLRQ4]
                               ,[COMPQ1]
                               ,[COMPQ2]
                               ,[COMPQ3]
                               ,[COMPQ4]
,CURRENT_STATUS
,CURRENT_COMPSTATUS
                               ,[AP_POS_BUY]
                               ,[GROSS_SPACE]
                               ,[SELLING_SPACE]
                               ,[STOCK_SPACE]
                               ,[FCST_MPSA_IN_KSR]
                               ,[FCST_MPSA]
                               ,[Buying_REGION]
                               ,[BIG_FORMAT]
                               ,[Biz_Mode]
                               ,[Product_Channel]
                               ,[PackagePOS]
                               ,[AssortmentPOS]
                               ,[Venue_Name]
                               ,[POS_Level_Buy]
                               ,[Comments]
                               ,[Grading_MB]
                               ,[Grading_Kids]
                               ,[OCS_NS_Target]
                               ,[Seg_Marketplace_recommended]
                               ,[Seg_Business_recommended]
                               ,[OCS_Grading_recommended]
                               ,[POS_Type_recommended]
                               ,[Grading_OCS_recommended]
                               ,[POS_Cluster_recommended]
                               ,[Category_Cluster_recommended_Running]
                               ,[Category_Cluster_recommended_Basketball]
                               ,[Category_Cluster_recommended_Football]
                               ,[Category_Cluster_recommended_HW]
                               ,[Category_Cluster_recommended_ACC]
                               ,[Category_Cluster_recommended_Outdoor]
                               ,[Category_Cluster_recommended_TrainingW]
                               ,[Category_Cluster_recommended_TrainingM]
                               ,[Category_Cluster_recommended_Swim]
                               ,[Category_Cluster_recommended_Tennis]
                               ,[Category_Cluster_recommended_OutdoorWJ]
                               ,[Division_Gender_Cluster_recommended_AppMen]
                               ,[Division_Gender_Cluster_recommended_AppWomen]
                               ,[Division_Gender_Cluster_recommended_FTWMen]
                               ,[Division_Gender_Cluster_recommended_FTWWomen]
                               ,[Division_Gender_Cluster_recommended_ACCHW]
                               ,[Seg_Marketplace]
                               ,[Seg_Business]
                               ,[OCS_Grading]
                               ,[POS_Type]
                               ,[Grading_OCS]
                               ,[POS_Cluster]
                               ,[Category_Cluster_Running]
                               ,[Category_Cluster_Basketball]
                               ,[Category_Cluster_Football]
                               ,[Category_Cluster_HW]
                               ,[Category_Cluster_ACC]
                               ,[Category_Cluster_Outdoor]
                               ,[Category_Cluster_TrainingW]
                               ,[Category_Cluster_TrainingM]
                               ,[Category_Cluster_Swim]
                               ,[Category_Cluster_Tennis]
                               ,[Category_Cluster_OutdoorWJ]
                               ,[Division_Gender_Cluster_AppMen]
                               ,[Division_Gender_Cluster_AppWomen]
                               ,[Division_Gender_Cluster_FTWMen]
                               ,[Division_Gender_Cluster_FTWWomen]
                               ,[Division_Gender_Cluster_ACCHW]
                               ,[Comments2]
                               ,[Seg_Marketplace_forbuy]
                               ,[Seg_Business_forbuy]
                               ,[OCS_Grading_forbuy]
                               ,[POS_Type_forbuy]
                               ,[Grading_OCS_forbuy]
                               ,[POS_Cluster_forbuy]
                               ,[Category_Cluster_forbuy_Running]
                               ,[Category_Cluster_forbuy_Basketball]
                               ,[Category_Cluster_forbuy_Football]
                               ,[Category_Cluster_forbuy_HW]
                               ,[Category_Cluster_forbuy_ACC]
                               ,[Category_Cluster_forbuy_Outdoor]
                               ,[Category_Cluster_forbuy_TrainingW]
                               ,[Category_Cluster_forbuy_TrainingM]
                               ,[Category_Cluster_forbuy_Swim]
                               ,[Category_Cluster_forbuy_Tennis]
                               ,[Category_Cluster_forbuy_OutdoorWJ]
                               ,[Division_Gender_Cluster_forbuy_AppMen]
                               ,[Division_Gender_Cluster_forbuy_AppWomen]
                               ,[Division_Gender_Cluster_forbuy_FTWMen]
                               ,[Division_Gender_Cluster_forbuy_FTWWomen]
                               ,[Division_Gender_Cluster_forbuy_ACCHW]
                                )VALUES(
                                @TM_YEAR
                               ,@TM_SEASON
                               ,@POS_Status
                               ,@POS_CODE
                               ,@OLD_POS_CODE
                               ,@POS_NAME
                               ,@POS_CHANNEL
                               ,@SUB_CHANNEL
                               ,@CUSTOMER_CODE
                               ,@CUSTOMER_GROUP
                               ,@CUSTOMER_NAME
                               ,@BRANCH_EN
                               ,@BRANCH_CN
                               ,@WHS_CHANNEL
                               ,@STC_CORE
                               ,@STC_OCS
                               ,@STC_NEO
                               ,@STC_KIDS
                               ,@STA_BULK_CORE
                               ,@STA_BULK_OCS
                               ,@STA_BULK_NEO
                               ,@STA_BULK_KIDS
                               ,@Kids_in_Large_Store
                               ,@OCS_Type_in_Large_Store
                               ,@NEO_In_Large_Store
                               ,@Terrex_In_Large_Store
                               ,@Region
                               ,@PROVINCE_CN
                               ,@PROVINCE_EN
                               ,@CITY_CN
                               ,@CITY_EN
                               ,@CITY_TIER
                               ,@ADDRESS
                               ,@BLRQ1
                               ,@BLRQ2
                               ,@BLRQ3
                               ,@BLRQ4
                               ,@COMPQ1
                               ,@COMPQ2
                               ,@COMPQ3
                               ,@COMPQ4
,@CURRENT_STATUS
,@CURRENT_COMPSTATUS
                               ,@AP_POS_BUY
                               ,@GROSS_SPACE
                               ,@SELLING_SPACE
                               ,@STOCK_SPACE
                               ,@FCST_MPSA_IN_KSR
                               ,@FCST_MPSA
                               ,@Buying_REGION
                               ,@BIG_FORMAT
                               ,@Biz_Mode
                               ,@Product_Channel
                               ,@PackagePOS
                               ,@AssortmentPOS
                               ,@Venue_Name
                               ,@POS_Level_Buy
                               ,@Comments
                               ,@Grading_MB
                               ,@Grading_Kids
                               ,@OCS_NS_Target
                               ,@Seg_Marketplace_recommended
                               ,@Seg_Business_recommended
                               ,@OCS_Grading_recommended
                               ,@POS_Type_recommended
                               ,@Grading_OCS_recommended
                               ,@POS_Cluster_recommended
                               ,@Category_Cluster_recommended_Running
                               ,@Category_Cluster_recommended_Basketball
                               ,@Category_Cluster_recommended_Football
                               ,@Category_Cluster_recommended_HW
                               ,@Category_Cluster_recommended_ACC
                               ,@Category_Cluster_recommended_Outdoor
                               ,@Category_Cluster_recommended_TrainingW
                               ,@Category_Cluster_recommended_TrainingM
                               ,@Category_Cluster_recommended_Swim
                               ,@Category_Cluster_recommended_Tennis
                               ,@Category_Cluster_recommended_OutdoorWJ
                               ,@Division_Gender_Cluster_recommended_AppMen
                               ,@Division_Gender_Cluster_recommended_AppWomen
                               ,@Division_Gender_Cluster_recommended_FTWMen
                               ,@Division_Gender_Cluster_recommended_FTWWomen
                               ,@Division_Gender_Cluster_recommended_ACCHW
                               ,@Seg_Marketplace
                               ,@Seg_Business
                               ,@OCS_Grading
                               ,@POS_Type
                               ,@Grading_OCS
                               ,@POS_Cluster
                               ,@Category_Cluster_Running
                               ,@Category_Cluster_Basketball
                               ,@Category_Cluster_Football
                               ,@Category_Cluster_HW
                               ,@Category_Cluster_ACC
                               ,@Category_Cluster_Outdoor
                               ,@Category_Cluster_TrainingW
                               ,@Category_Cluster_TrainingM
                               ,@Category_Cluster_Swim
                               ,@Category_Cluster_Tennis
                               ,@Category_Cluster_OutdoorWJ
                               ,@Division_Gender_Cluster_AppMen
                               ,@Division_Gender_Cluster_AppWomen
                               ,@Division_Gender_Cluster_FTWMen
                               ,@Division_Gender_Cluster_FTWWomen
                               ,@Division_Gender_Cluster_ACCHW
                               ,@Comments2
                               ,@Seg_Marketplace_forbuy
                               ,@Seg_Business_forbuy
                               ,@OCS_Grading_forbuy
                               ,@POS_Type_forbuy
                               ,@Grading_OCS_forbuy
                               ,@POS_Cluster_forbuy
                               ,@Category_Cluster_forbuy_Running
                               ,@Category_Cluster_forbuy_Basketball
                               ,@Category_Cluster_forbuy_Football
                               ,@Category_Cluster_forbuy_HW
                               ,@Category_Cluster_forbuy_ACC
                               ,@Category_Cluster_forbuy_Outdoor
                               ,@Category_Cluster_forbuy_TrainingW
                               ,@Category_Cluster_forbuy_TrainingM
                               ,@Category_Cluster_forbuy_Swim
                               ,@Category_Cluster_forbuy_Tennis
                               ,@Category_Cluster_forbuy_OutdoorWJ
                               ,@Division_Gender_Cluster_forbuy_AppMen
                               ,@Division_Gender_Cluster_forbuy_AppWomen
                               ,@Division_Gender_Cluster_forbuy_FTWMen
                               ,@Division_Gender_Cluster_forbuy_FTWWomen
                               ,@Division_Gender_Cluster_forbuy_ACCHW)";
            await Insert(entity, insertSql);
        }
        /// <summary>
        /// Put Entity
        /// </summary>
        /// <returns></returns>
        public async Task PutEntity(POS_Master entity)
        {
            string updateSql = @"UPDATE [dbo].[SIDE_PLM_POS_Master] SET 
                               [TM_YEAR] = @TM_YEAR
                              ,[TM_SEASON] = @TM_SEASON
                              ,[POS_Status] = @POS_Status
                              ,[POS_CODE] = @POS_CODE
                              ,[OLD_POS_CODE] = @OLD_POS_CODE
                              ,[POS_NAME] =@POS_NAME
                              ,[POS_CHANNEL] = @POS_CHANNEL
                              ,[SUB_CHANNEL] = @SUB_CHANNEL
                              ,[CUSTOMER_CODE] = @CUSTOMER_CODE
                              ,[CUSTOMER_GROUP] = @CUSTOMER_GROUP
                              ,[CUSTOMER_NAME] = @CUSTOMER_NAME
                              ,[BRANCH_EN] = @BRANCH_EN
                              ,[BRANCH_CN] = @BRANCH_CN
                              ,[WHS_CHANNEL] = @WHS_CHANNEL
                              ,[STC_CORE] = @STC_CORE
                              ,[STC_OCS] = @STC_OCS
                              ,[STC_NEO] = @STC_NEO
                              ,[STC_KIDS] = @STC_KIDS
                              ,[STA_BULK_CORE] = @STA_BULK_CORE
                              ,[STA_BULK_OCS] = @STA_BULK_OCS
                              ,[STA_BULK_NEO] = @STA_BULK_NEO
                              ,[STA_BULK_KIDS] = @STA_BULK_KIDS
                              ,[Kids_in_Large_Store] = @Kids_in_Large_Store
                              ,[OCS_Type_in_Large_Store] = @OCS_Type_in_Large_Store
                              ,[NEO_In_Large_Store] = @NEO_In_Large_Store
                              ,[Terrex_In_Large_Store] = @Terrex_In_Large_Store
                              ,[Region] = @Region
                              ,[PROVINCE_CN] = @PROVINCE_CN
                              ,[PROVINCE_EN] = @PROVINCE_EN
                              ,[CITY_CN] =@CITY_CN
                              ,[CITY_EN] = @CITY_EN
                              ,[CITY_TIER] = @CITY_TIER
                              ,[ADDRESS] = @ADDRESS
                              ,[BLRQ1] = @BLRQ1
                              ,[BLRQ2] = @BLRQ2
                              ,[BLRQ3] = @BLRQ3
                              ,[BLRQ4] = @BLRQ4
                              ,[COMPQ1] = @COMPQ1
                              ,[COMPQ2] = @COMPQ2
                              ,[COMPQ3] = @COMPQ3
                              ,[COMPQ4] = @COMPQ4
                              ,[AP_POS_BUY] = @AP_POS_BUY
                              ,[GROSS_SPACE] = @GROSS_SPACE
                              ,[SELLING_SPACE] = @SELLING_SPACE
                              ,[STOCK_SPACE] = @STOCK_SPACE
                              ,[FCST_MPSA_IN_KSR] = @FCST_MPSA_IN_KSR
                              ,[FCST_MPSA] = @FCST_MPSA
                              ,[Buying_REGION] = @Buying_REGION
                              ,[BIG_FORMAT] = @BIG_FORMAT
                              ,[Biz_Mode] = @Biz_Mode
                              ,[Product_Channel] = @Product_Channel
                              ,[PackagePOS] = @PackagePOS
                              ,[AssortmentPOS] = @AssortmentPOS
                              ,[Venue_Name] = @Venue_Name
                              ,[POS_Level_Buy] = @POS_Level_Buy
                              ,[Comments] = @Comments
                              ,[Grading_MB] = @Grading_MB
                              ,Grading_Kids=@Grading_Kids
                              ,[OCS_NS_Target] = @OCS_NS_Target
                              ,[Seg_Marketplace_recommended] = @Seg_Marketplace_recommended
                              ,[Seg_Business_recommended] = @Seg_Business_recommended
                              ,[OCS_Grading_recommended] = @OCS_Grading_recommended
                              ,[POS_Type_recommended] = @POS_Type_recommended
                              ,[Grading_OCS_recommended] = @Grading_OCS_recommended
                              ,[POS_Cluster_recommended] = @POS_Cluster_recommended
                              ,[Category_Cluster_recommended_Running] = @Category_Cluster_recommended_Running
                              ,[Category_Cluster_recommended_Basketball] = @Category_Cluster_recommended_Basketball
                              ,[Category_Cluster_recommended_Football] = @Category_Cluster_recommended_Football
                              ,[Category_Cluster_recommended_HW] = @Category_Cluster_recommended_HW
                              ,[Category_Cluster_recommended_ACC] = @Category_Cluster_recommended_ACC
                              ,[Category_Cluster_recommended_Outdoor] = @Category_Cluster_recommended_Outdoor
                              ,[Category_Cluster_recommended_TrainingW] = @Category_Cluster_recommended_TrainingW
                              ,[Category_Cluster_recommended_TrainingM] = @Category_Cluster_recommended_TrainingM
                              ,[Category_Cluster_recommended_Swim] = @Category_Cluster_recommended_Swim
                              ,[Category_Cluster_recommended_Tennis] = @Category_Cluster_recommended_Tennis
                              ,[Category_Cluster_recommended_OutdoorWJ] = @Category_Cluster_recommended_OutdoorWJ
                              ,[Division_Gender_Cluster_recommended_AppMen] = @Division_Gender_Cluster_recommended_AppMen
                              ,[Division_Gender_Cluster_recommended_AppWomen] = @Division_Gender_Cluster_recommended_AppWomen
                              ,[Division_Gender_Cluster_recommended_FTWMen] = @Division_Gender_Cluster_recommended_FTWMen
                              ,[Division_Gender_Cluster_recommended_FTWWomen] = @Division_Gender_Cluster_recommended_FTWWomen
                              ,[Division_Gender_Cluster_recommended_ACCHW] = @Division_Gender_Cluster_recommended_ACCHW
                              ,[Seg_Marketplace] = @Seg_Marketplace
                              ,[Seg_Business] = @Seg_Business
                              ,[OCS_Grading] = @OCS_Grading
                              ,[POS_Type] = @POS_Type
                              ,[Grading_OCS] = @Grading_OCS
                              ,[POS_Cluster] = @POS_Cluster
                              ,[Category_Cluster_Running] = @Category_Cluster_Running
                              ,[Category_Cluster_Basketball] = @Category_Cluster_Basketball
                              ,[Category_Cluster_Football] = @Category_Cluster_Football
                              ,[Category_Cluster_HW] = @Category_Cluster_HW
                              ,[Category_Cluster_ACC] = @Category_Cluster_ACC
                              ,[Category_Cluster_Outdoor] = @Category_Cluster_Outdoor
                              ,[Category_Cluster_TrainingW] = @Category_Cluster_TrainingW
                              ,[Category_Cluster_TrainingM] = @Category_Cluster_TrainingM
                              ,[Category_Cluster_Swim] = @Category_Cluster_Swim
                              ,[Category_Cluster_Tennis] = @Category_Cluster_Tennis
                              ,[Category_Cluster_OutdoorWJ] = @Category_Cluster_OutdoorWJ
                              ,[Division_Gender_Cluster_AppMen] = @Division_Gender_Cluster_AppMen
                              ,[Division_Gender_Cluster_AppWomen] = @Division_Gender_Cluster_AppWomen
                              ,[Division_Gender_Cluster_FTWMen] = @Division_Gender_Cluster_FTWMen
                              ,[Division_Gender_Cluster_FTWWomen] = @Division_Gender_Cluster_FTWWomen
                              ,[Division_Gender_Cluster_ACCHW] = @Division_Gender_Cluster_ACCHW
                              ,[Comments2] = @Comments2
                              ,[Seg_Marketplace_forbuy] = @Seg_Marketplace_forbuy
                              ,[Seg_Business_forbuy] = @Seg_Business_forbuy
                              ,[OCS_Grading_forbuy] = @OCS_Grading_forbuy
                              ,[POS_Type_forbuy] = @POS_Type_forbuy
                              ,[Grading_OCS_forbuy] = @Grading_OCS_forbuy
                              ,[POS_Cluster_forbuy] = @POS_Cluster_forbuy
                              ,[Category_Cluster_forbuy_Running] = @Category_Cluster_forbuy_Running
                              ,[Category_Cluster_forbuy_Basketball] = @Category_Cluster_forbuy_Basketball
                              ,[Category_Cluster_forbuy_Football] = @Category_Cluster_forbuy_Football
                              ,[Category_Cluster_forbuy_HW] = @Category_Cluster_forbuy_HW
                              ,[Category_Cluster_forbuy_ACC] = @Category_Cluster_forbuy_ACC
                              ,[Category_Cluster_forbuy_Outdoor] = @Category_Cluster_forbuy_Outdoor
                              ,[Category_Cluster_forbuy_TrainingW] = @Category_Cluster_forbuy_TrainingW
                              ,[Category_Cluster_forbuy_TrainingM] = @Category_Cluster_forbuy_TrainingM
                              ,[Category_Cluster_forbuy_Swim] = @Category_Cluster_forbuy_Swim
                              ,[Category_Cluster_forbuy_Tennis] = @Category_Cluster_forbuy_Tennis
                              ,[Category_Cluster_forbuy_OutdoorWJ] = @Category_Cluster_forbuy_OutdoorWJ
                              ,[Division_Gender_Cluster_forbuy_AppMen] = @Division_Gender_Cluster_forbuy_AppMen
                              ,[Division_Gender_Cluster_forbuy_AppWomen] = @Division_Gender_Cluster_forbuy_AppWomen
                              ,[Division_Gender_Cluster_forbuy_FTWMen] = @Division_Gender_Cluster_forbuy_FTWMen
                              ,[Division_Gender_Cluster_forbuy_FTWWomen] = @Division_Gender_Cluster_forbuy_FTWWomen
                              ,[Division_Gender_Cluster_forbuy_ACCHW] = @Division_Gender_Cluster_forbuy_ACCHW
                                WHERE TM_YEAR=@TM_YEAR AND TM_SEASON=@TM_SEASON AND POS_CODE=@POS_CODE";
            await Update(entity, updateSql);
        }
        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <returns></returns>
        public async Task DeleteEntity(int Id)
        {
            string deleteSql = "DELETE FROM [dbo].[SIDE_PLM_POS_Master] WHERE Id=@Id";
            await Delete(Id, deleteSql);
        }
        /// <summary>
        /// 获取AS待提交的POS
        /// </summary>
        /// <param name="YEAR"></param>
        /// <param name="SEASON"></param>
        /// <param name="AS"></param>
        /// <returns></returns>
        public async Task<List<POS_Master>> GetApprovalPOS(string YEAR, string SEASON, string AS_ID)
        {
            string sql = @"  SELECT  
                        pos_master.*
                        FROM [dbo].[SIDE_PLM_POS_Master]  pos_master
	                        left join SIDE_PLM_BLR_CustomersInfo  customer
	                        on pos_master.CUSTOMER_CODE= customer.CUSTOMER_CODE
                            where TM_YEAR=2020 and TM_SEASON='Q1' and Account_SalesID=470
	                        and (not exists (select * from SIDE_PLM_BLR_POSReview where POSCode=pos_master.POS_CODE
							 and [YEAR]=pos_master.TM_YEAR and Season=pos_master.TM_SEASON )
							 or 
							  exists (select * from SIDE_PLM_BLR_POSReview where POSCode=pos_master.POS_CODE
							 and [YEAR]=pos_master.TM_YEAR and Season=pos_master.TM_SEASON and PosState='In Processing')
							 )";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<POS_Master>(sql, new { YEAR, SEASON, AS_ID }).ToList());
            }
        }
        /// <summary>
        /// 获取ASM审批数据
        /// </summary>
        /// <param name="YEAR"></param>
        /// <param name="SEASON"></param>
        /// <param name="AS_ID"></param>
        /// <param name="ASM_ID"></param>
        /// <returns></returns>
        public async Task<List<ASApprovalModel>> GetASMApproval(string YEAR, string SEASON, int ASM_ID, bool isSO, string userName)
        {
            List<ASApprovalModel> list;
            string filer = "";
            if (!isSO)
            {
                //非SO 角色
                filer = @" and  [PosState]='Submitted By AS' and AccountSales in 
                          (select AS_ID  from SIDE_PLM_BLR_Approve_Process WHERE ASM_ID = @ASM_ID)";
            }
            string sql = string.Format(@"SELECT  pos_master.POS_CHANNEL,pos_master.CUSTOMER_GROUP,pos_master.BRANCH_EN ,customer.Account_Sales,customer.Account_SalesID,sum(1) Total,
                        SUM(CASE WHEN pos_master.TM_SEASON ='Q1' AND pos_master.BLRQ1 = 'Y' THEN 1 
				                                                                    WHEN pos_master.TM_SEASON = 'Q2' AND pos_master.BLRQ2='Y' THEN 1
				                                                                    WHEN pos_master.TM_SEASON = 'Q3' AND pos_master.BLRQ3 = 'Y' THEN 1
				                                                                    WHEN pos_master.TM_SEASON = 'Q4' AND pos_master.BLRQ4='Y' THEN 1 
				                                                                    ELSE 0 END) [BLR_Y],
                        SUM(CASE WHEN pos_master.TM_SEASON ='Q1' AND pos_master.BLRQ1 = 'N' THEN 1 
				                                                                    WHEN pos_master.TM_SEASON = 'Q2' AND pos_master.BLRQ2='N' THEN 1
				                                                                    WHEN pos_master.TM_SEASON = 'Q3' AND pos_master.BLRQ3 = 'N' THEN 1
				                                                                    WHEN pos_master.TM_SEASON = 'Q4' AND pos_master.BLRQ4='N' THEN 1 
				                                                                    ELSE 0 END) [BLR_N],PosState
                          FROM (select *  from (SELECT 
                              [POSCode]
                              ,[AccountSales]
                              ,[AccountSalesName]
                              ,[AccountSalesCreateTime]
                              ,[Season]
                              ,[Year]
                              ,[PosState]
                          FROM [dbo].[SIDE_PLM_BLR_POSReview] WHERE [Year]=@YEAR and [Season]=@SEASON  {0}
                           ) POS_Review
                          inner join SIDE_PLM_POS_Master on SIDE_PLM_POS_Master.TM_YEAR=POS_Review.[Year]  and SIDE_PLM_POS_Master.POS_CODE=POS_Review.POSCode 
                           and SIDE_PLM_POS_Master.TM_SEASON=POS_Review.Season )  pos_master
                           left join SIDE_PLM_BLR_CustomersInfo  customer
                          on pos_master.CUSTOMER_CODE= customer.CUSTOMER_CODE
	                        group by pos_master.POS_CHANNEL,pos_master.CUSTOMER_GROUP,pos_master.BRANCH_EN,customer.Account_Sales,customer.Account_SalesID,pos_master.PosState", filer);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                list = await Task.Run(() => conn.Query<ASApprovalModel>(sql, new { YEAR, SEASON, ASM_ID }).ToList());
            }
            if (isSO)
            {
                string sql1 = "select  Branch  from SIDE_PLM_BLR_SalesOperation where SalesName=@userName";
                string sql2 = "select Branch  from SIDE_PLM_BLR_Customer_Group where ID in (@branchId)";
                using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
                {
                    SIDE_PLM_BLR_SalesOperation branchentity = await Task.Run(() => conn.QueryFirstOrDefault<SIDE_PLM_BLR_SalesOperation>(sql1, new { userName }));
                    if (branchentity != null)
                    {
                        List<string> branch_list = new List<string>();
                        foreach (var item in branchentity.Branch.Split(','))
                        {
                            CustomerGroup customer = await Task.Run(() => conn.QueryFirstOrDefault<CustomerGroup>(sql2, new { branchId = item }));
                            if (customer != null)
                                branch_list.Add(customer.Branch);
                        }
                        if (branch_list != null)
                            list = list.Where(t => branch_list.Contains(t.BRANCH_EN)).ToList();
                        else
                            list = null;
                    }
                }
            }
            return list;
        }

        public async Task<object> StatisBigFormat(string year, string season, string ASM_ID)
        {
            string sql = @"select 
                          isnull(MAX(case BIG_FORMAT when 'mini BC' then Total else 0 end),0) mini_BC,
                          isnull(MAX(case BIG_FORMAT when 'SWC' then Total else 0 end),0) SWC,
                          isnull(MAX(case BIG_FORMAT when 'Mega L1' then Total else 0 end),0) MegaL1,
                          isnull(MAX(case BIG_FORMAT when 'Mega L2' then Total else 0 end),0) MegaL2,
                          isnull(MAX(case BIG_FORMAT when 'Mega L2 in FOC' then Total else 0 end),0) MegaL2_INFOC,
                          isnull(MAX(case BIG_FORMAT when 'Core Normal' then Total else 0 end),0) Core_Normal,
                           isnull(MAX(case BIG_FORMAT when 'CFS-BB' then Total else 0 end),0)CFS_BB,
                           isnull(MAX(case BIG_FORMAT when 'CFS-RN' then Total else 0 end),0) CFS_RN,
                           isnull(MAX(case BIG_FORMAT when 'CFS-FB' then Total else 0 end),0) CFS_FB,
                           isnull(MAX(case BIG_FORMAT when 'CFS-OD' then Total else 0 end),0) CFS_OD,
                           isnull(MAX(case BIG_FORMAT when 'FWCS L1' then Total else 0 end),0) FWCSL1,
                           isnull(MAX(case BIG_FORMAT when 'FWCS L2' then Total else 0 end),0) FWCSL2,
                           isnull(MAX(case BIG_FORMAT when 'Kids' then Total else 0 end),0) Kids,
                           isnull(MAX(case BIG_FORMAT when 'OCS MONO' then Total else 0 end),0) OCSMONO,
                           isnull(MAX(case BIG_FORMAT when 'Neo' then Total else 0 end),0) Neo,
                           isnull(MAX(case BIG_FORMAT when 'MB' then Total else 0 end),0) MB
                          from (
                        select  SIDE_PLM_POS_Master.BIG_FORMAT,SUM(1) Total  from (SELECT 
                                                      [POSCode]
                                                      ,[AccountSales]
                                                      ,[AccountSalesName]
                                                      ,[AccountSalesCreateTime]
                                                      ,[Season]
                                                      ,[Year]
                                                      ,[PosState]
                                                  FROM [dbo].[SIDE_PLM_BLR_POSReview] WHERE [Year]=@year and [Season]=@season and  [PosState]='Submitted By AS'
                                                  and AccountSales in 
                                                  (select AS_ID  from SIDE_PLM_BLR_Approve_Process WHERE ASM_ID=@ASM_ID)
                                                   ) POS_Review
                                                  inner join SIDE_PLM_POS_Master on SIDE_PLM_POS_Master.TM_YEAR=POS_Review.[Year]  and SIDE_PLM_POS_Master.POS_CODE=POS_Review.POSCode 
                                                   and SIDE_PLM_POS_Master.TM_SEASON=POS_Review.Season  group by SIDE_PLM_POS_Master.BIG_FORMAT) t";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<object>(sql, new { year, season, ASM_ID }).FirstOrDefault());
            }
        }

        public async Task<object> GetPreviousSeason(string year, string season, string pos_code)
        {
            string sql = @"select 
                                  Grading_Kids,
                                  Grading_MB,
                                  Seg_Marketplace,
                                  Seg_Business,
                                  OCS_Grading,
                                  POS_Type,
                                  Grading_OCS,
                                  POS_Cluster,
                                  Category_Cluster_Running,
                                  Category_Cluster_Basketball,
                                  Category_Cluster_Football,
                                  Category_Cluster_HW,
                                  Category_Cluster_ACC,
                                  Category_Cluster_Outdoor,
                                  Category_Cluster_TrainingW,
                                  Category_Cluster_TrainingM,
                                  Category_Cluster_Swim,
                                  Category_Cluster_Tennis,
                                  Category_Cluster_OutdoorWJ,
                                  Division_Gender_Cluster_AppMen,
                                  Division_Gender_Cluster_AppWomen,
                                  Division_Gender_Cluster_FTWMen,
                                  Division_Gender_Cluster_FTWWomen,
                                  Division_Gender_Cluster_ACCHW  from [dbo].[SIDE_PLM_POS_Master] where TM_YEAR=@year and TM_SEASON=@season AND POS_CODE=@pos_code";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<object>(sql, new { year, season, pos_code }).FirstOrDefault());
            }
        }

    }
}
public class ASApprovalModel
{
    public string POS_CHANNEL { get; set; }
    public string CUSTOMER_GROUP { get; set; }
    public string BRANCH_EN { get; set; }
    public string Account_Sales { get; set; }
    public string Account_SalesID { get; set; }
    public string Total { get; set; }
    public string BLR_Y { get; set; }
    public string BLR_N { get; set; }
    public string PosState { get; set; }
}
