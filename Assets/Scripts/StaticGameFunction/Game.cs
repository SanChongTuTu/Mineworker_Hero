using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static readonly int IntMask = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

    public static void ExitGame()
    {
        Application.Quit();
    }
}
