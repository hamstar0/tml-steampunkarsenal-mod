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

		public abstract float BoilerTemperature { get; }



		////////////////

		public abstract void SetBoilerHeat( float heatAmount );


		////////////////

		public void EmitSteam( Vector2 position, float steamAmount ) {
			float waterOverflow = steamAmount / this.WaterTemperature;

			//

			waterOverflow = -this.AddWater( -waterOverflow, this.WaterTemperature, out _ );

			steamAmount = waterOverflow * this.WaterTemperature;

			//

			Fx.CreateSteamEruptionFx( position, steamAmount );
		}


		////////////////

		internal protected abstract void PreUpdate( Player owner );

		internal protected abstract void PostUpdate( Player owner );
	}
}