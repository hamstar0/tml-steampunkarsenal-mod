using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SteampunkArsenal.Logic.Steam.SteamSources {
	public abstract class Boiler : SteamSource {
		public static Boiler GetBoilerForItem( Item item ) {
			SteamSource src = SteamSource.GetSteamSourceForItem( item );
			if( src == null || !(src is Boiler) ) {
				return null;
			}

			return src as Boiler;
		}



		////////////////
		
		public Boiler( bool canConverge ) : base( canConverge ) { }



		////////////////

		public abstract float BoilerHeat { get; }



		////////////////

		public abstract void SetBoilerHeat( float heatAmount );


		////////////////

		public void EmitSteam( Vector2 position, float steamAmount ) {
			float waterDrainAmount = steamAmount / this.WaterHeat;

			//

			waterDrainAmount = this.DrainWater( waterDrainAmount, out _ );

			steamAmount = waterDrainAmount * this.WaterHeat;

			//

			Fx.CreateSteamEruptionFx( position, steamAmount );
		}
	}
}