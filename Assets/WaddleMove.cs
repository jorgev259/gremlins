using UnityEngine;
using System.Timers;

public class WaddleMove : MonoBehaviour
{
    [SerializeField]
    private bool walkValue = false;
    private System.Random rand = new System.Random();

    [SerializeField]
    private string direction; 
    private const float decisionInterval = 5;
    private static Timer aTimer;
    private Rigidbody2D rigidBody;
    private GameObject Cheeb;

    void decideState (object sender, ElapsedEventArgs e){
        // set 1:5 chance to walk
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
        Animator animator = GetComponentInChildren<Animator>();
        if(animator != null){
            Vector3 vel = Vector3.zero;   

            if(walkValue){
                float x = transform.position.x;
                if (x>8.4) direction = "left";
                else if (x<-8.4) direction = "right";

                if(direction == "right") {
                    vel = Vector3.right;
                    Cheeb.transform.rotation = Quaternion.Euler(Cheeb.transform.rotation.x, 90, Cheeb.transform.rotation.z); 
                } else if(direction == "left") {
                    vel = Vector3.left; 
                    Cheeb.transform.rotation = Quaternion.Euler(Cheeb.transform.rotation.x, -95, Cheeb.transform.rotation.z); 
                }     
            } else {
                Cheeb.transform.rotation = Quaternion.Euler(Cheeb.transform.rotation.x, 180, Cheeb.transform.rotation.z); 
            }

            rigidBody.velocity = vel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Cheeb = transform.Find("Cheeb").gameObject;

        aTimer = new Timer();
        aTimer.Interval = decisionInterval * 1000;
 
        aTimer.Elapsed += decideState;
        aTimer.Enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator?.SetBool("walk", walkValue);
    }
}
