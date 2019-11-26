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

    //make locations/buildings manager ?
    public enum LOCATION_TYPE { TAVERN, GUILD_HALL, BLACKSMITH, MAGE_LOCATION, KNIGHT_LOCATION, HUNTER_LOCATION };
    public Transform[] locations;
    public Building[] buildings;
    public AudioSource[] audios;

    void Awake()
    {
        if (manager != null)
            Destroy(manager);
        else
            manager = this;

        DontDestroyOnLoad(this);
    }
}
