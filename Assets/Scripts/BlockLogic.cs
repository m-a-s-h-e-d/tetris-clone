using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    private bool _movable = true;
    private float _timer = 0f;
    private float _horizontalTimer = 0f;

    private bool _firstInput = true;
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
    }

    private void ResetBlock()
    {
        _movable = false;
        _transform.position += new Vector3(0, 1, 0);
        AddBlockToGrid();
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

        // Soft drop functionality
        if (Input.GetKey(KeyCode.DownArrow) && _timer > GameLogic.SoftDropTime)
        {
            _transform.position -= new Vector3(0, 1, 0);
            _timer = 0;
            if (!CheckValid())
            {
                ResetBlock();
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

        // Implement hard drop, check lowest position on y axis and place block down

        // Repeated horizontal block movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            switch (_firstInput)
            {
                case true:
                {
                    _transform.position -= new Vector3(1, 0, 0);
                    _firstInput = false;
                    _horizontalTimer = 0;
                    if (!CheckValid())
                    {
                        _transform.position += new Vector3(1, 0, 0);
                    }

                    break;
                }
                case false when _dasTimer > GameLogic.DelayedAutoShiftTime && _horizontalTimer > GameLogic.HorizontalMoveTime:
                {
                    _transform.position -= new Vector3(1, 0, 0);
                    _horizontalTimer = 0;
                    if (!CheckValid())
                    {
                        _transform.position += new Vector3(1, 0, 0);
                    }

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
                    _transform.position += new Vector3(1, 0, 0);
                    _firstInput = false;
                    _horizontalTimer = 0;
                    if (!CheckValid())
                    {
                        _transform.position -= new Vector3(1, 0, 0);
                    }

                    break;
                }
                case false when _dasTimer > GameLogic.DelayedAutoShiftTime && _horizontalTimer > GameLogic.HorizontalMoveTime:
                {
                    _transform.position += new Vector3(1, 0, 0);
                    _horizontalTimer = 0;
                    if (!CheckValid())
                    {
                        _transform.position -= new Vector3(1, 0, 0);
                    }

                    break;
                }
            }
        }
        else
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
