using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public int speed;
    float horizontal;
    float vertical;
    float movementAdjustment = 1;
    public bool godMode = false;
    public GameObject camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (godMode == true)
            {
                updateGodMode();
            } else
            {
                updateGodMode();
            }
        }
    }

    public void updateGodMode()
    {
        if (godMode == true)
        {
            speed = 5;
            camera.GetComponent<Camera>().orthographicSize = 5;
            godMode = false;
        } else
        {
            speed = 30;
            camera.GetComponent<Camera>().orthographicSize = 30;
            godMode = true;
        }
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0)
        {
            movementAdjustment = 0.7f;
        } else
        {
            movementAdjustment = 1;
        }
        CharMovement();
    }

    void CharMovement()
    {
        myRigidBody.linearVelocity = new Vector2(horizontal * speed * movementAdjustment, vertical * speed * movementAdjustment);
    }
}
