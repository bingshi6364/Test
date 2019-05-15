using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Data;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterdataController : ControllerBase
    {
        private readonly IMasterDataRepository masterdataRepository;
        private readonly IPOS_MasterRepository PosRepository;
        public MasterdataController(IMasterDataRepository _masterdataRepository, IPOS_MasterRepository _posRepository)
        {
            masterdataRepository = _masterdataRepository;
            PosRepository= _posRepository;
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_BulkShipToSingleModel/{Id}")]
        public async Task<object> GetMD_BulkShipToSingleModel(int Id)
        {
            return Ok(await masterdataRepository.GetMD_BulkShipToSingleModel(Id));
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_BulkShipToByWhere")]
        public async Task<object> GetMD_BulkShipToByWhere([FromQuery]string BranchCN, string BranchEN, string Province, string City, string StoreFormat, string WHSChannel)
        {
            string selectSql = @"SELECT top 1 * FROM SIDE_MD_BulkShipTo where BRANCH_CN = @BranchCN and BRANCH_EN=@BranchEN and WHS_Channel=@WHSChannel ";

            if (!string.IsNullOrEmpty(StoreFormat) && StoreFormat != "0")
                selectSql = selectSql + " and ID in (select ID from  SIDE_MD_BulkShipTo where(charindex(',' + @StoreFormat + ',',',' + StoreFormat + ',')> 0 or  isnull(StoreFormat,'')='' )   )";
            else
            {
                selectSql = selectSql + " and   isnull(StoreFormat,'')='' ";
            }

            if (!string.IsNullOrEmpty(Province) && Province != "0")
                selectSql = selectSql + " and ID in (select ID from  SIDE_MD_BulkShipTo where(charindex(',' + @Province + ',',',' + Province + ',')> 0  or isnull(Province,'')='' )   )";
            else
            {
                selectSql = selectSql + " and   isnull(Province,'')='' ";
            }
            if (!string.IsNullOrEmpty(City) && City != "0")
                selectSql = selectSql + " and ID in (select ID from  SIDE_MD_BulkShipTo where(charindex(',' + @City + ',',',' + City + ',')> 0 or isnull(City,'')='' )  )";
               else
            {
                selectSql = selectSql + " and isnull(City,'')='' ";
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BranchEN", BranchEN, DbType.String, ParameterDirection.Input);
            parameters.Add("@BranchCN", BranchCN, DbType.String, ParameterDirection.Input);
            parameters.Add("@Province", Province, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", City, DbType.String, ParameterDirection.Input);
             
            parameters.Add("@StoreFormat", StoreFormat, DbType.String, ParameterDirection.Input);
            parameters.Add("@WHSChannel", WHSChannel, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return Ok(await Task.Run(() => conn.Query<MD_BulkShipTo>(selectSql, parameters).FirstOrDefault()));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet("GetMD_BulkShipToPageList")]
        public async Task<object> GetMD_BulkShipToPageList([FromQuery]string Branch, int PageIndex = 1, int PageSize = 10)
        {
            StringBuilder sbstring = new StringBuilder(" 1=1");
            if (!string.IsNullOrEmpty(Branch))
            {
                sbstring.Append(" AND  Branch_EN ='" + Branch + "'");
            }
            Tuple<List<MD_BulkShipTo>, int> tp = await masterdataRepository.GetPageByProcList_MD_BulkShipTo("SIDE_MD_BulkShipTo", "*", sbstring.ToString(), "ID", PageIndex, PageSize);
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { pageTotal = tp.Item2 }));
            return Ok(tp.Item1);
        }

        /// <summary>
        /// 获取SIDE_MD_Big_Format
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetBig_Formats")]
        public async Task<object> GetBig_Formats()
        {
            List<MD_Big_Format> list = await masterdataRepository.GetBig_Formats();

            return Ok(list);
        }
        /// <summary>
        /// 获取SIDE_MD_Big_Format
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetBig_FormatsByPOSChannel/{POSChannel}")]
        public async Task<object> GetBig_FormatsPosChannel(string POSChannel)
        {
            List<MD_Big_Format> list = await masterdataRepository.GetBig_FormatsPOSChannel(POSChannel);
            // return JsonResult
            return Ok(list);
        }
        /// <summary>
        /// 获取GetMD_Buying_Region
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_Buying_Region")]
        public async Task<object> GetMD_Buying_Region()
        {
            List<MD_Buying_Region> list = await masterdataRepository.GetMD_Buying_Region();
            return Ok(list);
        }

        /// <summary>
        /// 获取Grading
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_Grading")]
        public async Task<object> GetMD_Grading(string channel)
        {
            List<MD_Grading> list = await masterdataRepository.GetMD_Grading(channel);
            return Ok(list);
        }

        /// <summary>
        /// 获取GetMD_POS_Channel
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_POS_Channel")]
        public async Task<object> GetMD_POS_Channel()
        {
            List<MD_POS_Channel> list = await masterdataRepository.GetMD_POS_Channel();

            return Ok(list);
        }

        [HttpGet]
        [Route("GetMDProduct_Channel")]
        public async Task<object> GetMD_Product_Channel()
        {
            List<MD_Product_Channel> list = await masterdataRepository.GetMD_Product_Channel();
            return Ok(list);
        }

        /// <summary>
        /// 获取GetMD_POS_Type
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_POS_Type")]
        public async Task<object> GetMD_POS_Type()
        {
            List<MD_POS_Type> list = await masterdataRepository.GetMD_POS_Type();

            return Ok(list);
        }

        /// <summary>
        /// 获取GetMD_Segmentation
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_Segmentation")]
        public async Task<object> GetMD_Segmentation()
        {
            List<MD_Segmentation> list = await masterdataRepository.GetMD_Segmentation();

            return Ok(list);
        }

        /// <summary>
        /// 获取GetMD_Segmentation
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetMD_SubChannel")]
        public async Task<object> GetMD_SubChannel()
        {
            List<MD_SubChannel> list = await masterdataRepository.GetMD_SubChannel();

            return Ok(list);
        }
        /// <summary>
        /// 通过pos channle 获取sub channel
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("GetMD_SubChannelByChannel")]
        public async Task<object> GetMD_SubChannelByChannel(string channel)
        {
            List<MD_SubChannel> list = await masterdataRepository.GetMD_SubChannelByChannel(channel);

            return Ok(list);
        }
        /// <summary>
        /// 获取所有WHS Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMD_WHS_Channel")]
        public async Task<object> GetAllGroup()
        {
            List<MD_WHS_Channel> Groups = await masterdataRepository.GetMD_WHS_Channel();
            return Ok(Groups);
        }

        /// <summary>
        /// 获取所有WHS Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBLR_OCSGrading")]
        public async Task<object> GetBLR_OCSGradingList()
        {
            List<BLR_OCSGrading> Groups = await masterdataRepository.GetBLR_OCSGrading();
            return Ok(Groups);
        }

        #region Add/ Update

        /// <summary>
        ///  BulkShipTo添加和修改同一个接口
        /// </summary>
        /// <returns></returns>
        [HttpPost("EditMD_BulkShipTo")]
        public OkObjectResult EditMD_BulkShipTo(MD_BulkShipTo entity)
        {
            ResultModel result = new ResultModel();
            try
            {
                result.IsSuccess = false;
                int Total = 0;
                //获取customerInfo
                #region 验证 ship to Code 的输入是否正确
                if (!string.IsNullOrEmpty(entity.Bulk_Ship_to_Code_Core))
                {
                 
                    entity.Bulk_Ship_to_Code_Core = entity.Bulk_Ship_to_Code_Core.ToUpper();
                    if (entity.Bulk_Ship_to_Code_Core != "TBD")
                    {
                        string[] codelist = entity.Bulk_Ship_to_Code_Core.Split('/');
                        foreach (string Code in codelist)
                        {
                            string Productchannel = PosRepository.GetProductchannel(Code);
                            if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToString().ToLower() != "core")
                            {
                                result.Message = "Bulk Ship to Code(Core) is Error!";
                                return Ok(result);
                            }
                        }
                    }              

                }
                  if ( !string.IsNullOrEmpty(entity.Bulk_Ship_to_Code_Originals))
                {
                    entity.Bulk_Ship_to_Code_Originals = entity.Bulk_Ship_to_Code_Originals.ToUpper();
                    if (entity.Bulk_Ship_to_Code_Originals != "TBD")
                    {
                        string[] codelist = entity.Bulk_Ship_to_Code_Originals.Split('/');
                        foreach (string Code in codelist)
                        {
                            string Productchannel = PosRepository.GetProductchannel(Code);
                            if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToLower() != "originals")
                            {
                                result.Message = "Bulk Ship to Code(Originals) is Error!";
                                return Ok(result);
                            }
                        }
                    }
               

                }
                  if (!string.IsNullOrEmpty(entity.Bulk_Ship_to_Code_Neo))
                {
                    entity.Bulk_Ship_to_Code_Neo = entity.Bulk_Ship_to_Code_Neo.ToUpper();
                    if (entity.Bulk_Ship_to_Code_Neo != "TBD")
                    {
                        string[] codelist = entity.Bulk_Ship_to_Code_Neo.Split('/');
                        foreach (string Code in codelist)
                        {
                            string Productchannel = PosRepository.GetProductchannel(Code);
                            if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToLower() != "neo")
                            {
                                result.Message = "Bulk Ship to Code(Neo) is Error!";
                                return Ok(result);
                            }
                        }
                    }
               

                }
                  if (!string.IsNullOrEmpty(entity.Bulk_Ship_to_Code_Kids))
                {
                    entity.Bulk_Ship_to_Code_Kids = entity.Bulk_Ship_to_Code_Kids.ToUpper();
                    if (entity.Bulk_Ship_to_Code_Kids != "TBD")
                    {
                        string[] codelist = entity.Bulk_Ship_to_Code_Kids.Split('/');
                        foreach (string Code in codelist)
                        {
                            string Productchannel = PosRepository.GetProductchannel(Code);
                            if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToLower() != "kids")
                            {
                                result.Message = "Bulk Ship to Code(Kids) is Error!";
                                return Ok(result);
                            }
                        }
                    }
              

                }
                #endregion
                //string sqlstr = "select count(ID) from SIDE_MD_BulkShipTo where BRANCH_CN=@BRANCH_CN and BRANCH_EN=@BRANCH_EN and WHS_Channel=@WHS_Channel ";
                //if (entity.ID > 0)
                //{
                //    sqlstr = sqlstr + " and ID<>@ID ";
                //}
                //DynamicParameters parameters = new DynamicParameters();
                //parameters.Add("@ID", entity.ID, DbType.Int32, ParameterDirection.Input);
                //parameters.Add("@BRANCH_CN", entity.BRANCH_CN, DbType.String, ParameterDirection.Input);
                //parameters.Add("@BRANCH_EN", entity.BRANCH_EN, DbType.String, ParameterDirection.Input);
                //parameters.Add("@WHS_Channel", entity.WHS_Channel, DbType.String, ParameterDirection.Input);

                //using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
                //{
                //    Total = (Int32)conn.ExecuteScalar(sqlstr, parameters);
                //}
                if (Total > 0)
                {
                    result.IsSuccess = false;
                    result.Message = "Already exist";
                    result.StatusCode = "-4";
                }
                else
                {
                    if (entity.ID == 0)
                    {
                        int row = masterdataRepository.AddMD_BulkShipTo(entity);
                        result.StatusCode = row.ToString();
                        result.Message = "OK";
                        result.IsSuccess = true;
                    }
                    else
                    {
                        int row = masterdataRepository.UpdateMD_BulkShipTo(entity);
                        result.StatusCode = row.ToString();
                        result.Message = "OK";
                        result.IsSuccess = true;
                    }

                }
            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return Ok(result);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddMD_POS_Type")]
        public OkObjectResult AddMD_POS_Type(MD_POS_Type entity)
        {
            ResultModel result = new ResultModel();
            result.IsSuccess = true;
            int Total = 0;//这一步在优先次序上加一
            //string sqlstr = "select count(ID) from SIDE_MD_POS_Type where POS_Channel=@POS_Channel and POS_Segment=@POS_Segment and Type_Sequence=@Type_Sequence ";
            //DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("@POS_Channel", entity.POS_Channel, DbType.String, ParameterDirection.Input);
            //parameters.Add("@POS_Segment", entity.POS_Segment, DbType.String, ParameterDirection.Input);
            //parameters.Add("@Type_Sequence", entity.Type_Sequence, DbType.Int32, ParameterDirection.Input);
            //entity.Type_Status = 1;
            //using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            //{
            //    Total = (Int32)conn.ExecuteScalar(sqlstr, parameters);
            //}
            if (Total > 0)
            {
                result.IsSuccess = false;
                result.Message = "Already exist";
                result.StatusCode = "-4";
            }
            else
            {
                int row = masterdataRepository.AddMD_POS_Type(entity);
                result.StatusCode = row.ToString();
                result.Message = "OK";
            }
            return Ok(result);

        }


        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpPost("MD_POS_TypeEdit")]
        public OkObjectResult MD_POS_TypeEdit(MD_POS_Type entity)
        {
            ResultModel result = new ResultModel();
            result.IsSuccess = true;
            int Total = 0;//这一步在优先次序上加一
            string sqlstr = "select count(ID) from SIDE_MD_POS_Type where POS_Channel=@POS_Channel and POS_Segment=@POS_Segment and POS_Type=@POS_Type ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@POS_Channel", entity.POS_Channel, DbType.String, ParameterDirection.Input);
            parameters.Add("@POS_Segment", entity.POS_Segment, DbType.String, ParameterDirection.Input);
            parameters.Add("@POS_Type", entity.POS_Type, DbType.Int32, ParameterDirection.Input);
          
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                Total = (Int32)conn.ExecuteScalar(sqlstr, parameters);
            }
            if (Total > 0)
            {
                result.IsSuccess = false;
                result.Message = "Already exist";
                result.StatusCode = "-4";
            }
            else
            {
                if (entity.ID > 0)
                {
                    int row = masterdataRepository.UpdateMD_POS_Type(entity);
                    result.StatusCode = row.ToString();
                    result.Message = "OK";
                }
                else
                {
                    entity.Type_Status = 1;
                    int row = masterdataRepository.AddMD_POS_Type(entity);
                    result.StatusCode = row.ToString();
                    result.Message = "OK";
                }
            }
            return Ok(result);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpGet("UpdateTypeStatus/{ID}/{status}")]
        public OkObjectResult UpdateMD_POS_TypeStatus(int ID, int status)
        {
            return Ok(masterdataRepository.UpdateMD_POS_TypeStatus(status, ID));
        }

        #endregion
        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        ///        
        [HttpGet("GetPageList/{PageIndex}/{PageSize}")]
        public async Task<object> GetPageList(int PageIndex, int PageSize = 10)
        {
            Tuple<List<MD_POS_Type>, int> tp = await masterdataRepository.GetPageByProcList_MD_POS_Type("SIDE_MD_POS_Type", "*", " 1=1 ", "ID", PageIndex, PageSize);
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { pageTotal = tp.Item2 }));
            return Ok(tp.Item1);
        }
        [HttpGet("GetOSCGrading")]
        public async Task<object> GetOSCGrading()
        {
            List<object> list = await masterdataRepository.GetOSCGrading();
            return Ok(list);
        }
        [HttpGet("GetPOSType/{segmentation}")]
        public async Task<object> GetPOSType(string segmentation)
        {
            List<object> list = await masterdataRepository.GetPOSType(segmentation);
            return Ok(list);
        }

        [HttpGet("GetPOSTypeByID/{Id}")]
        public async Task<object> GetPOSTypeByID(int Id)
        {
            MD_POS_Type model = await masterdataRepository.GetMD_POS_TypeModel(Id);
            return Ok(model);
        }
    }
}
