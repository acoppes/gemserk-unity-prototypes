using UnityEngine;

namespace VirtualVillagers.Components
{
    public struct SimulationComponentData
    {
//            public int Length;
        public SimulationTime simulationTime;
    }
    
    public class SimulationTime : MonoBehaviour
    {
        public int totalFrames;
        public int frames;
        public float dt;

        public float fixedDeltaTime;

        public float accumulator;
//        public float maxDeltaTime;
    }
}