using System;
using Unity.Entities;

public partial class GameManagerSystem : SystemBase
{
    public Action<double> OnGameOver;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        var gameState = SystemAPI.GetSingleton<GameState>();
        
        if (gameState.IsGameOver || !gameState.IsGameRunning) 
        {
            return;
        }
        
        if(gameState.PlayerCount == 0)
        {
            gameState.IsGameOver = true;
            gameState.IsGameRunning = false;
            gameState.FinishTime = SystemAPI.Time.ElapsedTime;
            SystemAPI.SetSingleton(gameState);
            OnGameOver?.Invoke(gameState.FinishTime);
        }
    }
}