using UnityEngine;

public class EnemyMoney : MonoBehaviour
{
    [SerializeField] float m_Gear = 1;

    [SerializeField] float m_Screw = 1;
    [SerializeField] GameObject m_GearPrefab;
    [SerializeField] GameObject m_ScrewPrefab;



    public float GetGear()
    { return m_Gear; }
    public void SetGear(float Gear)
    { m_Gear = Gear; }

    public float GetScrew()
    { return m_Screw; }
    public void SetScrew(float Screw)
    { m_Screw = Screw; }

    public void DropGearandScrew()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        Vector3 randomOffset2 = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

        for (int i = 0; i < m_Gear; i++)
        {
            Instantiate(m_GearPrefab, transform.position + randomOffset, Quaternion.identity);
        }
        for (int i = 0; i < m_Screw; i++)
        {
            Instantiate(m_ScrewPrefab, transform.position + randomOffset2, Quaternion.identity);
        }
    }



}
