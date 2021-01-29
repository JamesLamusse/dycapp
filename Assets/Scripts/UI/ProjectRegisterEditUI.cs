using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectRegisterEditUI : MonoBehaviour
{
    public Text Username;
    public string uid;
    public bool IsSelected;
    public Image SelectedImage;

    ProjectRegisterEdit ProjectEditMgr;

    private void Awake()
    {
        ProjectEditMgr = FindObjectOfType(typeof(ProjectRegisterEdit)) as ProjectRegisterEdit;
    }

    private void Start()
    {
        for (int i = 0; i < ProjectEditMgr.users.Count; i++)
        {
            if (ProjectEditMgr.users[i] == this.uid)
            {
                IsSelected = true;
            }
        }
    }

    private void Update()
    {
        SelectedImage.gameObject.SetActive(IsSelected);
    }

    public void SelectUser()
    {
        if (!IsSelected)
        {
            ProjectEditMgr.users.Add(uid);
            IsSelected = true;
        }
        else
        {
            ProjectEditMgr.users.Remove(uid);
            IsSelected = false;
        }
    }
}
