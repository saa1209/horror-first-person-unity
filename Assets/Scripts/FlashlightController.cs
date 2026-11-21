using UnityEngine;

namespace HorrorGame
{
    [RequireComponent(typeof(Light))]
    public sealed class FlashlightController : MonoBehaviour
    {
        [SerializeField] private Light flashlight;
        [SerializeField] private float maxBattery = 100f;
        [SerializeField] private float drainPerSecond = 2f;
        [SerializeField] private float rechargePerSecond = 15f;

        public float Battery01 => Mathf.Clamp01(maxBattery - batteryDrain);

        private float batteryDrain;

        private void Awake()
        {
            if (flashlight == null)
                flashlight = GetComponent<Light>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                flashlight.enabled = !flashlight.enabled;

            if (flashlight.enabled)
            {
                batteryDrain = Mathf.Min(maxBattery, batteryDrain + drainPerSecond * Time.deltaTime);
                if (batteryDrain >= maxBattery)
                    flashlight.enabled = false;
            }
            else
            {
                batteryDrain = Mathf.Max(0f, batteryDrain - rechargePerSecond * Time.deltaTime);
            }

            flashlight.intensity = Mathf.Lerp(0.35f, 1f, Battery01);
        }

        public void Recharge(float amount)
        {
            batteryDrain = Mathf.Max(0f, batteryDrain - amount);
        }
    }
}
