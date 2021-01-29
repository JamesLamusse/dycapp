using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RememberItemUI : MonoBehaviour
{
    public string CommitteeText;
    public Text DescriptionText;
    public string Key;

    private UIManager UIMgr;

    private void Awake()
    {
        UIMgr = FindObjectOfType(typeof(UIManager)) as UIManager;
    }

    public void Open()
    {
        UIMgr.OpenEditRememberItem(this);
    }
}
