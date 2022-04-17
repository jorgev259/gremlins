using UnityEngine;
using TMPro;

public class DisconnectCircle : MonoBehaviour
{

    void onConnect(){
        transform.localScale = Vector3.zero;
    }
    void onDisconnect(){
        transform.localScale = new Vector3(1,1,1);
    }

    void Start()
    {            
        TwitchManager.Instance.onConnect += onConnect;
        TwitchManager.Instance.onDisconnect += onDisconnect;
    }
}
