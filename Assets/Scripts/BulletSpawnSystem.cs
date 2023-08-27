using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(TransformSystemGroup))] // TransformSystemGroup 이전에 실행하여 총알 위치가 반영 안되는 문제 해결
public partial struct BulletSpawnSystem : ISystem
{
    private Random _random; // 랜덤값 생성기

    // OnCreate는 시스템이 생성될 때 한번만 실행된다.
    public void OnCreate(ref SystemState state)
    {
        _random = new Random(999);
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<BulletSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var aimPoint = new float3(0, 0, 0);
        // 여러개의 플레이어 캐릭터 위치 중 가장 처음 쿼리된 것만 사용
        foreach (var playerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>())
        {
            aimPoint = playerTransform.ValueRO.Position;
            break;
        }

        var bulletSpawner = SystemAPI.GetSingleton<BulletSpawner>();
        if (bulletSpawner.NextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            var spawnCount = _random.NextInt(1, bulletSpawner.MaxConcurrentSpawnCount);

            for (var i = 0; i < spawnCount; i++)
            {
                var bulletEntity = state.EntityManager.Instantiate(bulletSpawner.BulletPrefab);

                var radius = bulletSpawner.SpawnPositionRadius;

                // 방향벡터는 길이가 1 => 반지름이 1인 원 위의 한점과 같다.
                var randomPositionFloat2 = _random.NextFloat2Direction() * radius;
                var randomPosition = new float3(randomPositionFloat2.x, 0, randomPositionFloat2.y);

                var localTransform = LocalTransform.FromPosition(randomPosition);
                localTransform.Scale = 0.2f;

                state.EntityManager.SetComponentData(bulletEntity, localTransform);
                state.EntityManager.SetComponentData(bulletEntity, new Movement
                {
                    Velocity = math.normalizesafe(aimPoint - randomPosition)
                               * _random.NextFloat(bulletSpawner.BulletSpeedMin, bulletSpawner.BulletSpeedMax)
                });

                bulletSpawner.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime
                                              + _random.NextFloat(bulletSpawner.TimeBetSpawnMin,
                                                  bulletSpawner.TimeBetSpawnMax);
            }
        }
    }
}