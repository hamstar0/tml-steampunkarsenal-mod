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


		////

		public float TransferContentsToMeFromSource_If(
					SteamSource source,
					float intendedContentsXferAmt,
					out float waterUnderflow,
					out float waterOverflow ) {
			if( !this.IsActive ) {
				waterUnderflow = intendedContentsXferAmt / source.WaterHeat;
				waterOverflow = 0f;
				return 0f;
			}

			return this.TransferContentsToMeFromSource(
				source,
				intendedContentsXferAmt,
				out waterUnderflow,
				out waterOverflow
			);
		}

		private float TransferContentsToMeFromSource(
					SteamSource source,
					float intendedContentsXferAmt,
					out float waterUnderflow,
					out float waterOverflow ) {
			if( source.TotalPressure <= 0f ) {
				waterUnderflow = intendedContentsXferAmt;
				waterOverflow = 0f;
				return 0f;
			}

			//

			float srcHeat = source.WaterHeat;
			float srcWaterDrawAmt = intendedContentsXferAmt / srcHeat;

			float drawnWater = source.DrainWater_If( srcWaterDrawAmt, out waterUnderflow );
			if( drawnWater <= 0f ) {
				waterOverflow = 0f;
				return 0f;
			}

			//

//float prevHeat = this.WaterHeat;
			float finalAddedWater = this.AddWater( drawnWater, srcHeat, out waterOverflow );
//Main.NewText( "Xferred "+((float)srcWaterDrawAmt).ToString()
//	+" ("+((float)drawnWater).ToString()+")"
//	+" -> "+((float)finalAddedWater).ToString()
//	+", temp1 "+((float)prevHeat).ToString()
//	+" -> temp2 "+((float)this.WaterHeat).ToString()+")"
//);

			return finalAddedWater * srcHeat;
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

			return this.TransferSteamToMeFromSource(
				source,
				intendedSteamXferAmt,
				out waterUnderflow,
				out waterOverflow
			);
		}

		private float TransferSteamToMeFromSource(
					SteamSource source,
					float intendedSteamXferAmt,
					out float waterUnderflow,
					out float waterOverflow ) {
			if( source.SteamPressure <= 0f ) {
				waterUnderflow = intendedSteamXferAmt;
				waterOverflow = 0f;
				return 0f;
			}

			//

			float srcHeat = source.WaterHeat;

			float drawnWater = source.DrainWater_If( intendedSteamXferAmt / srcHeat, out waterUnderflow );
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