using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPole : MonoBehaviour
{
    private ColorType poleColor;
    private MeshRenderer poleMaterial;
    private void Start()
    {
        OnInit();
    }

    void OnInit()
    {
        poleColor = ColorType.None;
    }

    public ColorType getPoleColor()
    {
        return poleColor;
    }

}
