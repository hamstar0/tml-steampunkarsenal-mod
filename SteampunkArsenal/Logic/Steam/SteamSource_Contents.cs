using System;
using Terraria;


namespace SteampunkArsenal.Logic.Steam {
	public abstract partial class SteamSource {
		public float AddWater_If( float addedWater, float heatOfAddedWater, out float waterOverflow ) {
			if( !this.IsActive ) {
				waterOverflow = addedWater;
				return 0f;
			}

			return this.AddWater( addedWater, heatOfAddedWater, out waterOverflow );
		}

		protected abstract float AddWater( float addedWater, float heatOfAddedWater, out float waterOverflow );


		////

		public float DrainWater_If( float waterDrained, out float waterUnderflow ) {
			if( !this.IsActive ) {
				waterUnderflow = waterDrained;
				return 0f;
			}

			return this.DrainWater( waterDrained, out waterUnderflow );
		}

		protected abstract float DrainWater( float waterDrained, out float waterUnderflow );


		////////////////

		public float TransferWaterToMeFromSource_If(
					SteamSource source,
					float intendedWaterXferAmt,
					out float waterUnderflow,
					out float waterOverflow ) {
			if( !this.IsActive ) {
				waterUnderflow = intendedWaterXferAmt;
				waterOverflow = 0f;
				return 0f;
			}

			if( source.TotalPressure <= 0f ) {
				waterUnderflow = intendedWaterXferAmt;
				waterOverflow = 0f;
				return 0f;
			}

			//

			float drawnWater = source.DrainWater_If( intendedWaterXferAmt, out waterUnderflow );
			if( drawnWater <= 0f ) {
				waterOverflow = 0f;
				return 0f;
			}

			//

			return this.AddWater( drawnWater, source.WaterHeat, out waterOverflow );
		}


		////

		public float TransferSteamToMeFromSource_If(
					SteamSource source,
					float intendedSteamXferAmt,
					out float waterUnderflow,
					out float waterOverflow ) {
			if( !this.IsActive ) {
				waterUnderflow = intendedSteamXferAmt / source.WaterHeat;
				waterOverflow = 0f;
				return 0f;
			}

			if( source.SteamPressure <= 0f ) {
				waterUnderflow = intendedSteamXferAmt;
				waterOverflow = 0f;
				return 0f;
			}

			//

			if( intendedSteamXferAmt > source.SteamPressure ) {
				intendedSteamXferAmt = source.SteamPressure;
			}

			float srcHeat = source.WaterHeat;
			float srcWaterDrawAmt = intendedSteamXferAmt / srcHeat;

			float drawnWater = source.DrainWater_If( srcWaterDrawAmt, out waterUnderflow );
			if( drawnWater <= 0f ) {
				waterOverflow = 0f;
				return 0f;
			}

			//

			float finalAddedWater = this.AddWater( drawnWater, srcHeat, out waterOverflow );

			return finalAddedWater * srcHeat;
		}
	}
}