using UnityEngine;
using TMPro;

public class TwitchListener : MonoBehaviour
{
    public string username;
    private TextMeshPro textMesh;
    private GameObject bubble;
    private GameObject Cheeb;


    [SerializeField]
    private float bubbleTimer = 0;
    private const float bubbleTime = 5;
    private const int disconnectSeconds = 5 * 60;
    [SerializeField]
    private float disconnectTime = 0;

    void onMessage(ChatMessage m){
        if(m.user == username){
            textMesh.text = m.message;

            Cheeb.SetActive(true);
            bubble.SetActive(true);

            bubbleTimer = bubbleTime;
            disconnectTime = disconnectSeconds;
        }
    }

    void onConnect(){
        bubble.SetActive(false);
        Cheeb.SetActive(false);

        disconnectTime = disconnectSeconds;
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

        Cheeb.SetActive(false);
        bubble.SetActive(false);

        TwitchManager.Instance.OnMessage += onMessage;
        TwitchManager.Instance.onConnect += onConnect;
        TwitchManager.Instance.onDisconnect += onDisconnect;
    }

    // Update is called once per frame
    void Update()
    {           
        if (bubbleTimer > 0) bubbleTimer -= Time.deltaTime;
        if (disconnectTime > 0) disconnectTime -= Time.deltaTime;

        if (bubbleTimer <= 0  && bubble.activeSelf) bubble.SetActive(false);
        if (disconnectTime <= 0 && Cheeb.activeSelf) Cheeb.SetActive(false);
    }
}
