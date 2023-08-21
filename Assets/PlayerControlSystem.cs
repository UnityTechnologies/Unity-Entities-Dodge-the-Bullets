using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial struct PlayerControlSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var movementVelocity = new float3(horizontalInput, 0, verticalInput);

        foreach (var (movement, player) in SystemAPI.Query<RefRW<Movement>, RefRO<Player>>()) 
        {
            movement.ValueRW.Velocity = movementVelocity * player.ValueRO.Speed;
        }
    }
}