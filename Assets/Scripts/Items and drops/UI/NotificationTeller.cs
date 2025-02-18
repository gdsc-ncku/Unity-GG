using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �b�D��UI�������T���������
/// </summary>
public class NotificationTeller : MonoBehaviour
{
    public TextMeshProUGUI[] notificationTexts = new TextMeshProUGUI[4];  // 4 �� UI Text
    private Queue<string> messageQueue = new Queue<string>(); // �s��T������C
    private Coroutine displayCoroutine;
    private bool isPaused = false; // UI �O�_����

    public float displayDuration = 2f; // �T����ܮɶ�
    public float fadeDuration = 0.5f;  // �T���H�J/�H�X�ɶ�

    void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        // ��l�ơG�M�� UI
        foreach (TextMeshProUGUI text in notificationTexts)
        {
            text.text = "";
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0); // �]���z��
        }
    }

    /// <summary>
    /// ��ܭn�ǹF����T
    /// </summary>
    /// <param name="message"></param>
    private void ShowMessage(string message)
    {
        messageQueue.Enqueue(message);  // �[�J�s�T��

        if (displayCoroutine == null && !isPaused)
        {
            displayCoroutine = StartCoroutine(DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        while (messageQueue.Count > 0)
        {
            if (isPaused) // �p�G UI �����A�N�� UI �}�ҫ�A���
            {
                yield return new WaitUntil(() => !isPaused);
            }

            // 1. ���X�s�T��
            string newMessage = messageQueue.Dequeue();

            // 2. �W���ª��T��
            for (int i = notificationTexts.Length - 1; i > 0; i--)
            {
                notificationTexts[i].text = notificationTexts[i - 1].text;
                notificationTexts[i].color = notificationTexts[i - 1].color;
            }

            // 3. ��ܷs�T��
            notificationTexts[0].text = newMessage;
            StartCoroutine(FadeText(notificationTexts[0], 0, 1, fadeDuration)); // �H�J

            // 4. ������ܮɶ�
            yield return new WaitForSeconds(displayDuration);

            // 5. �������ª��T���]�H�X�̤U������r�^
            StartCoroutine(FadeText(notificationTexts[notificationTexts.Length - 1], 1, 0, fadeDuration));

            // 6. ���ݲH�X�ɶ�
            yield return new WaitForSeconds(fadeDuration);
        }

        displayCoroutine = null; // ���� Coroutine
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0;
        Color color = text.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        text.color = color;

        if (endAlpha == 0) text.text = ""; // �H�X��M�Ť�r
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        isPaused = false; // UI �}�ҮɡA��_�T�����

        //��l��UI
        InitUI();
        // �p�G��C���٦��T���A���s�Ұ� Coroutine
        if (messageQueue.Count > 0 && displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(DisplayMessages());
        }

        // ���U��  �ƥ󪺭q�\
        disposables.Add(EventManager.StartListening<string>(
            NameOfEvent.ShowMessage,
            (msg) => ShowMessage(msg)
        ));
    }

    private void OnDisable()
    {
        isPaused = true; // UI �����ɡA�Ȱ��T�����
        // ���� Coroutine �òM���ܼ�
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null; // �o��ܭ��n�I
        }

        disposables.Clear(); // �۰ʨ����Ҧ��ƥ�q�\
    }
    #endregion
}
