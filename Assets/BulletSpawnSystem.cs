using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct BulletSpawnSystem : ISystem
{
    private Random random;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletSpawner>();
        random = new Random(123);
    }

    public void OnUpdate(ref SystemState state)
    {
        float3 aimPoint = new float3(0, 0, 0);
        // 여러개의 플레이어 캐릭터 위치 중 가장 처음 쿼리된 것만 사용
        foreach (var playerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>())
        {
            aimPoint = playerTransform.ValueRO.Position;
            break;
        }
        
        var bulletSpawner = SystemAPI.GetSingleton<BulletSpawner>();
        if (bulletSpawner.NextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            var spawnCount = random.NextInt(1, bulletSpawner.MaxConcurrentSpawnCount);

            for (var i = 0; i < spawnCount; i++)
            {
                var bulletEntity = state.EntityManager.Instantiate(bulletSpawner.BulletPrefab);

                var radius = bulletSpawner.SpawnPositionRadius;

                // 방향벡터는 길이가 1 => 반지름이 1인 원 위의 한점과 같다.
                var randomPositionFloat2 = random.NextFloat2Direction() * radius;
                var randomPosition = new float3(randomPositionFloat2.x, 0, randomPositionFloat2.y);

                var localTransform = state.EntityManager.GetComponentData<LocalTransform>(bulletEntity);
                localTransform.Position = randomPosition;

                state.EntityManager.SetComponentData(bulletEntity, localTransform);
                state.EntityManager.SetComponentData(bulletEntity, new Movement
                {
                    Velocity = math.normalizesafe(aimPoint - randomPosition) * 5f
                });

                bulletSpawner.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime
                                              + random.NextFloat(bulletSpawner.TimeBetSpawnMin,
                                                  bulletSpawner.TimeBetSpawnMax);
            }
        }
    }
}