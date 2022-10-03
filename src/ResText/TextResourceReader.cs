using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System.Resources
{
    public class TextResourceReader : IResourceReader
    {
        private static readonly char[] commentChars = new char[3]
        {
            ';',
            '#',
            '/'
        };

        private readonly TextReader _reader;

        private bool _disposed;
        private bool _useTextResourceDataNodes;
        private List<TextResourceDataNode>? _nodes;

        public bool UseTextResourceDataNodes
        {
            get
            {
                return _useTextResourceDataNodes;
            }
            set
            {
                if (_nodes == null)
                {
                    _useTextResourceDataNodes = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public TextResourceReader(TextReader reader)
        {
            _reader = reader;
        }

        private IEnumerable<TextResourceDataNode> Read()
        {
            if (_nodes == null)
            {
                _nodes = new List<TextResourceDataNode>();

                string? line;
                List<string> comments = new List<string>();

                while ((line = _reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (Array.IndexOf(commentChars, line[0]) != -1)
                        {
                            string comment = line
                                .TrimStart(commentChars)
                                .Trim();

                            if (!string.IsNullOrWhiteSpace(comment))
                            {
                                comments.Add(comment);
                            }
                        }
                        else
                        {
                            int index = line.IndexOf(value: '=');

                            if (index == -1)
                            {
                                throw new FormatException();
                            }
                            else
                            {
                                string name = line
                                    .Substring(startIndex: 0, index)
                                    .Trim();
                                string value = line
                                    .Substring(index + 1)
                                    .Trim();

                                const char quoteChar = '"';

                                if (value.Length > 1 && value[0] == quoteChar && value[value.Length - 1] == quoteChar)
                                {
                                    value = value.Substring(startIndex: 1, value.Length - 2);
                                }

                                _nodes.Add(new TextResourceDataNode(name, value, comments));

                                comments.Clear();
                            }
                        }
                    }
                }
            }

            return _nodes;
        }

        public void Close()
        {
            ((IDisposable)this).Dispose();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return new TextResourceDictionaryEnumerator(Read().GetEnumerator(), UseTextResourceDataNodes);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IResourceReader)this).GetEnumerator();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reader.Dispose();
                }

                _disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        private sealed class TextResourceDictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<TextResourceDataNode> _enumerator;
            private readonly bool _useTextResourceDataNodes;

            public DictionaryEntry Entry
            {
                get
                {
                    return new DictionaryEntry(Key, Value);
                }
            }

            public object Key
            {
                get
                {
                    return _enumerator.Current.Name;
                }
            }

            public object? Value
            {
                get
                {
                    return _enumerator.Current.Value;
                }
            }

            public object Current
            {
                get
                {
                    if (_useTextResourceDataNodes)
                    {
                        return _enumerator.Current;
                    }
                    else
                    {
                        return Entry;
                    }
                }
            }

            public TextResourceDictionaryEnumerator(IEnumerator<TextResourceDataNode> enumerator, bool useTextResourceDataNodes)
            {
                _enumerator = enumerator;
                _useTextResourceDataNodes = useTextResourceDataNodes;
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}
