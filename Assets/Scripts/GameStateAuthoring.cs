using Unity.Entities;
using UnityEngine;

// Game State 게임 오브젝트를 엔티티로 변환
public class GameStateAuthoring : MonoBehaviour
{
    private class Baker : Baker<GameStateAuthoring>
    {
        public override void Bake(GameStateAuthoring authoring)
        {
            // 움직일 필요가 없으므로 TransformUsageFlags.None 플래그를 사용
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<GameState>(entity);
        }
    }
}

// 게임 현재 상태에 대응
public struct GameState : IComponentData
{
    public bool IsGameRunning; // 게임이 시작되어 진행중인가
    public bool IsGameOver; // 게임 오버 상태
    public int PlayerCount; // 살아있는 플레이어 수
}