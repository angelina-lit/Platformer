using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    public PlayerData Data => _data;

    private void Awake()
    {
        if (IsSessionExit())
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    private bool IsSessionExit()
    {
        var sessions = FindObjectsOfType<GameSession>();
        foreach (var gameSession in sessions)
        {
            if (gameSession != this) 
                return true;
        }
        return false;
    }
}
