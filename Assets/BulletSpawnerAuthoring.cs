using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class BulletSpawnerAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int maxConcurrentSpawnCount = 5;
    public float timeBetSpawnMin = 0.2f;
    public float timeBetSpawnMax = 3f;
    public float spawnRadius = 10f;
    
    private class Baker : Baker<BulletSpawnerAuthoring> {
        public override void Bake(BulletSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BulletSpawner
            {
                BulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                MaxConcurrentSpawnCount = authoring.maxConcurrentSpawnCount,
                TimeBetSpawnMax = authoring.timeBetSpawnMax,
                TimeBetSpawnMin = authoring.timeBetSpawnMin,
                SpawnPositionRadius = authoring.spawnRadius,
            });
        }
    }
}

public partial struct BulletSpawner : IComponentData
{
    public Entity BulletPrefab;
    // 한번에 동시에 스폰할 랜덤 갯수의 맥스값
    public int MaxConcurrentSpawnCount;
    public float SpawnPositionRadius;
    public float TimeBetSpawnMin;
    public float TimeBetSpawnMax;
    public float NextSpawnTime;
}
