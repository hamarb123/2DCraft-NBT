using System;

namespace NBT.Exceptions
{
	public sealed class NBT_EndOfStreamException : Exception
	{
		public NBT_EndOfStreamException() : base("")
		{
		}
		public NBT_EndOfStreamException(string message) : base(message)
		{
		}
		public NBT_EndOfStreamException(string message, Exception innerException) : base(message, innerException)
		{
		}
		public static void Throw() => throw new NBT_EndOfStreamException();
	}
}
