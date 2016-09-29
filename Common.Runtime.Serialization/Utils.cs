using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Runtime.Serialization
{
	static class Utils
	{
		internal static Type GetBaseArrayType(Type type)
		{
			if (!type.IsArray)
			{
				
			}
		
			Type baseType = type.GetElementType();

			while (baseType.IsArray)
			{
				baseType = baseType.GetElementType();
			}
			return baseType;
		}
	}
}
