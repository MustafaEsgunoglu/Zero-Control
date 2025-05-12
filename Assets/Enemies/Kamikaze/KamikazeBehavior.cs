using UnityEngine;

public class KamikazeBehavior : MonoBehaviour
{
    [SerializeField] float m_ActivationRange = 15f;
    [SerializeField] float m_Speed = 10f;
    [SerializeField] float m_Damage = 10f;
    [SerializeField] float m_RotationSpeed = 5f; // Controls how quickly the ship rotates

    Rigidbody2D m_RigidBody;
    Transform m_Player;
    bool isActive = false;

    public float GetDamage()
    {
        return m_Damage;
    }

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Player = FindAnyObjectByType<PlayerMover>().transform;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(m_Player.position, transform.position);

        // Activate if the player is within range
        if (distanceToPlayer <= m_ActivationRange)
        {
            isActive = true;
        }

        if (isActive)
        {
            // Compute the normalized direction toward the player.
            Vector2 direction = (m_Player.position - transform.position).normalized;
            m_RigidBody.linearVelocity = direction * m_Speed;

            // Calculate the target rotation angle in degrees.
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            // Smoothly interpolate from the current rotation to the target angle.
            float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetAngle, m_RotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
