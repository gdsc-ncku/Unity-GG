using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 在道具UI介面的訊息撥放欄位
/// </summary>
public class NotificationTeller : MonoBehaviour
{
    public TextMeshProUGUI[] notificationTexts = new TextMeshProUGUI[4];  // 4 個 UI Text
    private Queue<string> messageQueue = new Queue<string>(); // 存放訊息的佇列
    private Coroutine displayCoroutine;
    private bool isPaused = false; // UI 是否關閉

    public float displayDuration = 2f; // 訊息顯示時間
    public float fadeDuration = 0.5f;  // 訊息淡入/淡出時間

    void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        // 初始化：清空 UI
        foreach (TextMeshProUGUI text in notificationTexts)
        {
            text.text = "";
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0); // 設為透明
        }
    }

    /// <summary>
    /// 顯示要傳達的資訊
    /// </summary>
    /// <param name="message"></param>
    private void ShowMessage(string message)
    {
        messageQueue.Enqueue(message);  // 加入新訊息

        if (displayCoroutine == null && !isPaused)
        {
            displayCoroutine = StartCoroutine(DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        while (messageQueue.Count > 0)
        {
            if (isPaused) // 如果 UI 關閉，就等 UI 開啟後再顯示
            {
                yield return new WaitUntil(() => !isPaused);
            }

            // 1. 取出新訊息
            string newMessage = messageQueue.Dequeue();

            // 2. 上移舊的訊息
            for (int i = notificationTexts.Length - 1; i > 0; i--)
            {
                notificationTexts[i].text = notificationTexts[i - 1].text;
                notificationTexts[i].color = notificationTexts[i - 1].color;
            }

            // 3. 顯示新訊息
            notificationTexts[0].text = newMessage;
            StartCoroutine(FadeText(notificationTexts[0], 0, 1, fadeDuration)); // 淡入

            // 4. 等待顯示時間
            yield return new WaitForSeconds(displayDuration);

            // 5. 移除最舊的訊息（淡出最下面的文字）
            StartCoroutine(FadeText(notificationTexts[notificationTexts.Length - 1], 1, 0, fadeDuration));

            // 6. 等待淡出時間
            yield return new WaitForSeconds(fadeDuration);
        }

        displayCoroutine = null; // 結束 Coroutine
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

        if (endAlpha == 0) text.text = ""; // 淡出後清空文字
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        isPaused = false; // UI 開啟時，恢復訊息顯示

        //初始化UI
        InitUI();
        // 如果佇列內還有訊息，重新啟動 Coroutine
        if (messageQueue.Count > 0 && displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(DisplayMessages());
        }

        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening<string>(
            NameOfEvent.ShowMessage,
            (msg) => ShowMessage(msg)
        ));
    }

    private void OnDisable()
    {
        isPaused = true; // UI 關閉時，暫停訊息顯示
        // 停止 Coroutine 並清除變數
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null; // 這行很重要！
        }

        disposables.Clear(); // 自動取消所有事件訂閱
    }
    #endregion
}
