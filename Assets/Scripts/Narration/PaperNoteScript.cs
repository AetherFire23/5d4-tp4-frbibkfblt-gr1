using System;
using TMPro;
using UnityEngine;

namespace Narration
{
    public class PaperNoteScript : MonoBehaviour
    {
        [SerializeField] private string text;

        private TextMeshProUGUI narrativePanel;

        public void Awake()
        {
        }

        public void Start()
        {
            this.narrativePanel = GameObject.FindGameObjectWithTag("NarrativeText").GetComponent<TextMeshProUGUI>();
        }

        public void OnTriggerEnter(Collider collision)
        {
            NoclipPlayerController player = collision.gameObject.GetComponent<NoclipPlayerController>();
            if (player is null)
            {
                return;
            }

            this.narrativePanel.enabled = true;
            this.narrativePanel.text = this.text;
        }

        public void OnTriggerExit(Collider collision)
        {
            NoclipPlayerController player = collision.gameObject.GetComponent<NoclipPlayerController>();
            if (player is null)
            {
                return;
            }

            this.narrativePanel.enabled = false;
            this.narrativePanel.text = "";
        }
    }
}