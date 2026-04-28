using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerController Controller;
    public Vector2 adjustmentDir;
    public int adjustmentSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = new Vector3(Controller.transform.position.x, Controller.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, adjustmentSpeed * Time.deltaTime);
    }
}
