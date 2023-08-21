using Unity.Entities;
using Unity.Transforms;

public partial struct PlayerVelocitySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerMovement, playerTransform) 
                 in SystemAPI.Query<RefRO<Movement>, RefRW<LocalTransform>>().WithAll<Player>()) 
        {
            var movement = playerMovement.ValueRO.Velocity * SystemAPI.Time.DeltaTime;
            var newPosition = playerTransform.ValueRO.Position + movement;

            playerTransform.ValueRW.Position = newPosition;
        }
    }
}