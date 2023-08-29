using TMPro;
using Unity.Entities;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    private TMP_Text _text;
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        // use GetExistingSystemManaged to get the system instance
        var gameManagerSystem =
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameManagerSystem>();
        gameManagerSystem.OnTimeUpdate += UpdateTimeText;
    }

    private void UpdateTimeText(double elapsedTime)
    {
        _text.text = $"{elapsedTime:00.00}";
    }
}
