using System.Reflection;
using BuildAnywhere;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Player.PlayerItems;
using Il2CppMonomiPark.SlimeRancher.World;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(Mod), "BuildAnywhere", "1.0.0", "Bread-Chan", "")]
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

        [HarmonyPostfix]
        private static void Postfix(ref bool __result) => __result = false;
    }

    [HarmonyPatch(typeof(Gadget), "IsOverlapping", typeof(float), typeof(LayerMask), typeof(bool))]
    [HarmonyPostfix]
    private static void Gadget_IsOverlapping(ref bool __result) => __result = false;

    [HarmonyPatch(typeof(GadgetItem), nameof(GadgetItem.IsPlacementValid))]
    [HarmonyPostfix]
    private static void GadgetItem_IsPlacementValid(GadgetItem __instance, ref bool __result)
    {
        __instance.SetGadgetPlacementValidity(true);
        __result = true;
    }

    #endregion
}
