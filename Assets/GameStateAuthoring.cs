using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameStateAuthoring : MonoBehaviour
{
    private class Baker : Baker<GameStateAuthoring>
    {
        public override void Bake(GameStateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<GameState>(entity);
        }
    }
}


public partial struct GameState : IComponentData
{
    public bool IsGameOver;
}