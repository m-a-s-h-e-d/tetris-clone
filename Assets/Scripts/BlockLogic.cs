using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    private bool _movable = true;
    private bool _firstInput = true;
    private float _timer = 0f;
    private float _horizontalTimer = 0f;
    private float _dasTimer = 0f;

    private GameLogic _gameLogic;
    private Transform _transform;
    private Transform _rigTransform;

    public GameObject Rig;

    // Start is called before the first frame update
    private void Start()
    {
        _gameLogic = FindObjectOfType<GameLogic>();
        _transform = transform;
        _rigTransform = Rig.transform;

        if (CheckValid()) return;

        _movable = false;
        _gameLogic.EndGame();
    }

    private void ResetBlock()
    {
        _movable = false;
        _transform.position += new Vector3(0, 1, 0);
        AddBlockToGrid();
        _gameLogic.ClearLines();
        _gameLogic.SpawnBlock();
    }

    // Change the x-axis height to the highest y-axis position in the stored grid
    private void AddBlockToGrid()
    {
        foreach (Transform subBlock in _rigTransform)
        {
            _gameLogic.Grid[Mathf.RoundToInt(subBlock.position.x), Mathf.FloorToInt(subBlock.position.y)] = subBlock;
        }
    }

    // Shift the current block's transform left
    private void ShiftLeft()
    {
        _transform.position -= new Vector3(1, 0, 0);
        _horizontalTimer = 0;
        if (!CheckValid())
        {
            _transform.position += new Vector3(1, 0, 0);
        }
    }

    // Shift the current block's transform right
    private void ShiftRight()
    {
        _transform.position += new Vector3(1, 0, 0);
        _horizontalTimer = 0;
        if (!CheckValid())
        {
            _transform.position -= new Vector3(1, 0, 0);
        }
    }

    // Check for valid positions
    private bool CheckValid()
    {
        foreach (Transform subBlock in _rigTransform)
        {
            if (subBlock.transform.position.x >= GameLogic.Width ||
                Math.Round(subBlock.transform.position.x) < 0 ||
                subBlock.transform.position.y < 0)
            {
                return false;
            }
            if (subBlock.position.y < GameLogic.Height && _gameLogic.Grid[Mathf.RoundToInt(subBlock.position.x), Mathf.FloorToInt(subBlock.position.y)] != null)
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    private void Update()
    {
        // If the block has already landed
        if (!_movable) return; // This is a guard clause

        // Update the _timer
        _timer += 1 * Time.deltaTime;
        _horizontalTimer += 1 * Time.deltaTime;

        // Update das timer if activated
        if (!_firstInput)
        {
            _dasTimer += 1 * Time.deltaTime;
        }

        // Implement hard drop, check lowest position on y axis and place block down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Move the game object down based on greatest y value out of all x-axis of the rig
            int cells = 5;
            _gameLogic.IncrementDropScore(true, cells);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && _timer > GameLogic.SoftDropTime) // Soft drop functionality
        {
            _transform.position -= new Vector3(0, 1, 0);
            _timer = 0;
            if (!CheckValid())
            {
                ResetBlock();
            }
            else
            {
                _gameLogic.IncrementDropScore(false);
            }
        }
        else if (_timer > GameLogic.DropTime) // Automatically falling block
        {
            _transform.position -= new Vector3(0, 1, 0);
            _timer = 0;
            if (!CheckValid())
            {
                ResetBlock();
            }
        }

        // Repeated horizontal block movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            switch (_firstInput)
            {
                case true:
                {
                    _firstInput = false;
                    ShiftLeft();
                    break;
                }
                case false when _dasTimer > GameLogic.DelayedAutoShiftTime && _horizontalTimer > GameLogic.HorizontalMoveTime:
                {
                    ShiftLeft();
                    break;
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            switch (_firstInput)
            {
                case true:
                {
                    _firstInput = false;
                    ShiftRight();
                    break;
                }
                case false when _dasTimer > GameLogic.DelayedAutoShiftTime && _horizontalTimer > GameLogic.HorizontalMoveTime:
                {
                    ShiftRight();
                    break;
                }
            }
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            _dasTimer = 0;
            _firstInput = true;
        }

        // Block rotation
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _rigTransform.eulerAngles -= new Vector3(0, 0, 90);
            if (!CheckValid())
            {
                _rigTransform.eulerAngles += new Vector3(0, 0, 90);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _rigTransform.eulerAngles += new Vector3(0, 0, 90);
            if (!CheckValid())
            {
                _rigTransform.eulerAngles -= new Vector3(0, 0, 90);
            }
        }
    }
}
