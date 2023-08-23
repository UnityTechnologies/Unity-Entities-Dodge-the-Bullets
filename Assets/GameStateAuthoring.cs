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
    public double FinishTime;
    public bool IsGameRunning;
    public bool IsGameOver;
    public int PlayerCount;
}