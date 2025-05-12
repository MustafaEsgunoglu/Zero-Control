using UnityEngine;

public class ChangeSprites : MonoBehaviour
{
    [Header("Ship Parts")]
    [SerializeField] GameObject[] m_Parts;

    // Body, engines and cannon

    [Header("Upgraded Ship Sprites")]
    [SerializeField] Sprite[] m_Bodies;
    [SerializeField] Sprite[] m_Engines;
    [SerializeField] Sprite[] m_Cannons;
    [SerializeField] RuntimeAnimatorController[] m_Animators;

    Sprite[][] m_Sprites;

    // body1, body2, body3, body4, body5
    // engines1, engines2, engines3, engines4, engines5
    // cannon1, cannon2, cannon3, cannon4, cannon5


    void Awake()
    {
        m_Sprites = new Sprite[][] { m_Bodies, m_Engines, m_Cannons };
    }

    public void ChangeSprite(int section, int number)
    {
        m_Parts[section].GetComponent<SpriteRenderer>().sprite = m_Sprites[section][number];
    }

    public void ChangeAnimator(int number)
    {
        if (number < 0 || number >= m_Animators.Length)
        {
            return;
        }

        Animator animator = m_Parts[2].GetComponent<Animator>(); // Assuming m_Parts[2] is the cannon
        animator.runtimeAnimatorController = m_Animators[number];
    }
}