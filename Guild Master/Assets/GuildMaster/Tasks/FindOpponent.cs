using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GuildMaster{

	[Category("Custom")]
	public class FindOpponent : ActionTask<KnightMember>{

        public BBParameter<List<KnightMember>> free_members;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
            if(free_members.value.Count > 0)
            {
                foreach (KnightMember free_agent in free_members.value)
                {
                    GameManager.manager.locations.AssignDuelLocations(free_agent, agent);
                    free_members.value.Remove(free_agent);
                    EndAction(true);
                    return;
                }
            }
            free_members.value.Add(agent);
            EndAction(true);
        }
	}
}