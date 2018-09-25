using System;

namespace SpchListBuilder
{
#pragma warning disable 1591
    [Serializable]
    class VCSDataException : Exception
    {
        public VCSDataException() { }
        public VCSDataException(string message) : base(message) { }
        public VCSDataException(string message, Exception inner) : base(message, inner) { }
        protected VCSDataException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
