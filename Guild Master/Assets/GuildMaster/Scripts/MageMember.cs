using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMember : Member
{
    override public void GenerateInfo()
    {
        base.GenerateInfo();
        info.name = "Magox";
        info.type = MEMBER_TYPE.MAGE;
    }
}
