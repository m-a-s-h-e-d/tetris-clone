using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static float DropTime = 0.9f;
    public static float SoftDropTime = 0.05f;
    public static float HorizontalMoveTime = 0.03f;
    public static float DelayedAutoShiftTime = 0.35f;
    public static int Width = 30, Height = 24;
    public static long Score = 0;

    private static int _combo = 0;

    private BagLogic _bagLogic;
    private Queue<GameObject> _blockQueue;

    public Transform[,] Grid = new Transform[Width, Height];

    // Start is called before the first frame update
    private void Start()
    {
        _bagLogic = FindObjectOfType<BagLogic>();
        Score = 0;
        _combo = 0;

        // Create the 7-bag block queue and spawn the first block
        _blockQueue = new Queue<GameObject>(_bagLogic.InitializeBag());
        SpawnBlock();
    }

    public void EndGame()
    {
        // End the game and switch scene to game over with score
        Debug.Log($"Game Over! Total game score: {Score}");
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
                Score += 800;
                break;
            case 2:
                _combo = 0;
                Score += 2000;
                break;
            case 3:
                _combo = 0;
                Score += 4200;
                break;
            case 4:
                _combo++;
                Score += 10000;
                Score += 1000 * _combo;
                break;
        }
        Debug.Log(Score);
    }

    public void IncrementDropScore(bool isHardDrop, int cells = 1)
    {
        Score += isHardDrop switch
        {
            // Hard drop score
            true => (cells * 10),
            // Soft drop score
            false => (cells * 5)
        };
        //Debug.Log(Score);
    }

    public void ClearLines()
    {
        var linesCleared = 0;
        for (var y = Height - 1; y >= 0; y--)
        {
            if (!IsLineComplete(y)) continue;
            DestroyLine(y);
            MoveLines(y);
            linesCleared++;
        }
        IncrementLineClearScore(linesCleared);
    }

    private void MoveLines(int y)
    {
        for (var i = y; i < Height - 1; i++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (Grid[x, i + 1] == null) continue;
                Grid[x, i] = Grid[x, i + 1];
                Grid[x, i].gameObject.transform.position -= new Vector3(0, 1, 0);
                Grid[x, i + 1] = null;
            }
        }
    }

    private void DestroyLine(int y)
    {
        for (var x = 0; x < Width; x++)
        {
            Destroy(Grid[x, y].gameObject);
            Grid[x, y] = null;
        }
    }

    private bool IsLineComplete(int y)
    {
        for (var x = 0; x < Width; x++)
        {
            if (Grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void SpawnBlock()
    {
        if (_blockQueue.Count <= 5)
        {
            foreach (var block in _bagLogic.InitializeBag())
            {
                _blockQueue.Enqueue(block);
            }
        }
        Instantiate(_blockQueue.Dequeue());
    }
}
