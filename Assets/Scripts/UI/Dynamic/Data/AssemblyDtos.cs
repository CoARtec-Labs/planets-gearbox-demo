using System;
using System.Collections.Generic;

namespace CoARtec.UI.Dynamic.Data
{
    [Serializable]
    public class AssemblyStepDto
    {
        // JSON-Keys from API
        public string id;
        public int order;
        public string title;
        public string description;
        public string[] visiblePartKeys;
        public string[] highlightPartKeys;
        public int? yoloClassId;
    }

    [Serializable]
    public class AssemblyProcedureDto
    {
        public string id; //procedureId
        public string name;
        public List<AssemblyStepDto> steps;
    }
}
