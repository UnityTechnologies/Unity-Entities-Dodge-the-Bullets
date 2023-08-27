using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerVelocitySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Non Job Version
        // foreach (var (playerMovement, playerTransform)
        //          in SystemAPI.Query<RefRO<Movement>, RefRW<LocalTransform>>().WithAll<Player>())
        // {
        //     var movement = playerMovement.ValueRO.Velocity * SystemAPI.Time.DeltaTime;
        //     var newPosition = playerTransform.ValueRO.Position + movement;
        //     playerTransform.ValueRW.Position = newPosition;
        // }

        // Job Version
        var deltaTime = SystemAPI.Time.DeltaTime;
        new PlayerVelocityJob
        {
            DeltaTime = deltaTime,
        }.ScheduleParallel();
    }

}

[WithAll(typeof(Player))]
[BurstCompile]
public partial struct PlayerVelocityJob : IJobEntity
{
    public float DeltaTime;
    public void Execute(Movement playerMovement, ref LocalTransform playerTransform)
    {
        var movement = playerMovement.Velocity * DeltaTime;
        var newPosition = playerTransform.Position + movement;

        playerTransform.Position = newPosition;
    }
}