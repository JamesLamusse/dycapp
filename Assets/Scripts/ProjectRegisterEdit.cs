using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectRegisterEdit : MonoBehaviour
{
    public string owner;
    public string currentuser;
    public string key;
    public string status;
    public List<string> users;
    public List<ProjectRegisterEditUI> userButtons;
    public Transform userButtonsParent;
    public ProjectRegisterEditUI userButtonPrefab;
    public GameObject MyActiveTopMenu;
    public GameObject MyCompletedTopMenu;

    FirebaseManager FirebaseMgr;

    private void Awake()
    {
        FirebaseMgr = FindObjectOfType(typeof(FirebaseManager)) as FirebaseManager;
    }

    public void LoadNewSelectedUser(string user)
    {
        for (int i = 0; i < userButtons.Count; i++)
        {
            if(userButtons[i].uid == user)
            {
                userButtons[i].IsSelected = true;
                Debug.Log("?HELLO???");
            }
        }
    }

    public void LoadFromProjectRegister(ProjectRegister projectRegister)
    {
        owner = projectRegister.owner;
        currentuser = projectRegister.currentuser;
        key = projectRegister.key;
        status = projectRegister.status;
    }

    public ProjectRegister ReturnProjectRegister()
    {
        return new ProjectRegister(owner, currentuser, key, status);
    }
}
