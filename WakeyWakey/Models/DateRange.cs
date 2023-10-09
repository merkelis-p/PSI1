using System;
namespace WakeyWakey.Models
{
	public struct DateRange
	{
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool Contains(DateTime date)
        {
            return StartDate <= date && date <= EndDate;
        }
    }
}

