using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberUser
{
    public string username;
    public string email;
    public string committee;
    public string position;

    public MemberUser()
    {
    }

    public MemberUser(string username, string email, string committee, string position)
    {
        this.username = username;
        this.email = email;
        this.committee = committee;
        this.position = position;
    }
}
