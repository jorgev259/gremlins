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
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float bubbleTimer = 0;
    
    private static Timer aTimer;
    [SerializeField]
    private bool walkValue = false;
    private const float decisionInterval = 5;
    private const float bubbleTime = 5;
    private System.Random rand = new System.Random();
    [SerializeField]
    private string direction; 

    void onMessage(ChatMessage m){
        if(m.user == username){
            textMesh.text = m.message;
            bubble.SetActive(true);
            bubbleTimer = bubbleTime;
        }
    }

    void onConnect(){
        Cheeb.SetActive(true);
        bubble.SetActive(false);
    }
    void onDisconnect(){
        Cheeb.SetActive(false);
        bubble.SetActive(false);
    }

    void decideState (object sender, ElapsedEventArgs e){
        bool decision = rand.NextDouble() > 0.5;

        if(decision){
            bool decision2 = rand.NextDouble() > 0.5;

            if(decision2) {
                direction = "right";
            } else {
                direction = "left";
            };
        }

        walkValue = decision;
    }

    void Start()
    {     
        bubble = transform.Find("Bubble").gameObject;
        textMesh = bubble.GetComponent<TextMeshPro>();
        Cheeb = transform.Find("Cheeb").gameObject;
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        TwitchManager.Instance.OnMessage += onMessage;
        TwitchManager.Instance.onConnect += onConnect;
        TwitchManager.Instance.onDisconnect += onDisconnect;

        Cheeb.SetActive(false);
        bubble.SetActive(false);

        aTimer = new Timer();
        aTimer.Interval = decisionInterval * 1000;
 
        aTimer.Elapsed += decideState;
        aTimer.Enabled = true;
    }

    void FixedUpdate(){  
        Vector3 vel = Vector3.zero;   

        if(walkValue){
            float x = transform.position.x;
            if (x>8.4) direction = "left";
            else if (x<-8.4) direction = "right";

            if(direction == "right") {
                animator.SetInteger("direction", 2);
                vel = Vector3.right;
            } else if(direction == "left") {
                animator.SetInteger("direction", 1);
                vel = Vector3.left;
            }     
        } else animator.SetInteger("direction", 0);

        rigidBody.velocity = vel;
    }

    // Update is called once per frame
    void Update()
    {           
       animator.SetBool("walk", walkValue);

        if (bubbleTimer > 0) {
            bubbleTimer -= Time.deltaTime;
        }

        if(bubbleTimer <= 0  && bubble.activeSelf) {
           bubble.SetActive(false);
        } 
    }
}
