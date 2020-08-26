using System;
using System.Collections.Generic;
using System.Linq;

namespace Siterm.Domain.Models
{
    public class ServiceReport : DomainObject
    {
        public enum ValidityType
        {
            Weekly = 7,
            Biweekly = 14,
            Monthly = 30,
            QuarterYearly = 91,
            HalfYearly = 182,
            Yearly = 365,
            Biyearly = 730
        }

        public static Dictionary<ValidityType, string> ValidityStringMap => new Dictionary<ValidityType, string>
        {
            {ValidityType.Weekly, "w"},
            {ValidityType.Biweekly, "zw"},
            {ValidityType.Monthly, "m"},
            {ValidityType.QuarterYearly, "vj"},
            {ValidityType.HalfYearly, "hj"},
            {ValidityType.Yearly, "j"},
            {ValidityType.Biyearly, "zj"},
        };

        public static Dictionary<string, ValidityType> StringValidityMap =>
            ValidityStringMap.ToDictionary(m => m.Value, m => m.Key);

        public string Path { get; set; }
        public virtual User PerformingUser { get; set; }
        public virtual Device Device { get; set; }
        public DateTime CreatedAt { get; set; }
        public ValidityType Validity { get; set; }
        public virtual IEnumerable<ServiceTask> ServiceTasks { get; set; }
        public DateTime ValidTill => CreatedAt.AddDays((int)Validity);
        public bool HasWarning => ValidTill < DateTime.Today.AddDays(-14);
        public bool IsInvalid => ValidTill <= DateTime.Today;
    }
}