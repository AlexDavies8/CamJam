using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour
{
    [SerializeField] private float _textSpeed = 10f;
    [SerializeField] private float _dialogueStayTime = 2f;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private Coroutine _dialogueCache;
    
    public void Say(string message)
    {
        if (_dialogueCache != null) StopCoroutine(_dialogueCache);
        _dialogueCache = StartCoroutine(SayCoroutine(message));
    }

    IEnumerator SayCoroutine(string message)
    {
        _dialogueCanvas.SetActive(true);
        _dialogueText.text = message;
        for (float letter = 0; letter < message.Length + 1; letter += Time.deltaTime * _textSpeed)
        {
            yield return null;
            int displayLength = (int)letter;
            _dialogueText.maxVisibleCharacters = displayLength;
        }

        yield return new WaitForSeconds(_dialogueStayTime);
        _dialogueCanvas.SetActive(false);
    }
}
