using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class PowerupManager : MonoBehaviour
{

    public void Cannon(GameObject player, float multiplier, float duration)
    {
        StartCoroutine(CannonCoroutine(player, multiplier, duration));
    }
    public void Engine(GameObject player, float multiplier, float duration)
    {
        StartCoroutine(EngineCoroutine(player, multiplier, duration));
    }
    public void Dodge(GameObject player, float multiplier, float duration)
    {
        StartCoroutine(DodgeCoroutine(player, multiplier, duration));
    }

    IEnumerator EngineCoroutine(GameObject player, float multiplier, float duration)
    {
        PlayerMover engine = player.GetComponent<PlayerMover>();
        float originalValue = engine.GetThrustForce();
        float originalValue2 = engine.GetMaxSpeed();
        float originalValue3 = engine.GetRotationSpeed();
        float originalValue4 = engine.GetStopDamping();

        engine.SetThrustForce(originalValue * multiplier);
        engine.SetMaxSpeed(originalValue2 * multiplier);
        engine.SetRotationSpeed(originalValue3 * multiplier);
        engine.SetStopDamping(originalValue4 * multiplier);

        // Wait for the specified duration
        yield return new WaitForSecondsRealtime(duration);
        // Revert the value back to the original
        engine.SetThrustForce(originalValue);
        engine.SetMaxSpeed(originalValue2);
        engine.SetRotationSpeed(originalValue3);
        engine.SetStopDamping(originalValue4);
    }

    IEnumerator CannonCoroutine(GameObject player, float multiplier, float duration)
    {
        Cannon cannon = player.GetComponent<Cannon>();
        float originalValue = cannon.GetReloadSpeed();
        float originalValue3 = cannon.GetBulletSpeed();
        float originalValue4 = cannon.GetBulletDamage();

        cannon.SetReloadSpeed(cannon.GetReloadSpeed() / multiplier);
        cannon.SetBulletSpeed(cannon.GetBulletSpeed() * multiplier);
        cannon.SetBulletDamage(cannon.GetBulletDamage() * multiplier);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Revert the value back to the original
        cannon.SetReloadSpeed(originalValue);
        cannon.SetBulletSpeed(originalValue3);
        cannon.SetBulletDamage(originalValue4);

        cannon.ResetLastFiredAngle();
    }

    IEnumerator DodgeCoroutine(GameObject player, float multiplier, float duration)
    {
        Health health = player.GetComponent<Health>();
        float originalValue = health.GetDodgeChance();
        Cannon cannon = player.GetComponent<Cannon>();

        health.SetDodgeChance(health.GetDodgeChance() * multiplier);

        // Wait for the specified duration
        yield return new WaitForSecondsRealtime(duration);

        // Revert the value back to the original
        health.SetDodgeChance(originalValue);
    }
    
}
