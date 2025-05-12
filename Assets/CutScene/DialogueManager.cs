using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import SceneManager

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_DialogueText;
    [SerializeField] private Image m_CharacterImage;
    [SerializeField] private GameObject m_DialogueBox;

    [SerializeField] private string[] m_Dialogues; // Dialogue lines
    [SerializeField] private Sprite[] m_CharacterSprites; // Character images

    [SerializeField] private float m_TypingSpeed = 0.05f; // Speed of letter appearance
    [SerializeField] private float m_EndDelay = 2f; // Time to wait before loading next scene
    [SerializeField] private string m_NextSceneName = "Level1"; // Name of the first level

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        m_DialogueBox.SetActive(true); // Show dialogue box when scene starts
        ShowDialogue();
    }

    void ShowDialogue()
    {
        if (currentIndex < m_Dialogues.Length)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(m_Dialogues[currentIndex]));

            // Set character image (if available)
            if (m_CharacterSprites.Length > currentIndex)
            {
                m_CharacterImage.sprite = m_CharacterSprites[currentIndex];
                m_CharacterImage.SetNativeSize();
            }
        }
        else
        {
            StartCoroutine(EndDialogueSequence()); // Wait and load next scene
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        m_DialogueText.text = "";

        foreach (char letter in text)
        {
            m_DialogueText.text += letter;
            yield return new WaitForSeconds(m_TypingSpeed);
        }

        isTyping = false;
    }

    void Update()
    {
        if (m_DialogueBox.activeSelf && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                m_DialogueText.text = m_Dialogues[currentIndex]; // Show full text instantly
                isTyping = false;
            }
            else
            {
                currentIndex++;
                ShowDialogue();
            }
        }
    }

    IEnumerator EndDialogueSequence()
    {
        yield return new WaitForSeconds(m_EndDelay); // Wait for a few seconds
        SceneManager.LoadScene(m_NextSceneName); // Load the next scene
    }
}
