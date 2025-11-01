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
        
        private void CreateGreenScreens()
		{
			if(CurrentScene.Contains("map"))
			{
				Color gsColor;

				if (!ColorUtility.TryParseHtmlString(GreenScreenColor.Value, out gsColor))
				{
					Log($"Failed to parse color from: {GreenScreenColor.Value}", false, 2);
					gsColor = Color.black;
				}

				GameObject derCylinder;
				derCylinder = GameObject.Instantiate(BaseCylinder);
				derCylinder.SetActive(CurrentScene.Contains("map"));
				derCylinder.GetComponent<MeshRenderer>().material.color = gsColor;
				derCylinder.transform.SetParent(CurrentMapProduction.transform, true);

				
				if(CurrentScene == "map1")
				{
					GameObject derPitMask;
					derPitMask = GameObject.Instantiate(BasePitMask);
					derPitMask.GetComponent<MeshRenderer>().material.color = gsColor;
					derPitMask.transform.SetParent(CurrentMapProduction.transform, true);
				}
			}
		}

		/// <summary>
		/// Gives Rumblehud a chance to take portraits before moving map production layers
		/// </summary>
		/// <returns></returns>
		private IEnumerator HideFromLiv()
		{
			GameObject mapProduction = new();
			List<int> objectsToHide = new();
			GameObject arenaParent;
			GameObject tournamentScorer = GameObject.Find("NewTextGameObject(Clone)");
			int combatFloorIndex
			tournamentScorer.layer = NO_LIV_LAYER;

			//TODO: Include combat floor in hiding when enabled in prefs
			if (CurrentScene == "map0")
			{
				objectsToHide = new List<int> { 0, 1, 3, 4, 6, /*combat floor index*/};
				arenaParent = Calls.GameObjects.Map0.Map0production.Mainstaticgroup.GetGameObject();
				mapProduction = Calls.GameObjects.Map0.Map0production.GetGameObject();
				

			}
			else if (CurrentScene == "map1")
			{
				objectsToHide = new List<int> { 0, 2, 3, 4, /*combat floor index*/ };
				arenaParent = Calls.GameObjects.Map1.Map1production.Mainstaticgroup.GetGameObject();
				mapProduction = Calls.GameObjects.Map1.Map1production.GetGameObject();
			} 
			else
			{
				arenaParent = new();
			}

			if(!HideFloor.Value)
			{
				objectsToHide.RemoveAt(objectsToHide.Count - 1);
			}
			this.CurrentMapProduction = mapProduction;

			yield return new WaitForSeconds(DelayEnvHide.Value);

			for (int i = 0; i < arenaParent.transform.childCount; i++)
			{
				GameObject child = arenaParent.transform.GetChild(i).gameObject;
				if (objectsToHide.Contains(i))
				{
					child.layer = NO_LIV_LAYER;
				}
			}
		}
    }
}