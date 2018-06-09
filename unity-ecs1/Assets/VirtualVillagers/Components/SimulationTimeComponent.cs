using UnityEngine;

namespace VirtualVillagers.Components
{
    public struct SimulationComponentData
    {
//            public int Length;
        public SimulationTimeComponent simulationTime;
    }
    
    public class SimulationTimeComponent : MonoBehaviour
    {
        public int totalFrames;
        public int frames;
        public float dt;

        public float fixedDeltaTime;

        public float accumulator;
//        public float maxDeltaTime;
    }
}