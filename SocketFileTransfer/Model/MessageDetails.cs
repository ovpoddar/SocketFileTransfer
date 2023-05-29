using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Model;
internal class MessageDetails
{
	private MessageDetails()
	{

	}

	public MessageDetails(string content)
	{
		EncodingCodePage = Encoding.Unicode.CodePage;
		Length = content.Length;
	}
	public int EncodingCodePage { get; set; }
	public int Length { get; set; }

	public static explicit operator byte[](MessageDetails messageDetails)
	{
		var size = Marshal.SizeOf(messageDetails.EncodingCodePage)
			+ Marshal.SizeOf(messageDetails.Length);
		var result = new byte[size];
		Unsafe.WriteUnaligned(ref result[0], messageDetails.EncodingCodePage);
		Unsafe.WriteUnaligned(ref result[4], messageDetails.Length);
		return result;
	}

	public static explicit operator MessageDetails(byte[] messageDetails)
	{
		var result = new MessageDetails();
		result.Length = Unsafe.ReadUnaligned<int>(ref messageDetails[0]);
		result.EncodingCodePage = Unsafe.ReadUnaligned<int>(ref messageDetails[4]);
		return result;
	}
}
