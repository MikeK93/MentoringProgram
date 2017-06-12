using System;

namespace Convestudo.Unmanaged
{
    public interface IFileWriter : IDisposable
    {
        void Write(string str);
        void WriteLine(string str);
    }
}