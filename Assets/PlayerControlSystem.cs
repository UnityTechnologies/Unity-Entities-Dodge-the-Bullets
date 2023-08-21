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
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        new PlayerControlJob
        {
            HorizontalInput = horizontalInput,
            VerticalInput = verticalInput,
        }.ScheduleParallel();
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