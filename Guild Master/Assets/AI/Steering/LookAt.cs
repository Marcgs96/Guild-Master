using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GuildMaster{

	[Category("Custom")]
	[Description("Look at GameObject")]
	public class LookAt : ActionTask<SteeringAlign>{

        public BBParameter<GameObject> target;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
            if (agent.Steer(target.value.transform.position))
                EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
            if (agent.Steer(target.value.transform.position))
                EndAction(true);
        }

		//Called when the task is disabled.
		protected override void OnStop(){
            EndAction(true);
        }

		//Called when the task is paused.
		protected override void OnPause(){
            EndAction(true);
        }
	}
}