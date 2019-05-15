using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;

namespace PLMSide.Data.Repository
{
   public class MD_OCSGrading_TypeRepository : RepositoryBase<MD_OCSGrading_Type>, IMD_OCSGrading_TypeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Delete(int Id)
        {
            string deleteSql = "DELETE FROM SIDE_MD_OCSGrading_Type WHERE Id=@Id";
            await Delete(Id, deleteSql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<MD_OCSGrading_Type> GetModel(int ID)
        {
            string selectSql = @"SELECT ID, POS_Channel, POS_OCSGrading, POS_Type, Type_Sequence, Type_Status  FROM [dbo].SIDE_MD_OCSGrading_Type WHERE ID=@ID";
            return await Select(ID, selectSql); 

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<MD_OCSGrading_Type>> GetModelList()
        {
            string selectSql = @"SELECT ID, POS_Channel, POS_OCSGrading, POS_Type, Type_Sequence, Type_Status  FROM [dbo].SIDE_MD_OCSGrading_Type T";

            return await Select(selectSql);
        }
        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<List<MD_OCSGrading_Type>, int>> GetPageModelList(string whereStr)
        {
            Tuple<List<MD_OCSGrading_Type>, int> tp = await GetPageByProcList("SIDE_MD_OCSGrading_Type", "*", whereStr, "ID", 1, 10);
            return tp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataSetList(string strWhere)
        {
            DataTable dt = new DataTable();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID, POS_Channel, POS_OCSGrading, POS_Type, Type_Sequence, Type_Status  FROM [dbo].SIDE_MD_OCSGrading_Type ");
            strSql.Append(" FROM SIDE_MD_OCSGrading_Type T ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                dt.Load(conn.ExecuteReader(strSql.ToString()));
            }
            return dt;
        }

        /// <summary>
        /// Put Entity
        /// </summary>
        /// <returns></returns>
        public async Task UpdateEntity(MD_OCSGrading_Type entity)
        {
            string updateSql = @"update SIDE_MD_OCSGrading_Type set POS_Channel=@POS_Channel, POS_OCSGrading=@POS_OCSGrading, POS_Type=@POS_Type, Type_Sequence=@Type_Sequence, Type_Status=@Type_Status  FROM [dbo].SIDE_MD_OCSGrading_Type WHERE ID=@ID";
            await Update(entity, updateSql);

        }
        /// <summary>
        /// Post Entity
        /// </summary>
        /// <returns></returns>
        public async Task InsertEntity(MD_OCSGrading_Type entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SIDE_MD_OCSGrading_Type(POS_Channel, POS_OCSGrading, POS_Type, Type_Sequence, Type_Status )");
            strSql.Append(" values (@POS_Channel, @POS_OCSGrading, @POS_Type, @Type_Sequence, @Type_Status )");
            await Insert(entity, strSql.ToString());
        }

       

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public int UpdateTypeStatus(int Status, int ID)
        {
            string updateSql = @"update SIDE_MD_OCSGrading_Type set   Type_Status=" + Status + "    WHERE ID=" + ID;

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return conn.Execute(updateSql);
            }
        }

    }
}
