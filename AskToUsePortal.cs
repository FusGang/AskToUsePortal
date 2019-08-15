using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VRCMenuUtils;

using Harmony;

using VRCModLoader;

namespace AskToUsePortal
{
    [VRCModInfo("AskToUsePortal", "0.1.0", "AtiLion", "https://github.com/AtiLion/AskToUsePortal/releases")]
    internal class AskToUsePortal : VRCMod
    {
        private static PortalInternal portalInternal = null;
        private static HarmonyInstance harmonyInstance = null;

        void OnApplicationStart()
        {
            VRCModLogger.Log("Starting AskToUsePortal....");
            harmonyInstance = HarmonyInstance.Create("atilion.asktouseportal");

            // Patch
            try
            {
                harmonyInstance.Patch(
                        typeof(PortalInternal).GetMethod("Enter", BindingFlags.Public | BindingFlags.Instance),
                        new HarmonyMethod(GetType().GetMethod("Enter", BindingFlags.Static | BindingFlags.NonPublic))
                    );
                VRCModLogger.Log("Patched PortalInternal.Enter");
            }
            catch(Exception ex)
            {
                VRCModLogger.LogError(ex.ToString());
            }
            VRCModLogger.Log("Started AskToUsePortal!");
        }

        private static bool Enter(PortalInternal __instance, MethodInfo __originalMethod)
        {
            if (portalInternal == __instance)
                return true;

            VRCMenuUtilsAPI.Alert("Enter Portal", "Do you really want to enter the portal?", "No", () =>
            {
                VRCMenuUtilsAPI.HideCurrentPopup();
            }, "Yes", () =>
            {
                portalInternal = __instance;
                VRCMenuUtilsAPI.HideCurrentPopup();
                __originalMethod.Invoke(__instance, new object[0]);
            });
            return false;
        }
    }
}
