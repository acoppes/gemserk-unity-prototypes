using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class SimulationSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentDataArray<SimulationTime> _simulation;
        }

        [Inject] private Data _data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < _data.Length; i++)
            {
                var simulation = _data._simulation[i];

                if (simulation.fixedDeltaTime < 0.001f)
                    continue;

                simulation.accumulator += dt;
                simulation.dt = 0;
                simulation.frames = 0;

                while (simulation.accumulator > simulation.fixedDeltaTime)
                {
                    simulation.totalFrames++;
                    simulation.frames++;
                    simulation.accumulator -= simulation.fixedDeltaTime;
                    simulation.dt += simulation.fixedDeltaTime;
                }
                
                _data._simulation[i] = simulation;
            }
        }
    }
}