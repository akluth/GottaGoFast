using GottaGoFast.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace GottaGoFast
{
    public class ModEntry : Mod
    {
        private ModConfig Config;

        private readonly SButton sprintKey = SButton.RightShoulder;
        private bool isAlreadyGoingFast = false;
        private int addedSpeed;
        private ITranslationHelper i18n;

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            this.addedSpeed = this.Config.DefaultSpeed;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            i18n = helper.Translation;
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            Helper.Input.Suppress(sprintKey);

            if (!Context.IsPlayerFree)
            {
                return;
            }

            if (!e.Button.Equals(sprintKey)) return;
            if (!isAlreadyGoingFast)
            {
                isAlreadyGoingFast = true;
                addedSpeed = this.Config.RunningSpeed;

                if (this.Config.ShowHudMessage)
                {
                    Game1.addHUDMessage(new HUDMessage(i18n.Get("gotta-go-fast.activated.hudmessage"), 2));
                }
            }

            else if (isAlreadyGoingFast)
            {
                isAlreadyGoingFast = false;
                addedSpeed = this.Config.DefaultSpeed;

                if (this.Config.ShowHudMessage)
                {
                    Game1.addHUDMessage(new HUDMessage(i18n.Get("gotta-go-fast.deactivated.hudmessage"), 2));
                }
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (Context.IsPlayerFree)
            {
                Game1.player.addedSpeed = addedSpeed;
            }
        }
    }
}