using System.Collections.Generic;
using Siterm.Domain.Models;
using Siterm.Support.Misc;

namespace Siterm.ServiceReports.Models
{
    public class ServiceReportTemplate : CanNotifyPropertyChanged
    {
        private string _path;

        private IEnumerable<ServiceTask> _serviceTasks;
        private ServiceReport.ValidityType _validity;

        public ServiceReport.ValidityType Validity
        {
            get => _validity;
            set => SetField(ref _validity, value);
        }

        public IEnumerable<ServiceTask> ServiceTasks
        {
            get => _serviceTasks;
            set => SetField(ref _serviceTasks, value);
        }

        public string Path
        {
            get => _path;
            set => SetField(ref _path, value);
        }
    }
}