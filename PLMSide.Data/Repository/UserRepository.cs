using Dapper;
using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Data.Repository
{
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        public async Task DeleteUser(int Id)
        {
            string deleteSql = "DELETE FROM [dbo].[PLMSide_User] WHERE Id=@Id";
            await Delete(Id, deleteSql);
        }

        public string ExecExecQueryParamSP(string spName, string name, int Id)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserName", name, DbType.String, ParameterDirection.Output, 100);
                parameters.Add("@Id", Id, DbType.String, ParameterDirection.Input);
                conn.Execute(spName, parameters, null, null, CommandType.StoredProcedure);
                string strUserName = parameters.Get<string>("@UserName");
                return strUserName;
            }
        }

        public async Task<Users> GetUserAndRoles(int Id)
        {
            Users userList = new Users();
            string selectSql = @"SELECT id,ntAccount,name,RoleName,RoleNameShort
                                   FROM SIDE_PLM_V_Users
                                   WHERE ID=@Id
                                  ";
            using (IDbConnection connection = DataBaseConfig.GetSqlConnection())
            {
                var lookUp = new Dictionary<int, Users>();
                userList = connection.Query<Users, Roles, Users>(selectSql,
                   (user, role) =>
                   {
                       if (!lookUp.TryGetValue(user.ID, out Users u))
                       {
                           lookUp.Add(user.ID, u = user);
                           u = user;
                       }
                       u.Roles.Add(role);

                       return u;
                   }, splitOn: "RoleName", param: new { Id = Id }
               ).First();
                return userList;
            }
        }

        public async Task<Users> GetUserAndRolesByAccount(string account)
        {
            account = "ap\\" + account;
            Users userList = new Users();
            string selectSql = @"SELECT id,ntAccount,name,RoleName,RoleNameShort,RoleID
                                   FROM SIDE_PLM_V_Users
                                   WHERE ntAccount=@ntAccount
                                  ";
            using (IDbConnection connection = DataBaseConfig.GetSqlConnection())
            {
                var lookUp = new Dictionary<int, Users>();
                var result = await connection.QueryAsync<Users, Roles, Users>(selectSql,
                   (user, role) =>
                   {
                       if (!lookUp.TryGetValue(user.ID, out Users u))
                       {
                           lookUp.Add(user.ID, u = user);
                           u = user;
                       }
                       u.Roles.Add(role);

                       return u;
                   }, splitOn: "RoleName", param: new { ntAccount = account }
               );

                return result.First();
            }
        }

        public Users GetUserByEmail(string email)
        {

            Users userList = new Users();
            string selectSql = @"SELECT [id]
                                      ,[region]
                                      ,[name]
                                      ,[ntAccount]
                                      ,[isValid]
                                      ,[isHeadquarter]
                                      ,[email]
                                      ,[is_internal]
                                      ,[password_encryption]
                                   FROM SIDE_PLM_Employee
                                   WHERE email=@email";
            using (IDbConnection connection = DataBaseConfig.GetSqlConnection())
            {
                Users _user =  connection.QueryFirstOrDefault<Users>(selectSql, new { email });
                return _user;
            }
        }

        public async Task<Users> GetUserDetail(int Id)
        {
            string detailSql = @"SELECT Id, UserName, Password, Gender, CreateDate, IsDelete FROM [dbo].[PLMSide_User] WHERE Id=@Id";
            return await Detail(Id, detailSql);
        }

        public async Task<List<Users>> GetUsers()
        {
            string selectSql = @"SELECT Id, UserName, Password, Gender, CreateDate, IsDelete FROM [dbo].[PLMSide_User]";
            return await Select(selectSql);
        }

        public async Task PostUser(Users entity)
        {
            string insertSql = @"INSERT INTO [dbo].[PLMSide_User]( UserName, Password, Gender, CreateDate, IsDelete) VALUES( @UserName, @Password, @Gender, @CreateDate, @IsDelete)";
            await Insert(entity, insertSql);
        }

        public async Task PutUser(Users entity)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {

                    string deleteSql = "DELETE FROM [dbo].[SIDE_PLM_EmployeeRole] WHERE EMPLOYEE=@ID";
                    await conn.ExecuteAsync(deleteSql, new { ID = entity.ID }, transaction);


                    foreach (var role in entity.Roles)
                    {
                        string insertSql = @"INSERT INTO SIDE_PLM_EmployeeRole(EMPLOYEE,ROLE) SELECT 
                       @ID,ID FROM SIDE_PLM_Role WHERE NameCN=@RoleName";

                        await conn.ExecuteAsync(insertSql, new { ID = entity.ID, RoleName = role.RoleName }, transaction);
                    }


                    transaction.Commit();
                }
                catch (Exception exception)
                {

                    transaction.Rollback();

                }
            }

        }
    }
}
