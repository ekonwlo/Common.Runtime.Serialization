using System;
using System.Xml;
using System.Xml.Linq;

namespace Common.Runtime.Serialization.Xml.Parsers
{
    using Common.Runtime.Serialization.Parsers;

    internal class XStringParser : StringParser<XObject>
    {
        public override XObject ParseFrom(string input)
        {
            if (input == null) throw new ArgumentNullException("input", "Input is required");
                        
            try
            {                
                return XElement.Parse(string.Format("<root>{0}</root>", input), LoadOptions.PreserveWhitespace).FirstNode;
            }
            catch (XmlException ex)
            {
                throw new SerializationException("Cannot parse input string", ex);
            }
        }

        public override string ParseTo(XObject input)
        {
            if (input == null) throw new ArgumentNullException("input", "Input is required");

            return input.ToString();
        }
    }
}
