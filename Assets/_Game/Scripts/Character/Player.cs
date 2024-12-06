using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Character
{

    [Header ("Drag Object")]
    [SerializeField] private Transform orientation;
    [SerializeField] Joystick joystick;
    [SerializeField] protected Rigidbody rb;

    float horizontalInput;
    float verticalInput;


    void Start()
    {
        OnInit();
    }

    void OnInit()
    {
        //set mau nhan vat va vong tron
        colorCircle.material = DataManager.instance.colorData.GetMaterial(characterColor);
        transform.position = new Vector3(0, 0, 2);

        //set thong so rigidBody
        rb.freezeRotation = true;

        //set trang thai nhan vat
        isRunning = false;
        currenState = CharState.OnGround;
        currentFloor = 1;

        //set animation
        animator.SetBool("Torch Idle", true);

    }
    
    void Update()
    {

        //lay input
        getInput();
        //chay anim
        RunAnim();

        //Ban raycast xuong duoi
        CheckState();
    }

    private void FixedUpdate()
    {
        if(isRunning)
        {
            MoveCharacter();
        }
        if(currenState == CharState.Flying)
        {
            AddGravity();
        }
        //tat trong luc khi character o tren doc
        //rb.useGravity = !OnSlope();
        RotatePlayer();
    }

    void getInput()
    {
        //Lay dau vao tu joystick
        horizontalInput = joystick.Horizontal;
        verticalInput = joystick.Vertical;

        //tinh toan huong di chuyen
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //kiem tra nhan vat co dang chay hay khong
        if (moveDirection.magnitude >0.1f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    protected void MoveCharacter()
    {
        if (currenState == CharState.OnSlope)
        {
            //di chuyen len doc
            Debug.DrawRay(transform.position, GetSlopeDirection(), Color.cyan, 3f);
            rb.AddForce(GetSlopeDirection() * moveSpeed, ForceMode.Force);

            //if(rb.velocity.y > 0)
            //{
            //    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            //}
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
    }

    protected void AddGravity()
    {
        rb.AddForce(Vector3.down * rb.mass * 500, ForceMode.Force);
    }

    private Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, downHit.normal).normalized;
    }



}
