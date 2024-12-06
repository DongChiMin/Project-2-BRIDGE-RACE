using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBrick : MonoBehaviour
{
    public GBrickManager brickManager;
    [SerializeField] private ColorType colorType;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] public Collider collider;


    public void OnInit(List<ColorType> colorTypes) // khoi tao
    {
        ///random mau
        int index = Random.Range(0, colorTypes.Count);

        colorType = colorTypes[index];
        //doi mau material
        SetBrickColor(colorType);

        //bat lai mesh
        meshRenderer.enabled = true;
        //bat collider
        collider.enabled = true;
    }

    public void SetBrickColor(ColorType colorType)
    {
        this.colorType = colorType;
        meshRenderer.material = DataManager.instance.colorData.GetMaterial(colorType);
    }

    public bool ColorCompare(ColorType otherColorType)
    {
        return otherColorType == this.colorType;
    }
    
    public void OnDespawn() // khi gach chet di
    {
        //tat mesh renderer 
        meshRenderer.enabled = false;
        //tat collider
        collider.enabled = false;
    }

    public void SpawnOnGround()
    {
        //Sau 2s thi spawn lai gach
        brickManager.SpawnBrick(Random.Range(0f, 4f), this);
    }
}
