using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StickyFix
{
    internal class ShipItemEmbarkPatches
    {

        [HarmonyPatch(typeof(ShipItem))]
        private static class EmbarkPatches
        {

            [HarmonyPatch("Awake")]
            [HarmonyPrefix]
            public static void Awake(ShipItem __instance)
            {
                __instance.gameObject.AddComponent<EmbarkTracker>();
            }

            [HarmonyPatch("OnTriggerEnter")]
            [HarmonyPrefix]
            public static bool OnTriggerEnter(Collider other, ShipItem __instance, Collider ___currentBoatCollider)
            {
                List<Collider> colliders = __instance.GetComponent<EmbarkTracker>().embarkColliders;
                if (other.CompareTag("EmbarkCol"))
                {
                    colliders.Add(other);
                    if (___currentBoatCollider != null)
                    {
                        return false;
                    }
                }
                return true;
            }

            [HarmonyPatch("OnTriggerExit")]
            [HarmonyPostfix]
            public static void OnTriggerExit(ShipItem __instance, Collider other, ref Collider ___currentlyStayedEmbarkCol)
            {
                List<Collider> colliders = __instance.GetComponent<EmbarkTracker>().embarkColliders;
                colliders.Remove(other);

                if (colliders.Count >= 1)
                {
                    ___currentlyStayedEmbarkCol = colliders[0];
                }
            }

            [HarmonyPatch("EnterBoat")]
            [HarmonyPrefix]
            public static void EnterBoat(ShipItem __instance, Transform ___currentActualBoat)
            {
                __instance.InvokePrivateMethod("ExitBoat");
            }

        }
    }
}
