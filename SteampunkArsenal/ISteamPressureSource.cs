using Terraria;


namespace SteampunkArsenal {
	public interface ISteamPressureSource {
		float GetPressurePercent();

		void AddPressurePercent( float amount );

		float TransferPressureToMeFromSource( ISteamPressureSource source, float amount );
	}
}