using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GuildMaster{

	[Category("Custom")]
	public class CustomWander : ActionTask<Member>
    {
        public bool repeat = true;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
			return null;
		}

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            agent.wander.GeneratePoint();
        }

        protected override void OnUpdate()
        {
            if (agent.steer.ReachedDestination())
            {
                if (repeat) agent.wander.GeneratePoint();
                else EndAction(true);
            }
        }

        protected override void OnStop()
        {
           agent.steer.ClearPath();
           agent.OnNewTask();
        }

        protected override void OnPause()
        {
           OnStop();
        }
    }
}