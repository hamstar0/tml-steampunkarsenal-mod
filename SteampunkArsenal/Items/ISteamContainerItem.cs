using System;
using Terraria;
using Terraria.ID;
using SteampunkArsenal.Logic.Steam;


namespace SteampunkArsenal.Items {
	public interface ISteamContainerItem {
		SteamSource SteamSupply { get; }
	}
}
