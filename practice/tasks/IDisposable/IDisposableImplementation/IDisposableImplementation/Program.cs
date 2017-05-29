using System;
using System.Diagnostics;
using System.IO;

namespace NetMentoring
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var t = new Stopwatch();

            DisposingLoggerAndTarget(t);

            DisposingOnlyTarget(t);

            DisposingLoggerAndTargetInsideLoop(t);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void DisposingLoggerAndTargetInsideLoop(Stopwatch t)
        {
            Console.WriteLine(nameof(DisposingLoggerAndTargetInsideLoop));

            t.Start();

            for (var i = 0; i < 10000; i++)
            {
                using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
                using (var logger = new MemoryStreamLogger(file))
                {
                    logger.Log($"Interation number #{i}");
                }
            }

            t.Stop();
            Console.WriteLine($"{t.Elapsed}");
        }

        private static void DisposingOnlyTarget(Stopwatch t)
        {
            Console.WriteLine(nameof(DisposingOnlyTarget));

            t.Start();
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

            t.Stop();
            Console.WriteLine($"{t.Elapsed}");
        }

        private static void DisposingLoggerAndTarget(Stopwatch t)
        {
            Console.WriteLine(nameof(DisposingLoggerAndTarget));

            t.Start();

            using (var file = new FileStream(@"\log.txt", FileMode.OpenOrCreate | FileMode.Append))
            using (var logger = new MemoryStreamLogger(file))
            {
                for (var i = 0; i < 10000; i++)
                {
                    logger.Log($"Interation number #{i}");
                }
            }

            t.Stop();
            Console.WriteLine($"{t.Elapsed}");
        }
    }
}
