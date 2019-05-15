using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public UserController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        ///// <summary>
        ///// 获取所有用户
        ///// </summary>
        ///// <returns></returns>
        ///// 
        //[HttpGet]
        //public async Task<object> GetUsers()
        //{
        //    List<Users> list = await userRepository.GetUsers();
        //    // return JsonResult
        //    return Ok(list.Select(users => new {
        //        users.UserName,
        //        users.Account
        //    }));
        //}

        [HttpGet]
        public async Task<object> GetUserBySearch([FromQuery] string name, string role, int pageIndex,int PageSize)
        {
            StringBuilder sql = new StringBuilder(" 1=1");
            Tuple<List<Users>, int> tp = new Tuple<List<Users>, int>(new List<Users>(), 0);
            if (!string.IsNullOrEmpty(name))
            {
                sql.Append(" AND name like '%");
                sql.Append(name);
                sql.Append("%'");

            }

            if (!string.IsNullOrEmpty(role))
            {
                sql.Append(" AND RoleName='");
                sql.Append(role);
                sql.Append("'");
                tp = await userRepository.GetPageByProcList("SIDE_PLM_V_Users", "ID,ntAccount,name", sql.ToString(), "ID", pageIndex, PageSize);
            }
            else
            {
                tp = await userRepository.GetPageByProcList("SIDE_PLM_Employee", "ID,ntAccount,name", sql.ToString(), "ID", pageIndex, PageSize);
            }


            var meta = new
            {
                pageTotal = tp.Item2
            };
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));
            return Ok(tp.Item1.Select(user => new
            {
                user.ID,
                user.ntAccount,
                user.name
            }));
        }

        [HttpGet("{id}")]
        public async Task<object> GetUsers(int id)
        {
            Users list = await userRepository.GetUserAndRoles(id);
            return Ok(list);
        }

        [HttpPost]
        public async Task<object> UpdateRole(Users user)
        {
            await userRepository.PutUser(user);
            return Ok(new
            {
                result = "success"
            });
        }

        [HttpGet]
        [Route("CurrentUser")]
        public async Task<object> GetCurrentUser()
        {
            //Users _users = new Users();
            //_users.name = HttpContext.User.Identity.Name;
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            return Ok(_users);
        }
    }
}