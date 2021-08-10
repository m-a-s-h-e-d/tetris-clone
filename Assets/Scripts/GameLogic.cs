using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    public static float DropTime = 0.9f;
    public static float SoftDropTime = 0.05f;
    public static float HorizontalMoveTime = 0.15f;
    public static int Width = 30, Height = 27;
    public GameObject[] Blocks;
    // Height is +3 to allow blocks above the play field as long as it doesn't block the next block from spawning
    public Transform[,] Grid = new Transform[Width, Height];

    // Start is called before the first frame update
    private void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        var guess = Random.Range(0, 1f);
        guess *= Blocks.Length;
        Instantiate(Blocks[Mathf.FloorToInt(guess)]);
    }
}
