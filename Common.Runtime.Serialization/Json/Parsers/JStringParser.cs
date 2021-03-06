﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.Json.Parsers
{
    using Common.Runtime.Serialization.Parsers;

    internal class JStringParser : StringParser<JToken>
    {
        public override JToken ParseFrom(string input)
        {
            if (input == null) throw new ArgumentNullException("input", "Input is required");

            try
            {
                return JToken.Parse(input);
            } 
            catch (JsonException ex)
            {
                throw new SerializationException("Cannot serialize input string", ex);
            }
        }

        public override string ParseTo(JToken input)
        {
            if (input == null) throw new ArgumentNullException("input", "Input is required");
            
            return input.ToString(Formatting.None);
        }
    }
}
