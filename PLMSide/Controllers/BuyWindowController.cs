using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyWindowController : ControllerBase
    {
        private readonly ISIDE_PLM_BLR_BuyWindowRepository buyRepository;

        public BuyWindowController(ISIDE_PLM_BLR_BuyWindowRepository _buyRepository)
        {
            buyRepository = _buyRepository;
        }
        // GET: api/BuyWindow
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 获取所有xiang
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("GetModel/{id}")]
        public async Task<object> GetModel(int id)
        {

            SIDE_PLM_BLR_BuyWindow model = await buyRepository.GetModel(id);
            return Ok(model);

        }

        /// <summary>
        /// 获取所有xiang
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("GetModelByYear_Season/{Year}/{Season}")]
        public async Task<object> GetModel(string Year, string Season)
        {
            SIDE_PLM_BLR_BuyWindow model = await buyRepository.GetModel(Year, Season);
            return Ok(model);

        }


        /// <summary>
        /// 查询未添加的季度
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        [HttpGet("GetSeasonListByYear/{Year}")]
        public async Task<object> GetSeasonListByYear(string Year)
        {

            List<string> list = await buyRepository.GetSeasonListByYear(Year);
            return Ok(list);
        }
        /// <summary>
        /// 查询不同状态的buywindowslist
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        [HttpGet("GetBuyWindowListByState/{State}")]
        public async Task<object> GetSIDE_PLM_BLR_BuyWindowListByState(int State)
        {

            List<SIDE_PLM_BLR_BuyWindow> list = await buyRepository.GetSIDE_PLM_BLR_BuyWindowListByState(State);

            return Ok(list);
        }
        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        ///        
        [HttpGet("GetPageListByState/{State}/{PageIndex}/{PageSize}")]
        public async Task<object> GetPageListByState(int State, int PageIndex, int PageSize = 10)
        {

            Tuple<List<SIDE_PLM_BLR_BuyWindow>, int> tp = await buyRepository.GetPageByProcList("SIDE_PLM_BLR_BuyWindow", "*", " State =" + State, "ID", PageIndex, PageSize);
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { pageTotal = tp.Item2 }));


            return Ok(tp.Item1);
        }
        /// <summary>
        /// 添加买货信息接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/BuyWindow
        [HttpPost]
        public string Post(SIDE_PLM_BLR_BuyWindow model)
        {
            return JsonConvert.SerializeObject(buyRepository.AddBuyWindowSP(model));
        }


        /// <summary>
        /// 添加买货信息接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/BuyWindow
        [HttpPost("NewBuyingWindow")]
        public string NewBuyingWindow(SIDE_PLM_BLR_BuyWindow model)
        {
            return JsonConvert.SerializeObject(buyRepository.NewBuyWindowSP(model));
        }
        [HttpPut("NewBuyingWindowUpdate")]
        public async Task Put(SIDE_PLM_BLR_BuyWindow model)
        {
            SIDE_PLM_BLR_BuyWindow entity = null;
            if (model.ID > 0)
                entity = await buyRepository.GetModel(model.ID);
            else if (string.IsNullOrEmpty(model.Year) && string.IsNullOrEmpty(model.Season))
                entity = await buyRepository.GetModel(model.Year, model.Season);
            if (entity != null)
            {
                entity.POSConfirmationStartDate = model.POSConfirmationStartDate;
                entity.POSConfirmationEndDate = model.POSConfirmationEndDate;
                entity.TargetPOSConfirmationStartDate = model.TargetPOSConfirmationStartDate;
                entity.TargetPOSConfirmationEndDate = model.TargetPOSConfirmationEndDate;
                entity.Segment_POSTypeStartDate = model.Segment_POSTypeStartDate;
                entity.Segment_POSTypeEndDate = model.Segment_POSTypeEndDate;
                entity.STC_MandatoryStartDate = model.STC_MandatoryStartDate;
                entity.STC_MandatoryEndDate = model.STC_MandatoryEndDate;
                entity.TradingMeetingStartDate = model.TradingMeetingStartDate;
                entity.TradingMeetingEndDate = model.TradingMeetingEndDate;
                entity.V1StartDate = model.V1StartDate;
                entity.V1EndDate = model.V1EndDate;
                entity.V2StartDate = model.V2StartDate;
                entity.V2EndDate = model.V2EndDate;
                await buyRepository.UpdateEntity(entity);
            }
        }
        // PUT: api/BuyWindow/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {



        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("GetNewestBuyWindow")]
        public async Task<object> GetNewestBuyWindow()
        {
            SIDE_PLM_BLR_BuyWindow entity = await buyRepository.GetNewestBuyWindow();
            return Ok(entity);
        }



    }
}