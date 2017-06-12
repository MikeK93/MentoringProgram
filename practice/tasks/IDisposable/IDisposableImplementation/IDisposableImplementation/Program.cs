using System;
using System.Diagnostics;
using System.IO;

namespace NetMentoring
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TimeLogger(DisposingLoggerAndTarget);

            TimeLogger(DisposingOnlyTarget);

            TimeLogger(DisposingLoggerAndTargetInsideLoop);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void DisposingLoggerAndTargetInsideLoop()
        {
            for (var i = 0; i < 10000; i++)
            {
                using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
                using (var logger = new MemoryStreamLogger(file))
                {
                    logger.Log($"Interation number #{i}");
                }
            }
        }

        private static void DisposingOnlyTarget()
        {
            using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
            {
                for (var i = 0; i < 10000; i++)
                {
                    var logger = new MemoryStreamLogger(file);
                    logger.Log($"Interation number #{i}");
                }
            }
        }

        private static void DisposingLoggerAndTarget()
        {
            using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
            using (var logger = new MemoryStreamLogger(file))
            {
                for (var i = 0; i < 10000; i++)
                {
                    logger.Log($"Interation number #{i}");
                }
            }
        }

        private static void TimeLogger(Action action)
        {
            var actionName = action.Method.Name;

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