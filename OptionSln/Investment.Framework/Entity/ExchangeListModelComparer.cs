using Investment.Framework.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Investment.Framework.Entity
{
    public class ExchangeListModelComparer : IEqualityComparer<tbStockExchangeList>
    {

        public bool Equals(tbStockExchangeList x, tbStockExchangeList y)
        {
            if (x.成交编号 == y.成交编号 && x.证券代码 == y.证券代码 && x.成交日期 == y.成交日期 && x.剩余金额 == y.剩余金额 && x.剩余数量 == y.剩余数量 && x.业务名称 == y.业务名称 && x.清算金额 == y.清算金额)
            {
                return true;
            }
            else
                return false;
        }

        public int GetHashCode(tbStockExchangeList obj)
        {
            return (obj.成交编号 + obj.成交日期).GetHashCode();
        }
    }

}
