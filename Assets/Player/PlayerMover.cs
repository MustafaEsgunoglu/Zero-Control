using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform m_EngineTransform;   // The transform of the engine sprite.
    Rigidbody2D m_RigidBody;                        // The player's Rigidbody2D.

    [Header("Movement Settings")]
    [SerializeField] float m_ThrustForce = 5f;      // How much force is applied when thrusting.
    [SerializeField] float m_RotationSpeed = 200f;  // How fast the engine rotates (degrees per second).
    [SerializeField] float m_StopDamping = 0.1f;    // Damping factor for the sideways (lateral) velocity.
    [SerializeField] float m_MaxSpeed = 50f;        // Max speed limit.

    [Header("Sprite Settings")]
    [SerializeField] float m_SpriteRotationOffset = 0; // If sprite’s direction is off, apply an offset.

    // Input values.
    Vector2 moveInput = Vector2.zero;
    public bool thrusting = false;

    // Current engine rotation in degrees.
    float currentEngineAngle = 0f;
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    //Getter and Setters
    public float GetThrustForce()
    { return m_ThrustForce; }
    public void SetThrustForce(float ThrustForce)
    { m_ThrustForce = ThrustForce; }

    public float GetMaxSpeed()
    { return m_MaxSpeed; }
    public void SetMaxSpeed(float MaxSpeed)
    { m_MaxSpeed = MaxSpeed; }

    public float GetRotationSpeed()
    { return m_RotationSpeed; }
    public void SetRotationSpeed(float RotationSpeed)
    { m_RotationSpeed = RotationSpeed; }

    public float GetStopDamping()
    { return m_StopDamping; }
    public void SetStopDamping(float StopDamping)
    { m_StopDamping = StopDamping; }



    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.volume = 1f; // Set volume
        audioSource.clip = audioClip; // Assign the audio clip
    }

    void FixedUpdate()
    {
        // Update engine rotation if there's input.
        if (moveInput.sqrMagnitude > Mathf.Epsilon)
        {
            // Calculate the target angle using the input vector plus the sprite offset.
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg + m_SpriteRotationOffset;
            // Smoothly rotate towards the target angle.
            currentEngineAngle = Mathf.MoveTowardsAngle(currentEngineAngle, targetAngle, m_RotationSpeed * Time.fixedDeltaTime);
            m_EngineTransform.rotation = Quaternion.Euler(0, 0, currentEngineAngle);
        }

        if (thrusting)
        {
            // When thrusting, apply force in the engine's up direction.
            Vector2 thrustDirection = m_EngineTransform.up;
            m_RigidBody.AddForce(thrustDirection * m_ThrustForce);

            // Decompose velocity relative to the engine's current rotation.
            Vector2 currentVelocity = m_RigidBody.linearVelocity;
            Vector2 engineDirection = m_EngineTransform.up;  // "Forward" direction.
            float forwardSpeed = Vector2.Dot(currentVelocity, engineDirection);
            Vector2 forwardComponent = engineDirection * forwardSpeed;
            Vector2 lateralComponent = currentVelocity - forwardComponent;

            // Apply damping only to the lateral component.
            lateralComponent = Vector2.Lerp(lateralComponent, Vector2.zero, m_StopDamping);

            // Recombine the velocity.
            m_RigidBody.linearVelocity = forwardComponent + lateralComponent;
            PlayAudio();
        }
        else
        {
            // When not thrusting, apply full damping to the entire velocity.
            audioSource.Stop();
            m_RigidBody.linearVelocity = Vector2.Lerp(m_RigidBody.linearVelocity, Vector2.zero, m_StopDamping);
        }

        // Clamp the velocity so that it does not exceed m_MaxSpeed.
        m_RigidBody.linearVelocity = Vector2.ClampMagnitude(m_RigidBody.linearVelocity, m_MaxSpeed);
    }


    /* --- Input System Methods --- */

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnThrust(InputValue value)
    {
        thrusting = value.isPressed;
    }

    public void PlayAudio()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
            return;
        }
        audioSource.pitch = Random.Range(1.5f, 1.8f);
        audioSource.Play(); // Play the sound from the enemy
    }
}
