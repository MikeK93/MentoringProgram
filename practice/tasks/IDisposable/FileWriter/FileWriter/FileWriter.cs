using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Convestudo.Unmanaged
{
    public class FileWriter : IFileWriter
    {
        private readonly SafeFileHandle _fileHandle;

        /// <summary>
        /// Creates or opens a file or I/O device.
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/aa363858(v=vs.85).aspx"/>
        /// </summary>
        /// <param name="lpFileName">Name of the file or device to be opened or created.</param>
        /// <param name="dwDesiredAccess">The requested access to the file or device, which can be specified using <see cref="DesiredAccess"/>.</param>
        /// <param name="dwShareMode">The requested sharing mode of the file or device, which can be specified using <see cref="ShareMode"/>.</param>
        /// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members: an optional security descriptor, and a Boolean value that determines whether the returned handle can be inherited by child processes.</param>
        /// <param name="dwCreationDisposition">An action to take on a file or device that exists or does not exist. Can be specified using <see cref="CreationDisposition"/>.</param>
        /// <param name="dwFlagsAndAttributes">The file or device attributes and flags, <see cref="FlagsAndAttributes.Normal"/> being the most common default value for files in <see cref="FlagsAndAttributes"/> enum.</param>
        /// <param name="hTemplateFile">A valid handle to a template file with the GENERIC_READ access right.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot. 
        /// If the function fails, the return value is INVALID_HANDLE_VALUE.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName, DesiredAccess dwDesiredAccess, ShareMode dwShareMode, IntPtr lpSecurityAttributes, CreationDisposition dwCreationDisposition, FlagsAndAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// Writes data to the specified file or input/output (I/O) device.
        /// </summary>
        /// <param name="hFile">A handle to the file or I/O device.</param>
        /// <param name="aBuffer">A pointer to the buffer containing the data to be written to the file or device.</param>
        /// <param name="cbToWrite">The number of bytes to be written to the file or device.</param>
        /// <param name="cbThatWereWritten">A pointer to the variable that receives the number of bytes written when using a synchronous <see cref="hFile"/> parameter. </param>
        /// <param name="pOverlapped">A pointer to an OVERLAPPED structure is required if the <see cref="hFile"/> parameter was opened with FILE_FLAG_OVERLAPPED, otherwise this parameter can be NULL.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(IntPtr hFile, Byte[] aBuffer, UInt32 cbToWrite, ref UInt32 cbThatWereWritten, IntPtr pOverlapped);

        private static void ThrowLastWin32Err()
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Converts string to byte array
        /// </summary>
        /// <param name="str">Input string to be converted to byte array.</param>
        /// <returns>Byte representation of a <see cref="str"/>.</returns>
        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes.Where(@byte => @byte != 0).ToArray();
        }

        public FileWriter(string fileName, CreationDisposition disposition = CreationDisposition.CreateAlways)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException(nameof(fileName));
            }

            var pointer = CreateFile(fileName,
                DesiredAccess.Write,
                ShareMode.None,
                IntPtr.Zero,
                disposition,
                FlagsAndAttributes.Normal,
                IntPtr.Zero
            );

            _fileHandle = new SafeFileHandle(pointer, true);

            if (_fileHandle.IsInvalid)
            {
                ThrowLastWin32Err();
            }
        }

        public void Write(string str)
        {
            if (_fileHandle.IsClosed)
            {
                throw new InvalidOperationException($"Cannot write to a closed file.");
            }

            var bytes = GetBytes(str);
            uint bytesWritten = 0;
            WriteFile(_fileHandle.DangerousGetHandle(), bytes, (uint)bytes.Length, ref bytesWritten, IntPtr.Zero);
        }

        public void WriteLine(string str)
        {
            Write($"{str}{Environment.NewLine}");
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fileHandle.Close();
            }
        }
    }
}