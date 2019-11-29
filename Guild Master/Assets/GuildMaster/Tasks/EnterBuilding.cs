using NodeCanvas.Framework;
using ParadoxNotion.Design;



namespace GuildMaster{

	[Category("Custom")]
	[Description("Enters building ")]
	public class EnterBuilding : ActionTask<Member>
    {
        public Building building;

        protected override string OnInit(){
			return null;
		}

		protected override void OnExecute(){
            building.EnterBuilding(agent);
            agent.OnBuildingEnter();
		}

		protected override void OnUpdate(){
			
		}

		protected override void OnStop(){
            building.RequestExit(agent);
        }

        protected override void OnPause(){
			
		}
	}
}