using Investment.Framework.DB;
using Investment.Framework.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Investment.Framework.Biz
{
    public class BzOption
    {

        public static string CalculateOption()
        {
            string err = string.Empty;
            try
            {
                using (StocksDbDataContext db = new StocksDbDataContext())
                {
                    #region 期权
                    using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutputLocation"] + "data.js"))
                    {
                        StringBuilder sb = new StringBuilder();
                        #region 日

                        int startDay = 0;
                        DateTime startDate = DateTime.Parse("2017-2-3");

                        List<tbStockOptionSummary> olist = db.tbStockOptionSummary.ToList();
                        for (int i = 0; i < olist.Count; i++)
                        {
                            olist[i].年化收益率 = decimal.Parse(((olist[i].资产 - olist[i].本金) / olist[i].本金 / ((olist[i].recordtime - startDate).Days + 1) * 36500).ToString("#0.00"));
                        }

                        db.SubmitChanges();

                        List<StockOptionModel> list = olist.Select(s1 => new StockOptionModel
                        {
                            asset = s1.资产,
                            risk = s1.风险度,
                            sz = s1.上证指数,
                            sz50 = s1.上证50ETF,
                            t = s1.recordtime,
                            yue = s1.可用资金,
                            annualrate = s1.年化收益率.Value,
                            dt = s1.DateType,
                            capital = s1.本金

                        }).ToList();
                        foreach (StockOptionModel item in list)
                        {
                            item.asset = decimal.Parse((item.asset / item.capital).ToString("#0.000000"));
                        }

                        list = list.OrderByDescending(o => o.t).ToList();

                        IsoDateTimeConverter iso = new IsoDateTimeConverter() { DateTimeFormat = "yy-MM-dd" };
                        sb.Append("var dayinfo =" + JsonConvert.SerializeObject(list, iso) + ";");


                        #endregion
                        #region 周
                        int startWeek = 0;
                        List<StockOptionModel> weeklist = list.Where(w => w.dt.Contains("1")).ToList();

                        // 取最新一条记录
                        DateTime time = DateTime.MinValue;
                        if (weeklist.Count > 0)
                        {
                            time = weeklist[0].t;
                        }
                        StockOptionModel s = list.OrderByDescending(o => o.t).FirstOrDefault(f => f.t > time);
                        if (s != null)
                            weeklist.Add(s);
                        // 取最新一条记录 end
                        weeklist = weeklist.OrderByDescending(o => o.t).ToList();

                        for (int i = weeklist.Count - 1; i >= 0; i--)
                        {
                            weeklist[i].annualrate = decimal.Parse(((weeklist[i].asset - 1) / (weeklist.Count - i + startWeek) * 5200).ToString("#0.00"));

                        }
                        sb.Append("\r\nvar weekinfo =" + Newtonsoft.Json.JsonConvert.SerializeObject(weeklist, iso) + ";");
                        #endregion


                        #region 月

                        List<StockOptionModel> monthlist = list.Where(w => w.dt.Contains("2")).ToList();

                        // 取最新一条记录
                        time = DateTime.MinValue;
                        if (monthlist.Count > 0)
                        {
                            time = monthlist[0].t;
                        }
                        s = list.OrderByDescending(o => o.t).FirstOrDefault(f => f.t > time);
                        if (s != null)
                            monthlist.Add(s);
                        // 取最新一条记录 end
                        monthlist = monthlist.OrderByDescending(o => o.t).ToList();

                        for (int i = monthlist.Count - 1; i >= 0; i--)
                        {
                            monthlist[i].annualrate = decimal.Parse(((monthlist[i].asset - 1) / (monthlist.Count - i) * 1200).ToString("#0.00"));

                        }
                        sb.Append("\r\nvar monthinfo =" + Newtonsoft.Json.JsonConvert.SerializeObject(monthlist, iso) + ";");
                        #endregion


                        sw.Write(sb.ToString());
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

                err = ex.Message;
            }

            return err;
        }
    }
}
