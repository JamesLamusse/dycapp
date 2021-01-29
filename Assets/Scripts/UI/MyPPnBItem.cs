using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPPnBItem : MonoBehaviour
{
    public Text ProjectNameText;
    public Text CurrentUserName;
    public Text ProjectStatus;
    public Text EvaluationText;
    public string Key;
    public string OwnerID;
    public string RawStatus;
    public string CurrentUserID;
    public string RequestFixComment;
    public string RequestFixUser;
    public bool Evaluated;

    private FirebaseManager FirebaseMgr;
    private UIManager UIMgr;

    private void Awake()
    {
        UIMgr = FindObjectOfType(typeof(UIManager)) as UIManager;
        FirebaseMgr = FindObjectOfType(typeof(FirebaseManager)) as FirebaseManager;
    }

    public void Open()
    {
        if(GetComponent<MyCompletedPPnBItem>()!= null)
        {
            UIMgr.OpenMyPPnBItemInstance(this, GetComponent<MyCompletedPPnBItem>().RawStatus);
        }
        else
        {
            UIMgr.OpenMyPPnBItemInstance(this, null);
        }
    }
}
