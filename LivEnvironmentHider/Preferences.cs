using MelonLoader;
using RumbleModdingAPI;
using UnityEngine;
using System.Collections;
//using System.Drawing;
using UnityEngine.Rendering.UI;

namespace LivEnvironmentHider
{


	public partial class LivEnvironmentHider : MelonMod
	{
        private const string USER_DATA = $"UserData/{BuildInfo.Name}/";
		private const string CONFIG_FILE = "config.cfg";

		private MelonPreferences_Category modCategory;
		private MelonPreferences_Entry<string> GreenScreenColor;
        private MelonPreferences_Entry<bool> HideFloor;
        private MelonPreferences_Entry<string> FloorBackgroundColor;

        private MelonPreferences_Category Compat;
        private MelonPreferences_Entry<double> DelayEnvHide;

        private MelonPrefernces_Category Diagnostics;
        private MelonPreferences_Entry<bool> IsDebugMode;

        
        private void InitPreferences()
        {
            if (!Directory.Exists(USER_DATA))
            {
                Log("Userdata folder not found. Creating...");
				Directory.CreateDirectory(USER_DATA);
            }


			modCategory = MelonPreferences.CreateCategory(BuildInfo.Name);
			modCategory.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));

			GreenScreenColor = modCategory.CreateEntry("Green Screen Color", "#000000", null,"Plot Twist: Doesn't actually have to be green. Use luma key filter with black3.");
            FloorBackgroundColor = modCategory.CreateEntry("Floor Background Color", "#FFFFFF", null, "Background color for a transparent floor to make structures more visible. Inva;lid color keeps original floor. (not implemented)");

			
            Compat = MelonPreferences.CreateCategory("Compatability with other mods");
            Compat.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));
            DelayEnvHide = Compat.CreateEntry("Hide Environment Delay", 2, null, "Delay in seconds for hiding map environment from Liv. Needed for Rumble HUD's portrait creation");

            Diagnostics = MelonPreferences.CreateCategory("Diagnostic options");
            Diagnostics.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));

            IsDebugMode = Diagnostics.CreateEntry("Enable Debug Mode", true, null, "Enables more verbose logging and other debugging helper tools");
        }

        private void UpdatePrefs()
        {
            modCategory.LoadFromFile();
            Compat.LoadFromFile();
            Diagnostics.LoadFromFile();
        }
    }
}