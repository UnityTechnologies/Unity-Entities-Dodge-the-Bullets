using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(BulletSpawnSystem))]
public partial struct BulletVelocitySystem : ISystem
{
    private const float BulletRadius = 0.2f;
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (bulletTransform, bulletMovement, entity)
                 in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Movement>>().WithAll<Bullet>().WithEntityAccess())
        {
            var movement = bulletMovement.ValueRO.Velocity * SystemAPI.Time.DeltaTime;
            var newPosition = bulletTransform.ValueRO.Position + movement;

            foreach (var (otherBulletTransform, otherBulletMovement, otherEntity)
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Movement>>().WithAll<Bullet>().WithEntityAccess())
            {
                if (entity.Index == otherEntity.Index)
                {
                    continue;
                }
                
                // reflect if collision
                if (math.distance(newPosition, otherBulletTransform.ValueRO.Position) < BulletRadius)
                {
                    bulletMovement.ValueRW.Velocity = math.reflect(bulletMovement.ValueRO.Velocity,
                        math.normalize(otherBulletMovement.ValueRO.Velocity));
                    
                    newPosition = (BulletRadius + movement - math.distance(bulletTransform.ValueRO.Position,
                            otherBulletTransform.ValueRO.Position) ) 
                        * math.normalize(bulletMovement.ValueRO.Velocity) + otherBulletTransform.ValueRO.Position;
                    break;
                }
            }
            bulletTransform.ValueRW.Position = newPosition;
        }
    }
}
