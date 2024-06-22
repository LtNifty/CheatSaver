using ThunderRoad;
using System.IO;
using Newtonsoft.Json;

namespace CheatSaver
{
    public class CheatSaverScript : ThunderScript
    {
        private static string debugFileName = FileManager.aaModPath + "/CheatSaver/DebugOptions.opt";
        private static DebugOptions debugOptions;
        private bool isFlipped;

        public override void ScriptLoaded(ModManager.ModData modData)
        {
            base.ScriptLoaded(modData);
            debugOptions = new DebugOptions();
            EventManager.onPossess += OnPossessionEvent;
            EventManager.onUnpossess += OnUnpossessionEvent;
        }

        private void OnPossessionEvent(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
            {
                if (!File.Exists(debugFileName))
                    SaveDebugSettings();

                LoadDebugSettings();
            }
        }

        private void OnUnpossessionEvent(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
                isFlipped = false;
        }

        public override void ScriptUpdate()
        {
            base.ScriptUpdate();
            if (Player.currentCreature != null)
            {
                if (UIPlayerMenu.instance.IsShownToPlayer != isFlipped)
                {
                    isFlipped = !isFlipped;
                    if (!UIPlayerMenu.instance.IsShownToPlayer && DebugSettingsChanged())
                        SaveDebugSettings();
                }
            }
        }

        private static bool DebugSettingsChanged()
        {
            return debugOptions.invincibility != Player.invincibility
                || debugOptions.instantSpellcasting != Mana.fastCast
                || debugOptions.infiniteFocus != Mana.infiniteFocus
                || debugOptions.infiniteImbue != Imbue.infiniteImbue
                || debugOptions.infiniteArrows != ItemModuleBow.forceAutoSpawnArrow
                || debugOptions.bottomlessQuivers != Holder.infiniteSupply
                || debugOptions.freeClimb != RagdollHandClimb.climbFree
                || debugOptions.easyDismemberment != Damager.easyDismemberment
                || debugOptions.fallDamage != Player.fallDamage
                || debugOptions.armorDetection != Creature.meshRaycast
                || debugOptions.slowMotionScale != Player.currentCreature.mana.GetPowerSlowTime().scale
                || debugOptions.selfCollision != Player.selfCollision
                || debugOptions.useBreakables != Breakable.AllowBreaking
                || debugOptions.collisionMarkers != GameManager.local.collisionDebug;
                
        }

        private static void LoadDebugSettings()
        {
            DebugOptions options;

            TextReader reader = null;
            try
            {
                reader = new StreamReader(debugFileName);
                var fileContents = reader.ReadToEnd();
                options = JsonConvert.DeserializeObject<DebugOptions>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            if (options != null)
            {
                Player.invincibility = options.invincibility;
                Mana.fastCast = options.instantSpellcasting;
                Mana.infiniteFocus = options.infiniteFocus;
                Imbue.infiniteImbue = options.infiniteImbue;
                ItemModuleBow.forceAutoSpawnArrow = options.infiniteArrows;
                Holder.infiniteSupply = options.bottomlessQuivers;
                RagdollHandClimb.climbFree = options.freeClimb;
                Damager.easyDismemberment = options.easyDismemberment;
                Player.fallDamage = options.fallDamage;
                Creature.meshRaycast = options.armorDetection;
                Player.currentCreature.mana.GetPowerSlowTime().scale = options.slowMotionScale;
                Player.selfCollision = options.selfCollision;
                Breakable.AllowBreaking = options.useBreakables;
                GameManager.local.collisionDebug = options.collisionMarkers;
            }
        }

        private static void SaveDebugSettings()
        {
            debugOptions.invincibility = Player.invincibility;
            debugOptions.instantSpellcasting = Mana.fastCast;
            debugOptions.infiniteFocus = Mana.infiniteFocus;
            debugOptions.infiniteImbue = Imbue.infiniteImbue;
            debugOptions.infiniteArrows = ItemModuleBow.forceAutoSpawnArrow;
            debugOptions.bottomlessQuivers = Holder.infiniteSupply;
            debugOptions.freeClimb = RagdollHandClimb.climbFree;
            debugOptions.easyDismemberment = Damager.easyDismemberment;
            debugOptions.fallDamage = Player.fallDamage;
            debugOptions.armorDetection = Creature.meshRaycast;
            debugOptions.slowMotionScale = Player.currentCreature.mana.GetPowerSlowTime().scale;
            debugOptions.selfCollision = Player.selfCollision;
            debugOptions.useBreakables = Breakable.AllowBreaking;
            debugOptions.collisionMarkers = GameManager.local.collisionDebug;

            TextWriter writer = null;
            try
            {
                var contents = JsonConvert.SerializeObject(debugOptions);
                writer = new StreamWriter(debugFileName);
                writer.Write(contents);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}