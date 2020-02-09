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
			// InterMyTextSurface Actions
			public void CopyTo(Sandbox.ModAPI.Ingame.IMyTextSurface Target, bool IncludeContent = true) {
				Target.FontSize = FontSize;
				Target.FontColor = FontColor;
				Target.BackgroundColor = BackgroundColor;
				Target.BackgroundAlpha = BackgroundAlpha;
				Target.ChangeInterval = ChangeInterval;
				Target.Font = Font;
				Target.Alignment = Alignment;

				Target.PreserveAspectRatio = PreserveAspectRatio;
				Target.TextPadding = TextPadding;
				Target.ScriptBackgroundColor = ScriptBackgroundColor;
				Target.ScriptForegroundColor = ScriptForegroundColor;

				if (IncludeContent) {
					Target.ContentType = ContentType;
					Target.Script = Script;
					Target.ClearImagesFromSelection(); List<string> Images = new List<string>(); GetSelectedImages(Images); Target.AddImagesToSelection(Images);
					Target.WriteText(GetText());
				}
			}
			public void CopyFrom(Sandbox.ModAPI.Ingame.IMyTextSurface Source, bool IncludeContent = true) {
				FontSize = Source.FontSize;
				FontColor = Source.FontColor;
				BackgroundColor = Source.BackgroundColor;
				BackgroundAlpha = Source.BackgroundAlpha;
				ChangeInterval = Source.ChangeInterval;
				Font = Source.Font;
				Alignment = Source.Alignment;

				PreserveAspectRatio = Source.PreserveAspectRatio;
				TextPadding = Source.TextPadding;
				ScriptBackgroundColor = Source.ScriptBackgroundColor;
				ScriptForegroundColor = Source.ScriptForegroundColor;

				if (IncludeContent) {
					ContentType = Source.ContentType;
					Script = Source.Script;
					ClearImagesFromSelection(); List<string> Images = new List<string>(); Source.GetSelectedImages(Images); AddImagesToSelection(Images);
					WriteText(Source.GetText());
				}
			}


			// Default Interface
			public string CurrentlyShownImage => null;

			public float FontSize { get; set; }
			public Color FontColor { get; set; }
			public Color BackgroundColor { get; set; }
			public byte BackgroundAlpha { get; set; }
			public float ChangeInterval { get; set; }
			public string Font { get; set; }
			public TextAlignment Alignment { get; set; }
			public string Script { get; set; }
			public ContentType ContentType { get; set; }

			public Vector2 SurfaceSize => new Vector2();    // Not Implemented

			public Vector2 TextureSize => new Vector2( );    // Not Implemented

			public bool PreserveAspectRatio { get; set; }
			public float TextPadding { get; set; }
			public Color ScriptBackgroundColor { get; set; }
			public Color ScriptForegroundColor { get; set; }

			public string Name => "Emulation";

			public string DisplayName => "Emulation";

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

			public void GetFonts(List<string> fonts) {	// Not Implemented
			}

			public void GetScripts(List<string> scripts) {  // Not Implemented
			}

			public void GetSelectedImages(List<string> output) {
				output.AddRange(ImageSelection);
			}

			public void GetSprites(List<string> sprites) {  // Not Implemented
			}

			public string GetText() {
				return Text.ToString();
			}

			public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale) {    // Not Implemented
				return new Vector2();
			}

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
