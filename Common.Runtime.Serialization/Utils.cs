using System;

namespace Common.Runtime.Serialization
{
	static class Utils
    {
		internal static Type GetArrayBaseType(this Type type)
		{
            if (type == null) throw new ArgumentNullException("type", "Type must not be null");

            if (!type.IsArray) throw new ArgumentException(string.Format("type: {0} is not an array", type.FullName), "type");

            Type baseType = type.GetElementType();

			while (baseType.IsArray)
			{
				baseType = baseType.GetElementType();
			}
			return baseType;
		}
	}
}
