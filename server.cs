//basically robbed this from BLG default prefs
//https://forum.blockland.us/index.php?topic=320521.0 heres a good reference
function TOA_registerPref(%cat, %title, %type, %variable, %addon, %default, %params, %callback, %legacy, %isSecret, %isHostOnly) {
    new ScriptObject(Preference)
    {
        className     = "SOA_preference";

        addon         = %addon;
        category      = %cat;
        title         = %title;

        type          = %type;
        params        = %params;

        variable      = %variable;

        defaultValue  = %default;

        hostOnly      = %isHostOnly;
        secret        = %isSecret;

        loadNow        = false; // load value on creation instead of with pool (optional)
        noSave         = false; // do not save (optional)
        requireRestart = false; // denotes a restart is required (optional)
    };
}

if(!isObject(ScriptThrottleOnActivatePrefs))
{
    registerPreferenceAddon("Script_ThrottleOnActivate", "Settings", "clock_add");

    //general regen settings
    WV_registerPref("Throttle Activate Options", "Enabled",             "bool", "$Pref::ThrottleActivate::Enabled",     "Script_ThrottleOnActivate", 0,   "");
	WV_registerPref("Throttle Activate Options", "Player Delay (sec)",  "num",  "$Pref::ThrottleActivate::PlayerDelay", "Script_ThrottleOnActivate", 0.1, "0 100 0.01");
	WV_registerPref("Throttle Activate Options", "Brick  Delay (sec)",  "num",  "$Pref::ThrottleActivate::BrickDelay",  "Script_ThrottleOnActivate", 0, "0 100 0.01");
}


package Script_ThrottleOnActivate
{
	function fxDTSBrick::onActivate (%obj, %player, %client, %pos, %vec)
	{
		if($Pref::ThrottleActivate::PlayerDelay > 0 && $Sim::Time - %player.lastOnActivateCall < $Pref::ThrottleActivate::PlayerDelay
		|| $Pref::ThrottleActivate::BrickDelay  > 0 && $Sim::Time - %obj.lastOnActivateCall    < $Pref::ThrottleActivate::BrickDelay )
			return;
		
		%obj.lastOnActivateCall = %player.lastOnActivateCall = $Sim::Time;
		return parent::onActivate (%obj, %player, %client, %pos, %vec);
	}
};
activatePackage(Script_ThrottleOnActivate);