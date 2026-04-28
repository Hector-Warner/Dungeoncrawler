using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public int speed;
    float horizontal;
    float vertical;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        CharMovement();
    }

    void CharMovement()
    {
        myRigidBody.linearVelocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
