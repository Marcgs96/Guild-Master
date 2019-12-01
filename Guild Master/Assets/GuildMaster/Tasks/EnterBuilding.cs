using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GuildMaster{

	[Category("Custom")]
	[Description("Enters building ")]
	public class EnterBuilding : ActionTask<Member>
    {
        public Building building;
        public float time_inside = 0.0f;
        private float timer = 0.0f;

        protected override string OnInit(){
			return null;
		}

		protected override void OnExecute(){
            building.EnterBuilding(agent);
            agent.OnBuildingEnter();           
		}

        protected override void OnUpdate()
        {
            if(time_inside > 0.0f)
            {
                timer += Time.deltaTime;
                if(timer >= time_inside)
                {
                    OnStop();
                }
            }
        }

        protected override void OnStop()
        {
            building.RequestExit(agent);
            EndAction(true);
        }
    }
}