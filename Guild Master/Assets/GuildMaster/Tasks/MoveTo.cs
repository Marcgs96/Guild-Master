using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GuildMaster{

	[Category("Custom")]
	[Description("Moves the agent to the specified position.")]
	public class MoveTo : ActionTask<Member>
    {
        public BBParameter<GameObject> target;

        protected override string OnInit(){
			return null;
        }

		protected override void OnExecute(){
            if(!agent.steer.CreatePath(target.value.transform.position)) EndAction(true);
        }

		protected override void OnUpdate(){
            if (agent.steer.ReachedDestination()) EndAction(true);
        }

		protected override void OnStop(){
            agent.steer.ClearPath();
        }

        protected override void OnPause(){
            OnStop();
        }
	}
}