using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SteampunkArsenal.Logic.Steam;
using SteampunkArsenal.Logic.Steam.SteamSources;
using SteampunkArsenal.Logic.Steam.SteamSources.Boilers;
using SteampunkArsenal.Items;


namespace SteampunkArsenal {
	partial class SteamArsePlayer : ModPlayer {
		public float CurrentBodyLayerShakeAmount { get; internal set; } = 0f;


		////////////////

		internal ConvergentBoiler ImplicitConvergingBoiler { get; private set; }
			= new ConvergentBoiler( PlumbingType.Manual );



		////////////////

		public override void PreUpdate() {
			// Update non-convergent boilers in inventory
			foreach( Item item in this.player.inventory ) {
				Boiler boiler = Boiler.GetBoilerForItem( item );

				if( boiler?.IsActive ?? false ) {
					boiler.PreUpdate( this.player, false );
					boiler.PostUpdate( this.player, false );
				}
			}

			//
			
			this.UpdateBoiler();

			if( SteampunkArsenalConfig.Instance.DebugModeInfo && this.player.whoAmI == Main.myPlayer ) {
				DebugLibraries.Print(
					"boilers",
					"Water: " + this.ImplicitConvergingBoiler.Water.ToString( "N2" )
						+ ", BTemp: " + this.ImplicitConvergingBoiler.BoilerHeat.ToString( "N2" )
						+ ", WTemp: " + this.ImplicitConvergingBoiler.WaterHeat.ToString( "N2" )
						+ ", Steam: " + this.ImplicitConvergingBoiler.TotalPressure.ToString( "N2" )
						+ ", Gun: " + SteamSource.GetSteamSourceForItem( this.player.HeldItem )?.TotalPressure.ToString( "N2" )
				);
			}
		}


		////////////////
		
		public override bool PreItemCheck() {
			Item item = this.player.HeldItem;

			if( item?.active == true ) {
				if( item.type == ModContent.ItemType<RivetLauncherItem>() ) {
					RivetLauncherItem.RunHeldBehavior_Local_If( this.player, item );
				}
			}

			return base.PreItemCheck();
		}


		////////////////

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			int bodyLayerIdx = layers.FindIndex( layer => layer == PlayerLayer.Body );
			if( bodyLayerIdx != -1 ) {
				(var wrap1, var wrap2) = OffsetWrapperPlayerLayer.CreateLayers(
					baseLayer: PlayerLayer.Body,
					shakeAmountGetter: () => this.CurrentBodyLayerShakeAmount
				);

				layers.Insert( bodyLayerIdx+1, wrap2 );
				layers.Insert( bodyLayerIdx, wrap1 );
			}
		}
	}
}