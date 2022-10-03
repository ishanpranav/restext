using System.Collections.Generic;

namespace System.Resources
{
    public class TextResourceDataNode
    {
        public string Name { get; }
        public string Value { get; }

        private readonly List<string> _comments = new List<string>();

        public IEnumerable<string> Comments
        {
            get
            {
                return _comments;
            }
        }

        public TextResourceDataNode(string name, string value, IEnumerable<string> comments)
        {
            Name = name;
            Value = value;

            foreach (string comment in comments)
            {
                _comments.Add(comment);
            }
        }
    }
}
