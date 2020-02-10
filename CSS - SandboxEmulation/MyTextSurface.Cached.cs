using System.Collections.Generic;
using System.Text;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript {
	public static partial class SandboxEmulation {
		public partial class MyTextSurface : Sandbox.ModAPI.Ingame.IMyTextSurface {
			// Generate with
			/*
			List<string> s = new List<string>();
			Me.GetSurface(0).GetScripts(s);
			Me.GetSurface(0).WriteText("");
			foreach (string X in s) {
				Echo(X.ToString());
			Me.GetSurface(0).WriteText("@\"" + X + "\",\n", true);
			}
			*/

			/// <summary>
			/// Cached method list outputs.
			/// Cached on SE version 1.193.103
			/// </summary>
			public static class Cached {
				/// <summary>
				/// Gets a list of available fonts.
				/// Cached on SE version 1.193.103
				/// </summary>
				public static readonly List<string> GetFonts = new List<string>{
					@"Debug",
					@"Red",
					@"Green",
					@"Blue",
					@"White",
					@"DarkBlue",
					@"UrlNormal",
					@"UrlHighlight",
					@"ErrorMessageBoxCaption",
					@"ErrorMessageBoxText",
					@"InfoMessageBoxCaption",
					@"InfoMessageBoxText",
					@"ScreenCaption",
					@"GameCredits",
					@"LoadingScreen",
					@"BuildInfo",
					@"BuildInfoHighlight",
					@"Monospace"
				};
				/// <summary>
				/// Gets a list of available fonts.
				/// Cached on SE version 1.193.103
				/// </summary>
				public static readonly List<string> GetScripts = new List<string>{
					@"TSS_ClockAnalog",
					@"TSS_ArtificialHorizon",
					@"TSS_ClockDigital",
					@"TSS_EnergyHydrogen",
					@"TSS_FactionIcon",
					@"TSS_Gravity",
					@"TSS_Velocity",
					@"TSS_VendingMachine",
					@"TSS_Jukebox"
				};
				/// <summary>
				/// Gets a list of available sprites.
				/// Cached on SE version 1.193.103
				/// </summary>
				public static readonly List<string> GetSprites = new List<string>{
					@"Offline",
					@"Online",
					@"Arrow",
					@"Cross",
					@"Danger",
					@"No Entry",
					@"Construction",
					@"White screen",
					@"Grid",
					@"DecorativeBracketLeft",
					@"DecorativeBracketRight",
					@"SquareTapered",
					@"SquareSimple",
					@"IconEnergy",
					@"IconHydrogen",
					@"IconOxygen",
					@"IconTemperature",
					@"AH_GravityHudNegativeDegrees",
					@"AH_GravityHudPositiveDegrees",
					@"AH_TextBox",
					@"AH_PullUp",
					@"AH_VelocityVector",
					@"AH_BoreSight",
					@"RightTriangle",
					@"Triangle",
					@"Circle",
					@"SemiCircle",
					@"CircleHollow",
					@"SquareHollow",
					@"UVChecker",
					@"OutOfOrder",
					@"StoreBlock2",
					@"LCD_Economy_Charts",
					@"LCD_Economy_SC_Here",
					@"LCD_Economy_Coins",
					@"LCD_Economy_SingleCoin",
					@"LCD_Economy_SC_Logo",
					@"LCD_Economy_SC_Blueprint",
					@"LCD_Economy_SC_Logo_2",
					@"LCD_Economy_Faction_1",
					@"LCD_Economy_Poster_1",
					@"LCD_Economy_Trade",
					@"LCD_Economy_Clear",
					@"LCD_Economy_Graph",
					@"LCD_Economy_Graph_2",
					@"LCD_Economy_Graph_3",
					@"LCD_Economy_Graph_4",
					@"LCD_Economy_Graph_5",
					@"LCD_Economy_SE_Logo_1",
					@"LCD_Economy_SE_Logo_2",
					@"LCD_Economy_Blueprint_2",
					@"LCD_Economy_Blueprint_3",
					@"LCD_Economy_Trinity",
					@"LCD_Economy_KeenSWH",
					@"LCD_Economy_Badge",
					@"LCD_Economy_Vending_Bg",
					@"Screen_LoadingBar",
					@"Screen_LoadingBar2",
					@"LCD_Economy_Detail",
					@"MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm",
					@"MyObjectBuilder_AmmoMagazine/NATO_25x184mm",
					@"MyObjectBuilder_AmmoMagazine/Missile200mm",
					@"MyObjectBuilder_Component/Construction",
					@"MyObjectBuilder_Component/MetalGrid",
					@"MyObjectBuilder_Component/InteriorPlate",
					@"MyObjectBuilder_Component/SteelPlate",
					@"MyObjectBuilder_Component/Girder",
					@"MyObjectBuilder_Component/SmallTube",
					@"MyObjectBuilder_Component/LargeTube",
					@"MyObjectBuilder_Component/Motor",
					@"MyObjectBuilder_Component/Display",
					@"MyObjectBuilder_Component/BulletproofGlass",
					@"MyObjectBuilder_Component/Superconductor",
					@"MyObjectBuilder_Component/Computer",
					@"MyObjectBuilder_Component/Reactor",
					@"MyObjectBuilder_Component/Thrust",
					@"MyObjectBuilder_Component/GravityGenerator",
					@"MyObjectBuilder_Component/Medical",
					@"MyObjectBuilder_Component/RadioCommunication",
					@"MyObjectBuilder_Component/Detector",
					@"MyObjectBuilder_Component/Explosives",
					@"MyObjectBuilder_Component/SolarCell",
					@"MyObjectBuilder_Component/PowerCell",
					@"MyObjectBuilder_Component/Canvas",
					@"MyObjectBuilder_Component/ZoneChip",
					@"MyObjectBuilder_PhysicalGunObject/GoodAIRewardPunishmentTool",
					@"MyObjectBuilder_Ore/Stone",
					@"MyObjectBuilder_Ore/Iron",
					@"MyObjectBuilder_Ore/Nickel",
					@"MyObjectBuilder_Ore/Cobalt",
					@"MyObjectBuilder_Ore/Magnesium",
					@"MyObjectBuilder_Ore/Silicon",
					@"MyObjectBuilder_Ore/Silver",
					@"MyObjectBuilder_Ore/Gold",
					@"MyObjectBuilder_Ore/Platinum",
					@"MyObjectBuilder_Ore/Uranium",
					@"MyObjectBuilder_Ingot/Stone",
					@"MyObjectBuilder_Ingot/Iron",
					@"MyObjectBuilder_Ingot/Nickel",
					@"MyObjectBuilder_Ingot/Cobalt",
					@"MyObjectBuilder_Ingot/Magnesium",
					@"MyObjectBuilder_Ingot/Silicon",
					@"MyObjectBuilder_Ingot/Silver",
					@"MyObjectBuilder_Ingot/Gold",
					@"MyObjectBuilder_Ingot/Platinum",
					@"MyObjectBuilder_Ingot/Uranium",
					@"MyObjectBuilder_PhysicalGunObject/AutomaticRifleItem",
					@"MyObjectBuilder_PhysicalGunObject/PreciseAutomaticRifleItem",
					@"MyObjectBuilder_PhysicalGunObject/RapidFireAutomaticRifleItem",
					@"MyObjectBuilder_PhysicalGunObject/UltimateAutomaticRifleItem",
					@"MyObjectBuilder_OxygenContainerObject/OxygenBottle",
					@"MyObjectBuilder_GasContainerObject/HydrogenBottle",
					@"MyObjectBuilder_PhysicalGunObject/WelderItem",
					@"MyObjectBuilder_PhysicalGunObject/Welder2Item",
					@"MyObjectBuilder_PhysicalGunObject/Welder3Item",
					@"MyObjectBuilder_PhysicalGunObject/Welder4Item",
					@"MyObjectBuilder_PhysicalGunObject/AngleGrinderItem",
					@"MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item",
					@"MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item",
					@"MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item",
					@"MyObjectBuilder_PhysicalGunObject/HandDrillItem",
					@"MyObjectBuilder_PhysicalGunObject/HandDrill2Item",
					@"MyObjectBuilder_PhysicalGunObject/HandDrill3Item",
					@"MyObjectBuilder_PhysicalGunObject/HandDrill4Item",
					@"MyObjectBuilder_Ore/Scrap",
					@"MyObjectBuilder_Ingot/Scrap",
					@"MyObjectBuilder_Ore/Ice",
					@"MyObjectBuilder_Ore/Organic",
					@"MyObjectBuilder_TreeObject/DesertTree",
					@"MyObjectBuilder_TreeObject/DesertTreeDead",
					@"MyObjectBuilder_TreeObject/LeafTree",
					@"MyObjectBuilder_TreeObject/PineTree",
					@"MyObjectBuilder_TreeObject/PineTreeSnow",
					@"MyObjectBuilder_TreeObject/LeafTreeMedium",
					@"MyObjectBuilder_TreeObject/DesertTreeMedium",
					@"MyObjectBuilder_TreeObject/DesertTreeDeadMedium",
					@"MyObjectBuilder_TreeObject/PineTreeMedium",
					@"MyObjectBuilder_TreeObject/PineTreeSnowMedium",
					@"MyObjectBuilder_TreeObject/DeadBushMedium",
					@"MyObjectBuilder_TreeObject/DesertBushMedium",
					@"MyObjectBuilder_TreeObject/LeafBushMedium_var1",
					@"MyObjectBuilder_TreeObject/LeafBushMedium_var2",
					@"MyObjectBuilder_TreeObject/PineBushMedium",
					@"MyObjectBuilder_TreeObject/SnowPineBushMedium",
					@"MyObjectBuilder_ConsumableItem/ClangCola",
					@"MyObjectBuilder_ConsumableItem/CosmicCoffee",
					@"MyObjectBuilder_Datapad/Datapad",
					@"MyObjectBuilder_Package/Package",
					@"MyObjectBuilder_ConsumableItem/Medkit",
					@"MyObjectBuilder_ConsumableItem/Powerkit",
					@"MyObjectBuilder_PhysicalObject/SpaceCredit",
					@"Textures\FactionLogo\Empty.dds",
					@"Textures\FactionLogo\PirateIcon.dds",
					@"Textures\FactionLogo\Spiders.dds",
					@"Textures\FactionLogo\Miners\MinerIcon_1.dds",
					@"Textures\FactionLogo\Miners\MinerIcon_2.dds",
					@"Textures\FactionLogo\Miners\MinerIcon_3.dds",
					@"Textures\FactionLogo\Miners\MinerIcon_4.dds",
					@"Textures\FactionLogo\Traders\TraderIcon_1.dds",
					@"Textures\FactionLogo\Traders\TraderIcon_2.dds",
					@"Textures\FactionLogo\Traders\TraderIcon_3.dds",
					@"Textures\FactionLogo\Traders\TraderIcon_4.dds",
					@"Textures\FactionLogo\Traders\TraderIcon_5.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_1.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_2.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_3.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_4.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_5.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_6.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_7.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_8.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_9.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_10.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_11.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_12.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_13.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_14.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_15.dds",
					@"Textures\FactionLogo\Builders\BuilderIcon_16.dds",
					@"Textures\FactionLogo\Others\OtherIcon_1.dds",
					@"Textures\FactionLogo\Others\OtherIcon_2.dds",
					@"Textures\FactionLogo\Others\OtherIcon_3.dds",
					@"Textures\FactionLogo\Others\OtherIcon_4.dds",
					@"Textures\FactionLogo\Others\OtherIcon_5.dds",
					@"Textures\FactionLogo\Others\OtherIcon_6.dds",
					@"Textures\FactionLogo\Others\OtherIcon_7.dds",
					@"Textures\FactionLogo\Others\OtherIcon_8.dds",
					@"Textures\FactionLogo\Others\OtherIcon_9.dds",
					@"Textures\FactionLogo\Others\OtherIcon_10.dds",
					@"Textures\FactionLogo\Others\OtherIcon_11.dds",
					@"Textures\FactionLogo\Others\OtherIcon_12.dds",
					@"Textures\FactionLogo\Others\OtherIcon_13.dds",
					@"Textures\FactionLogo\Others\OtherIcon_14.dds",
					@"Textures\FactionLogo\Others\OtherIcon_15.dds",
					@"Textures\FactionLogo\Others\OtherIcon_16.dds",
					@"Textures\FactionLogo\Others\OtherIcon_17.dds",
					@"Textures\FactionLogo\Others\OtherIcon_18.dds",
					@"Textures\FactionLogo\Others\OtherIcon_19.dds",
					@"Textures\FactionLogo\Others\OtherIcon_20.dds",
					@"Textures\FactionLogo\Others\OtherIcon_21.dds",
					@"Textures\FactionLogo\Others\OtherIcon_22.dds",
					@"Textures\FactionLogo\Others\OtherIcon_23.dds",
					@"Textures\FactionLogo\Others\OtherIcon_24.dds",
					@"Textures\FactionLogo\Others\OtherIcon_26.dds",
					@"Textures\FactionLogo\Others\OtherIcon_27.dds",
					@"Textures\FactionLogo\Others\OtherIcon_28.dds",
					@"Textures\FactionLogo\Others\OtherIcon_29.dds",
					@"Textures\FactionLogo\Others\OtherIcon_30.dds",
					@"Textures\FactionLogo\Others\OtherIcon_31.dds",
					@"Textures\FactionLogo\Others\OtherIcon_32.dds",
					@"Textures\FactionLogo\Others\OtherIcon_33.dds"
				};
				/// <summary>
				/// Calculates how many pixels a string of a given font and scale will take up.
				/// Cached on SE version 1.193.103
				/// </summary>
				/// <remarks>
				/// function is currently broken and returns 0. So default return 0 will be accurate guess.
				/// </remarks>
				public static readonly Vector2 MeasureStringInPixels = new Vector2(0, 0);
			}
		}
	}
}