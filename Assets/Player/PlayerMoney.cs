using System.Collections;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [Header(" --- Pickup Info --- ")]
    [SerializeField] float initialSpeed = 2f; // Initial speed of the pickup
    [SerializeField] float maxSpeed = 10f;    // Maximum speed the pickup can reach
    [SerializeField] float acceleration = 10f; // Rate at which the pickup speeds up

    [Header(" --- Player Stats --- ")]
    public int m_HealthMoney = 0;
    public int m_EngineMoney = 0;
    public int m_CanonMoney = 0;

    public int GetHealthMoney() { return m_HealthMoney; }
    public void SetHealthMoney(int healthMoney) { m_HealthMoney = healthMoney; }
    public int GetEngineMoney() { return m_EngineMoney; }
    public void SetEngineMoney(int engineMoney) { m_EngineMoney = engineMoney; }
    public int GetCannonMoney() { return m_CanonMoney; }
    public void SetCannonMoney(int canonMoney) { m_CanonMoney = canonMoney; }

    [Header(" --- References --- ")]
    private Transform player;

    void Start()
    {
        // Find the player object (assuming it's tagged as "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Trigger event when a pickup enters the player's attraction range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Health") ||
            other.CompareTag("Engine") ||
            other.CompareTag("Canon") ||
            other.CompareTag("Health Screw") ||
            other.CompareTag("Engine Screw") ||
            other.CompareTag("Canon Screw"))
        {
            // Check if the pickup already has been collected using the Pickup component.
            PickupMoney pickupComponent = other.GetComponent<PickupMoney>();
            if (pickupComponent != null)
            {
                if (pickupComponent.isCollected)
                    return;
                pickupComponent.isCollected = true;
            }
            else
            {
                // Fallback: disable the collider if no Pickup component is attached.
                other.enabled = false;
            }
            StartCoroutine(AttractToPlayer(other.gameObject, other.tag));
        }
    }

    // Coroutine to make the pickup accelerate towards the player
    IEnumerator AttractToPlayer(GameObject pickup, string pickupTag)
    {
        float currentSpeed = initialSpeed;
        while (pickup != null && Vector2.Distance(pickup.transform.position, player.position) > 0.1f)
        {
            // Increase the speed over time (acceleration)
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
            // Move the pickup towards the player
            pickup.transform.position = Vector2.MoveTowards(pickup.transform.position, player.position, currentSpeed * Time.deltaTime);
            yield return null;
        }
        // Once the pickup reaches the player, collect it.
        CollectPickup(pickup, pickupTag);
    }

    // Collect the pickup and add the corresponding money to player stats based on its tag
    void CollectPickup(GameObject pickup, string pickupTag)
    {
        if (pickupTag == "Health")
        {
            m_HealthMoney += 5;
        }
        else if (pickupTag == "Engine")
        {
            m_EngineMoney += 5;
        }
        else if (pickupTag == "Canon")
        {
            m_CanonMoney += 5;
        }
        else if (pickupTag == "Health Screw")
        {
            m_HealthMoney += 1;
        }
        else if (pickupTag == "Engine Screw")
        {
            m_EngineMoney += 1;
        }
        else if (pickupTag == "Canon Screw")
        {
            m_CanonMoney += 1;
        }
        // Destroy the pickup immediately upon reaching the player.
        Destroy(pickup);
    }
}
