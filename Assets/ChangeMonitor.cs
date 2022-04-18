using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMonitor : MonoBehaviour {
    [SerializeField]
    private bool pressed = false;
    [SerializeField]
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = PlayerPrefs.GetInt("UnitySelectMonitor", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pressed && Input.GetKeyDown(KeyCode.F3)) {
            index++;

            if(index ==  Display.displays.Length) index = 0;

            PlayerPrefs.SetInt("UnitySelectMonitor", index);
            pressed = true;
        }   

        if(Input.GetKeyUp(KeyCode.F3)) {
            pressed = false;
        } 
    }
}
