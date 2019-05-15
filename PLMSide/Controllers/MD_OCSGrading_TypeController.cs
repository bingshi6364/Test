using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLMSide.Data.IRepository;
using PLMSide.Data.Entites;
using Dapper;
using System.Data;
using PLMSide.Data;
using Newtonsoft.Json;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MD_OCSGrading_TypeController : ControllerBase
    {
        private readonly IMD_OCSGrading_TypeRepository Repository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_repository"></param>
        public MD_OCSGrading_TypeController(IMD_OCSGrading_TypeRepository _repository)
        {
            Repository = _repository;
        }

        

        /// <summary>
        /// 修改 信息
        /// </summary>
        /// <param name = "entity" ></ param >
        /// < returns ></ returns >
        [HttpPut("")]
        public async Task Putentity(MD_OCSGrading_Type entity, int id)
        {
            try
            {
                entity.ID = id;
                await Repository.UpdateEntity(entity);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<object> GetEntities()
        {
           List<MD_OCSGrading_Type>  list=  await Repository.GetModelList();
            return Ok(list);
        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpGet("UpdateTypeStatus/{ID}/{status}")] 
        public OkObjectResult UpdateTypeStatus(int ID, int status)
        {
            return Ok(Repository.UpdateTypeStatus(status, ID));
        }
        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        ///        
        [HttpGet("GetPageList/{PageIndex}/{PageSize}")]
        public async Task<object> GetPageList(int PageIndex,int PageSize=10)
        {

            Tuple<List<MD_OCSGrading_Type>, int> tp = await Repository.GetPageByProcList("SIDE_MD_OCSGrading_Type", "*", " 1=1 ", "ID", PageIndex, PageSize);
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { pageTotal = tp.Item2 }));
            return Ok(tp.Item1);
        }
    }
}