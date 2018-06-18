using Unity.Entities;

namespace VirtualVillagers.Components
{
    public struct SimulationTime : IComponentData
    {
        public int totalFrames;
        public int frames;
        public float dt;

        public float fixedDeltaTime;

        public float accumulator;
//        public float maxDeltaTime;
    }

    public static class SimulationTimeSingleton
    {
        private static Entity _simulationTimeInstance;
        
        public static SimulationTime GetInstance()
        {
            var entityManager = World.Active.GetExistingManager<EntityManager>();
            
            if (!entityManager.Exists(_simulationTimeInstance))
            {
                _simulationTimeInstance = entityManager.CreateEntity();
                entityManager.AddComponentData(_simulationTimeInstance, new SimulationTime()
                {
                    fixedDeltaTime = 0.01f
                });
            }
            
            return entityManager.GetComponentData<SimulationTime>(_simulationTimeInstance);
        }
    }
}