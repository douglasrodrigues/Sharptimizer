namespace Sharptimizer.Utils
{
    using System;

    /// <summary>
    /// A generic Error class derived from Exception.
    /// </summary>
    class Error : Exception
    {
        /// <summary>
        /// Initialization method.
        /// </summary>
        /// <param name="cls">Class identifier.</param>
        /// <param name="msg">Message to be logged.</param>
        public Error(string cls, string msg)
        : base(String.Format("{0}: {1}", cls, msg))
        {

        }
    }

    /// <summary>
    /// A SizeError class for logging errors related to wrong length or size of variables.
    /// </summary>
    class SizeError : Error
    {
        /// <summary>
        /// Initialization method.
        /// </summary>
        /// <param name="error">Error message to be logged.</param>
        public SizeError(string error)
        : base("SizeError", error)
        {

        }
    }
}