using Terraria;
using Terraria.ModLoader;
using SteampunkArsenal.Items.Armor;


namespace SteampunkArsenal {
	public class Boiler {
		public static void RefreshConnectedBoilers( Player player, ConvergentBoiler boiler ) {
			boiler.ConnectedBoilers.Clear();

			//

			Item bodyArmor = player.armor[1];

			if( bodyArmor?.active == true && bodyArmor.type == ModContent.ItemType<BoilerOBurdenItem>() ) {
				var myitem = bodyArmor.modItem as BoilerOBurdenItem;

				if( myitem != null ) {
					boiler.ConnectedBoilers.Add( myitem.MyBoiler );
				}
			}

			//

			/*int beg = PlayerItemLibraries.VanillaAccessorySlotFirst;
			int max = PlayerItemLibraries.GetCurrentVanillaMaxAccessories( player );
			int end = beg + max;

			for( int i = beg; i < end; i++ ) {
				Item item = player.armor[i];
				var myitem = item?.modItem as PortABoilerItem;
				if( myitem == null ) {
					continue;
				}

				boiler.ConnectedBoilers.Add( myitem.Boiler );
			}*/
		}



		////////////////

		public virtual float Water { get; protected internal set; } = 0f;

		public virtual float Heat { get; protected internal set; } = 1f;


		////////////////

		public float SteamPressure => this.Water * this.Heat;



		////////////////

		public void AddWater( float waterAmount, float heatAmount ) {
			if( waterAmount > 0f ) {
				this.AddHeat( waterAmount, heatAmount );
			}

			this.Water += waterAmount;
		}

		public void AddHeat( float waterAmount, float heatAmount ) {
			float percentWaterToXferHeatFrom = waterAmount / (this.Water + waterAmount);
			float percentHeatAdded = heatAmount * percentWaterToXferHeatFrom;

			this.Heat -= this.Heat * percentWaterToXferHeatFrom;

			this.Heat += percentHeatAdded;
		}


		////////////////

		public void TransferPressureToMeFromSource( Boiler source, float amount ) {
			float srcWater = source.Water;
			float srcHeat = source.Heat;

			float srcWaterDrawPerc = amount / ( srcWater * srcHeat );

			source.AddWater( srcWaterDrawPerc * -srcWater, srcHeat );

			//

			this.AddWater( srcWaterDrawPerc * srcWater, srcHeat );
		}
	}
}