using System;

namespace SocketFileTransfer.Model;
internal struct ProgressReport
{
	public decimal Percentage { get; }
	public decimal Total { get; set; }
	public decimal Complete { get; set; }
	public ProgressReport(decimal total, decimal complete)
	{
		Total = total;
		Complete = complete;
		Percentage = Math.Round(decimal.Multiply(decimal.Divide(100, total), complete), 2, MidpointRounding.ToPositiveInfinity);
	}
}
