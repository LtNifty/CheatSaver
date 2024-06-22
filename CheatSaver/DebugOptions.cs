using System;
using ThunderRoad;

namespace CheatSaver
{
    [Serializable]
    public class DebugOptions
    {
        public bool invincibility;
        public bool instantSpellcasting;
        public bool infiniteFocus;
        public bool infiniteImbue;
        public bool infiniteArrows;
        public bool bottomlessQuivers;
        public bool freeClimb;
        public bool easyDismemberment;
        public bool fallDamage;
        public bool armorDetection;
        public float slowMotionScale;
        public bool selfCollision;
        public bool useBreakables;
        public GameManager.CollisionDebug collisionMarkers;
    }
}