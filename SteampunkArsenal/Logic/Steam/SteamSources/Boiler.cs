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

		public bool SetBoilerHeat_If( float heatAmount ) {
			if( this.IsActive ) {
				this.SetBoilerHeat( heatAmount );
			}

			return this.IsActive;
		}

		protected abstract void SetBoilerHeat( float heatAmount );


		////////////////

		public void EmitSteam_If( Vector2 position, float steamAmount ) {
			if( this.IsActive ) {
				this.EmitSteam( position, steamAmount );
			}
		}

		protected void EmitSteam( Vector2 position, float steamAmount ) {
			float waterDrainAmount = steamAmount / this.WaterHeat;
			waterDrainAmount = this.DrainWater( waterDrainAmount, out _ );

			float drainedSteamAmount = waterDrainAmount * this.WaterHeat;
			
			//

//Main.NewText("emit "+drainedSteamAmount );
			Fx.CreateSteamEruptionFx(
				position: position,
				dispersalRadius: 16f,
				steamAmount: Math.Min( 8f, drainedSteamAmount )
			);
		}
	}
}