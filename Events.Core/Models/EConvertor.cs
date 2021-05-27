using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models
{
    public interface EConvertor<N,O> where N : Model
    {
        O Convert(N entity);
        N Reverse(O entity);
    }
}
