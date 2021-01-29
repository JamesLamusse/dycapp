using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberList : MonoBehaviour
{
    public List<RememberItemUI> Flag;
    public List<RememberItemUI> Advo;
    public List<RememberItemUI> Enviro;
    public List<RememberItemUI> Sports;
    public List<RememberItemUI> Art;
    public List<RememberItemUI> Feeding;
    public List<RememberItemUI> Marketing;
    public List<RememberItemUI> Other;

    public List<RememberItemUI> All;

    public Transform FlagParent;
    public Transform AdvoParent;
    public Transform EnviroParent;
    public Transform ArtParent;
    public Transform FeedingParent;
    public Transform SportsParent;
    public Transform MarketingParent;
    public Transform OtherParent;

    public GameObject FlagGO;
    public GameObject AdvoGO;
    public GameObject EnviroGO;
    public GameObject ArtGO;
    public GameObject FeedingGO;
    public GameObject SportsGO;
    public GameObject MarketingGO;
    public GameObject OtherGO;

    public RememberItemUI Prefab;
}
