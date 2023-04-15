using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using RWCustom;
using BepInEx;
using Debug = UnityEngine.Debug;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace StickyHUD;

[BepInPlugin("Splittikin.StickyHUD", "Sticky HUD", "1.0.0")]
public partial class StickyHUD : BaseUnityPlugin
{
    private StickyHUDOptions Options;

    public StickyHUD()
    {
        try
        {
            Options = new StickyHUDOptions(this, Logger);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }
    
    private void OnEnable()
    {
        On.RainWorld.OnModsInit += RainWorldOnOnModsInit;
    }

    private bool IsInit;
    private void RainWorldOnOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        try
        {
            if (IsInit) return;

            //Your hooks go here
            On.HUD.HUD.Update += HUDUpdateHook;
            On.HUD.RainMeter.Update += HUDRainmeterUpdateHook;
            On.HUD.KarmaMeter.Draw += HUDKarmaMeterDrawHook;
            
            On.RainWorldGame.ShutDownProcess += RainWorldGameOnShutDownProcess;
            On.GameSession.ctor += GameSessionOnctor;

            MachineConnector.SetRegisteredOI("Splittikin.StickyHUD", Options);
            IsInit = true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }
    
    
    
    void HUDUpdateHook(On.HUD.HUD.orig_Update orig, HUD.HUD self)
    {
        orig(self);
        self.showKarmaFoodRain = true;
    }

    public int halfTimeBlink;
    void HUDRainmeterUpdateHook(On.HUD.RainMeter.orig_Update orig, HUD.RainMeter self)
    {
        orig(self);
        halfTimeBlink = self.halfTimeBlink;
    }

    void HUDKarmaMeterDrawHook(On.HUD.KarmaMeter.orig_Draw orig, HUD.KarmaMeter self, float timeStacker)
    {
        orig(self, timeStacker);
        if (Options.BlinkKarma.Value && Mathf.Max(halfTimeBlink % 15) > 7)
        {
            // When the rain timer blinks once half of it has passed, blink the karma symbol as well so the user sees
            self.karmaSprite.alpha = 0;
        }
    }
    
    
    private void RainWorldGameOnShutDownProcess(On.RainWorldGame.orig_ShutDownProcess orig, RainWorldGame self)
    {
        orig(self);
        ClearMemory();
    }
    private void GameSessionOnctor(On.GameSession.orig_ctor orig, GameSession self, RainWorldGame game)
    {
        orig(self, game);
        ClearMemory();
    }

    #region Helper Methods

    private void ClearMemory()
    {
        //If you have any collections (lists, dictionaries, etc.)
        //Clear them here to prevent a memory leak
        //YourList.Clear();
    }

    #endregion
}
