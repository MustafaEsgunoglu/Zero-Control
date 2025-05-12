using UnityEngine;

public class WrapAround : MonoBehaviour
{
    // Define your world dimensions (assume centered at (0,0) for simplicity)
    public float worldWidth = 50f;
    public float worldHeight = 50f;

    void Update()
    {
        Vector3 pos = transform.position;

        // Check horizontal bounds
        if (pos.x > worldWidth / 2f)
            pos.x -= worldWidth;
        else if (pos.x < -worldWidth / 2f)
            pos.x += worldWidth;

        // Check vertical bounds
        if (pos.y > worldHeight / 2f)
            pos.y -= worldHeight;
        else if (pos.y < -worldHeight / 2f)
            pos.y += worldHeight;

        transform.position = pos;
    }
}
