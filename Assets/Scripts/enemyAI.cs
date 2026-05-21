using UnityEngine;
using Pathfinding;

public class enemyAI : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public Transform target;
    public float moveSpeed = 5;

    public float calculationInterval = 0.25f;
    public int lookAhead = 6;

    public Seeker seeker;
    public Rigidbody2D rb;
    Path currentPath;

    Vector3 moveDirection;
    bool followPath;
    int nextNode;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        timer = calculationInterval + 1;
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePath();
        FollowPath();
    }

    void CalculatePath()
    {
        if (timer > calculationInterval)
        {
            followPath = false;
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            timer -= calculationInterval;
        }
        timer += Time.deltaTime;
    }

    void OnPathComplete(Path path)
    {
        currentPath = path;
        nextNode = 0;
        followPath = true;
    }

    void FollowPath()
    {
        if (followPath)
        {
            Vector3 targetPosition = (Vector3)currentPath.path[nextNode].position;
            targetPosition.z = transform.position.z;
            moveDirection = targetPosition - transform.position;

            if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
            {
                nextNode++;
                if (nextNode >= currentPath.path.Count)
                {
                    followPath = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(moveDirection.x, 0) * moveSpeed);
    }
}
