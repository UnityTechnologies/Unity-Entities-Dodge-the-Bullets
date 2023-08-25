using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[StructLayout(LayoutKind.Auto)]
public partial struct PlayerSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<PlayerSpawner>();
    }

    public void OnUpdate(ref SystemState state)
    {
        // call only once
        state.Enabled = false;
        
        // get GameState
        var gameState = SystemAPI.GetSingleton<GameState>();
        
        var playerSpawner = SystemAPI.GetSingleton<PlayerSpawner>();
        var rowCount = playerSpawner.PlayerCountInRow;
        var colCount = playerSpawner.PlayerCountInColumn;
        var offset = playerSpawner.Offset;

        var startPosition = new float3(0, 0, 0);
        
        // spawn Players
        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < colCount; j++)
            {
                var playerEntity = state.EntityManager.Instantiate(playerSpawner.PlayerPrefab);
                // Get Local Transform of playerEntity
                var localTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
                localTransform.Position = startPosition + new float3(i * offset, 0, j * offset);
                state.EntityManager.SetComponentData(playerEntity, localTransform);
                gameState.PlayerCount++;
            }
        }
        gameState.IsGameRunning = true;
        
        // set GameState
        SystemAPI.SetSingleton(gameState);
    }
}
