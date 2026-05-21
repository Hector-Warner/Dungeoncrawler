using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class Dialogue : MonoBehaviour
{
    public static Dialogue Instance { get; private set; }

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private bool isTyping;

    private int index;

    public CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping == false)
            {
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                isTyping = false;
            }

        }
    }

    public void StartDialogue()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    void nextLine()
    {
        Debug.Log(index);
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
