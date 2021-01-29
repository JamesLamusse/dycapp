using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string username;
    public string email;
    public string nextuser;
    public string committee;
    public string position;
    public string secuser;
    public string phonenumber;

    public User()
    {
    }

    public User(string username, string email,string nextuser, string committee, string position, string secuser)
    {
        this.username = username;
        this.email = email;
        this.nextuser = nextuser;
        this.committee = committee;
        this.position = position;
        this.secuser = secuser;
    }
}
