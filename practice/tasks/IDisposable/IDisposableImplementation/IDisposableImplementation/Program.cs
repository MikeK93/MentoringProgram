using System;
using System.Diagnostics;
using System.IO;

namespace NetMentoring
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DisposingLoggerAndTarget();

            DisposingOnlyTarget();

            DisposingLoggerAndTargetInsideLoop();

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void DisposingLoggerAndTargetInsideLoop()
        {
            TimeLogger(nameof(DisposingLoggerAndTargetInsideLoop), () =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
                    using (var logger = new MemoryStreamLogger(file))
                    {
                        logger.Log($"Interation number #{i}");
                    }
                }
            });
        }

        private static void DisposingOnlyTarget()
        {
            TimeLogger(nameof(DisposingOnlyTarget), () =>
            {
                using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        WriteLog(file, $"Interation number #{i}");
                    }
                }

                void WriteLog(Stream to, string text)
                {
                    var logger = new MemoryStreamLogger(to);
                    logger.Log(text);
                }
            });
        }

        private static void DisposingLoggerAndTarget()
        {
            TimeLogger(nameof(DisposingLoggerAndTarget), () =>
            {
                using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
                using (var logger = new MemoryStreamLogger(file))
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        logger.Log($"Interation number #{i}");
                    }
                }
            });
        }

        private static void TimeLogger(string actionName, Action action)
        {
            using (var file = File.CreateText($"{actionName}-log.txt"))
            {
                var t = new Stopwatch();
                t.Start();

                action();

                t.Stop();
                string logMessage = $"{actionName} took {t.Elapsed}.";
                Console.WriteLine(logMessage);
                file.Write(logMessage);
            }
        }
    }
}