using Investment.Framework.DB;
using Investment.Framework.Entity;
using Investment.Framework.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Investment.Framework.Biz
{
    public class BzStock
    {
        /// <summary>
        /// 导入交割单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ImportExchangeList(ref string result)
        {
            string err = string.Empty;

            try
            {

                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "file");

                string ss = string.Empty;
                List<tbStockExchangeList> jgdlist = new List<tbStockExchangeList>();
                int count = 0;
                foreach (string item in files)
                {
                    StreamReader sr = new StreamReader(item, Encoding.Default);


                    int filetype = -1;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (filetype == -1)
                        {
                            filetype = line.IndexOf("发生日期") != -1 ? 1 : 0;
                            continue;
                        }
                        count++;
                        string[] arr = line.Replace("=", "").Replace("\"", "").Split('	');
                        tbStockExchangeList jgd = new tbStockExchangeList();
                        if (filetype == 1)
                        {
                            //宏信
                            ///="发生日期"	="成交时间"	="证券代码"	="证券名称"	="买卖标志"	="币种"	="成交数量"	="成交价格"	="成交金额"	="发生金额"	
                            ///="剩余金额"	="申报序号"	="股东代码"	="席位代码"	="委托编号"	="成交编号"	="证券数量"	="佣金"	="印花税"	="过户费"	="其他费"	="备注"	

                            string time = arr[0].Replace("\"", "").Replace("=", "");
                            string time1 = arr[1].Replace("\"", "").Replace("=", "");
                            time1 = (1000000 + int.Parse(time1)).ToString().Substring(1);
                            string dealtime = time.Substring(0, 4) + "-" + time.Substring(4, 2) + "-" + time.Substring(6, 2);
                            if (time1.Length == 5)
                            {
                                dealtime += " " + time1.Substring(0, 1) + ":" + time1.Substring(1, 2) + ":" + time1.Substring(3, 2);
                            }
                            else
                            {
                                dealtime += " " + time1.Substring(0, 2) + ":" + time1.Substring(2, 2) + ":" + time1.Substring(4, 2);
                            }
                            jgd.成交日期 = DateTime.Parse(dealtime);
                            jgd.业务名称 = arr[21];
                            jgd.证券代码 = arr[2];
                            jgd.证券名称 = arr[3];
                            jgd.成交价格 = decimal.Parse(arr[7]);
                            jgd.成交数量 = int.Parse(arr[6].Replace(".00", ""));
                            jgd.剩余数量 = int.Parse(arr[16].Replace(".00", ""));
                            jgd.成交金额 = decimal.Parse(arr[8]);
                            jgd.清算金额 = decimal.Parse(arr[9]);
                            jgd.剩余金额 = decimal.Parse(arr[10]);
                            jgd.净佣金 = decimal.Parse(arr[17]);
                            jgd.规费 = 0;
                            jgd.印花税 = decimal.Parse(arr[18]);
                            jgd.过户费 = decimal.Parse(arr[19]);
                            jgd.附加费 = decimal.Parse(arr[20]);
                            jgd.结算费 = 0;
                            jgd.成交编号 = arr[14];
                            jgd.股东代码 = arr[12];


                            jgdlist.Add(jgd);

                        }
                        else
                        {
                            //国泰君安
                            //="成交日期"	="业务名称"	="证券代码"	="证券名称"	="成交价格"	="成交数量"	="剩余数量"	
                            //    ="成交金额"	="清算金额"	="剩余金额"	="净佣金"	="规费"	="印花税"	="过户费"	
                            //    ="结算费"	="附加费"	="成交编号"	="股东代码"	


                            string time = arr[0].Replace("\"", "").Replace("=", "");
                            jgd.成交日期 = DateTime.Parse(time.Substring(0, 4) + "-" + time.Substring(4, 2) + "-" + time.Substring(6, 2));
                            jgd.业务名称 = arr[1];
                            jgd.证券代码 = arr[2];
                            jgd.证券名称 = arr[3];
                            jgd.成交价格 = decimal.Parse(arr[4]);
                            jgd.成交数量 = int.Parse(arr[5].Replace(".00", ""));
                            jgd.成交金额 = decimal.Parse(arr[7]);
                            jgd.清算金额 = decimal.Parse(arr[8]);
                            jgd.剩余金额 = decimal.Parse(arr[9]);
                            jgd.净佣金 = decimal.Parse(arr[10]);
                            jgd.规费 = decimal.Parse(arr[11]);
                            jgd.印花税 = decimal.Parse(arr[12]);
                            jgd.过户费 = decimal.Parse(arr[13]);
                            jgd.结算费 = decimal.Parse(arr[14]);
                            jgd.附加费 = decimal.Parse(arr[15]);
                            jgd.成交编号 = arr[16];
                            jgd.股东代码 = arr[17];
                            jgd.剩余数量 = int.Parse(arr[6]);

                            jgdlist.Add(jgd);
                        }
                    }

                    sr.Close();

                    ss += " " + Path.GetFileName(item) + "" + count;
                }
                if (jgdlist.Count > 0)
                {
                    using (StocksDbDataContext db = new StocksDbDataContext())
                    {
                        DateTime minTime = jgdlist.Select(s => s.成交日期).Min();
                        List<string> zqs = jgdlist.Select(s => s.证券代码).ToList();


                        ExchangeListModelComparer comparer = new ExchangeListModelComparer();
                        List<tbStockExchangeList> extlist = db.tbStockExchangeList.Where(w => w.成交日期 >= minTime && zqs.Contains(w.证券代码)).ToList();

                        jgdlist = jgdlist.Except(extlist, comparer).ToList();

                        db.tbStockExchangeList.InsertAllOnSubmit(jgdlist);
                        db.SubmitChanges();
                    }
                }
                result = jgdlist.Count + " " + count;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return err;
        }

        /// <summary>
        /// 导入股票每日价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ImportStockPrice()
        {
            string err = string.Empty;
            try
            {
                string url = "http://web.ifzq.gtimg.cn/appstock/app/fqkline/get?_var=kline_dayqfq&param={2}{0},day,,,{1},qfq";

                List<string> codePrefix = new List<string>() { "6", "3", "0", "1", "5" };
                using (StocksDbDataContext db = new StocksDbDataContext())
                {
                    List<tbStockPrice> stockPriceList = new List<tbStockPrice>();
                    int count = 0;
                    int perCount = 1000;

                    DateTime time = DateTime.Parse("2015-4-21");
                    tbConfig config = db.tbConfig.SingleOrDefault(s => s.Type == "StockPrice");
                    if (config != null)
                    {
                        time = config.StartTime.AddDays(-3);

                    }
                    List<tbStockPrice> existedList = db.tbStockPrice.Where(w => w.日期 >= time).ToList();
                    db.tbStockPrice.DeleteAllOnSubmit(existedList);
                    db.SubmitChanges();


                    List<Proc_GetStockFirstDealDayResult> stockCodeList = db.Proc_GetStockFirstDealDay(time).ToList();
                    foreach (Proc_GetStockFirstDealDayResult item in stockCodeList)
                    {

                        #region 价格

                        if (item.证券代码.Length == 6 && codePrefix.Contains(item.证券代码.Substring(0, 1)))
                        {
                            DateTime tempTime = time;
                            if (item.成交日期 >= tempTime)
                                tempTime = item.成交日期;

                            string result = WebSvcCaller.QueryGetWebService(string.Format(url, item.证券代码, Math.Ceiling((DateTime.Now - tempTime).TotalDays * 5 / 7) + 7, item.类型), string.Empty, null, ref err);
                            if (err == string.Empty)
                            {

                                result = result.Substring(result.IndexOf("{"));
                                JObject jsonObj = JObject.Parse(result);

                                JToken qtqday = jsonObj.First.Next.Next.First.First.First.First.First;
                                LogHelper.Info(item.证券名称 + "," + item.证券代码 + "," + qtqday.Count());
                                foreach (var qday in qtqday)
                                {
                                    DateTime dayTime = DateTime.Parse(qday[0].ToString());
                                    if (dayTime >= time)
                                    {

                                        tbStockPrice stockPrice = new tbStockPrice();
                                        stockPrice.日期 = dayTime;
                                        stockPrice.证券代码 = item.证券代码;
                                        stockPrice.证券名称 = item.证券名称;
                                        stockPrice.开盘价格 = decimal.Parse(qday[1].ToString());
                                        stockPrice.收盘价格 = decimal.Parse(qday[2].ToString());
                                        stockPrice.最高价格 = decimal.Parse(qday[3].ToString());
                                        stockPrice.最低价格 = decimal.Parse(qday[4].ToString());
                                        stockPrice.类型 = item.类型;

                                        stockPriceList.Add(stockPrice);
                                        count++;
                                    }

                                }

                            }
                            else
                            {
                                LogHelper.Info(item.证券代码 + " " + item.证券名称 + " " + err);
                            }
                        }

                        #endregion
                        if (count > perCount)
                        {
                            db.tbStockPrice.InsertAllOnSubmit(stockPriceList);
                            db.SubmitChanges();
                            stockPriceList = new List<tbStockPrice>();
                            count = 0;
                        }
                    }

                    if (count > 0)
                    {
                        db.tbStockPrice.InsertAllOnSubmit(stockPriceList);
                        db.SubmitChanges();
                        stockPriceList = new List<tbStockPrice>();
                        count = 0;
                    }
                    if (config == null)
                    {
                        config = new tbConfig();
                        config.StartTime = DateTime.Today;
                        config.Type = "stockprice";
                        db.tbConfig.InsertOnSubmit(config);
                    }
                    else
                    {
                        config.StartTime = DateTime.Today;
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {

                err = ex.Message;
            }

            return err;

        }

        public static string CalculateProfit()
        {
            string err = string.Empty;
            try
            {
                //          select sum(成交数量),证券代码,证券名称 from tbStockExchangeList where 成交日期<='2015-6-17'
                //and 业务名称 in ('证券买入','证券卖出','红股入账','新股入账' ) group by 证券代码,证券名称 having sum(成交数量)<>0;

                //       SELECT   SUM(清算金额) AS yingli FROM      tbStockExchangeList
                //where 业务名称 in ('证券买入','证券卖出','红利入账','新股IPO配售确认')
                //and 成交日期<='2015-6-17' 

                List<string> yewuTotalList = new List<string>() { "证券买入", "证券卖出", "红股入账", "新股入账", "红利入账", "新股IPO配售确认" };

                List<string> yewuList = new List<string>() { "证券买入", "证券卖出", "红股入账", "新股入账" };

                List<string> yewuqingsuanjineList = new List<string>() { "证券买入", "证券卖出", "红利入账", "新股IPO配售确认" };

                using (StocksDbDataContext db = new StocksDbDataContext())
                {
                    List<jiaogedanmodel> list = db.tbStockExchangeList.Where(w => yewuTotalList.Contains(w.业务名称)).Select(s => new jiaogedanmodel
                    {
                        成交日期 = s.成交日期,
                        成交数量 = s.成交数量,
                        清算金额 = s.清算金额,
                        证券代码 = s.证券代码,
                        证券名称 = s.证券名称,
                        业务名称 = s.业务名称

                    }).ToList();

                    List<tbStockPrice> priceList = db.tbStockPrice.ToList();
                    List<tbStockIndexPrice> szIndexPriceList = db.tbStockIndexPrice.ToList();

                    DateTime startTime = DateTime.Parse("2015-4-21");

                    tbConfig config = db.tbConfig.SingleOrDefault(s => s.Type == "Profit");
                    if (config != null)
                    {
                        startTime = config.StartTime.AddDays(-7);
                    }

                    List<tbStockProfit> existedList = db.tbStockProfit.Where(w => w.日期 >= startTime).ToList();
                    db.tbStockProfit.DeleteAllOnSubmit(existedList);
                    db.SubmitChanges();


                    List<tbStockProfit> profitList = new List<tbStockProfit>();

                    int days = (int)Math.Floor((DateTime.Today - startTime).TotalDays);

                    for (int i = 0; i <= days; i++)
                    {
                        if (startTime.DayOfWeek != DayOfWeek.Saturday && startTime.DayOfWeek != DayOfWeek.Sunday)
                        {
                            #region MyRegion

                            DateTime nextDay = startTime.AddDays(1);

                            decimal qingsuanjine = list.Where(w => yewuqingsuanjineList.Contains(w.业务名称) && w.成交日期 < nextDay).Sum(s => s.清算金额); //清算金额

                            // 获取当日证券价格
                            List<tbStockPrice> dayPricelist = priceList.Where(w => w.日期 < nextDay).ToList();
                            if (dayPricelist.Count > 0)
                            {
                                // 获取当日证券数量
                                var q = (from p in list
                                         where p.成交日期 < nextDay && yewuList.Contains(p.业务名称)
                                         group p by p.证券代码 into g
                                         select new
                                         {
                                             g.Key,
                                             shuliang = g.Sum(p => p.成交数量)
                                         } into t
                                         where t.shuliang > 0
                                         select t).ToList();

                                // 计算当日的市值; 
                                decimal shizhi = 0;

                                foreach (var item in q)
                                {
                                    tbStockPrice price = dayPricelist.Where(s => s.证券代码 == item.Key).OrderByDescending(o => o.日期).FirstOrDefault();
                                    if (price != null)
                                        shizhi += price.收盘价格 * item.shuliang;
                                    else
                                    {
                                        LogHelper.Info(string.Format("{0},{1},无价格\r\n", startTime.ToString("yyyy-MM-dd"), item.Key));
                                    }
                                }

                                tbStockIndexPrice shangzheng = szIndexPriceList.SingleOrDefault(s => s.日期 == startTime && s.证券代码 == "000001" && s.指数类型 == "sh");
                                if (shangzheng != null)
                                {
                                    profitList.Add(new tbStockProfit { 日期 = startTime, 盈亏 = qingsuanjine + shizhi, 证券市值 = shizhi });

                                }
                            }

                            #endregion
                        }
                        startTime = startTime.AddDays(1);
                    }
                    if (config == null)
                    {
                        config = new tbConfig();
                        config.StartTime = DateTime.Today;
                        config.Type = "profit";
                        db.tbConfig.InsertOnSubmit(config);
                    }
                    else
                    {
                        config.StartTime = DateTime.Today;
                    }
                    db.tbStockProfit.InsertAllOnSubmit(profitList);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {

                err = ex.Message;
            }
            return err;
        }


        public static string ExportProfit()
        {
            string err = string.Empty;
            try
            {
                //          select sum(成交数量),证券代码,证券名称 from tbStockExchangeList where 成交日期<='2015-6-17'
                //and 业务名称 in ('证券买入','证券卖出','红股入账','新股入账' ) group by 证券代码,证券名称 having sum(成交数量)<>0;

                //       SELECT   SUM(清算金额) AS yingli FROM      tbStockExchangeList
                //where 业务名称 in ('证券买入','证券卖出','红利入账','新股IPO配售确认')
                //and 成交日期<='2015-6-17' 


                using (StocksDbDataContext db = new StocksDbDataContext())
                {

                    List<tbStockProfit> profitList = db.tbStockProfit.ToList();
                    List<tbStockIndexPrice> szIndexPriceList = db.tbStockIndexPrice.ToList();

                    StringBuilder profitDayList = new StringBuilder();
                    StringBuilder profitWeekList = new StringBuilder();
                    StringBuilder profitMonthList = new StringBuilder();

                    foreach (tbStockProfit item in profitList)
                    {
                        DateTime startTime = item.日期;

                        if (startTime.DayOfWeek != DayOfWeek.Saturday && startTime.DayOfWeek != DayOfWeek.Sunday)
                        {
                            #region MyRegion

                            tbStockIndexPrice shangzheng = szIndexPriceList.SingleOrDefault(s => s.日期 == startTime && s.证券代码 == "000001" && s.指数类型 == "sh");
                            if (shangzheng != null)
                            {
                                string strProfit = string.Format(",[\"{0}\",{1},{2},{3}]", startTime.ToString("yyyy-MM-dd"), shangzheng.收盘价格, item.盈亏, item.证券市值);

                                profitDayList.Append(strProfit);
                                if (shangzheng.交易日类型.IndexOf("1") != -1)
                                {
                                    profitWeekList.Append(strProfit);
                                }
                                if (shangzheng.交易日类型.IndexOf("2") != -1)
                                {
                                    profitMonthList.Append(strProfit);
                                }
                            }

                            #endregion
                        }
                    }
                    using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutputLocation"]   +"idata.js"))
                    {
                        sw.Write("//时间,上证,收益,市值");
                        sw.Write("\r\nvar dayinfo =[" + profitDayList.ToString().Substring(1) + "];");
                        sw.Write("\r\nvar weekinfo =[" + profitWeekList.ToString().Substring(1) + "];");
                        sw.Write("\r\nvar monthinfo =[" + profitMonthList.ToString().Substring(1) + "];");
                    }
                }
            }
            catch (Exception ex)
            {

                err = ex.Message;
            }
            return err;
        }
        /// <summary>
        /// 导入每日上证指数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ImportSZPrice()
        {
            string err = string.Empty;
            try
            {
                using (StocksDbDataContext db = new StocksDbDataContext())
                {
                    string url = "http://web.ifzq.gtimg.cn/appstock/app/fqkline/get?_var=kline_dayqfq&param={2}{0},day,,,{1},qfq";

                    DateTime startTime = DateTime.Parse("2015-4-1");
                    tbConfig config = db.tbConfig.SingleOrDefault(s => s.Type == "szprice");
                    if (config != null)
                    {
                        startTime = config.StartTime.AddDays(-7);

                    }
                    List<tbStockIndexPrice> existedList = db.tbStockIndexPrice.Where(w => w.日期 >= startTime).ToList();
                    db.tbStockIndexPrice.DeleteAllOnSubmit(existedList);
                    db.SubmitChanges();


                    #region MyRegion
                    string result = WebSvcCaller.QueryGetWebService(string.Format(url, "000001", Math.Ceiling((DateTime.Now - startTime).TotalDays * 5 / 7) + 7, "sh"), string.Empty, null, ref err);

                    if (err == string.Empty)
                    {

                        List<tbStockIndexPrice> stockPriceList = new List<tbStockIndexPrice>();
                        result = result.Substring(result.IndexOf("{"));
                        JObject jsonObj = JObject.Parse(result);

                        JToken qtqday = jsonObj.First.Next.Next.First.First.First.First.First;

                        foreach (var qday in qtqday)
                        {
                            DateTime dayTime = DateTime.Parse(qday[0].ToString());
                            if (dayTime >= startTime)
                            {

                                tbStockIndexPrice stockPrice = new tbStockIndexPrice();
                                stockPrice.日期 = dayTime;
                                stockPrice.证券代码 = "000001";
                                stockPrice.证券名称 = "上证指数";
                                stockPrice.开盘价格 = decimal.Parse(qday[1].ToString());
                                stockPrice.收盘价格 = decimal.Parse(qday[2].ToString());
                                stockPrice.最高价格 = decimal.Parse(qday[3].ToString());
                                stockPrice.最低价格 = decimal.Parse(qday[4].ToString());
                                stockPrice.指数类型 = "sh";
                                stockPrice.交易日类型 = "0";

                                stockPriceList.Add(stockPrice);
                            }
                        }

                        // 计算交易日类型

                        for (int i = 0; i < stockPriceList.Count; i++)
                        {
                            string jiaoyirileixing = string.Empty;
                            DateTime time = stockPriceList[i].日期;

                            // 下一个交易时间跟当前的不连续，则为这周最后一个交易日

                            if (i == stockPriceList.Count - 1)
                            {
                                jiaoyirileixing = "1,2";

                            }
                            else
                            {
                                if (time.AddDays(1) != stockPriceList[i + 1].日期)
                                {
                                    if (time.Month != stockPriceList[i + 1].日期.Month)
                                        jiaoyirileixing = "1,2";
                                    else
                                    {
                                        jiaoyirileixing = "1";
                                    }

                                }
                                else
                                {
                                    if (time.Month != stockPriceList[i + 1].日期.Month)
                                        jiaoyirileixing = "0,2";
                                    else
                                    {
                                        jiaoyirileixing = "0";
                                    }

                                }
                            }
                            stockPriceList[i].交易日类型 = jiaoyirileixing;
                        }
                        if (config == null)
                        {
                            config = new tbConfig();
                            config.StartTime = DateTime.Today;
                            config.Type = "szprice";
                            db.tbConfig.InsertOnSubmit(config);
                        }
                        else
                        {
                            config.StartTime = DateTime.Today;
                        }

                        db.tbStockIndexPrice.InsertAllOnSubmit(stockPriceList);
                        db.SubmitChanges();
                    }
                    else
                    {

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

        /// <summary>
        /// 导出交割单概要
        /// </summary>
        /// <returns></returns>
        public static string ExportJgd()
        {
            string err = string.Empty;
            try
            {
                List<string> yewuTotalList = new List<string>() { "证券买入", "证券卖出", "红股入账", "新股入账", "红利入账", "新股IPO配售确认" };

                List<string> yewuList = new List<string>() { "证券买入", "证券卖出", "红股入账", "新股入账" };

                List<string> yewuqingsuanjineList = new List<string>() { "证券买入", "证券卖出", "红利入账", "新股IPO配售确认" };

                StringBuilder sb = new StringBuilder();

                using (StocksDbDataContext db = new StocksDbDataContext())
                {
                    List<jiaogedanmodel> list = db.tbStockExchangeList.Where(w => yewuTotalList.Contains(w.业务名称)).Select(s => new jiaogedanmodel
                    {
                        成交日期 = s.成交日期,
                        成交数量 = s.成交数量,
                        清算金额 = s.清算金额,
                        证券代码 = s.证券代码,
                        证券名称 = s.证券名称,
                        业务名称 = s.业务名称,
                        成交价格 = s.成交价格

                    }).ToList();

                    List<stockinfo> stockList = list.Select(s => new stockinfo { 证券代码 = s.证券代码, 证券名称 = s.证券名称 }).Distinct(new istockinfoequalitycomparer()).OrderBy(o => o.证券名称).ToList();

                    sb.Append("var stocks ={");

                    for (int i = 0; i < stockList.Count; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(",");
                        }
                        sb.AppendFormat("\"s{0}\":\"{1}\"", stockList[i].证券代码, stockList[i].证券名称);
                    }

                    sb.Append("};");
                    sb.AppendLine();
                    sb.Append("var jiaogedans =[");


                    List<DateTime> jgddates = list.Select(s => s.成交日期.Date).Distinct().ToList();

                    for (int i = 0; i < jgddates.Count; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(",");
                        }
                        sb.AppendFormat("{{t:\"{0}\",d:[", jgddates[i].ToString("yyyy-MM-dd"));

                        DateTime next = jgddates[i].AddDays(1);
                        List<jiaogedanmodel> subjgd = list.Where(w => w.成交日期 >= jgddates[i] && w.成交日期 < next).ToList();

                        for (int j = 0; j < subjgd.Count; j++)
                        {
                            if (j > 0)
                            {
                                sb.Append(",");
                            }
                            //[1,"002751",61.01,200,12202.00,-12208.10],
                            sb.AppendFormat("[{0},\"{1}\",{2},{3},{4},{5}]", (subjgd[j].业务名称 == "证券卖出" ? "0" : "1"), subjgd[j].证券代码, subjgd[j].成交价格, subjgd[j].成交数量, subjgd[j].成交价格 * subjgd[j].成交数量, subjgd[j].清算金额);

                        }

                        sb.Append("]}");
                    }

                    sb.Append("];");
                }

                using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutputLocation"] + "stockdata.js"))
                {
                    sw.Write(sb.ToString());
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
