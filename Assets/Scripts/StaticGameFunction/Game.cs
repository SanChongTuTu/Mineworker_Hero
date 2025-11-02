using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static readonly int IntMask = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
