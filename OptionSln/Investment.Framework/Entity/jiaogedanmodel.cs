using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Investment.Framework.Entity
{
    public class jiaogedanmodel
    {
        public DateTime 成交日期;
        public string 证券代码;
        public string 证券名称;
        public string 业务名称;
        public int 成交数量;

        public decimal 清算金额;
    }
}
