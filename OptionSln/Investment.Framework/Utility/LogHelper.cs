using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
namespace Investment.Framework.Utility
{
    /// <summary>
    /// 日志类型
    /// </summary>
    [Flags]
    public enum LogAction
    {
        Write = 1,
        Info = 2,
        Exception = 4

    }

    public class LogHelper
    {
        static Encoding encode = Encoding.UTF8;
        static string logPath = AppDomain.CurrentDomain.BaseDirectory + "logs\\";
        //static string bakPath = Config.LogBakPath;
        static string logName = "err";
        //static double bakLength = 1024000;
        static int timedLog = 1;
        static int logAction = (int)(LogAction.Info | LogAction.Exception);
        static object o = new object();
        static int refreshTime = 3000;
        //static double clearHour = 1;
        static DateTime bakTime = DateTime.Now;
        static DateTime clearTime = DateTime.Now;
        static Dictionary<LogAction, StringBuilder> logs = null;
        static Queue<KeyValuePair<LogAction, string>> logStack = null;
        static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        static readonly ReaderWriterLockSlim frwLock = new ReaderWriterLockSlim();
        static Mutex mut = null;
        static Thread proThread = null;
        static LogHelper()
        {
            lock (o)
            {
                Init();
            }
        }

        static void Init()
        {
            try
            {
                //timedLog = Consts.Mode ? 2 : 1;
                mut = new Mutex(false, "LogMutex" + Guid.NewGuid().ToString());
                if (logAction < 0) return;
                FileInfo f = new FileInfo(logPath);
                if (!f.Directory.Exists) f.Directory.Create();
                Array acs = Enum.GetValues(typeof(LogAction));
                logs = new Dictionary<LogAction, StringBuilder>();
                foreach (LogAction ac in acs)
                {
                    if (!logs.ContainsKey(ac)) logs.Add(ac, new StringBuilder());
                    if ((logAction & (int)ac) != 0 && ac != LogAction.Write)
                        Directory.CreateDirectory(logPath + ac.ToString());
                }
                logStack = new Queue<KeyValuePair<LogAction, string>>();
                if (logAction > -1 && (proThread == null || !proThread.IsAlive))
                {
                    proThread = new Thread(WriteLog);
                    proThread.IsBackground = true;
                    proThread.Start();
                }
            }
            catch (Exception e)
            {
                //LogHelper2.Exception(e.Message + e.StackTrace);

                e = null;
            }
        }

        public static void Write(string msg)
        {
            if (logAction >= 0)
                PushLog(LogAction.Write, DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg);
        }

        public static void Write(LogAction action, string msg)
        {
            if (logAction >= 0 && (logAction & (int)action) != 0)
                PushLog(action, DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg);
        }


        public static void Info(string msg)
        {
            if (logAction > -1 && (logAction & (int)LogAction.Info) != 0)
                PushLog(LogAction.Info, DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg);
        }

        public static void Exception(string msg)
        {
            if (logAction > -1 && (logAction & (int)LogAction.Exception) != 0)
                PushLog(LogAction.Exception, DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg);
        }


        private static void PushLog(LogAction action, string msg)
        {
            try
            {

                KeyValuePair<LogAction, string> log = new KeyValuePair<LogAction, string>(action, msg);
                rwLock.EnterWriteLock();
                logStack.Enqueue(log);
            }
            catch (Exception e) { e = null; }
            finally { rwLock.ExitWriteLock(); }
        }

        static void WriteLog()
        {
            while (true)
            {
                Thread.Sleep(refreshTime);
                int count = 0;
                try
                {
                    rwLock.EnterWriteLock();
                    count = logStack.Count;
                    int i = count;
                    while (i > 0)
                    {
                        KeyValuePair<LogAction, string> log = logStack.Dequeue();
                        i--;
                        if (null != log.Value)
                        {
                            logs[log.Key].AppendLine(log.Value);

                        }
                    }
                }
                catch (Exception e) { e = null; }
                finally
                {
                    rwLock.ExitWriteLock();
                }
                if (count > 0)
                {
                    try
                    {
                        DateTime now = DateTime.Now;
                        foreach (LogAction ac in logs.Keys)
                        {
                            if ((logAction & (int)ac) != 0)
                            {
                                string path = logPath;
                                //string bakDir = bakPath;
                                string act = (ac != LogAction.Write ? (ac.ToString() + "\\") : "");
                                //bakDir += act;
                                path += act;
                                if (timedLog == 1)
                                    path += now.ToString("yyyy-MM-dd HH-00-00") + ".txt";
                                else if (timedLog == 2)
                                    path += now.ToString("yyyy-MM-dd HH-mm-00") + ".txt";
                                else path += logName;
                                StringBuilder sb = logs[ac];
                                if (sb.Length > 0)
                                {
                                    WriteMsg(sb.ToString(), path, true);
                                    sb.Length = 0;
                                    //BakLog(path, bakDir);
                                }
                            }
                        }
                    }
                    catch (Exception e) { e = null; }
                }
            }
        }

        public static void WriteMsg(object msg, string logPath, bool isMutex)
        {
            if (!isMutex)
            {
                try
                {
                    frwLock.EnterWriteLock();
                    WriteMsg(msg, logPath);
                }
                catch (Exception e) { e = null; Init(); }
                finally
                {
                    frwLock.ExitWriteLock();
                }
                return;
            }
            try
            {
                mut.WaitOne();
                WriteMsg(msg, logPath);
            }
            catch (Exception e) { e = null; Init(); }
            finally
            {
                mut.ReleaseMutex();
            }
        }

        public static void WriteMsg(object msg, string logPath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logPath, true, encode))
                {
                    sw.Write(msg);
                }
            }
            catch (Exception e) { e = null; Init(); }
        }

        //public static void DeleteExpirateFiles(string dir, DateTime expDate)
        //{
        //    List<string> files = Directory.GetFileSystemEntries(dir, "*.txt").ToList().Where(x => ((File.GetLastWriteTime(x) < expDate))).ToList();
        //    foreach (string f in files)
        //    {
        //        File.Delete(f);
        //    }
        //}

        //static void BakLog(string logPath, string backDir)
        //{
        //try
        //{
        //    DateTime now = DateTime.Now;
        //    TimeSpan span = now - bakTime;
        //    if (span.Minutes > 5)  
        //    {
        //        FileInfo f = new FileInfo(logPath);
        //        if (f.Length > bakLength)
        //        {
        //            string path = backDir + now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
        //            f.MoveTo(path);
        //            bakTime = DateTime.Now;
        //        }
        //    }
        //    span = now - clearTime;
        //    if (span.Minutes > 10) 
        //    {
        //        DirectoryInfo dirInfo = new DirectoryInfo(backDir);
        //        DateTime expDate = now.AddHours(-clearHour);
        //        DeleteExpirateFiles(backDir, expDate);
        //        clearTime = DateTime.Now;
        //    }
        //}
        //catch (Exception e) { e = null; Init(); }
        //}

    }


}

