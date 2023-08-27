using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(BulletSpawnSystem))]
public partial struct BulletVelocitySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var otherBulletQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, Bullet>().Build();
        var otherBulletTransforms
            = otherBulletQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);
        var otherBulletEntities 
            = otherBulletQuery.ToEntityArray(state.WorldUpdateAllocator);
        
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        var job = new BulletVelocityJob
        {
            OtherBulletTransforms = otherBulletTransforms,
            OtherBulletEntities = otherBulletEntities,
            DeltaTime = deltaTime,
        };

        job.ScheduleParallel();
    }
}

[WithAll(typeof(Bullet))]
[BurstCompile]
public partial struct BulletVelocityJob : IJobEntity
{
    // 총알이 서로 충돌한 것으로 판정하는데 사용할 거리
    private const float ReflectDistanceBetBullet = 0.1f;
    private const float ReflectDistanceBetBulletSQ = ReflectDistanceBetBullet * ReflectDistanceBetBullet;

    [ReadOnly] public NativeArray<LocalTransform> OtherBulletTransforms;
    [ReadOnly] public NativeArray<Entity> OtherBulletEntities;
    public float DeltaTime;
    
    public void Execute(Entity bulletEntity, ref LocalTransform bulletTransform, ref Movement bulletMovement)
    {
        var velocity = bulletMovement.Velocity;
        var newPosition = bulletTransform.Position + velocity * DeltaTime;

        for (var i = 0; i < OtherBulletEntities.Length; i++)
        {
            var otherEntityIndex = OtherBulletEntities[i].Index;
            var otherBulletTransform = OtherBulletTransforms[i];
            if (bulletEntity.Index == otherEntityIndex)
            {
                continue;
            }
            
            if (math.distancesq(newPosition, otherBulletTransform.Position) >= ReflectDistanceBetBulletSQ)
            {
                continue;
            }
            
            // 만약 목표 위치로 이동시 충돌할것으로 보인다면
            var collisionSurfaceNormal = math.normalize(otherBulletTransform.Position - newPosition);
            // 반사
            velocity = math.reflect(velocity, collisionSurfaceNormal);
            newPosition = bulletTransform.Position + velocity * DeltaTime;
                
            // 새로운 속도 적용
            bulletMovement.Velocity = velocity;
            break;
        }
        
        bulletTransform.Position = newPosition;
    }
}
