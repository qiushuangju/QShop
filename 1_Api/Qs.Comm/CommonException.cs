
using System;

namespace Qs.Comm
{
    public class CommonException : Exception
    {
        private int _code;

        public CommonException(string message, int code)
            : base(message)
        {
            this._code = code;
        }

        public int Code
        {
            get { return _code; }
        }

    }
}
