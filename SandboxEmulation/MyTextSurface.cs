using System.Collections.Generic;
using System.Text;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript {
	public static partial class SandboxEmulation {
		private class MyTextSurface : Sandbox.ModAPI.Ingame.IMyTextSurface {
			// Back-end
			private List<string> ImageSelection = new List<string>();
			private StringBuilder Text;

			private List<string> GetFonts_Return;
			private List<string> GetScripts_Return;
			// InterMyTextSurface Actions
			public static void Copy(Sandbox.ModAPI.Ingame.IMyTextSurface Source, Sandbox.ModAPI.Ingame.IMyTextSurface Target, bool IncludeContent = true) {
				Target.FontSize = Source.FontSize;
				Target.FontColor = Source.FontColor;
				Target.BackgroundColor = Source.BackgroundColor;
				Target.BackgroundAlpha = Source.BackgroundAlpha;
				Target.ChangeInterval = Source.ChangeInterval;
				Target.Font = Source.Font;
				Target.Alignment = Source.Alignment;

				Target.PreserveAspectRatio = Source.PreserveAspectRatio;
				Target.TextPadding = Source.TextPadding;
				Target.ScriptBackgroundColor = Source.ScriptBackgroundColor;
				Target.ScriptForegroundColor = Source.ScriptForegroundColor;

				if (IncludeContent) {
					Target.ContentType = Source.ContentType;
					Target.Script = Source.Script;
					Target.ClearImagesFromSelection(); List<string> Images = new List<string>(); Source.GetSelectedImages(Images); Target.AddImagesToSelection(Images);
					Target.WriteText(Source.GetText());
				}
			}
			public static void Copy(Sandbox.ModAPI.Ingame.IMyTextSurface Source, MyTextSurface Target, bool IncludeContent = true) {
				Copy(Source, Target, IncludeContent);
				if (IncludeContent) {
					Target.CurrentlyShownImage = Source.CurrentlyShownImage;
					Target.SurfaceSize = Source.SurfaceSize;
					Target.TextureSize = Source.TextureSize;
					Target.Name = Source.Name;
					Target.DisplayName = Source.DisplayName;

					Target.GetFontsX = Source.GetFonts;
					Target.GetScriptsX = Source.GetScripts;
					Target.GetSpritesX = Source.GetSprites;
					Target.MeasureStringInPixelsX = Source.MeasureStringInPixels;
				}
			}
			public void CopyTo(Sandbox.ModAPI.Ingame.IMyTextSurface Target, bool IncludeContent = true) => Copy(this, Target, IncludeContent);
			public void CopyFrom(Sandbox.ModAPI.Ingame.IMyTextSurface Source, bool IncludeContent = true) => Copy(Source, this, IncludeContent);


			// Default Interface
			public string CurrentlyShownImage { get; set; }		// Interface Read-only

			public float FontSize { get; set; }
			public Color FontColor { get; set; }
			public Color BackgroundColor { get; set; }
			public byte BackgroundAlpha { get; set; }
			public float ChangeInterval { get; set; }
			public string Font { get; set; }
			public TextAlignment Alignment { get; set; }
			public string Script { get; set; }
			public ContentType ContentType { get; set; }

			public Vector2 SurfaceSize { get; private set; }	// Interface Read-only

			public Vector2 TextureSize { get; private set; }	// Interface Read-only

			public bool PreserveAspectRatio { get; set; }
			public float TextPadding { get; set; }
			public Color ScriptBackgroundColor { get; set; }
			public Color ScriptForegroundColor { get; set; }

			public string Name { get; set; }                    // Interface Read-only

			public string DisplayName { get; set; }             // Interface Read-only

			public void AddImagesToSelection(List<string> ids, bool checkExistence = false) {
				ImageSelection.AddRange(ids);
			}

			public void AddImageToSelection(string id, bool checkExistence = false) {
				ImageSelection.Add(id);
			}

			public void ClearImagesFromSelection() {
				ImageSelection.Clear();
			}

			public MySpriteDrawFrame DrawFrame() {
				MySpriteDrawFrame Frame = new MySpriteDrawFrame();
				return Frame;
			}

			public Action<List<string>> GetFontsX;											// Interface Method
			public void GetFonts(List<string> fonts) { GetFontsX(fonts); }

			private Action<List<string>> GetScriptsX;										// Interface Method
			public void GetScripts(List<string> scripts) { GetScriptsX(scripts); }

			public void GetSelectedImages(List<string> output) {
				output.AddRange(ImageSelection);
			}

			public Action<List<string>> GetSpritesX;										// Interface Method
			public void GetSprites(List<string> sprites) { GetSpritesX(sprites); }

			public string GetText() {
				return Text.ToString();
			}

			public Func<StringBuilder, string, float, Vector2> MeasureStringInPixelsX;		// Interface Method
			public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale) { return MeasureStringInPixelsX(text, font, scale); }

			public void ReadText(StringBuilder buffer, bool append = false) {
				if (!append) buffer.Clear();
				buffer.Append(Text);
			}

			public void RemoveImageFromSelection(string id, bool removeDuplicates = false) {
				if (removeDuplicates) {
					ImageSelection.RemoveAll((string item) => item == id);
				} else {
					ImageSelection.Remove(id);
				}
			}

			public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false) {
				foreach (string id in ids) {
					RemoveImageFromSelection(id, removeDuplicates);
				}
			}

			public bool WriteText(string value, bool append = false) {
				try {
					if(!append) {
						Text.Clear();
					}
					Text.Append(value);
				} catch (Exception) {
					return false;
				}
				return true;
			}

			public bool WriteText(StringBuilder value, bool append = false) {
				try {
					if (append) {
						Text.Append(value);
					} else {
						Text = value;
					}
				} catch (Exception) {
					return false;
				}
				return true;
			}

		}
	}
}
