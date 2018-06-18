using Unity.Entities;

namespace VirtualVillagers.Components
{
    public struct SimulationTimeComponent : IComponentData
    {
        public int totalFrames;
        public int frames;
        public float dt;

        public float fixedDeltaTime;

        public float accumulator;
//        public float maxDeltaTime;
    }

    public static class SimulationTime
    {
        private static Entity _simulationTimeInstance;
        
        public static SimulationTimeComponent GetTime()
        {
            var entityManager = World.Active.GetExistingManager<EntityManager>();
            
            if (!entityManager.Exists(_simulationTimeInstance))
            {
                _simulationTimeInstance = entityManager.CreateEntity();
                entityManager.AddComponentData(_simulationTimeInstance, new SimulationTimeComponent()
                {
                    fixedDeltaTime = 0.01f
                });
            }
            
            return entityManager.GetComponentData<SimulationTimeComponent>(_simulationTimeInstance);
        }
    }
}