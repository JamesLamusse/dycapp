using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePPnB
{
    public string key;
    public string startdate;
    public string enddate;
    public string venue;
    public string owner;
    public string projectname;
    public string committee;

    public ActivePPnB()
    {

    }

    public ActivePPnB(string key, string startdate, string enddate, string venue, string owner, string projectname, string committee)
    {
        this.key = key;
        this.startdate = startdate;
        this.enddate = enddate;
        this.venue = venue;
        this.owner = owner;
        this.projectname = projectname;
        this.committee = committee;
    }
}
