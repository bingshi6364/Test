using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using PLMSide.Data.Entites;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface IMD_OCSGrading_TypeRepository :  IRepositoryBase<MD_OCSGrading_Type>
    {


        Task<MD_OCSGrading_Type> GetModel(int ID);

        Task<List<MD_OCSGrading_Type>> GetModelList();

        Task Delete(int ID);
        Task UpdateEntity(MD_OCSGrading_Type entity);
    
        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        Task<Tuple<List<MD_OCSGrading_Type>, int>> GetPageModelList(string whereStr);

        int UpdateTypeStatus(int Status, int ID); 
 

    }
}
