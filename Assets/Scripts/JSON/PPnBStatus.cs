using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPnBStatus
{
    public string key;
    public string status;
    public string owner;
    public string currentuser;
    public string projectname;
    public string requestfixcomment;
    public string requestfixuser;
    public bool evaluated;

    public PPnBStatus()
    {

    }

    public PPnBStatus(string Key, string Status, string Owner, string CurrentUser, string ProjectName,string RequestFixUser, string RequestFixComment, bool Evaluated)
    {
        this.key = Key;
        this.status = Status;
        this.owner = Owner;
        this.currentuser = CurrentUser;
        this.projectname = ProjectName;
        this.requestfixcomment = RequestFixComment;
        this.requestfixuser = RequestFixUser;
        this.evaluated = Evaluated;
    }

}
