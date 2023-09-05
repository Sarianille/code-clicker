using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text message;

    private Vector3 offScreenPosition = new Vector3(-376, 595, 0);
    private Vector3 onScreenPosition = new Vector3(-376, 505, 0);
    private Vector3 position = new Vector3(-376, 595, 0);

    private float speed = 100;

    void Update()
    {
        float step = speed * Time.deltaTime;
        panel.transform.localPosition = Vector3.MoveTowards(panel.transform.localPosition, position, step);
    }

    /// <summary>
    /// Moves the message off screen.
    /// </summary>
    private void HideMessage() => position = offScreenPosition;

    /// <summary>
    /// Shows message for 5 seconds. If a new message is shown, resets the 5 seconds timer.
    /// </summary>
    /// <param name="newMessage">Message to be shown.</param>
    public void ShowMessage(string newMessage)
    {
        message.text = newMessage;
        position = onScreenPosition;

        if (IsInvoking(nameof(HideMessage))) 
        {
            CancelInvoke(nameof(HideMessage));
        }

        Invoke(nameof(HideMessage), 5);
    }
}
