using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common.Runtime.Serialization.Transformation
{
    public class Transformator
    {
        
        private object _object;
        private MethodInfo _transfomMethod;
        private MethodInfo _revertMethod;

        internal Type InputType
        {
            get { return _revertMethod.ReturnType; }
        }

        internal Type OutputType
        {
            get { return _transfomMethod.ReturnType; }
        }

        internal Transformator(Type type, MethodInfo transformMethod, MethodInfo revertMethod)
        {
            _object = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            _transfomMethod = transformMethod;
            _revertMethod = revertMethod;
            
        }

        internal object Transform(object item)
        {
            return _transfomMethod.Invoke(_object, new object[] {item});
        }

        internal object Revert(object item)
        {
            return _revertMethod.Invoke(_object, new object[] { item });
        }

    }
}
