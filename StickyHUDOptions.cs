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
        ToggleButton = this.config.Bind<KeyCode>("ToggleButton", KeyCode.Tab);
        DefaultEnabled = this.config.Bind<Boolean>("DefaultEnabled", true);
    }

    public readonly Configurable<Boolean> BlinkKarma;
    public readonly Configurable<KeyCode> ToggleButton;
    public readonly Configurable<Boolean> DefaultEnabled;

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
            new OpLabel(40f, 523f, "Blink karma indicator when rain timer blinks"),
            new OpCheckBox(BlinkKarma, new Vector2(10f, 520f)),
            
            new OpLabel(166f, 481f, "Toggle Sticky HUD"),
            new OpKeyBinder(ToggleButton, new Vector2(10f, 470f), new Vector2(150f, 40f)),
            
            new OpLabel(40f, 439f, "Toggle Sticky HUD on by default"),
            new OpCheckBox(DefaultEnabled, new Vector2(10f, 435f)){description = "If disabled, Sticky HUD will start toggled off"}
            
        };
        opTab.AddItems(UIArrPlayerOptions);
    }

}