using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerLifeSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerTransform, playerEntity)
                 in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>().WithEntityAccess())
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            
            foreach (var bulletTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Bullet>())
            {
                if (math.distance(playerTransform.ValueRO.Position, bulletTransform.ValueRO.Position) < 0.5f)
                {
                    // //SystemAPI.GetSingleton<GameState>().IsGameOver = true;
                    SystemAPI.SetComponentEnabled<Player>(playerEntity, false);
                    
                    // disable player rendering by adding DisableRendering with EntityCommandBuffer
                    ecb.AddComponent<DisableRendering>(playerEntity);
                }
            }
        }
    }
}
