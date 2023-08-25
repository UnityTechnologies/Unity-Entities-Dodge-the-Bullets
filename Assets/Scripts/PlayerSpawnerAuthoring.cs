using Unity.Entities;
using UnityEngine;

public class PlayerSpawnerAuthoring : MonoBehaviour
{
    public GameObject playerPrefab;
    public int playerCountInRow = 10;
    public int playerCountInColumn = 10;
    public float offset = 0.25f;

    private class Baker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerSpawner
            {
                PlayerPrefab = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic),
                PlayerCountInRow = authoring.playerCountInRow,
                PlayerCountInColumn = authoring.playerCountInColumn,
                Offset = authoring.offset,
            });
        }
    }
}

public struct PlayerSpawner : IComponentData
{
    public Entity PlayerPrefab;
    public int PlayerCountInRow;
    public int PlayerCountInColumn;
    public float Offset;
}