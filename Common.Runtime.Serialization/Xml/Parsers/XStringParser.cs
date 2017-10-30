using System;
using System.Xml.Linq;

namespace Common.Runtime.Serialization.Xml.Parsers
{
    using Common.Runtime.Serialization.Parsers;

    internal class XStringParser : StringParser<XObject>
    {
        public override XObject ParseFrom(string input)
        {
            if (input == null) throw new ArgumentNullException("input", "Input is required");

            return XElement.Parse(input);
        }

        public override string ParseTo(XObject input)
        {
            if (input == null) throw new ArgumentNullException("input", "Input is required");

            return input.ToString();
        }
    }
}
