using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "log.txt");
            var ini_path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "config.ini");
            var del = new ThreadStart(() =>
            {
                var lines = File.ReadAllLines(ini_path);
                try
                {
                    if (lines.Length >= 2)
                    {
                        System.Diagnostics.Process.Start(lines[0], lines[1]);
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText(path, DateTime.Now.ToString() + string.Format("异常:{0}\r\n", e.Message));
                }

                while (true)
                {
                    try
                    {
                        File.AppendAllText(path, DateTime.Now.ToString() + string.Format(" {0}\r\n", lines[0]));
                        //保留半个小时的日志
                        if(DateTime.Now - File.GetCreationTime(path) > new TimeSpan(0, 30, 0))
                        {
                            File.Delete(path);
                        }
                    }
                    catch (Exception e)
                    {
                        File.AppendAllText(path, DateTime.Now.ToString() + string.Format("异常:{0}\r\n", e.Message));
                    }
                    Console.WriteLine(DateTime.Now);
                    Int32 sleep_time = 1000;
                    Thread.Sleep(sleep_time);
                }
            });
            del.BeginInvoke(null, null);
            Console.Read();
        }
    }
}
