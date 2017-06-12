using System;
using System.IO;

namespace NetMentoring
{
    public class MemoryStreamLogger : IDisposable
    {
        private readonly StreamWriter _writer;

        public MemoryStreamLogger(Stream target)
        {
            _writer = new StreamWriter(target);
        }

        public void Log(string message)
        {
            _writer.Write(message);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}