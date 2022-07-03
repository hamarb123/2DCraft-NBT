using System;

namespace NBT.Exceptions
{
	public sealed class NBT_InvalidTagTypeException : Exception
	{
		public NBT_InvalidTagTypeException() : base("")
		{
		}
		public NBT_InvalidTagTypeException(string message) : base(message)
		{
		}
		public NBT_InvalidTagTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}
		public static void Throw() => throw new NBT_InvalidTagTypeException();
		public static void Throw(string message) => throw new NBT_InvalidTagTypeException(message);
	}
}
