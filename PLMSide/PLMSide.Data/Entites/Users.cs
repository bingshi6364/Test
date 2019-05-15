using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;

namespace PLMSide.Data.Entities
{
    public class Users 
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string ntAccount { get; set; }

        public List<Roles> Roles = new List<Roles>();
    }
}
