
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(VelocitySystem))]
public partial struct BulletDestroySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        // An EntityCommandBuffer created from EntityCommandBufferSystem.Singleton will be
        // played back and disposed by the EntityCommandBufferSystem when it next updates.
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        // WithAll() includes RotationSpeed in the query, but
        // the RotationSpeed component values will not be accessed.
        // WithEntityAccess() includes the Entity ID as the last element of the tuple.
        foreach (var (transform, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<Bullet>()
                     .WithEntityAccess())
        {
            if (math.abs(transform.ValueRO.Position.x) > 20 || math.abs(transform.ValueRO.Position.z) > 20 )
            {
                // Making a structural change would invalidate the query we are iterating through,
                // so instead we record a command to destroy the entity later.
                ecb.DestroyEntity(entity);
            }
        }
    }
}