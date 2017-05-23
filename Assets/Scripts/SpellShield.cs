using System;
using UnityEngine;
using Valve.VR;

namespace Assets.Scripts
{
    public class SpellShield : MonoBehaviour
    {
        private SteamVR_TrackedObject trackedObj;

        private enum Spell
        {
            EARTH,
            FIRE,
            WATER,
            AIR
        }

        Spell currentSpell = Spell.FIRE;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int) trackedObj.index); }
        }

        // Use this for initialization
        private void Start()
        {
            print("Initialized");
        }

        private void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        // Update is called once per frame
        private void Update()
        {
            var touchpadAxis = Controller.GetAxis();
            if (touchpadAxis != Vector2.zero)
            {
                Debug.Log(gameObject.name + touchpadAxis);
                if (isTopZone() && currentSpell != Spell.FIRE)
                {
                    Debug.Log(gameObject.name + " Top Zone (Fire)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.FIRE;
                }
                else if (isBottomZone() && currentSpell != Spell.AIR)
                {
                    Debug.Log(gameObject.name + " Bottom Zone (Air)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.AIR;
                }
                else if (isLeftZone() && currentSpell != Spell.EARTH)
                {
                    Debug.Log(gameObject.name + " Left Zone (Earth)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.EARTH;
                }

                else if (isRightZone() && currentSpell != Spell.WATER)
                {
                    Debug.Log(gameObject.name + " Right Zone (Water)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.WATER;
                }
            }
        }

        private Boolean isTopZone()
        {
            var touchpadAxis = Controller.GetAxis();
            float x = touchpadAxis.x;
            float y = touchpadAxis.y;
            return x > -0.5 && x < 0.5 && y > 0.5;
        }

        private Boolean isLeftZone()
        {
            var touchpadAxis = Controller.GetAxis();
            float x = touchpadAxis.x;
            float y = touchpadAxis.y;
            return x < -0.5 && y > -0.5 && y < 0.5;
        }

        private Boolean isRightZone()
        {
            var touchpadAxis = Controller.GetAxis();
            float x = touchpadAxis.x;
            float y = touchpadAxis.y;
            return x > 0.5 && y > -0.5 && y < 0.5;
        }

        private Boolean isBottomZone()
        {
            var touchpadAxis = Controller.GetAxis();
            float x = touchpadAxis.x;
            float y = touchpadAxis.y;
            return x > -0.5 && x < 0.5 && y < -0.5;
        }

        public String getCurrentSpell()
        {
            return currentSpell.ToString();
        }
    }
}