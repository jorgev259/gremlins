using UnityEngine;
using System.Timers;

public class WaddleMove : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private GameObject Cheeb;
    private System.Random rand;

    [SerializeField]
    private bool walkValue = false;

    [SerializeField]
    private string direction; 

    [SerializeField]
    private int seed;
    [SerializeField]
    private float decisionTimer = 0;

    private void decideState (){
        int decisionInt = rand.Next(0,5);
        bool walk = decisionInt == 0;

        if(walk){
            if(rand.Next(0,2) == 0) {
                direction = "right";
            } else {
                direction = "left";
            };
        }

        walkValue = walk;
        decisionTimer = rand.Next(10,21);
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
        rand = new System.Random(seed);
        rigidBody = GetComponent<Rigidbody2D>();
        Cheeb = transform.Find("Cheeb").gameObject;

        decisionTimer = rand.Next(5,11);
    }

    // Update is called once per frame
    void Update()
    {
        if (decisionTimer > 0) decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0) decideState();

        Animator animator = GetComponentInChildren<Animator>();
        animator?.SetBool("walk", walkValue);
    }
}
