using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Text;

namespace TestInfrastructure.AppSettings.InMemoryFileProvider
{
    internal class InMemoryFileProvider : IFileProvider
    {
        private readonly IFileInfo _fileInfo;

        public InMemoryFileProvider(string json)
        {
            _fileInfo = new InMemoryFile(json);
        }

        public IFileInfo GetFileInfo(string unused)
        {
            return _fileInfo;
        }

        public IDirectoryContents GetDirectoryContents(string unused)
        {
            return null;
        }

        public IChangeToken Watch(string unused)
        {
            return NullChangeToken.Singleton;
        }
    }

    internal class InMemoryFile : IFileInfo
    {
        private readonly byte[] _data;

        public InMemoryFile(string json)
        {
            _data = Encoding.UTF8.GetBytes(json);
        }

        public Stream CreateReadStream()
        {
            return new MemoryStream(_data);
        }

        public bool Exists { get; } = true;
        public long Length => _data.Length;
        public string PhysicalPath { get; } = string.Empty;
        public string Name { get; } = string.Empty;
        public DateTimeOffset LastModified { get; } = DateTimeOffset.UtcNow;
        public bool IsDirectory { get; } = false;
    }
}
