using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    public static float DropTime = 0.9f;
    public static float SoftDropTime = 0.05f;
    public static float HorizontalMoveTime = 0.05f;
    public static float DelayedAutoShiftTime = 0.35f;
    public static int Width = 30, Height = 27;
    
    private long _score;
    private int _combo;

    public GameObject[] Blocks;
    // Height is +3 to allow blocks above the play field as long as it doesn't block the next block from spawning
    public Transform[,] Grid = new Transform[Width, Height];

    // Start is called before the first frame update
    private void Start()
    {
        _score = 0;
        _combo = 0;
        // If switched to 7-bag method, need to instantiate a new bag at the start
        SpawnBlock();
    }

    public void EndGame()
    {
        // End the game and switch scene to game over with score
        Debug.Log("Ending game");
        // Use a coroutine to set up an animation to make a smooth transition to the next scene
        SceneManager.LoadScene("GameOver");
    }

    public void IncrementLineClearScore(int lines)
    {
        // No t-spin bonuses, only line increments
        switch (lines)
        {
            case 1:
                _combo = 0;
                _score += 100;
                break;
            case 2:
                _combo = 0;
                _score += 300;
                break;
            case 3:
                _combo = 0;
                _score += 500;
                break;
            default:
                _combo++;
                _score += 1000;
                _score += 50 * _combo;
                break;
        }
        Debug.Log(_score);
    }

    public void IncrementDropScore(bool isHardDrop, int cells = 1)
    {
        _score += isHardDrop switch
        {
            // Hard drop score
            true => (cells * 10),
            // Soft drop score
            false => (cells * 5)
        };
        Debug.Log(_score);
    }

    public void SpawnBlock()
    {
        // Possibly switch this with a generated bag object
        var guess = Random.Range(0, 1f);
        guess *= Blocks.Length;
        Instantiate(Blocks[Mathf.FloorToInt(guess)]);
    }
}
