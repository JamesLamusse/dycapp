using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectRegister
{
    public string owner;
    public string currentuser;
    public string key;
    public string status;

    public ProjectRegister()
    {

    }

    public ProjectRegister(string owner, string currentuser, string key, string status)
    {
        this.owner = owner;
        this.currentuser = currentuser;
        this.key = key;
        this.status = status;
    }
}
