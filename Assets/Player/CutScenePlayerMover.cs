using UnityEngine;

public class CutScenePlayerMover : MonoBehaviour
{
    [SerializeField] float m_Speed = 5f;
    Rigidbody2D m_RigidBody;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        m_RigidBody.linearVelocity = new Vector2(0, m_Speed);
    }
}
