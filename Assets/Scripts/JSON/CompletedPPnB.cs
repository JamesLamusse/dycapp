using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedPPnB
{
    public string projectname;
    public string key;
    public string owner;
    public string completeddate;
    public string evaluationformstatus;

    public CompletedPPnB()
    {

    }

    public CompletedPPnB(string projectname, string key, string owner, string completeddate, string evaluationformstatus)
    {
        this.projectname = projectname;
        this.key = key;
        this.owner = owner;
        this.completeddate = completeddate;
        this.evaluationformstatus = evaluationformstatus;
    }
}
