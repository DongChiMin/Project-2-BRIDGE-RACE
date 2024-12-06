using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorSO", order = 1)]

public class ColorSO : ScriptableObject
{
    [SerializeField] private Material[] materials;

    public Material GetMaterial(ColorType colorType)
    {
        return materials[(int)colorType];
    }

    public Material GetMaterial(int colorType)
    {
        return materials[colorType];
    }

    public ColorType GetColorType(Material material)
    {
        for (int i = 0;  i < materials.Length; i++)
        {
            if (materials[i] == material)
            {
                return (ColorType)i;
            }
        }
        return (ColorType)4;
    }
}

public enum ColorType
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
    None = 4
}