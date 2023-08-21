using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerLifeSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.IsGameOver)
        {
            return;
        }
        var playerCount = 0;
        foreach (var (playerTransform, playerEntity)
                 in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>().WithEntityAccess())
        {
            playerCount++;
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

        if (playerCount == 0)
        {
            gameState.IsGameOver = true;
            SystemAPI.SetSingleton(gameState);
        }
    }
}
