using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    MemberManager members;
    ResourceManager resources;

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
