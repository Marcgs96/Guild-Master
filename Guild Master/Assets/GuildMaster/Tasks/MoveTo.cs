using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GuildMaster{

	[Category("Custom")]
	[Description("Moves the agent to the specified position.")]
	public class MoveTo : ActionTask{

        public GameObject target;
        SteeringFollowNavMeshPath path;

        protected override string info
        {
            get { return "MoveTo " + target.name; }
        }

        protected override string OnInit(){
			return null;
		}

		protected override void OnExecute(){
            if(!path.CreatePath(target.transform.position)) EndAction(true);
        }

		protected override void OnUpdate(){
            if (path.ReachedDestination()) EndAction(true);
        }

		protected override void OnStop(){
            path.ClearPath();
		}

		protected override void OnPause(){
            OnStop();
        }
	}
}