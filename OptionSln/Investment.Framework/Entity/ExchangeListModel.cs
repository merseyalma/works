using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Investment.Framework.Entity
{
    

    public class ExchangeListModel
    {

        /// <summary>
        /// 证券代码
        /// </summary>
        public string zqdm { get; set; }

        /// <summary>
        /// 成交日期
        /// </summary>
        public DateTime cjrq { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string ywmc { get; set; }



        /// <summary>
        /// 成交数量 
        /// </summary>
        public int cjsl { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal cjjg { get; set; }

        /// <summary>
        /// 成交金额 
        /// </summary>
        public decimal cjje { get; set; }

        /// <summary>
        /// 清算金额
        /// </summary>
        public decimal qsje { get; set; }
    }
}
