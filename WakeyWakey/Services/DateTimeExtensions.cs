using System;
namespace WakeyWakey.Services
{
	public static class DateTimeExtensions
	{
        public static int DaysUntil(this DateTime currentDate, DateTime targetDate)
        {
            return (targetDate - currentDate).Days;
        }
    }
}

