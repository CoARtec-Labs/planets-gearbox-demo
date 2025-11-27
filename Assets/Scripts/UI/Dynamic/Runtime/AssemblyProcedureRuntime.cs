using System;
using System.Collections.Generic;
using System.Linq;
using CoARtec.UI.Dynamic.Data;

namespace CoARtec.UI.Dynamic.Runtime
{
    public class AssemblyStep
    {
        public string Id;
        public int Order;
        public string Title;
        public string Description;
        public string[] VisiblePartKeys = Array.Empty<string>();
        public string[] HighlightPartKeys = Array.Empty<string>();
        public int? YoloClassId;
    }

    public class AssemblyProcedureRuntime
    {
        public string Id { get; }
        public string Name { get; }
        public IReadOnlyList<AssemblyStep> Steps { get; }

        public int CurrentIndex { get; private set; }
        public AssemblyStep CurrentStep => Steps[CurrentIndex];

        public bool CanGoNext => CurrentIndex < Steps.Count - 1;
        public bool CanGoBack => CurrentIndex > 0;

        public AssemblyProcedureRuntime(AssemblyProcedureDto dto)
        {
            Id = dto.id;
            Name = dto.name;
            Steps = (dto.steps ?? new List<AssemblyStepDto>())
                .OrderBy(s => s.order)
                .Select(s => new AssemblyStep
                {
                    Id = s.id,
                    Order = s.order,
                    Title = s.title,
                    Description = s.description,
                    VisiblePartKeys = s.visiblePartKeys ?? Array.Empty<string>(),
                    HighlightPartKeys = s.highlightPartKeys ?? Array.Empty<string>(),
                    YoloClassId = s.yoloClassId
                })
                .ToList();
        }

        public void GoNext() { if (CanGoNext) CurrentIndex++; }
        public void GoBack() { if (CanGoBack) CurrentIndex--; }
    }
}
