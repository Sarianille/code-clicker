using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text message;

    private Vector3 offScreenPosition = new Vector3(-376, 595, 0);
    private Vector3 onScreenPosition = new Vector3(-376, 505, 0);
    private Vector3 position = new Vector3(-376, 595, 0);

    private float speed = 100;

    void Update()
    {
        float step = speed * Time.deltaTime;
        panel.transform.localPosition = Vector3.MoveTowards(panel.transform.localPosition, position, step);
    }

    private void HideMessage() => position = offScreenPosition;

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
