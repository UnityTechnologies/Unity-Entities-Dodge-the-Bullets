using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSpawnerAuthoring : MonoBehaviour
{
    public GameObject playerPrefab;
    public float spawnRadius = 2f;
    public int spawnCount = 100;

    private class Baker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerSpawner
            {
                PlayerPrefab = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic),
                SpawnRadius = authoring.spawnRadius,
                SpawnCount = authoring.spawnCount,
            });
        }
    }
}

public struct PlayerSpawner : IComponentData
{
    public Entity PlayerPrefab;
    public float SpawnRadius;
    public int SpawnCount;
}