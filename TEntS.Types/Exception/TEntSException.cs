using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types.Exception
{
    public class TEntSException : ApplicationException
    {
        public TEntSException()
            : base()
        { }

        public TEntSException(string message)
            : base(message)
        { }

        public TEntSException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }

    public class TEntSInvalidSession : TEntSException
    {
        public TEntSInvalidSession()
            : base()
        { }

        public TEntSInvalidSession(string message)
            : base(message)
        { }

        public TEntSInvalidSession(string message, System.Exception innerException) :
            base(message, innerException)
        { }
    }

    public class TEntSInvalidUserException : TEntSException
    {
        public TEntSInvalidUserException()
            : base()
        { }

        public TEntSInvalidUserException(string message)
            : base(message)
        { }

        public TEntSInvalidUserException(string message, System.Exception innerException) :
            base(message, innerException)
        { }
    }

    public class TEntSObjectAlreadyExistsException : TEntSException
    {
        public TEntSObjectAlreadyExistsException()
            : base()
        { }

        public TEntSObjectAlreadyExistsException(string message)
            : base(message)
        { }

        public TEntSObjectAlreadyExistsException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }

    public class TEntSObjectNotFound : TEntSException
    {
        public TEntSObjectNotFound()
            : base()
        { }

        public TEntSObjectNotFound(string message)
            : base(message)
        { }

        public TEntSObjectNotFound(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }

    public class TEntSInternalException : TEntSException
    {
        public TEntSInternalException()
            : base()
        { }

        public TEntSInternalException(string message)
            : base(message)
        { }

        public TEntSInternalException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
