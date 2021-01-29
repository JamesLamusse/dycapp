using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text UserName;
    public Text Email;
    public Text Hours;
    public Text Committee;
    public Text Position;
    public Text ProjectsDone;
    public Text ProjectsGoneTo;
    public Text PhoneNumber;

    // Start is called before the first frame update
    public void LoadFromUser_UserA(User user, UserAnalytics userAnalytics)
    {
        UserName.text = user.username;
        Email.text = user.email;
        Hours.text = userAnalytics.hours.ToString();
        Committee.text = user.committee;
        Position.text = user.position;
        ProjectsDone.text = userAnalytics.projectsdone.ToString();
        ProjectsGoneTo.text = userAnalytics.projectsgoneto.ToString();
        PhoneNumber.text = user.phonenumber;
    }
}
