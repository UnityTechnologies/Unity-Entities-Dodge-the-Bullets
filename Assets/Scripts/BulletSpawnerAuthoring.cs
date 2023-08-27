using Unity.Entities;
using UnityEngine;

// Bullet Spawner 게임 오브젝트를 엔티티로 변환하는 스크립트
public class BulletSpawnerAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int maxConcurrentSpawnCount = 5;
    public float timeBetSpawnMin = 0.2f;
    public float timeBetSpawnMax = 3f;
    public float spawnRadius = 20f;
    public float bulletSpeedMin = 2f;
    public float bulletSpeedMax = 8f;
    
    private class Baker : Baker<BulletSpawnerAuthoring> {
        public override void Bake(BulletSpawnerAuthoring authoring)
        {
            // 총알 생성기는 움직일 필요가 없으므로 TransformUsageFlags.None 플래그를 사용
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BulletSpawner
            {
                // 프리팹을 대상으로 GetEntity 사용시 프리팹을 엔티티로 구워준다
                BulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                MaxConcurrentSpawnCount = authoring.maxConcurrentSpawnCount,
                TimeBetSpawnMax = authoring.timeBetSpawnMax,
                TimeBetSpawnMin = authoring.timeBetSpawnMin,
                SpawnPositionRadius = authoring.spawnRadius,
                BulletSpeedMin = authoring.bulletSpeedMin,
                BulletSpeedMax = authoring.bulletSpeedMax,
            });
        }
    }
}

// 총알 스폰에 필요한 정보를 제공하는 컴포넌트
public struct BulletSpawner : IComponentData
{
    public Entity BulletPrefab; // 총알 프리팹
    public int MaxConcurrentSpawnCount; // 한번에 동시에 스폰할 랜덤 갯수의 맥스값
    public float SpawnPositionRadius; // 총알이 스폰될 반지름
    public float TimeBetSpawnMin; // 총알 스폰 간격 최소값
    public float TimeBetSpawnMax; // 총알 스폰 간격 최대값
    public float NextSpawnTime; // 다음 총알 스폰 시간
    public float BulletSpeedMin; // 총알 속도 최소값
    public float BulletSpeedMax; // 총알 속도 최대값
}
