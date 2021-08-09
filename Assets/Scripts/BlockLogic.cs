using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    private bool movable = true;
    private float timer = 0f;
    private float horizontalTimer = 0f;

    public GameObject rig;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Check for valid positions
    private bool CheckValid()
    {
        foreach (Transform subBlock in rig.transform)
        {
            if(subBlock.transform.position.x >= GameLogic.width ||
                Math.Round(subBlock.transform.position.x) < 0 ||
                subBlock.transform.position.y < 0)
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    private void Update()
    {
        // If the block has not landed on the ground yet
        if (movable)
        {
            // Update the timer
            timer += 1 * Time.deltaTime;
            horizontalTimer += 1 * Time.deltaTime;

            // Soft drop functionality
            if (Input.GetKey(KeyCode.DownArrow) && timer > GameLogic.softDropTime)
            {
                gameObject.transform.position -= new Vector3(0, 1, 0);
                timer = 0;
                if (!CheckValid())
                {
                    movable = false;
                    gameObject.transform.position += new Vector3(0, 1, 0);
                }
            }
            else if (timer > GameLogic.dropTime) // Automatically falling block
            {
                gameObject.transform.position -= new Vector3(0, 1, 0);
                timer = 0;
                if (!CheckValid())
                {
                    movable = false;
                    gameObject.transform.position += new Vector3(0, 1, 0);
                }
            }

            // Repeated horizontal block movement
            if (Input.GetKey(KeyCode.LeftArrow) && horizontalTimer > GameLogic.horizontalMoveTime)
            {
                gameObject.transform.position -= new Vector3(1, 0, 0);
                horizontalTimer = 0;
                foreach (Transform sub in rig.transform)
                {
                    Debug.Log(Math.Round(sub.transform.position.x));
                }
                if (!CheckValid())
                {
                    gameObject.transform.position += new Vector3(1, 0, 0);
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow) && horizontalTimer > GameLogic.horizontalMoveTime)
            {
                gameObject.transform.position += new Vector3(1, 0, 0);
                horizontalTimer = 0;
                if (!CheckValid())
                {
                    gameObject.transform.position -= new Vector3(1, 0, 0);
                }
            }

            // Block rotation
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                rig.transform.eulerAngles -= new Vector3(0, 0, 90);
                foreach (Transform sub in rig.transform)
                {
                    Debug.Log(Math.Round(sub.transform.position.x));
                }
                if (!CheckValid())
                {
                    rig.transform.eulerAngles += new Vector3(0, 0, 90);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                rig.transform.eulerAngles += new Vector3(0, 0, 90);
                if (!CheckValid())
                {
                    rig.transform.eulerAngles -= new Vector3(0, 0, 90);
                }
            }
        }
    }
}
