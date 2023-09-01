using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text message;
    private Vector3 offScreenPosition = new Vector3(-376, 595, 0);
    private Vector3 onScreenPosition = new Vector3(-376, 505, 0);
    private float speed = 100;
    private float timer = 5;
    bool show;
    bool hide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOffScreen())
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                show = false;
                hide = true;
                timer = 5;
            }
        }

        if (show)
        {
            float step = speed * Time.deltaTime;
            panel.transform.localPosition = Vector3.MoveTowards(panel.transform.localPosition, onScreenPosition, step);
        }

        if (hide) 
        {
            float step = speed * Time.deltaTime;
            panel.transform.localPosition = Vector3.MoveTowards(panel.transform.localPosition, offScreenPosition, step);
        }
    }

    private bool IsOffScreen()
    {
        return panel.transform.localPosition == offScreenPosition;
    }

    public void ShowMessage(string newMessage)
    {
        message.text = newMessage;
        show = true;
        hide = false;
    }
}
