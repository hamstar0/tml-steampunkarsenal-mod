using System;
using Terraria;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;
using SteampunkArsenal.Items.Accessories;


namespace SteampunkArsenal.Logic.Steam {
	public abstract partial class SteamSource {
		public static SteamSource GetSteamSourceForItem( Item item ) {
			if( item?.active != true || item.modItem == null ) {
				return null;
			}

			if( item.modItem is RivetLauncherItem ) {
				return ( (RivetLauncherItem)item.modItem ).SteamSupply;
			}

			if( item.modItem is BoilerOBurdenItem ) {
				return ( (BoilerOBurdenItem)item.modItem ).MyBoiler;
			}

			if( item.modItem is PortABoilerItem ) {
				return ( (PortABoilerItem)item.modItem ).MyBoiler;
			}

			if( item.modItem is SteamBallItem ) {
				return ( (SteamBallItem)item.modItem ).SteamSupply;
			}

			return null;
		}



		////////////////

		public bool IsActive => this.SteamCapacity > 0f;

		////

		public abstract float Water { get; }

		public abstract float WaterHeat { get; }

		public abstract float SteamCapacity { get; }

		////

		public bool CanConverge { get; protected set; }


		////////////////

		public virtual float SteamPressure => this.Water * this.WaterHeat;



		////////////////

		public SteamSource( bool canConverge ) {
			this.CanConverge = canConverge;
		}


		////////////////

		public abstract float AddWater( float addedWater, float heatOfAddedWater, out float waterOverflow );

		public abstract float DrainWater( float waterDrained, out float waterUnderflow );


		////

		public float TransferPressureToMeFromSource(
					SteamSource source,
					float steam,
					out float waterUnderflow,
					out float waterOverflow ) {
			if( source.SteamPressure <= 0f ) {
				waterUnderflow = steam;
				waterOverflow = 0f;
				return 0f;
			}

			//

			float srcHeat = source.WaterHeat;
			float srcWaterDrawAmt = steam / srcHeat;

			float drawnWater = source.DrainWater( srcWaterDrawAmt, out waterUnderflow );
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


		////////////////

		internal protected abstract void PreUpdate( Player owner, bool isChild );

		internal protected abstract void PostUpdate( Player owner, bool isChild );
	}
}