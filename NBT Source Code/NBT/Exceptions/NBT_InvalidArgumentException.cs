using System;

namespace NBT.Exceptions
{
	public sealed class NBT_InvalidArgumentException : Exception
	{
		public NBT_InvalidArgumentException() : base("")
		{
		}
		public NBT_InvalidArgumentException(string message) : base(message)
		{
		}
		public NBT_InvalidArgumentException(string message, Exception innerException) : base(message, innerException)
		{
		}
		public static void ThrowInvalidType(string typeName) => throw new NBT_InvalidArgumentException($"The parameter must be a { typeName }");
		public static void Throw() => throw new NBT_InvalidArgumentException();
		public static void Throw(string message) => throw new NBT_InvalidArgumentException(message);
	}
}
