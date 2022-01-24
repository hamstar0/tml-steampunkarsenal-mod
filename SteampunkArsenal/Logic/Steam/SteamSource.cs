using System;
using Terraria;
using SteampunkArsenal.Items;
using SteampunkArsenal.Items.Armor;
using SteampunkArsenal.Items.Accessories;


namespace SteampunkArsenal.Logic.Steam {
	public enum PlumbingType {
		Manual,
		Inventory,
		Worn
	}




	public abstract partial class SteamSource {
		public static SteamSource GetSteamSourceForItem( Item item ) {
			if( item?.active != true || item.modItem == null ) {
				return null;
			}

			if( item.modItem is RivetLauncherItem ) {
				return ( (RivetLauncherItem)item.modItem ).SteamSupply;
			}

			if( item.modItem is BoilerOBurdenItem ) {
				return ( (BoilerOBurdenItem)item.modItem ).SteamSupply;
			}

			if( item.modItem is PortABoilerItem ) {
				return ( (PortABoilerItem)item.modItem ).SteamSupply;
			}

			if( item.modItem is SteamBallItem ) {
				return ( (SteamBallItem)item.modItem ).SteamSupply;
			}

			return null;
		}



		////////////////

		public bool IsActive => this.TotalCapacity > 0f;

		////

		public abstract float Water { get; }

		public abstract float WaterHeat { get; }

		public abstract float TotalCapacity { get; }

		////
		
		public abstract float WaterLeakPerTick { get; }

		////

		public PlumbingType PlumbingType { get; protected set; }


		////////////////

		public virtual float SteamPressure => this.TotalPressure - this.Water;

		public virtual float TotalPressure => this.Water * this.WaterHeat;



		////////////////

		public SteamSource( PlumbingType plumbingType ) {
			this.PlumbingType = plumbingType;
		}


		////////////////

		internal protected abstract void PreUpdate( Player owner, bool isChild );

		internal protected abstract void PostUpdate( Player owner, bool isChild );
	}
}