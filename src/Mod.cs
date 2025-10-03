using System.Reflection;
using BuildAnywhere;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Player.PlayerItems;
using Il2CppMonomiPark.SlimeRancher.World;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(
    typeof(Mod),
    "BuildAnywhere",
    "1.1.0",
    "Bread-Chan",
    "https://www.nexusmods.com/slimerancher2/mods/107"
)]
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]

namespace BuildAnywhere;

public class Mod : MelonMod
{
    #region Setup

    public override void OnInitializeMelon()
    {
        var h = new HarmonyLib.Harmony("com.bread-chan.build_anywhere");
        h.PatchAll(typeof(Mod));

        MelonLogger.Msg("Initialized.");
    }

    #endregion

    #region Patches

    [HarmonyPatch]
    private class GadgetPatch
    {
        private static MethodBase TargetMethod() =>
            AccessTools.Method(
                typeof(Gadget),
                "IsOverlapping",
                new[]
                {
                    typeof(GadgetOverlapInfo).MakeByRefType(),
                    typeof(float),
                    typeof(LayerMask),
                    typeof(bool),
                }
            );

        [HarmonyPrefix]
        private static bool Postfix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }

    [HarmonyPatch(
        typeof(Gadget),
        nameof(Gadget.IsOverlapping),
        typeof(float),
        typeof(LayerMask),
        typeof(bool)
    )]
    [HarmonyPrefix]
    private static bool Gadget_IsOverlapping(ref bool __result)
    {
        __result = false;
        return false;
    }

    [HarmonyPatch(typeof(Gadget), nameof(Gadget.IsCompletelyGrounded))]
    [HarmonyPrefix]
    private static bool Gadget_IsCompletelyGrounded(ref bool __result)
    {
        __result = true;
        return false;
    }

    [HarmonyPatch(typeof(GadgetItem), nameof(GadgetItem.IsPlacementValid))]
    [HarmonyPrefix]
    private static bool GadgetItem_IsPlacementValid(GadgetItem __instance, ref bool __result)
    {
        __instance.SetGadgetPlacementValidity(true);
        __instance._isPlacementValid = true;
        __instance._isGrounded = true;
        __instance._isPlacementBlocked = false;
        __result = true;
        return false;
    }

    #endregion
}
