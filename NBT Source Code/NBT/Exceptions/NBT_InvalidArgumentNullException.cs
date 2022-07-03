using System;
using System.Runtime.CompilerServices;

namespace NBT.Exceptions
{
	public sealed class NBT_InvalidArgumentNullException : Exception
	{
		public NBT_InvalidArgumentNullException() : base("")
		{
		}
		public NBT_InvalidArgumentNullException(string message) : base(message)
		{
		}
		public NBT_InvalidArgumentNullException(string message, Exception innerException) : base(message, innerException)
		{
		}
		public static void ThrowIfNull<T>(T value, [CallerArgumentExpression("value")] string valueExpression = null)
		{
			if (value is null) throw new NBT_InvalidArgumentNullException("'" + valueExpression + "' was null.");
		}
	}
}
