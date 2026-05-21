using UnityEngine;

public class ScriptStorage : MonoBehaviour
{
    public string[] dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Dialogue.Instance.lines = dialogue;
            Dialogue.Instance.StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Dialogue.Instance.StopAllCoroutines();
            Dialogue.Instance.canvasGroup.alpha = 0;
            Dialogue.Instance.canvasGroup.blocksRaycasts = false;
        }
    }
}
