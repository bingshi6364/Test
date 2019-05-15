using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Entites
{
    public class Roles
    {
        /// <summary>
        /// ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsVaild { get; set; }

        /// <summary>
        /// 角色简称
        /// </summary>
        public string RoleNameShort { get; set; }
    }
}
