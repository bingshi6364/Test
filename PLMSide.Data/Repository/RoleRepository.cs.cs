using Dapper;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.Repository
{
    public class RoleRepository : RepositoryBase<Roles>, IRoleRepository
    {
        public async Task DeleteRole(int Id)
        {
            string deleteSql = "DELETE FROM [dbo].[PLMSide_Role] WHERE Id=@Id";
            await Delete(Id, deleteSql);
        }


        public async Task<Roles> GetRoleDetail(int Id)
        {
            string detailSql = @"SELECT Id, RoleName,IsUse FROM [dbo].[SIDE_PLM_Role] WHERE Id=@Id";
            return await Detail(Id, detailSql);
        }

        public async Task<List<Roles>> GetRoles()
        {
            string selectSql = @"SELECT ID, NameCN as RoleName,isValid FROM [dbo].[SIDE_PLM_Role]";
            return await Select(selectSql);
        }

        public async Task PostRole(Roles entity)
        {
            string insertSql = @"INSERT INTO [dbo].[SIDE_PLM_Role]( RoleName, IsUse) VALUES( @RoleName, @IsUse)";
            await Insert(entity, insertSql);
        }

        public async Task PutRole(Roles entity)
        {
            string updateSql = "UPDATE [dbo].[PLMSide_Role] SET RoleName=@RoleName, IsUse=@IsUse WHERE Id=@Id";
            await Update(entity, updateSql);
        }
    }
}
