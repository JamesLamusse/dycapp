using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberItem
{
    public string committee;
    public string description;
    public string key;

    public RememberItem()
    {

    }

    public RememberItem(string Committee, string Description, string Key)
    {
        this.committee = Committee;
        this.description = Description;
        this.key = Key;
    }
}
