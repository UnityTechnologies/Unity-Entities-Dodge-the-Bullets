using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[StructLayout(LayoutKind.Auto)]
public partial struct PlayerSpawnSystem : ISystem
{
    private Random _random;
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<PlayerSpawner>();

        _random = new Random(1);
    }

    public void OnUpdate(ref SystemState state)
    {
        // call only once
        state.Enabled = false;
        
        // get GameState
        var gameState = SystemAPI.GetSingleton<GameState>();
        
        var playerSpawner = SystemAPI.GetSingleton<PlayerSpawner>();
        var spawnCount = playerSpawner.SpawnCount;
        var spawnRadius = playerSpawner.SpawnRadius;
        
        // spawn Player in random position inside radius
        for (var i = 0; i < spawnCount; i++)
        {
            var playerEntity = state.EntityManager.Instantiate(playerSpawner.PlayerPrefab);
            // Get Local Transform of playerEntity
            var localTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
            var randomPosition = _random.NextFloat2Direction() * _random.NextFloat(0, spawnRadius);
            localTransform.Position = new float3(randomPosition.x, 0, randomPosition.y);
            
            state.EntityManager.SetComponentData(playerEntity, localTransform);

            gameState.PlayerCount++;
        }

        gameState.IsGameRunning = true;
        
        // set GameState
        SystemAPI.SetSingleton(gameState);
    }
}
