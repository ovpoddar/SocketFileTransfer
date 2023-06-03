using System;
using System.Text;

namespace SocketFileTransfer.Model;
public struct ProgressReport
{
	public decimal Percentage { get; }
	public decimal Total { get; }
	public decimal Complete { get; }
	public string TargetedItemName { get; set; }
	public ProgressReport(decimal total, decimal complete, byte[] fileHash)
	{
		Total = total;
		Complete = complete;
		Percentage = Math.Round(decimal.Multiply(decimal.Divide(100, total), complete), 2, MidpointRounding.ToPositiveInfinity);

		var hashName = Encoding.ASCII.GetString(fileHash);
		TargetedItemName = hashName;
	}
}
