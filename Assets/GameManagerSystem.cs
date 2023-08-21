using System;
using Unity.Entities;

public partial class GameManagerSystem : SystemBase
{
    public Action<float> OnGameOver;
    private bool isGameover;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        if (isGameover)
        {
            return;
        }

        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.IsGameOver)
        {
            isGameover = true;
            OnGameOver?.Invoke((float)SystemAPI.Time.ElapsedTime);
        }
    }
}