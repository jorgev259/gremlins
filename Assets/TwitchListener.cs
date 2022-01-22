using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TwitchListener : MonoBehaviour
{
    public string username;
    private TextMeshPro textMesh;
    private GameObject bubble;
    private GameObject Cheeb;
    private float clockDown = 0;
    private const float clockTime = 5;
    void onMessage(ChatMessage m){
        if(m.user == username){
            textMesh.text = m.message;
            bubble.SetActive(true);
            clockDown = clockTime;
        }
    }

    void onConnect(){
        Cheeb.SetActive(true);
    }
    void onDisconnect(){
        Cheeb.SetActive(false);
        bubble.SetActive(false);
    }

    void Start()
    {     
        bubble = transform.Find("Bubble").gameObject;
        textMesh = bubble.GetComponent<TextMeshPro>();
        Cheeb = transform.Find("Cheeb").gameObject;

        TwitchManager.Instance.OnMessage += onMessage;
        TwitchManager.Instance.onConnect += onConnect;
        TwitchManager.Instance.onDisconnect += onDisconnect;

        Cheeb.SetActive(false);
        bubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (clockDown > 0) {
            clockDown -= Time.deltaTime;
        }

        if(clockDown <= 0 && bubble.activeSelf) {
           bubble.SetActive(false);
        } 
    }
}
