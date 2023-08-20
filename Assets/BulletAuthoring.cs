
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour 
{
    private class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Bullet>(entity);
            AddComponent<Movement>(entity);
        }
    } 
}

public partial struct Movement : IComponentData
{
    public float3 Velocity;
}

public partial struct Bullet : IComponentData
{
}