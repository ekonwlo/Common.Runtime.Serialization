using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Runtime.Serialization.Transformation
{
    public interface IRestTransformation<T, U>
    {

        U Transform(T input);
        T Revert(U input);

    }
}
