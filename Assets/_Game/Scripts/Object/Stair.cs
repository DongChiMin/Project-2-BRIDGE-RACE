using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private GameObject stair1;
    [SerializeField] private GameObject stair2;
    [SerializeField] private GameObject block;


    public bool isStairActive()
    {
        if (!stair1.activeSelf && !stair2.activeSelf)
        {
            return false;
        }
        return true;
    }
    public void ActiveStair()
    {
        stair1.SetActive(true);
        stair2.SetActive(true);
    }

    public void UnActiveBlock()
    {
        block.SetActive(false);
    }

    public void ActiveBlock()
    {
        block.SetActive(true);
    }
}
