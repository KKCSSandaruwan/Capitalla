using System;
using System.ComponentModel.DataAnnotations;
namespace QuickAccounting.Data.HrPayroll
{
	public class SalaryPackageDetails
	{
		[Key]
		public int SalaryPackageDetailsId { get; set; }
		[Required]
		public int SalaryPackageId { get; set; }
		public int PayHeadId { get; set; }
		public decimal Amount { get; set; }
		public string Narration { get; set; }
	}
}
