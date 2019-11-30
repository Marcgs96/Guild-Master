using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMember : Member
{
    override public void GenerateInfo()
    {
        base.GenerateInfo();
        member_name = "Magox";
        type = MEMBER_TYPE.MAGE;
    }
}
