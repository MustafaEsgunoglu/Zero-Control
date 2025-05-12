using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoverNormal : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform m_EngineTransform;   // The transform of the engine sprite.
    Rigidbody2D m_RigidBody;                        // The player's Rigidbody2D.

    [Header("Movement Settings")]
    [SerializeField] float m_ThrustForce = 5f;      // How much force is applied when thrusting.
    [SerializeField] float m_StopDamping = 0.1f;    // Damping factor to help the ship slow down when not thrusting.

    [Header("Sprite Settings")]
    [SerializeField] float m_SpriteRotationOffset = 0;   // If sprite’s direction is not working wit inputs as intended, apply an offset.

    // Input values.
    Vector2 moveInput = Vector2.zero;


    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (moveInput.sqrMagnitude > Mathf.Epsilon)
        {
            // Calculate the target angle based on the input vector.
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg + m_SpriteRotationOffset;

            // Rotate towards the target angle.
            m_EngineTransform.rotation = Quaternion.Euler(0, 0, targetAngle);

            Vector2 thrustDirection = m_EngineTransform.up;
            m_RigidBody.AddForce(thrustDirection * m_ThrustForce);

        }
        else
        {
            // Damping effect to help the ship slow down when not thrusting.
            m_RigidBody.linearVelocity = Vector2.Lerp(m_RigidBody.linearVelocity, Vector2.zero, m_StopDamping);
        }
    }

    /* --- Input System Methods --- */

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}