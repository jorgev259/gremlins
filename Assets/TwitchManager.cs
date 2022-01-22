using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Timers;

[System.Serializable]
public class ChatMessage
{
    public string user;
    public string message;
    public string chatName;
}

public class TwitchManager : MonoBehaviour
{
    private  static TwitchManager instance;  
    private TwitchManager(){}

    public static TwitchManager Instance { 		
        get { 			
            if (instance ==  null)
                instance = GameObject.FindObjectOfType(typeof(TwitchManager)) as  TwitchManager; 			
                return instance; 		
        }  		
    }

    public string login;
    public string password; 
    public string channelName;
    public bool connected = false;

    private TcpClient twitchClient; // 2
    private StreamReader reader; // 3
    private StreamWriter writer; 

    private float inactiveTime = 0;
    private static Timer aTimer;
    public delegate void onMessageEvent (ChatMessage m);
    public event onMessageEvent OnMessage;
    public delegate void onConnectEvent();
    public event onConnectEvent onConnect;
    public delegate void onDisconnectEvent();
    public event onDisconnectEvent onDisconnect;

    public void ReadChat() {
        if (twitchClient.Available > 0) {
            inactiveTime = 0;
            string message = reader.ReadLine();

            if(connected == false) {
                connected = true;
                onConnect?.Invoke();
            }     

            if (message.Contains("PRIVMSG")) {
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
                chatMessage.chatName = chatName;

                OnMessage?.Invoke(chatMessage);
            }
        } else {
            inactiveTime += Time.deltaTime;
        }
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667); // 1
        reader = new StreamReader(twitchClient.GetStream()); // 2
        writer = new StreamWriter(twitchClient.GetStream()); 
        writer.WriteLine("PASS " + password); // 3
        writer.WriteLine("NICK " + login); 
        writer.WriteLine("USER " + login + " 8 *:" + login); 
        writer.WriteLine("JOIN #" + channelName); 
        writer.Flush(); 
    }

    private void DoPing (object sender, ElapsedEventArgs e){
        writer.WriteLine("PING");
        writer.Flush(); 
    }

    private void StartTimer(){
        aTimer = new Timer();
        aTimer.Interval = 15 * 1000;
 
        aTimer.Elapsed += DoPing;
        aTimer.Enabled = true;
    }

    // Start is called before the first frame update
    void Awake(){
        Connect();
        StartTimer();
    }

    // Update is called once per frame
    void Update(){   
        if(inactiveTime >= 20){
            inactiveTime = 0;
            connected = false;
            onDisconnect?.Invoke();
            Connect();
        }

        ReadChat();
    }
}
