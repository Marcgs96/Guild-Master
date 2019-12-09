using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMember : Member
{
    List<string> names = new List<string> { "Alejandro","Merlin","Gandalf","Magox","Ziqo",
        "Medivh","Antonidas","Veigar","Ryze","Tal Rasha","Lazarus","Zoltun","Vyr","Khadgar","Kel'Thuzad"};

    [Header("Mage")]
    public GameObject potion;
    public GameObject recipe;
    public GameObject material;

    override public void GenerateInfo()
    {
        base.GenerateInfo();
        member_name = names[Random.Range(0, names.Count)];
        type = MEMBER_TYPE.MAGE;
    }

    override public void ChangeState(MEMBER_STATE state, bool force = false)
    {
        base.ChangeState(state, force);

        potion.SetActive(false);
        recipe.SetActive(false);
        material.SetActive(false);
    }   

}
