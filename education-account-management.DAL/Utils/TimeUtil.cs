namespace Utils
{
    public static class TimeUtil
    {
        public static DateTime? TryConvertToDateTime(object? value)
        {
            if (value is DateTime dateTime) return dateTime.Date;
            if (value is DateOnly dateOnly) return dateOnly.ToDateTime(TimeOnly.MinValue);
            return null;
        }

        public static TimeOnly? TryConvertToTimeOnly(object? value)
        {
            if (value is TimeOnly timeOnly) return timeOnly;
            if (value is DateTime dateTime) return TimeOnly.FromDateTime(dateTime);
            if (value is TimeSpan timeSpan && timeSpan >= TimeSpan.Zero && timeSpan < TimeSpan.FromDays(1))
                return TimeOnly.FromTimeSpan(timeSpan);
            return null;
        }

        public static int? TryCalculateAge(object? value)
        {
            DateOnly? dateOfBirth = value switch
            {
                DateOnly dob => dob,
                DateTime dob => DateOnly.FromDateTime(dob),
                _ => null
            };

            if (dateOfBirth is null)
                return null;

            var today = DateOnly.FromDateTime(DateTime.Today);

            int age = today.Year - dateOfBirth.Value.Year;

            if (today < dateOfBirth.Value.AddYears(age))
                age--;

            return age;
        }

    }
}
