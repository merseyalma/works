using System;
using System.Collections;
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
        public decimal 成交价格;
    }

    public class stockinfo
    {
        public string 证券名称;
        public string 证券代码;
        public decimal yingli;
        public int 持仓;

    }

    public class istockinfoequalitycomparer : IEqualityComparer<stockinfo>
    {
        public bool Equals(stockinfo x, stockinfo y)
        {
            return x.证券代码 == y.证券代码;

        }

        public int GetHashCode(stockinfo obj)
        {
            return obj.ToString().GetHashCode();
        }
    }


}
