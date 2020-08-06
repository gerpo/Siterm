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
        public User PerformingUser { get; set; }
        public Device Device { get; set; }
        public DateTime CreatedAt { get; set; }
        public ValidityType Validity { get; set; }
        public IEnumerable<ServiceTask> ServiceTasks { get; set; }
    }
}