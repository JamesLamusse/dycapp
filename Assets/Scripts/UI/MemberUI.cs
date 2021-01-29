using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberUI : MonoBehaviour
{
    public Text Name;
    public string Key;
    public string Committee;
    public string Position;
    public string Email;

    private UIManager UIMgr;

    private void Awake()
    {
        UIMgr = FindObjectOfType(typeof(UIManager)) as UIManager;
    }

    public void Open()
    {
        UIMgr.OpenInstanceMembersPOP(this);
    }
}
