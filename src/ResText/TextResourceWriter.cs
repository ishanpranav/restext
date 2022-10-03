using System.IO;
using System.Text;

namespace System.Resources
{
    public class TextResourceWriter : IResourceWriter
    {
        private readonly TextWriter _writer;

        private bool _disposed;

        public TextResourceWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void AddResource(string name, byte[]? value)
        {
            if (value == null)
            {
                AddResource(name, null as string);
            }
            else
            {
                AddResource(name, Encoding.UTF8.GetString(value));
            }
        }

        public void AddResource(string name, object? value)
        {
            if (value is byte[] bytes)
            {
                AddResource(name, bytes);
            }
            else if (value is string text)
            {
                AddResource(name, text);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void AddResource(string name, string? value)
        {
            _writer.Write(name);
            _writer.Write(value: '=');
            _writer.WriteLine(value);
        }

        public void AddResource(string name, TextResourceDataNode node)
        {
            foreach (string comment in node.Comments)
            {
                _writer.Write(value: "# ");
                _writer.WriteLine(comment);
            }

            AddResource(name, node.Value);
        }

        public void Close()
        {
            ((IDisposable)this).Dispose();
        }

        public void Generate()
        {
            _writer.Flush();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _writer.Dispose();
                }

                _disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }
    }
}
