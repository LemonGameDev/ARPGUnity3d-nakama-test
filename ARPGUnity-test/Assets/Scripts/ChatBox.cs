using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects;

public class ChatBox : MonoBehaviour
{
    [SerializeField] private Transform contentContainer;

    [SerializeField] private Text textBoxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        BoxTextController.SetChatBox(this);
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void ShowText(string msg, Color color)
    {
        UnityMainThread.wkr.AddJob(() =>
        {
            var newText = Instantiate(textBoxPrefab, contentContainer);
            newText.text = msg;
            newText.color = color;
        });
    }
}