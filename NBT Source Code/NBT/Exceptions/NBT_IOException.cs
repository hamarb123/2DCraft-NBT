using System;

namespace NBT.Exceptions
{
	public sealed class NBT_IOException : Exception
	{
		public NBT_IOException() : base("")
		{
		}

		public NBT_IOException(string message) : base(message)
		{
		}

		public NBT_IOException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public static void Throw() => throw new NBT_IOException();
		public static void Throw(string message) => throw new NBT_IOException(message);
		public static void Throw(string message, Exception innerException) => throw new NBT_IOException(message, innerException);
	}
}
