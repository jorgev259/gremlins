using UnityEngine;
using TMPro;
using System.Timers;

public class TwitchListener : MonoBehaviour
{
    public string username;
    private TextMeshPro textMesh;
    private GameObject bubble;
    private GameObject Cheeb;
    private Animator animator;
    private float clockDown = 0;
    private const float clockTime = 5;
    private static Timer aTimer;
    private bool walkValue = false;

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

    void changeAnim (object sender, ElapsedEventArgs e){
        walkValue = !walkValue;
    }

    void Start()
    {     
        bubble = transform.Find("Bubble").gameObject;
        textMesh = bubble.GetComponent<TextMeshPro>();
        Cheeb = transform.Find("Cheeb").gameObject;
        animator = GetComponentInChildren<Animator>();

        TwitchManager.Instance.OnMessage += onMessage;
        TwitchManager.Instance.onConnect += onConnect;
        TwitchManager.Instance.onDisconnect += onDisconnect;

        Cheeb.SetActive(false);
        bubble.SetActive(false);

        aTimer = new Timer();
        aTimer.Interval = 5 * 1000;
 
        aTimer.Elapsed += changeAnim;
        aTimer.Enabled = true;
    }

    // Update is called once per frame
    void Update()
    {           
       animator.SetBool("walk", walkValue);

        if (clockDown > 0) {
            clockDown -= Time.deltaTime;
        }

        if(clockDown <= 0 && bubble.activeSelf) {
           bubble.SetActive(false);
        } 
    }
}
