using UnityEngine;
using System.Timers;

public class WaddleMove : MonoBehaviour
{
    private Rigidbody rigidBody;
    private GameObject Cheeb;
    private System.Random rand;
    private GameObject bubble;

    [SerializeField]
    private bool walkValue = false;
    [SerializeField]
    private int seed;
    [SerializeField]
    private float decisionTimer = 5;
    [SerializeField]
    private float debugDirection;
    [SerializeField]
    private bool initialized = true;

    private void setVelocity(Vector3 direction){
        rigidBody.velocity = direction;
        debugDirection = rigidBody.velocity.normalized.x;
    }

    private void decideState (){
        int decisionInt = rand.Next(0,5);
        int directionInt = rand.Next(0,2);
        walkValue = decisionInt == 0;   
        
        Animator animator = GetComponentInChildren<Animator>();
        animator?.SetBool("walk", walkValue);

        setVelocity(walkValue ? (directionInt == 0 ? Vector3.right : Vector3.left) : Vector3.zero);    
        decisionTimer = rand.Next(10,21);
    }

    void FixedUpdate(){  
        if(!Cheeb.activeSelf) return;

        if(walkValue){
            float x = Cheeb.transform.position.x;
            if (x > 7.4) setVelocity(Vector3.left);
            else if (x < -5.8) setVelocity(Vector3.right);
        }

        Vector3 lookVector = walkValue ? rigidBody.velocity.normalized : Vector3.back;
        float angleLookSpeed = 55;

        Quaternion rotateAngle = Quaternion.RotateTowards(rigidBody.rotation.normalized, Quaternion.LookRotation(lookVector), Time.fixedDeltaTime * angleLookSpeed);
        rigidBody.MoveRotation(rotateAngle);
    }

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random(seed);
        bubble = transform.Find("Bubble").gameObject;
        Cheeb = transform.Find("Cheeb").gameObject;
        rigidBody = Cheeb.GetComponent<Rigidbody>();

        decisionTimer = rand.Next(5,11);
    }

    // Update is called once per frame
    void Update()
    {
         if(Cheeb.activeSelf != initialized){
            if(Cheeb.activeSelf) decisionTimer = rand.Next(5,11) + Time.deltaTime;
            initialized = Cheeb.activeSelf;
        }

        if(!Cheeb.activeSelf) return;
        
        if (decisionTimer > 0) decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0) decideState();

        Vector3 temp = Vector3.zero + new Vector3(rigidBody.transform.position.x,0,0);
        bubble.transform.position = temp;
    }
}
