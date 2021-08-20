using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BagLogic : MonoBehaviour
{
    public GameObject[] Blocks;

    private List<GameObject> _bag;

    public GameObject[] InitializeBag()
    {
        _bag = new List<GameObject>();
        foreach (var block in Blocks.Shuffle())
        {
            _bag.Add(block);
        }
        return _bag.ToArray();
    }
}
