using Unity.Entities;
using UnityEngine;

// Bullet 게임 오브젝트를 엔티티로 변환하는 스크립트
public class BulletAuthoring : MonoBehaviour
{
    private class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            // 총알은 움직여야 하므로 TransformUsageFlags.Dynamic 플래그를 사용
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Bullet>(entity);
            AddComponent<Movement>(entity);
        }
    }
}

// 총알을 나타내는 컴포넌트
public struct Bullet : IComponentData
{
}