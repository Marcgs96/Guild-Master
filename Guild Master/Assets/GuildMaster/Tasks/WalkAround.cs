using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace GuildMaster{

	[Category("Custom")]
	public class WalkAround : ActionTask<Member>{
        [RequiredField]
        public float min_distance;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){

           CreateRandomPoint();
		}

        protected void CreateRandomPoint()
        {
            Vector3 random_point = Random.insideUnitSphere * min_distance;
            random_point += agent.transform.position;
            random_point.y = agent.transform.position.y;

            NavMeshHit hit;
            NavMesh.SamplePosition(random_point, out hit, float.PositiveInfinity, 1 << NavMesh.GetAreaFromName("Walkable"));

            random_point = Random.insideUnitSphere * min_distance;
            random_point += hit.position;
            random_point.y = agent.transform.position.y;

            NavMesh.SamplePosition(random_point, out hit, float.PositiveInfinity, 1 << NavMesh.GetAreaFromName("Walkable"));
            agent.steer.CreatePath(hit.position);
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
            if (agent.steer.ReachedDestination())
            {
                CreateRandomPoint();
            }
        }

        protected override void OnStop()
        {
            agent.steer.ClearPath();
        }

        protected override void OnPause()
        {
            OnStop();
        }
    }
}