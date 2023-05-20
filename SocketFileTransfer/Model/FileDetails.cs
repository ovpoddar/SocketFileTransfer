using SocketFileTransfer.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketFileTransfer.Model;

internal class FileDetails
{
	private FileDetails()
	{

	}
	public FileDetails(string context)
	{
		var fileInfo = new FileInfo(context);
		Name = fileInfo.Name;
		Size = fileInfo.Length;
		Type = fileInfo.Extension;
		ChunkSize = (int)Math.Ceiling((double)fileInfo.Length / ProjectStandardUtilitiesHelper.ChunkSize);
	}
	public string Name { get; set; }
	public long Size { get; set; }
	public int ChunkSize { get; set; }
	public string Type { get; set; }

	public static explicit operator byte[](FileDetails fileDetails)
	{
		var name = Encoding.ASCII.GetBytes(fileDetails.Name);
		var nameLength = name.Length;
		var type = Encoding.ASCII.GetBytes(fileDetails.Type);
		var typeLength = type.Length;
		var size = Marshal.SizeOf(fileDetails.Size)
			+ Marshal.SizeOf(fileDetails.ChunkSize)
			+ nameLength + Marshal.SizeOf(nameLength)
			+ typeLength + Marshal.SizeOf(typeLength);
		var result = new byte[size];
		var index = 0;
		Unsafe.WriteUnaligned(ref result[index], nameLength);
		index += Marshal.SizeOf(nameLength) - 1;
		Array.Copy(name, 0, result, index, nameLength);
		index += nameLength;
		Unsafe.WriteUnaligned(ref result[index], fileDetails.Size);
		index += Marshal.SizeOf(fileDetails.Size);
		Unsafe.WriteUnaligned(ref result[index], fileDetails.ChunkSize);
		index += Marshal.SizeOf(fileDetails.ChunkSize);
		Unsafe.WriteUnaligned(ref result[index], typeLength);
		index += Marshal.SizeOf(typeLength);
		Array.Copy(type, 0, result, index, typeLength);
		return result;
	}

	public static explicit operator FileDetails(byte[] fileDetails)
	{
		var result = new FileDetails();
		var index = 0;
		var nameSize = Unsafe.ReadUnaligned<int>(ref fileDetails[index]);
		index += sizeof(int) - 1;
		result.Name = Encoding.ASCII.GetString(fileDetails, index, nameSize);
		index += nameSize;
		result.Size = Unsafe.ReadUnaligned<long>(ref fileDetails[index]);
		index += Marshal.SizeOf(result.Size);
		result.ChunkSize = Unsafe.ReadUnaligned<int>(ref fileDetails[index]);
		index += Marshal.SizeOf(result.ChunkSize);
		var typeSize = Unsafe.ReadUnaligned<int>(ref fileDetails[index]);
		index += sizeof(int);
		result.Type = Encoding.ASCII.GetString(fileDetails, index, typeSize);
		return result;
	}
}
