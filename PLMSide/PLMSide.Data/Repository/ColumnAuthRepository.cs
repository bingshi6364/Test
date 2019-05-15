using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace PLMSide.Data.Repository
{
    public class ColumnAuthRepository : IColumnAuthRepository
    {
        public async Task<List<ColumnAuth>> GetColumnAuths(string roleid,string type)
        {
            string selectsql = @"select a.EXCEL_COLUMN,a.database_column,
case  when b.id is null then 'N' else 'Y' end as CanEdit from SIDE_PLM_ExcelExport a left join
" + $"[SIDE_PLM_RoleColumn_Mapping] b on a.id = b.columnid and b.roleid in ({roleid}) WHERE a.TYPE='{type}' ORDER BY a.ColumnIndex";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = await conn.QueryAsync<ColumnAuth>(selectsql);
                return result.ToList();
            }
        }

        public async Task<List<ColumnAuth>> GetColumnAuthsByName(string roleName, string type)
        {
            string selectsql = @"select a.EXCEL_COLUMN,a.database_column,
case  when b.id is null then 'N' else 'Y' end as CanEdit from SIDE_PLM_ExcelExport a left join
" + $"[SIDE_PLM_RoleColumn_Mapping] b on a.id = b.columnid and b.RoleName = '{roleName}' WHERE a.TYPE='{type}' ORDER BY a.ColumnIndex";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = await conn.QueryAsync<ColumnAuth>(selectsql);
                return result.ToList();
            }
        }

        public async Task<List<POS_Master>> GetPosMasterData(string selectColumn, string whereSql)
        {
            string selectsql = $"select top 10000 {selectColumn} from SIDE_PLM_POS_Master where {whereSql}";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = await conn.QueryAsync<POS_Master>(selectsql);
                return result.ToList();
            }
        }
    }
}
