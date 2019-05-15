using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;

        public RoleController(IRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<object> GetRoles()
        {
            List<Roles> list = await roleRepository.GetRoles();
            // return JsonResult
            return Ok(list);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("{id}")]
        public async Task<object> GetRoles(int id)
        {
            // Roles singleRole = await roleRepository.GetRoleDetail(id);
           Tuple<List<Roles>, int> tp = await roleRepository.GetPageByProcList("PLMSide_Role", "*", "ID", "ID", 1, 5);
            var meta = new
            {
                pageTotal= tp.Item2
             };
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));
            return Ok(tp.Item1);
            // return JsonResult
          //  return Ok(singleRole);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("search/{name}")]
        public async Task<object> GetRoles(string name)
        {
            List<Roles> list = await roleRepository.GetRoles();
            // return JsonResult
            return Ok(list.Where(role => role.RoleName == name).ToList().FirstOrDefault());
        }

        ///// <summary>
        ///// 新增用户
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task PostRole(Roles entity)
        //{

        //    await roleRepository.PostRole(entity);
        //}

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPut("{id}")]
        //public async Task PutRole(Roles entity, int id)
        //{
        //    try
        //    {
        //        entity.ID = id;
        //        await roleRepository.PutRole(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 删除用户
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //public async Task DeleteUser(int Id)
        //{
        //    try
        //    {
        //        await roleRepository.DeleteRole(Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }
        //}



    }
}