/*
 * Alberto Molero
 * Spain (Catalonia)
 * Last Revision: 16/10/2014
 */

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NBT.IO.Compression.ZLIB
{
	public enum FLevel
	{
		Faster = 0,
		Fast = 1,
		Default = 2,
		Optimal = 3,
	}
	public sealed class ZLibHeader
	{
		#region "Variables globales"
			private bool mIsSupportedZLibStream;
			private byte mCompressionMethod; //CMF 0-3
			private byte mCompressionInfo; //CMF 4-7
			private byte mFCheck; //Flag 0-4 (Check bits for CMF and FLG)
			private bool mFDict; //Flag 5 (Preset dictionary)
			private FLevel mFLevel; //Flag 6-7 (Compression level)
		#endregion
		#region "Propiedades"
			public bool IsSupportedZLibStream
			{
				get
				{
					return this.mIsSupportedZLibStream;
				}
				set
				{
					this.mIsSupportedZLibStream = value;
				}
			}
			public byte CompressionMethod
			{
				get
				{
					return this.mCompressionMethod;
				}
				set
				{
					if (value > 15)
					{
						Helper.ThrowArgumentOutOfRangeException("Argument cannot be greater than 15");
					}
					this.mCompressionMethod = value;
				}
			}
			public byte CompressionInfo
			{
				get
				{
					return this.mCompressionInfo;
				}
				set
				{
					if (value > 15)
					{
						Helper.ThrowArgumentOutOfRangeException("Argument cannot be greater than 15");
					}
					this.mCompressionInfo = value;
				}
			}
			public byte FCheck
			{
				get
				{
					return this.mFCheck;
				}
				set
				{
					if (value > 31)
					{
						Helper.ThrowArgumentOutOfRangeException("Argument cannot be greater than 31");
					}
					this.mFCheck = value;
				}
			}
			public bool FDict
			{
				get
				{
					return this.mFDict;
				}
				set
				{
					this.mFDict = value;
				}
			}
			public FLevel FLevel
			{
				get
				{
					return this.mFLevel;
				}
				set
				{
					this.mFLevel = value;
				}
			}
		#endregion
		#region "Constructor"
			public ZLibHeader()
			{

			}
		#endregion
		#region "Metodos privados"
			private void RefreshFCheck()
			{
				byte byteFLG = (byte)(((int)FLevel << 1) | (FDict ? 1 : 0));
				this.FCheck = (byte)(31 - (byte)((this.GetCMF() * 256 + byteFLG) % 31));
			}
			private byte GetCMF()
			{
				byte byteCMF = (byte)((CompressionInfo << 4) | CompressionMethod);
				return byteCMF;
			}
			private byte GetFLG()
			{
				byte byteFLG = (byte)(((int)FLevel << 6) | (FDict ? 32 : 0) | FCheck);
				return byteFLG;
			}
		#endregion
		#region "Metodos publicos"
			public byte[] EncodeZlibHeader()
			{
				byte[] result = new byte[2];

				this.RefreshFCheck();

				result[0] = this.GetCMF();
				result[1] = this.GetFLG();

				return result;
			}
		#endregion
		#region "Metodos estáticos"
			public static ZLibHeader DecodeHeader(byte pCMF, byte pFlag)
			{
				ZLibHeader result = new ZLibHeader();

				if ((pCMF < byte.MinValue) || (pCMF > byte.MaxValue))
				{
					Helper.ThrowArgumentOutOfRangeException("Argument 'CMF' must be a byte");
				}
				if ((pFlag < byte.MinValue) || (pFlag > byte.MaxValue))
				{
					Helper.ThrowArgumentOutOfRangeException("Argument 'Flag' must be a byte");
				}

				result.CompressionInfo = (byte)(pCMF >> 4);
				result.CompressionMethod = (byte)(pCMF & 15);

				result.FCheck = (byte)(pFlag & 31);
				result.FDict = ((pFlag >> 5) & 1) == 1;
				result.FLevel = (FLevel)(pFlag >> 6);

				result.IsSupportedZLibStream = (result.CompressionMethod == 8) && (result.CompressionInfo == 7) && (((pCMF * 256 + pFlag) % 31 == 0)) && (result.FDict == false);

				return result;
			}
		#endregion
	}
}
