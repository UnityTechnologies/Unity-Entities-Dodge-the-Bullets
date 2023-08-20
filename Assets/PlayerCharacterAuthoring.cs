using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerCharacterAuthoring : MonoBehaviour
{
    private class Baker : Baker<PlayerCharacterAuthoring>
    {
        public override void Bake(PlayerCharacterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Player>(entity);
            AddComponent<Movement>(entity);
        }
    }
}

public partial struct Player : IComponentData
{
    
}