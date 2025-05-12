using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    public GameObject cam;
    public float parallaxEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceX = cam.transform.position.x * parallaxEffect;
        float distanceY = cam.transform.position.y * parallaxEffect;

        transform.position = new Vector3(startPosX+distanceX, startPosY+distanceY, transform.position.z);
    }
}
