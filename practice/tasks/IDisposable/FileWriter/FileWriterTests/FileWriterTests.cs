using Convestudo.Unmanaged;
using NUnit.Framework;
using System;
using System.IO;

namespace FileWriterTests
{
    [TestFixture]
    public class FileWriterTests
    {
        private const string TestFileName = "test.txt";

        [TearDown]
        public void TearDown()
        {
            File.Delete(TestFileName);
        }

        [Test]
        public void Dispose_ShouldNotThrow()
        {
            var fileWriter = new FileWriter(TestFileName);

            Assert.DoesNotThrow(fileWriter.Dispose);
        }

        [Test]
        public void Disposing_CanBeCalledTwise()
        {
            var fileWriter = new FileWriter(TestFileName);

            fileWriter.Dispose();

            Assert.DoesNotThrow(fileWriter.Dispose);
        }

        [Test]
        public void CreateFileWriter_ShouldThrow_WhenResourceIsLocked()
        {
            var fileWriter1 = new FileWriter(TestFileName);
            fileWriter1.Write("Test");

            Assert.Throws<FileLoadException>(() =>
            {
                var file2 = new FileWriter(TestFileName);
            });

            fileWriter1.Dispose();
        }

        [Test]
        public void Write_ShouldThrow_WhenCallWriteAfterDisposing()
        {
            var writer = new FileWriter(TestFileName);

            writer.Write("Hello");

            writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => writer.Write("World!"));
        }

        [Test]
        public void Write_ShouldWriteWords()
        {
            const string testLine = "TestLine";
            var expected = String.Format("{0}{0}{0}{0}", testLine);

            using (var fileWriter = new FileWriter(TestFileName))
            {
                fileWriter.Write(testLine);
                fileWriter.Write(testLine);
                fileWriter.Write(testLine);
                fileWriter.Write(testLine);
            }

            var actual = File.ReadAllText(TestFileName);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteLine_ShouldWriteWithNewLine()
        {
            const string testLine = "TestLine";
            var expected = String.Format("{0}{1}{0}{1}{0}{1}{0}{1}", testLine, Environment.NewLine);

            using (var fileWriter = new FileWriter(TestFileName))
            {
                fileWriter.WriteLine(testLine);
                fileWriter.WriteLine(testLine);
                fileWriter.WriteLine(testLine);
                fileWriter.WriteLine(testLine);
            }

            var actual = File.ReadAllText(TestFileName);
            Assert.AreEqual(expected, actual);
        }
    }
}
