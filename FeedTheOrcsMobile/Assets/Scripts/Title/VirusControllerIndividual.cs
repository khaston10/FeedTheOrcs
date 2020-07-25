using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusControllerIndividual : MonoBehaviour
{
    public float speedMin;
    public float speedMax;
    public float speed;
    private Vector2 dir;
    private Rigidbody2D RB;
    private bool movingUp;
    private bool movingRight;


    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin, speedMax);

        RB = GetComponent<Rigidbody2D>();

        PickDirection();
    }

    // Update is called once per frame
    void Update()
    {
        RB.velocity = dir * speed;

    }

    public void PickDirection(float left = -.5f, float right = .5f, float up = .5f, float down = -.5f)
    {
        dir.x = Random.Range(left, right);
        dir.y = Random.Range(down, up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Top")
        {
            if (movingRight) PickDirection(0f, .5f, 0f, -.5f);
            else PickDirection(-.5f, 0f, 0f, -.5f);
        }
        else if (collision.transform.tag == "Bottom")
        {
            if (movingRight) PickDirection(0f, .5f, .5f, 0f);
            else PickDirection(-.5f, 0f, .5f, 0f);
        }
        else if (collision.transform.tag == "Right")
        {
            if (movingUp) PickDirection(-.5f, 0f, .5f, 0f);
            else PickDirection(-.5f, 0f, 0f, -.5f);
        }
        else if (collision.transform.tag == "Left") 
        {
            if (movingUp) PickDirection(0f, .5f, .5f, 0f);
            else PickDirection(0f, .5f, 0f, -.5f);
        }
        

        // Set moving bools correctly.
        if (RB.velocity.x > 0) movingRight = true;
        else movingRight = false;

        if (RB.velocity.y > 0) movingUp = true;
        else movingUp = false;
    }

}
