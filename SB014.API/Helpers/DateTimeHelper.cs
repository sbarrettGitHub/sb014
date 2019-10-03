using System;

namespace SB014.API.Helpers
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public virtual DateTime CurrentDateTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}