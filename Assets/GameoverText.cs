using TMPro;
using Unity.Entities;
using UnityEngine;

public class GameoverText : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        _text.text = string.Empty;
        // use GetExistingSystemManaged to get the system instance
        var gameManagerSystem =
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameManagerSystem>();
        gameManagerSystem.OnGameOver += Show;
    }

    private void Show(double time)
    {
        _text.text = $"Game Over\nTime: {time:F2}";
    }
}
