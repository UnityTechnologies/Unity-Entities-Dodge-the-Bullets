using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public partial struct PlayerControlSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        // job version
        new PlayerControlJob
        {
            HorizontalInput = horizontalInput,
            VerticalInput = verticalInput,
        }.ScheduleParallel();
        
        // non job version
        // foreach (var (movement, player) in SystemAPI.Query<RefRW<Movement>, RefRO<Player>>().WithAll<Player>())
        // {
        //     movement.ValueRW.Velocity = new float3(horizontalInput, 0, verticalInput) * player.ValueRO.Speed;
        // }
    }
}

[BurstCompile]
public partial struct PlayerControlJob : IJobEntity
{
    public float HorizontalInput;
    public float VerticalInput;
    
    void Execute(ref Movement movement, [ReadOnly] ref Player player)
    {
        movement.Velocity = new float3(HorizontalInput, 0, VerticalInput) * player.Speed;
    }
}