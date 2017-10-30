using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Runtime.Serialization.Parsers
{
    internal class ParserRepository<T> : IEnumerable<IParser>
    {
        private readonly IDictionary<Type, IParser> _parsers;

        internal ParserRepository()
        {
            _parsers = new Dictionary<Type, IParser>();
        }

        public void Register<U>(IParser<T, U> parser)
        {
            if (parser == null) throw new ArgumentNullException("parser", "Parser is required");

            _parsers.Add(typeof(U), parser);
        }

        public IParser<T, U> Find<U>()
        {
            Type type = typeof(U);

            if (!_parsers.ContainsKey(type)) throw new InvalidOperationException(string.Format("There is no parser of '{0}' type in the repository", type));

            return (IParser<T, U>) _parsers[type];            
        }

        IEnumerator<IParser> IEnumerable<IParser>.GetEnumerator()
        {
            return _parsers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _parsers.Values.GetEnumerator();
        }
    }
}
