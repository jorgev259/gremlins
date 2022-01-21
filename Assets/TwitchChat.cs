using UnityEngine;
using System.IO;
using System.Net.Sockets;
using TMPro;

[System.Serializable]
public class ChatMessage
{
    public string user;
    public string message;
}

public class TwitchChat : MonoBehaviour
{
    private TextMeshPro textMesh;
    private SpriteRenderer spriteMesh;
    public string loginUsername;
    public string username; // 1
    public string password; 
    public string channelName; 
    public float clockDown = 0;

    private TcpClient twitchClient; // 2
    private StreamReader reader; // 3
    private StreamWriter writer; 
    private float reconnectTimer; // 4 
    private float reconnectAfter; 

    public ChatMessage ReadChat() // 1
    {
        if (twitchClient.Available > 0)
        {
            string message = reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                // Get the username
                int splitPoint = message.IndexOf("!", 1); // 2
                string chatName = message.Substring(0, splitPoint); 
                chatName = chatName.Substring(1);

                //Get the message
                splitPoint = message.IndexOf(":", 1); 
                message = message.Substring(splitPoint + 1);

                ChatMessage chatMessage = new ChatMessage(); // 3
                chatMessage.user = chatName;
                chatMessage.message = message;

                if(chatMessage.user == username) {
                    textMesh.GetComponent<Renderer>().enabled = true;
                    clockDown = 5;
                    textMesh.text = chatMessage.message;
                }

                return chatMessage;
            }

        }
        return null; // 4
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667); // 1
        reader = new StreamReader(twitchClient.GetStream()); // 2
        writer = new StreamWriter(twitchClient.GetStream()); 
        writer.WriteLine("PASS " + password); // 3
        writer.WriteLine("NICK " + username); 
        writer.WriteLine("USER " + username + " 8 *:" + username); 
        writer.WriteLine("JOIN #" + channelName); 
        writer.Flush(); 
    }

    private void OnTest(string data)
    {
        Debug.Log("received test!");
        print(data);
    }


    // Start is called before the first frame update
    void Start(){
        textMesh = GetComponentInParent<TextMeshPro>();
        spriteMesh = GetComponentInParent<SpriteRenderer>();

        textMesh.GetComponent<Renderer>().enabled = false;
        reconnectAfter = 60.0f;
        Connect();   
    }

    // Update is called once per frame
    void Update()
    {
        if (clockDown > 0)
        {
            clockDown -= Time.deltaTime;
        } 

        if(clockDown <= 0 && textMesh.GetComponent<Renderer>().enabled) {
            textMesh.GetComponent<Renderer>().enabled = false;
        }

        if (twitchClient.Available == 0) // 1
        {
            reconnectTimer += Time.deltaTime; 
        }

        if (twitchClient.Available == 0 && reconnectTimer >= reconnectAfter) // 2
        {
            Connect(); 
            reconnectTimer = 0.0f; 
        }

        ReadChat(); // 3
    }
}
