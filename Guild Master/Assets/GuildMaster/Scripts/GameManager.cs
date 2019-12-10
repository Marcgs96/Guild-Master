using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public MemberManager members;
    public ResourceManager resources;
    public QuestManager quests;
    public UIManager ui;
    public DayNightCicle time;
    public LocationManager locations;

    //make locations/buildings manager ?
    public Building[] buildings;
    public AudioSource[] audios;

    void Awake()
    {
        manager = this;
    }
}
