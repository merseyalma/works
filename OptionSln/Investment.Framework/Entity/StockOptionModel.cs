using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Investment.Framework.Entity
{
    public class StockOptionModel
    {
        public DateTime t { get; set; }
        public decimal asset { get; set; }
        public decimal risk { get; set; }
        public decimal yue { get; set; }
        public decimal sz { get; set; }
        public decimal sz50 { get; set; }

        public decimal annualrate { get; set; }

        public string dt { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        public decimal capital { get; set; }
    }
}
