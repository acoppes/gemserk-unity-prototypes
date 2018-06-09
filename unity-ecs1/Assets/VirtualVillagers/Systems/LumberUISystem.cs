using Unity.Entities;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class LumberUISystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<LumberMillUIComponent> lumberUI;
        }
        
        private struct LumberMillData
        {
            public int Length;
            public ComponentArray<LumberMillComponent> lumberMill;
            public ComponentArray<LumberComponent> lumber;
        }
        
        [Inject] private Data _data;
        [Inject] private LumberMillData _lumberMillData;
        
        protected override void OnUpdate()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                var lumberUI = _data.lumberUI[i];
                lumberUI.currentLumber = 0;
                
                for (var j = 0; j < _lumberMillData.Length; j++)
                {
                    lumberUI.currentLumber += _lumberMillData.lumber[j].current;
                }
            }
        }
    }
}