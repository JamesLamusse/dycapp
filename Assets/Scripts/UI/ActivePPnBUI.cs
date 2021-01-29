using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivePPnBUI : MonoBehaviour
{
    public Text ProjectName;
    public Text Owner;
    public Text StartDate;
    public Text Committee;
    public string EndDate;
    public string Venue;
    public string Key;

    private UIManager UIMgr;

    private void Awake()
    {
        UIMgr = FindObjectOfType(typeof(UIManager)) as UIManager;
    }

    public void Open()
    {
        UIMgr.OpenInstanceActivePPnB(this);
    }
}
