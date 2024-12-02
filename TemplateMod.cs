using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using HarmonyLib;
using UnityEngine;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace TemplateMod;

// Comment this patch if you want to use HookGen instead of Harmony
// NOTE: If you need to work with ILGenerator for transpilers, you need to switch
//       from netstandard2.1 to net48 in the project's TargetFramework attribute
[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Set_MenuCondition))]
public static class MenuPatch
{
    [HarmonyPostfix]
    private static void SetMenuConditionPatch(MainMenuManager __instance)
    {
        __instance._versionDisplayText.text = Application.version + " with Mods :D";

        MainMenuManager _test = UnityEngine.Object.FindObjectOfType<MainMenuManager>();
    }
}

[BepInPlugin("Marioalexsan.TemplateMod", "Template mod for Atlyss using BepInEx", "1.0.0")]
public class TemplateMod : BaseUnityPlugin
{
    // The template mod patches the main menu version string to include extra text.

    private void Awake()
    {
        var harmony = new Harmony("TemplateMod");
        harmony.PatchAll();

        UnityEngine.Debug.Log("Hello from TemplateMod!");
        UnityEngine.Debug.Log($"Application version is ${Application.version}");


    }


}