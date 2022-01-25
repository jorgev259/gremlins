using UnityEngine;
using System.Timers;

public class WaddleMove : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private bool walkValue = false;
    private System.Random rand = new System.Random();

    [SerializeField]
    private string direction; 
    private const float decisionInterval = 5;
    private static Timer aTimer;
    private Rigidbody2D rigidBody;

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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        aTimer = new Timer();
        aTimer.Interval = decisionInterval * 1000;
 
        aTimer.Elapsed += decideState;
        aTimer.Enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("walk", walkValue);
    }
}