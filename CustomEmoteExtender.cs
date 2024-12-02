using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.IO;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace EmoteExtender;

// Comment this patch if you want to use HookGen instead of Harmony
// NOTE: If you need to work with ILGenerator for transpilers, you need to switch
//       from netstandard2.1 to net48 in the project's TargetFramework attribute
[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Set_MenuCondition))]
public static class CustomEmoteExtenderStatic
{
    [HarmonyPostfix]
    private static void EmoteExtender(MainMenuManager __instance)
    {
        __instance._versionDisplayText.text = Application.version + " with Mods :D";
    }
}

[BepInPlugin("AtlyssEmoteExtender", "Allows more than default amount of emotes to be added to the game", "1.0.0")]
public class CustomEmoteExtender : BaseUnityPlugin
{
    // The template mod patches the main menu version string to include extra text.

    private void Awake()
    {
        var harmony = new Harmony("TemplateMod");
        harmony.PatchAll();

        UnityEngine.Debug.Log("Hello from TemplateMod!");
        UnityEngine.Debug.Log($"Application version is ${Application.version}");

        SetupMonomodHooks();
    }

    public void AddEmotesToArray()
    {
        // emotes are stored in the ScriptableEmoteList
        // on start, organize all the animations in the animations folder into an array
        // then append them to the emote list scriptable object

        // there will be a subfolder within the animations folder for each particular animation
        // within the folder holds one or more animation clips, so that when an emote is called it can blend into each other when one ends.
        // each folder will also hold an xml file called AnimInfo.xml that will hold each anim within an AnimElement tag
        // within each AnimElement is an EmoteProperties that we will need to construct the emote
        // as well as some extra EmoteProperties that include things like _canLoop and _facialExpression which can be set to true or false or to the expression enum
        // the path will also be included, which the path should be the name of that particular animation clip asset bundle.
        // any AnimElement that has no EmoteCommand will be assumed to be a secondary animation to be played after the first one ends

        var path = @"mesh_assets";
        var bundle = AssetBundle.LoadFromFile(path);
        var CustomAnimations = new AnimationClip[1];
        CustomAnimations[0] = bundle.LoadAsset<AnimationClip>("TestAnimation");

        
    }

    public void OnEmoteCommand()
    {
        // if command is not handled by the default /emotes then handle it here
        // find the emote in the emote list and play it
        // if the animation is not a looping animation, then play the animation once and then check if there are any other animations to play from the AnimInfo
        // if there are, then play the next animation
        // we will have to use unity's Playables system to first pause the animator and then manually play the animation
        // transitioning between animations will also have to be done manually using a crossfade.
        // also set the facial expression associated with the emote.

        // when there is no more animations to play, then resume the animator

    }

    // Uncomment the following stuff if you want to use AutoHookGenPatcher / HookGen
    // Also comment out the Harmony patch if you do so
    // (you'll need MMHOOK_Assembly-CSharp.dll)

    private void SetupMonomodHooks()
    {
        //On.MainMenuManager.Set_MenuCondition += MainMenuManager_Set_MenuCondition;
    }

    //public void MainMenuManager_Set_MenuCondition(On.MainMenuManager.orig_Set_MenuCondition orig, MainMenuManager self, int _index)
    //{
    //    orig(self, _index);
    //    self._versionDisplayText.text = Application.version + " with Mods :D";
    //}
}