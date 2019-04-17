using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.BLL.Session
{
    internal class TEntSComparer : IEqualityComparer
    {
        public CaseInsensitiveComparer _comparer;

        public TEntSComparer()
        {
            _comparer = CaseInsensitiveComparer.Default;
        }

        public new bool Equals(object x, object y)
        {
            return _comparer.Compare(x, y) == 0 ? true : false;
        }

        public int GetHashCode(object obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}
