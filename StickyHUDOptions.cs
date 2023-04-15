using System;
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;

namespace StickyHUD;

public class StickyHUDOptions : OptionInterface
{
    private readonly ManualLogSource Logger;

    public StickyHUDOptions(StickyHUD modInstance, ManualLogSource loggerSource)
    {
        Logger = loggerSource;
        BlinkKarma = this.config.Bind<Boolean>("BlinkKarma", true);
    }

    public readonly Configurable<Boolean> BlinkKarma;
    private UIelement[] UIArrPlayerOptions;
    
    public override void Initialize()
    {
        var opTab = new OpTab(this, "Options");
        this.Tabs = new[]
        {
            opTab
        };

        UIArrPlayerOptions = new UIelement[]
        {
            new OpLabel(10f, 550f, "Options", true),
            new OpLabel(10f, 520f, "Blink karma indicator when rain timer blinks"),
            
            new OpCheckBox(BlinkKarma, new Vector2(10f, 490f))
        };
        opTab.AddItems(UIArrPlayerOptions);
    }

}