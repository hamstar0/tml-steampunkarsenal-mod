using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources.Boilers {
	public partial class TankBoiler : Boiler {
		internal protected override void PreUpdate( Player owner ) {
			float addedTemp = this.Water > 0f
				? this.BoilerTemperature / this.Water   // more water? more heat needed!
				: 0f;

			this._WaterTemperature += addedTemp;

			if( this.WaterTemperature > this.BoilerTemperature ) {
				this._WaterTemperature = this.BoilerTemperature;
			}
		}


		internal protected override void PostUpdate( Player owner ) {
			if( this.SteamPressure > this.Capacity ) {
				this.EmitSteam( owner.MountedCenter, this.SteamPressure - this.Capacity );
			}
		}
	}
}