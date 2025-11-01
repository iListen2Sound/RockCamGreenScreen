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
		GameObject DerivedCylinder;
		GameObject DerivedPitMask;
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

				
				DerivedCylinder = GameObject.Instantiate(BaseCylinder);
				DerivedCylinder.SetActive(true);
				DerivedCylinder.GetComponent<MeshRenderer>().material.color = gsColor;
				


				if (CurrentScene == "map1")
				{
					
					DerivedPitMask = GameObject.Instantiate(BasePitMask);
					DerivedPitMask.GetComponent<MeshRenderer>().material.color = gsColor;
					DerivedPitMask.SetActive(true);

				}
			}
		}

		/// <summary>
		/// Gives Rumblehud a chance to take portraits before moving map production layers
		/// </summary>
		/// <returns></returns>
		private IEnumerator HideFromLiv()
		{
			GameObject mapProduction;
			List<int> objectsToHide;
			GameObject arenaParent;
			GameObject tournamentScorer = GameObject.Find("NewTextGameObject(Clone)");
			int combatFloorIndex;
			tournamentScorer.layer = NO_LIV_LAYER;

			//TODO: Include combat floor in hiding when enabled in prefs
			if (CurrentScene == "map0")
			{
				objectsToHide = new List<int> { 0, 1, 3, 4, 6, /*combat floor:*/ 2 };
				arenaParent = Calls.GameObjects.Map0.Map0production.Mainstaticgroup.GetGameObject();
				mapProduction = Calls.GameObjects.Map0.Map0production.GetGameObject();
				

			}
			else if (CurrentScene == "map1")
			{
				objectsToHide = new List<int> { 0, 2, 3, 4, /*combat floor:*/ 1 };
				arenaParent = Calls.GameObjects.Map1.Map1production.Mainstaticgroup.GetGameObject();
				mapProduction = Calls.GameObjects.Map1.Map1production.GetGameObject();
				DerivedPitMask.transform.SetParent(mapProduction.transform, true);
			} 
			else
			{
				yield break;
			}

			DerivedCylinder.transform.SetParent(mapProduction.transform, true);

			if (!HideFloor.Value)
			{
				objectsToHide.RemoveAt(objectsToHide.Count - 1);
			}
			this.CurrentMapProduction = mapProduction;

            float secondsToWait;
            secondsToWait = CurrentScene == LastScene ? 0 : ((float) DelayEnvHide.Value);

			yield return new WaitForSeconds(secondsToWait);

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