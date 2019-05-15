using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Entites
{
   public class MD_POS_Type
    {
        public int ID { get; set; }

        /// <summary>
        /// 【无效字段】
        /// </summary>
        public string Channel { get; set; }

        public string POS_Type { get; set; }

        /// <summary>
        /// POS_Channel
        /// </summary>		

        public string POS_Channel
        {
            get;
            set;
        }
        /// <summary>
        /// POS_Segment
        /// </summary>		

        public string POS_Segment
        {
            get;
            set;
        }
     
        /// <summary>
        /// Type_Sequence
        /// </summary>		

        public int Type_Sequence
        {
            get;
            set;
        }
        /// <summary>
        /// Type_Status
        /// </summary>		

        public int Type_Status
        {
            get;
            set;
        }



    }
}
