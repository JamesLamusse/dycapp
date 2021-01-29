using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PendingPPnBUI : MonoBehaviour
{
    public Text ProjectName;
    public Text Date;
    public string Key;

    private UIManager UIMgr;

    private void Awake()
    {
        UIMgr = FindObjectOfType(typeof(UIManager)) as UIManager;
    }

    public void Open()
    {
        UIMgr.OpenInstancePendingPPnB(this);   
    }
}
