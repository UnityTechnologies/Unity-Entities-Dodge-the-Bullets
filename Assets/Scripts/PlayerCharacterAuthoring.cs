using Unity.Entities;
using UnityEngine;

public class PlayerCharacterAuthoring : MonoBehaviour
{
    public float speed = 3f;
    private class Baker : Baker<PlayerCharacterAuthoring>
    {
        public override void Bake(PlayerCharacterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Player { Speed = authoring.speed });
            AddComponent<Movement>(entity);
        }
    }
}

public struct Player : IEnableableComponent, IComponentData
{
    public float Speed;
}