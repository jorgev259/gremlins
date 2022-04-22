using UnityEngine;
using TMPro;

public class TwitchListener : MonoBehaviour
{
    public string username;
    private TextMeshPro textMesh;
    private GameObject bubble;
    private GameObject Cheeb;
    private Animator animator;


    [SerializeField]
    private float bubbleTimer = 0;
    private const float bubbleTime = 5;
    private const int disconnectSeconds = 5 * 60;
    [SerializeField]
    private float disconnectTime = 0;
    [SerializeField]
    private string currentText  = "";
    [SerializeField]
    private int currentTextInt = 0;

    static float NextFloat(float min, float max){
        System.Random random = new System.Random();
        double val = random.NextDouble() * (max - min) + min;
        return (float)val;
    }

    void SetTransformX(float n){
        transform.position = new Vector3(n, transform.position.y, transform.position.z);
    }

    void onMessage(ChatMessage m){
        if(m.user == username){
            currentText = m.message;
            currentTextInt = 0;
          
            if(!Cheeb.activeSelf){
                Cheeb.SetActive(true);
                SetTransformX(NextFloat((float)-8.2, (float)8.2));
            }

            bubble.SetActive(true);
            animator.SetBool("talk", true);

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

    void Blink(){
        if(Cheeb.activeSelf) animator.SetTrigger("blink");
    }

    void TypeTalk () {
        if((textMesh.isTextTruncated && currentTextInt > 0) || currentText.Length == currentTextInt) {
            currentText = "";
            currentTextInt = 0;
        } else {
            if(currentText.Length > currentTextInt){
                currentTextInt++;
                textMesh.text = currentText.Substring(0, currentTextInt);

                bubbleTimer = bubbleTime;
                disconnectTime = disconnectSeconds;
            }
        }
    }

    void Start()
    {            
        bubble = transform.Find("Bubble").gameObject;
        textMesh = bubble.GetComponent<TextMeshPro>();
        Cheeb = transform.Find("Cheeb").gameObject;
        animator = GetComponentInChildren<Animator>();

        Cheeb.SetActive(false);
        bubble.SetActive(false);

        TwitchManager.Instance.OnMessage += onMessage;
        TwitchManager.Instance.onConnect += onConnect;
        TwitchManager.Instance.onDisconnect += onDisconnect;

        InvokeRepeating("Blink", 0f, 8f);
        InvokeRepeating("TypeTalk", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {           
        if (bubbleTimer > 0) bubbleTimer -= Time.deltaTime;
        if (disconnectTime > 0) disconnectTime -= Time.deltaTime;

        if (bubbleTimer <= 0  && bubble.activeSelf) {
            animator.SetBool("talk", false);
            bubble.SetActive(false);
        }
        if (disconnectTime <= 0 && Cheeb.activeSelf) Cheeb.SetActive(false);
    }
}
