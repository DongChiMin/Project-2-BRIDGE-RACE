using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    void OnInit()
    {
        transform.position = new Vector3(0, 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard_Move();
    }

    void Keyboard_Move()
    {
        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        transform.position += new Vector3(X, 0, Y) * speed * Time.deltaTime;
    }
}
