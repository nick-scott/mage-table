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
                SkinnedMeshRenderer skinnedMeshRenderer = GameObject.Find("Wand").GetComponentInChildren<SkinnedMeshRenderer>();
                Material[] materials = skinnedMeshRenderer.materials;

                Debug.Log(gameObject.name + touchpadAxis);
                if (isTopZone() && currentSpell != Spell.FIRE)
                {
                    Debug.Log(gameObject.name + " Top Zone (Fire)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.FIRE;
                    skinnedMeshRenderer.material = materials[0];

                }
                else if (isBottomZone() && currentSpell != Spell.AIR)
                {
                    Debug.Log(gameObject.name + " Bottom Zone (Air)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.AIR;
                    skinnedMeshRenderer.material = materials[1];
                }
                else if (isLeftZone() && currentSpell != Spell.EARTH)
                {
                    Debug.Log(gameObject.name + " Left Zone (Earth)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.EARTH;
                    skinnedMeshRenderer.material = materials[2];
                }

                else if (isRightZone() && currentSpell != Spell.WATER)
                {
                    Debug.Log(gameObject.name + " Right Zone (Water)");
                    Controller.TriggerHapticPulse(3999);
                    currentSpell = Spell.WATER;
                    skinnedMeshRenderer.material = materials[3];
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