using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBrickManager : MonoBehaviour
{
    [SerializeField] private GroundBrick GroundBrick;
    [SerializeField] private Collider doorCollider;
    [SerializeField] private Transform gbrickList;
    [SerializeField] private List<ColorType> colorTypes = new List<ColorType>();
    

    private Vector3 StartBrickPos;
    private Vector3 EndBrickPos;

    //luu cac GBrick da sinh ra
    List<GroundBrick> GBrickOnGroundList = new List<GroundBrick>();

    //luu nhung vi tri duoc phep sinh ra gach moi
    List<Vector3> BrickPosAvailable = new List<Vector3>();

    void Start()
    {
        OnInit();
    }

    void OnInit()
    {
        StartBrickPos = transform.position + new Vector3(-11.5f, 0.6f, 2.5f);
        EndBrickPos = transform.position + new Vector3(11.5f, 0.6f, 8.5f);

        CreateBrickPosList();
    }

    void CreateBrickPosList()
    {
        ///vi tri
        float x = EndBrickPos.x - StartBrickPos.x;
        float z = EndBrickPos.z - StartBrickPos.z;

        for (int i = 0; i <= x; i++)
        {
            for (int j = 0; j <= z; j++)
            {
                Vector3 pos = StartBrickPos + new Vector3(i, 0, j);
                BrickPosAvailable.Add(pos);
            }
        }
    }

    private void SpawnGround(ColorType color)
    {
        int spawnAmount = (168*colorTypes.Count)/4;
        while(GBrickOnGroundList.Count < spawnAmount)
        {
            
            //lay random 1 vi tri, sau do xoa vi tri do khoi danh sach
            int randomIndex = Random.Range(0, BrickPosAvailable.Count);
            Vector3 spawnPos = BrickPosAvailable[randomIndex];
            BrickPosAvailable.Remove(spawnPos);

            GroundBrick brick = Instantiate<GroundBrick>(GroundBrick, spawnPos, Quaternion.Euler(0, 0, 0), gbrickList);
            //brick duoc manager nay quan ly
            brick.brickManager = this;
            //khoi tao mau cua brick
            brick.OnInit(colorTypes);
            brick.SetBrickColor(color);
            //luu GBrick moi duoc sinh ra
            GBrickOnGroundList.Add(brick);
        }
    }

    public List<GroundBrick> GetBrickOnGround() {
        return GBrickOnGroundList;
    }

    public void SpawnBrick(float time, GroundBrick groundBrick)
    {
        StartCoroutine(WaitForTimeToSpawn(time, groundBrick));
    }

    IEnumerator WaitForTimeToSpawn(float time, GroundBrick groundBrick)
    {
        ///ToDO: tawst mesh
        ///

        yield return new WaitForSeconds(time);

        groundBrick.OnInit(colorTypes);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Character")
        {
            ColorType characterColor = other.GetComponent<Character>().characterColor;
            //neu colorTypes chua co mau -> add
            if (!colorTypes.Contains(characterColor))
            {

                colorTypes.Add(characterColor);

                //sinh ra gach tren ground
                SpawnGround(characterColor);

                //bat trigger collider de character di qua
                doorCollider.isTrigger = true;
            }
            else
            {
                Debug.Log("FALSE");
                doorCollider.isTrigger = false;
            }
        }
    }
}
