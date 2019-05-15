using Dapper;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace PLMSide.Data.Repository
{
    public class MasterDataRepository : IMasterDataRepository
    {

        /// <summary>
        ///  GetMD_POS_TypeModel
        /// </summary>
        /// <returns></returns>
        public async Task<MD_POS_Type> GetMD_POS_TypeModel(int Id)
        {
            string selectSql = @"SELECT top 1 * FROM SIDE_MD_POS_Type where Id=@Id";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {


                return await Task.Run(() => conn.Query<MD_POS_Type>(selectSql, parameters).FirstOrDefault());
            }
        }

        /// <summary>
        ///  MD_BulkShipTo
        /// </summary>
        /// <returns></returns>
        public async Task<MD_BulkShipTo> GetMD_BulkShipToByBranch(string Branch)
        {
            string selectSql = @"SELECT top 1 * FROM SIDE_MD_BulkShipTo where BRANCH_EN=@Branch";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Branch", Branch, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {


                return await Task.Run(() => conn.Query<MD_BulkShipTo>(selectSql,parameters).FirstOrDefault());
            }
        }

        /// <summary>
        ///  MD_BulkShipTo
        /// </summary>
        /// <returns></returns>
        public async Task<MD_BulkShipTo> GetMD_BulkShipToSingleModel(int Id)
        {
            string selectSql = @"SELECT top 1 * FROM SIDE_MD_BulkShipTo where ID=@ID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", Id, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_BulkShipTo>(selectSql, parameters).FirstOrDefault());
            }
        }
        /// <summary>
        /// big_format
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_Big_Format>> GetBig_Formats()
        {
            string selectSql = @"SELECT Id, Channel, Big_Format FROM SIDE_MD_Big_Format";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_Big_Format>(selectSql).ToList());
            }
        }
        /// <summary>
        /// big_format
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_Big_Format>> GetBig_FormatsPOSChannel(string Channel)
        {
            string selectSql = @"SELECT Id, Channel, Big_Format FROM SIDE_MD_Big_Format where Channel=@Channel";
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Channel", Channel, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_Big_Format>(selectSql, parameters).ToList());
            }
        }



        /// <summary>
        /// Buying_Region
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_Buying_Region>> GetMD_Buying_Region()
        {
            string selectSql = @"SELECT Id, Channel FROM SIDE_MD_Buying_Region";



            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_Buying_Region>(selectSql).ToList());
            }
        }



        /// <summary>
        /// MD_Grading
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_Grading>> GetMD_Grading(string channel)
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(channel))
            {
                filter = string.Format("where channel ='{0}'", channel);
            }
            string selectSql = string.Format(@"SELECT Id, Channel, Grading FROM SIDE_MD_Grading {0}", filter);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<MD_Grading>(selectSql).ToList());
            }
        }

        /// <summary>
        /// MD_POS_Channel
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_POS_Channel>> GetMD_POS_Channel()
        {
            string selectSql = @"SELECT Id, CodeName FROM SIDE_MD_POS_Channel";



            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_POS_Channel>(selectSql).ToList());
            }
        }





        /// <summary>
        /// MD_POS_Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_POS_Type>> GetMD_POS_Type()
        {
            string selectSql = @"SELECT * FROM SIDE_MD_POS_Type";



            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_POS_Type>(selectSql).ToList());
            }
        }



        /// <summary>
        /// MD_POS_Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_Segmentation>> GetMD_Segmentation()
        {
            string selectSql = @"SELECT Id, Segmentation, Sequences FROM SIDE_MD_Segmentation";



            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_Segmentation>(selectSql).ToList());
            }
        }




        /// <summary>
        /// MD_POS_Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_SubChannel>> GetMD_SubChannel()
        {
            string selectSql = @"SELECT Id, Channel, Sub_Channel FROM SIDE_MD_SubChannel";



            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<MD_SubChannel>(selectSql).ToList());
            }
        }
        public async Task<List<MD_SubChannel>> GetMD_SubChannelByChannel(string Channel)
        {
            string selectSql = @"SELECT Id, Channel, Sub_Channel FROM SIDE_MD_SubChannel where Channel=@Channel";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<MD_SubChannel>(selectSql, new { Channel }).ToList());
            }
        }




        public async Task<List<MD_WHS_Channel>> GetMD_WHS_Channel()
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<MD_WHS_Channel>("select distinct [WHS_Channel]   from SIDE_MD_WHS_Channel").ToList());
            }
        }

        public async Task<List<MD_Product_Channel>> GetMD_Product_Channel()
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<MD_Product_Channel>("select ID,CodeName from SIDE_MD_Product_Channel").ToList());
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public async Task<List<BLR_OCSGrading>> GetBLR_OCSGrading()
        {
            string selectSql = @"SELECT ID, OCSGrading FROM SIDE_PLM_BLR_OCSGrading";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<BLR_OCSGrading>(selectSql).ToList());
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public int AddMD_POS_Type(MD_POS_Type model)
        {

            StringBuilder strSql = new StringBuilder();
            StringBuilder strSqlupdate = new StringBuilder();   // 修改有限顺序序号 
            strSqlupdate.Append("  if exists(select * from SIDE_MD_POS_Type where POS_Channel = @POS_Channel and POS_Segment = @POS_Segment and Type_Sequence = @Type_Sequence)");
            strSqlupdate.Append(" begin");
            strSqlupdate.Append(" update SIDE_MD_POS_Type set Type_Sequence = Type_Sequence + 1 where POS_Channel = @POS_Channel and POS_Segment = @POS_Segment and Type_Sequence>= @Type_Sequence");
            strSqlupdate.Append(" end ");

            strSql.Append("insert into SIDE_MD_POS_Type( POS_Channel, POS_Segment, POS_Type, Type_Sequence, Type_Status )");
            strSql.Append(" values ( @POS_Channel, @POS_Segment, @POS_Type, @Type_Sequence, @Type_Status )");
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@POS_Channel", model.POS_Channel, DbType.String, ParameterDirection.Input);
            parameters.Add("@POS_Segment", model.POS_Segment, DbType.String, ParameterDirection.Input);
            parameters.Add("@POS_Type", model.POS_Type, DbType.String, ParameterDirection.Input);
            parameters.Add("@Type_Sequence", model.Type_Sequence, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Type_Status", 1, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                conn.Execute(strSqlupdate.ToString(), parameters);
                return conn.Execute(strSql.ToString(), parameters);
            }
        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public int UpdateMD_POS_Type(MD_POS_Type model)
        {
            StringBuilder strSqlupdate = new StringBuilder();   // 修改有限顺序序号 
            strSqlupdate.Append("  if exists(select * from SIDE_MD_POS_Type where POS_Channel = @POS_Channel and POS_Segment = @POS_Segment and Type_Sequence = @Type_Sequence and ID<>@ID)");
            strSqlupdate.Append(" begin");
            strSqlupdate.Append(" update SIDE_MD_POS_Type set Type_Sequence = Type_Sequence + 1 where POS_Channel = @POS_Channel and POS_Segment = @POS_Segment and Type_Sequence>= @Type_Sequence");
            strSqlupdate.Append(" end ");
            string updateSql = @"update SIDE_MD_POS_Type set    POS_Channel=@POS_Channel, POS_Segment=@POS_Segment, POS_Type=@POS_Type, Type_Sequence=@Type_Sequence, Type_Status=@Type_Status  FROM [dbo].SIDE_MD_POS_Type WHERE ID=@ID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", model.ID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@POS_Channel", model.POS_Channel, DbType.String, ParameterDirection.Input);
            parameters.Add("@POS_Segment", model.POS_Segment, DbType.String, ParameterDirection.Input);
            parameters.Add("@POS_Type", model.POS_Type, DbType.String, ParameterDirection.Input);
            parameters.Add("@Type_Sequence", model.Type_Sequence, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Type_Status", model.Type_Status, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                conn.Execute(strSqlupdate.ToString(), parameters);
                return conn.Execute(updateSql, parameters);
            }

        }


        public int AddMD_BulkShipTo(MD_BulkShipTo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SIDE_MD_BulkShipTo(BRANCH_EN,BRANCH_CN,BLR,Province,City,StoreFormat,WHS_Channel,Bulk_Ship_to_Code_Core,Bulk_Ship_to_Code_Originals,Bulk_Ship_to_Code_Neo,Bulk_Ship_to_Code_Kids )");
            strSql.Append(" values (@BRANCH_EN,@BRANCH_CN,@BLR,@Province,@City,@StoreFormat,@WHS_Channel,@Bulk_Ship_to_Code_Core,@Bulk_Ship_to_Code_Originals,@Bulk_Ship_to_Code_Neo,@Bulk_Ship_to_Code_Kids )");
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@BRANCH_EN", model.BRANCH_EN, DbType.String, ParameterDirection.Input);
            parameters.Add("@BRANCH_CN", model.BRANCH_CN, DbType.String, ParameterDirection.Input);
            parameters.Add("@WHS_Channel", model.WHS_Channel, DbType.String, ParameterDirection.Input);
            parameters.Add("@BLR", model.BLR, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", model.City, DbType.String, ParameterDirection.Input);
            
            parameters.Add("@Province", model.Province, DbType.String, ParameterDirection.Input);
            parameters.Add("@StoreFormat", model.StoreFormat, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Core", model.Bulk_Ship_to_Code_Core, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Originals", model.Bulk_Ship_to_Code_Originals, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Neo", model.Bulk_Ship_to_Code_Neo, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Kids", model.Bulk_Ship_to_Code_Kids, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return conn.Execute(strSql.ToString(), parameters);
            }
        }
        public int UpdateMD_BulkShipTo(MD_BulkShipTo model)
        {
            string updateSql = @"update SIDE_MD_BulkShipTo set BRANCH_EN=@BRANCH_EN,City=@City, BRANCH_CN=@BRANCH_CN,BLR=@BLR,Province=@Province,StoreFormat=@StoreFormat, WHS_Channel=@WHS_Channel, Bulk_Ship_to_Code_Core=@Bulk_Ship_to_Code_Core, Bulk_Ship_to_Code_Originals=@Bulk_Ship_to_Code_Originals, Bulk_Ship_to_Code_Neo=@Bulk_Ship_to_Code_Neo, Bulk_Ship_to_Code_Kids=@Bulk_Ship_to_Code_Kids   WHERE ID=@ID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", model.ID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BRANCH_EN", model.BRANCH_EN, DbType.String, ParameterDirection.Input);
            parameters.Add("@BRANCH_CN", model.BRANCH_CN, DbType.String, ParameterDirection.Input);
            parameters.Add("@WHS_Channel", model.WHS_Channel, DbType.String, ParameterDirection.Input);
            parameters.Add("@BLR", model.BLR, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", model.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@Province", model.Province, DbType.String, ParameterDirection.Input);
            parameters.Add("@StoreFormat", model.StoreFormat, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Core", model.Bulk_Ship_to_Code_Core, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Originals", model.Bulk_Ship_to_Code_Originals, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Neo", model.Bulk_Ship_to_Code_Neo, DbType.String, ParameterDirection.Input);
            parameters.Add("@Bulk_Ship_to_Code_Kids", model.Bulk_Ship_to_Code_Kids, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return conn.Execute(updateSql, parameters);
            }
        }

        public async Task<Tuple<List<MD_POS_Type>, int>> GetPageByProcList_MD_POS_Type(string viewName, string fieldName = "*", string whereString = " 1=1", string orderString = "ID", int page = 1, int pageSize = 10)
        {
            int recordTotal = 0;

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //conn.Open();
                DynamicParameters parm = new DynamicParameters();
                parm.Add("viewName", viewName);
                parm.Add("fieldName", fieldName);
                parm.Add("whereString", whereString);
                parm.Add("pageSize", pageSize);
                parm.Add("pageNo", page);
                parm.Add("orderString", orderString);
                parm.Add("recordTotal", 0, DbType.Int32, ParameterDirection.Output);

                var list = await Task.Run(() => conn.Query<MD_POS_Type>("SIDE_PLM_ProcViewPager", parm, commandType: CommandType.StoredProcedure).ToList());
                recordTotal = parm.Get<int>("@recordTotal");//返回总页数

                //  conn.Close();
                return new Tuple<List<MD_POS_Type>, int>(list, recordTotal);

            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="fieldName"></param>
        /// <param name="whereString"></param>
        /// <param name="orderString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Tuple<List<MD_BulkShipTo>, int>> GetPageByProcList_MD_BulkShipTo(string viewName, string fieldName = "*", string whereString = " 1=1", string orderString = "ID", int page = 1, int pageSize = 10)
        {
            int recordTotal = 0;

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //conn.Open();
                DynamicParameters parm = new DynamicParameters();
                parm.Add("viewName", viewName);
                parm.Add("fieldName", fieldName);
                parm.Add("whereString", whereString);
                parm.Add("pageSize", pageSize);
                parm.Add("pageNo", page);
                parm.Add("orderString", orderString);
                parm.Add("recordTotal", 0, DbType.Int32, ParameterDirection.Output);

                var list = await Task.Run(() => conn.Query<MD_BulkShipTo>("SIDE_PLM_ProcViewPager", parm, commandType: CommandType.StoredProcedure).ToList());
                recordTotal = parm.Get<int>("@recordTotal");//返回总页数

                //  conn.Close();
                return new Tuple<List<MD_BulkShipTo>, int>(list, recordTotal);

            }


        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public int UpdateMD_POS_TypeStatus(int Status, int ID)
        {
            string updateSql = @"update SIDE_MD_POS_Type set   Type_Status=" + Status + "  FROM [dbo].SIDE_MD_POS_Type WHERE ID=" + ID;

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return conn.Execute(updateSql);
            }
        }
        public async Task<List<object>> GetOSCGrading()
        {
            string sql = @"SELECT 
	            distinct [POS_Segment]
              FROM  [dbo].[SIDE_MD_POS_Type] where POS_Channel='Originals'";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<object>(sql).ToList());
            }
        }

        public async Task<List<object>> GetPOSType(string segmentation)
        {
            string sql = @"SELECT 
	                DISTINCT [POS_Type]
                  FROM  [dbo].[SIDE_MD_POS_Type] where POS_Channel='Core' and POS_Segment=@segmentation";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<object>(sql,new { segmentation }).ToList());
            }
        }



    }


}

