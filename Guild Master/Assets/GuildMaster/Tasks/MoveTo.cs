using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GuildMaster{

	[Category("Custom")]
	[Description("Moves the agent to the specified position.")]
	public class MoveTo : ActionTask<SteeringFollowNavMeshPath>
    {
        public GameObject target;

        protected override string OnInit(){
			return null;
        }

		protected override void OnExecute(){
            if(!agent.CreatePath(target.transform.position)) EndAction(true);
        }

		protected override void OnUpdate(){
            if (agent.ReachedDestination()) EndAction(true);
        }

		protected override void OnStop(){
            agent.ClearPath();
        }

        protected override void OnPause(){
            OnStop();
        }
	}
}