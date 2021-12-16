using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SteampunkArsenal.Items {
	public partial class SteamPoweredRivetLauncherItem : ModItem, ISteamPressureSource {
		public float GetPressurePercent() {
			return this.SteamPressure;
		}

		public void AddPressurePercent( float amount ) {
			this.SteamPressure += amount;
		}


		////////////////

		public float TransferPressureToMeFromSource( ISteamPressureSource source, float amount ) {
			float sourcePressure = source.GetPressurePercent();

			float transferAmount = Math.Min( sourcePressure, amount );

			source.AddPressurePercent( -transferAmount );

			this.SteamPressure += transferAmount;

			return transferAmount;
		}
	}
}