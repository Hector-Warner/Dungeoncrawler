using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public int speed;
    float horizontal;
    float vertical;
    float movementAdjustment = 1;
    public bool godMode = false;
    public GameObject Camera;
    public TileGenerationScript tileGenerationScript;
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
            }
            else
            {
                updateGodMode();
            }
        }
        if (godMode == false)
        {
            if (tileGenerationScript.checkTile(new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y))) == 1)
            {
                speed = 10;
            }
            else
            {
                speed = 5;
            }
        }
    }

    public void updateGodMode()
    {
        if (godMode == true)
        {
            speed = 5;
            Camera.GetComponent<Camera>().orthographicSize = 5;
            gameObject.GetComponent<Collider2D>().enabled = true;
            godMode = false;
        } else
        {
            speed = 30;
            Camera.GetComponent<Camera>().orthographicSize = 30;
            gameObject.GetComponent<Collider2D>().enabled = false;
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
