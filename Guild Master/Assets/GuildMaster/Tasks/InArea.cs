using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GuildMaster {

    [Category("Custom")]
	[Description("Check if agent is in area")]
	public class InArea : ConditionTask<Transform>{
        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> area = 30;

        protected override bool OnCheck()
        {
            if (Vector3.Distance(target.value.transform.position, agent.position) <= area.value)
                return true;
            else
                return false;
        }

    }
}