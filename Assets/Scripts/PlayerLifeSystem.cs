using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerLifeSystem : ISystem
{
    private const float MinDistanceBetBulletAndPlayer = 0.1f;
    private const float MinDistanceBetBulletAndPlayerSQ = MinDistanceBetBulletAndPlayer * MinDistanceBetBulletAndPlayer; 
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.IsGameOver)
        {
            return;
        }

        var playerCount = gameState.PlayerCount;
        
        foreach (var (playerTransform, playerEntity)
                 in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>().WithEntityAccess())
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var bulletTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Bullet>())
            {
                if (math.distancesq(playerTransform.ValueRO.Position, bulletTransform.ValueRO.Position)
                    < MinDistanceBetBulletAndPlayerSQ)
                {
                    // //SystemAPI.GetSingleton<GameState>().IsGameOver = true;
                    SystemAPI.SetComponentEnabled<Player>(playerEntity, false);
                    
                    // disable player rendering by adding DisableRendering with EntityCommandBuffer
                    ecb.AddComponent<DisableRendering>(playerEntity);
                    playerCount--;
                    break;
                }
            }
        }

        if (playerCount != gameState.PlayerCount)
        {
            gameState.PlayerCount = playerCount;
            SystemAPI.SetSingleton(gameState);
        }
    }
}
