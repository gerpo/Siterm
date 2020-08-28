using System.Collections.Generic;
using System.Linq;

namespace Siterm.DatabaseInitialization.Services
{
    public class ImporterPipeline
    {
        private readonly IEnumerable<IImporter> _pipelineItems;

        public ImporterPipeline(IEnumerable<IImporter> pipelineItems)
        {
            _pipelineItems = pipelineItems;
        }

        public async void Run()
        {
            if (!_pipelineItems.Any()) return;

            var orderedItems = _pipelineItems.OrderBy(i => i.Order);

            foreach (var pipelineItem in orderedItems) pipelineItem.Execute().Wait();
        }
    }
}