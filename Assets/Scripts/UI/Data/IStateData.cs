using UnityEngine;

namespace UI.Data
{
    public interface IStateData
    {
        private static int _searchObjectClassId = -1;

        public static int SearchObjectClassId
        {
            get => _searchObjectClassId;
            set => _searchObjectClassId = value;
        }
    }
}