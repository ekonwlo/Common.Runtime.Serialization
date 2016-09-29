using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common.Runtime.Serialization.Transformation
{
    public sealed class TransofmationSelector
    {
        public delegate Type DynamicExpression<T>(T content);

        private readonly Delegate _selector;
        private readonly Type[] _types;

        internal Type this [object item]
        {
            get 
            {

                Type type = (Type) _selector.DynamicInvoke(item);

                return type;
            }
        }

        internal Type[] Types
        {
            get { return _types; }
        }
        
        internal TransofmationSelector(Delegate selector, Type[] types)
        {
            _selector = selector;
            _types = types;
        }

    }
}
