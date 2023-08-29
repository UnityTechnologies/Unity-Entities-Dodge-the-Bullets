using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public partial class GameManagerSystem : SystemBase
{
    public Action OnGameOver = () => { };
    public Action<double> OnTimeUpdate = _ => { };

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        var gameState = SystemAPI.GetSingleton<GameState>();
        var elapsedTime = SystemAPI.Time.ElapsedTime;

        if (!gameState.IsGameRunning || gameState.IsGameOver)
        {
            return;
        }
        
        OnTimeUpdate(elapsedTime);
        
        if (gameState.PlayerCount == 0)
        {
            gameState.IsGameOver = true;
            gameState.IsGameRunning = false;
            SystemAPI.SetSingleton(gameState);
            OnGameOver.Invoke();
        }
    }
}