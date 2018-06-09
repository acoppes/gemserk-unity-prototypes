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
            public ComponentArray<SimulationTimeComponent> _simulation;
        }

        [Inject] private Data _data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < _data.Length; i++)
            {
                var simulation = _data._simulation[i];
                
                simulation.accumulator += dt;
                simulation.dt = 0;
                simulation.frames = 0;

                if (simulation.fixedDeltaTime <= 0.001f)
                    continue;
                
                while (simulation.accumulator > simulation.fixedDeltaTime)
                {
                    simulation.totalFrames++;
                    simulation.frames++;
                    simulation.accumulator -= simulation.fixedDeltaTime;
                    simulation.dt += simulation.fixedDeltaTime;
                }
            }
        }
    }
}