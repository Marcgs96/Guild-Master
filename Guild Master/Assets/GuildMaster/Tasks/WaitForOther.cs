using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GuildMaster{

	[Category("Custom")]
	[Description("Waits for other agent to reach destination")]
	public class WaitForOther : ActionTask<Member>{

        public BBParameter<Member> target;


        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
            if(target.value.steer.ReachedDestination())
			    EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
            if (target.value.steer.ReachedDestination())
                EndAction(true);
        }

		//Called when the task is disabled.
		protected override void OnStop(){
			
		}

		//Called when the task is paused.
		protected override void OnPause(){
			
		}
	}
}