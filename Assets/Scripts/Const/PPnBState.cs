using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PPnBState 
{
    public const string APPROVED = "approved";
    public const string DRAFT = "draft";
    public const string PENDING = "pending";
    public const string COMPLETED = "completed";
    public const string ACTIVE = "active";
    public const string REQUEST_FIX = "declined";
    public const string LOGGED = "logged";
    public const string DECLINED = "deletedeclined";

    public static string ReturnApplicableStatus(string s)
    {
        string returnstring = "";

        switch (s)
        {
            case (APPROVED):
                returnstring = "APPROVED";
                break;
            case (DRAFT):
                returnstring = "DRAFT";
                break;
            case (PENDING):
                returnstring = "PENDING";
                break;
            case (COMPLETED):
                returnstring = "COMPLETED";
                break;
            case (ACTIVE):
                returnstring = "ACTIVE";
                break;
            case (REQUEST_FIX):
                returnstring = "NEEDS FIX";
                break;
            case (DECLINED):
                returnstring = "DECLINED";
                break;
            case (""):
                Debug.LogError("No status - Database Error ?");
                returnstring = "Database Error";
                break;
        }

        return returnstring;
    }
}
