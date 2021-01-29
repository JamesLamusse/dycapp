using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationFormStatus
{
    public string projectname;
    public string owner;
    public string key;
    public string ppnbkey;
    public string currentsecuser;
    public string status;

    public EvaluationFormStatus()
    {

    }

    public EvaluationFormStatus(string projectname, string owner, string key, string ppnbkey, string currentsecuser, string status)
    {
        this.projectname = projectname;
        this.owner = owner;
        this.key = key;
        this.ppnbkey = ppnbkey;
        this.currentsecuser = currentsecuser;
        this.status = status;
    }
}
