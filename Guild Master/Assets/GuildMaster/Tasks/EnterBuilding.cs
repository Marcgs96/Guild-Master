using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace GuildMaster{

	[Category("Custom")]
	[Description("Enters building ")]
	public class EnterBuilding : ActionTask{

        Member member;
        public Building building;

        protected override string info
        {
            get { return "Enters " + building.name; }
        }

        protected override string OnInit(){
			return null;
		}

		protected override void OnExecute(){
            building.EnterBuilding(member);
            member.OnBuildingEnter();
		}

		protected override void OnUpdate(){
			
		}

		protected override void OnStop(){
            building.RequestExit(member);
        }

        protected override void OnPause(){
			
		}
	}
}