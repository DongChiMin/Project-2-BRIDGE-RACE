using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header ("Body")]
    [SerializeField] GameObject playerModel;

    [Header("Parameters")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] float force;

    [Header("Colors")]
    [SerializeField] public ColorType characterColor;
    [SerializeField] protected MeshRenderer colorCircle;

    [Header("Animator")]
    [SerializeField] protected Animator animator;

    [Header("Brick")]
    [SerializeField] GroundBrick groundBrick;
    //noi luu tru groundbrick
    [SerializeField] Transform transformBrick;

    [Header("Slope")]
    [SerializeField] private float maxAngleSlope;

    protected RaycastHit downHit;

    public Vector3 moveDirection;

    protected bool isRunning;
    string currentAnimName = "Torch Idle"; 

    float currentBrickHeight = 2;
    [SerializeField] protected Stack<GroundBrick> BrickList = new Stack<GroundBrick>();
    List<GroundBrick> collectedBrickPos = new List<GroundBrick>();

    protected CharState currenState;

    protected int currentFloor;
    public enum CharState
    {
        OnGround,
        OnSlope,
        Flying,
        Pushing
    }


   
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GroundBrick")
        {
            GroundBrick groundBrick = other.GetComponent<GroundBrick>();
            //neu gach dang active va neu mau nhan vat = mau gach thi disable gach
            if (other.gameObject.activeSelf && groundBrick.ColorCompare(characterColor))
            {
                //Despawn gach
                groundBrick.OnDespawn();
                //them gach dang sau nhan vat
                AddBrick();
                //them vao danh sach cac gach da duoc nguoi choi nhat
                collectedBrickPos.Add(groundBrick);
            }   
        }
        if (other.tag == "Stair")
        {
            Stair stair = other.GetComponent<Stair>();

            ////neu stair khong active: hien stair va removeBrick
            //if (!stair.isStairActive() && BrickList.Count > 0)
            //{
            //    //hien ra gach
            //    stair.ActiveStair();
            //    //doi mau pole
            //    CheckPole();
            //    //remove Brick
            //    RemoveBrick();
            //}

            CheckPole(stair);
        }
    }

    protected void RunAnim()
    {
        if (isRunning)
        {
            ChangeAnim("Run");
        }
        else
        {
            ChangeAnim("Torch Idle");
        }
    }

    protected void ChangeAnim(string s)
    {
        if (currentAnimName != s)
        {
            animator.SetBool(currentAnimName, false);
            currentAnimName = s;
            animator.SetBool(s, true);
        }
    }

    void AddBrick()
    {
        currentBrickHeight += 0.5f;
        GroundBrick brick = Instantiate(groundBrick,transformBrick);
        brick.transform.localPosition = new Vector3(0, currentBrickHeight,- 1);
        brick.transform.localRotation = Quaternion.Euler(0,90,0);
        brick.collider.enabled = false;

        brick.SetBrickColor(characterColor);
        BrickList.Push(brick);

    }

    void RemoveBrick()
    {
            //xoa gach
            GroundBrick brick = BrickList.Pop();
            Destroy(brick.gameObject);
            //giam chieu cao hien tai cua gach
            currentBrickHeight -= 0.5f;

        //neu nguoi choi dat gach xuong stair thi moi spawn lai gach o floor
        int index = Random.Range(0, collectedBrickPos.Count);
        collectedBrickPos[index].SpawnOnGround();
        Bot bot = this.GetComponent<Bot>();
        if(bot != null)
        {
            bot.AddBrickOnGround(collectedBrickPos[index]);
        }
        collectedBrickPos.Remove(collectedBrickPos[index]);
    }

    //protected bool OnSlope()
    //{
    //    LayerMask mask = LayerMask.GetMask("Slope");
    //    Debug.DrawRay(transform.position + Vector3.up * 2f, Vector3.down * 2f, Color.red, 2f);
    //    if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out slopeHit, 2f, mask))
    //    {
    //        isOnSlope = true;
    //        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
    //        return angle < maxAngleSlope && angle != 0;
    //    }
    //    isOnSlope = false;
    //    return false;
    //}

    //protected bool OnGround()
    //{
    //    LayerMask mask = LayerMask.GetMask("Ground");
    //    Debug.DrawRay(transform.position + Vector3.up * 2f, Vector3.down*3, Color.white, 2f);
    //    if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out slopeHit, 2f, mask))
    //    {
    //        isOnSlope = false;
    //        return true;
    //    }
    //    return false;
    //}



    

    protected void CheckPole(Stair stair)
    {
        LayerMask mask = LayerMask.GetMask("StairPole");
        RaycastHit poleHit;
        Debug.DrawRay(transform.position + Vector3.up * 2 + Vector3.forward, Vector3.right * 4f, Color.blue, 2f);
        //ban raycast tu phia truoc, huong sang phai, check neu trung stairpole
        if (Physics.Raycast(transform.position + Vector3.up * 2 + Vector3.forward, Vector3.right, out poleHit, 4f, mask))
        {
            //lay material cua pole
            MeshRenderer mesh = Cache.GetStairMeshRenderer(poleHit.collider.gameObject);

            //neu ban trung stairpole, so sanh pole color voi character color
            //neu mau giong nhau
            if (mesh.material.HasProperty("_Color") && mesh.material.color == colorCircle.material.color)
            {
                stair.UnActiveBlock();
            }
            //neu mau khac nhau
            else
            {
                //kiem tra xem con gach hay khong
                //neu con gach
                if(BrickList.Count > 0)
                {
                    //doi mau pole, tru gach, mo khoa block
                    mesh.material = colorCircle.material;
                    RemoveBrick();
                    stair.UnActiveBlock();

                    //kiem tra xem hinh anh cua cau thang da mo chua
                    if (!stair.isStairActive())
                    {
                        stair.ActiveStair();
                    }
                }

                //neu het gach
                else
                {
                    stair.ActiveBlock();
                }
            }
        }
    }

    protected void CheckState()
    {
        Debug.DrawRay(transform.position + Vector3.up * 2f, Vector3.down * 2f, Color.red, 2f);
        if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out downHit, 2f, LayerMask.GetMask("Slope")))
        {
            float angle = Vector3.Angle(Vector3.up, downHit.normal);
            if (angle < maxAngleSlope && angle != 0 && currenState != CharState.Pushing)
            {
                ChangeState(CharState.OnSlope);
            }
        }
        else if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out downHit, 2f, LayerMask.GetMask("Ground")))
        {
            ChangeState(CharState.OnGround);
        }
        else if(currenState != CharState.Pushing)
        {
            ChangeState(CharState.Flying);
        }
    }

    void ChangeState(CharState newState)
    {
        currenState = newState;
    }

    protected void RotatePlayer()
    {
        //xoay nhan vat theo huong di chuyen
        if (moveDirection != Vector3.zero)
        {
            playerModel.transform.rotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
        }
    }
}
