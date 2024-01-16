using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        [SerializeField] private string input;
        private Text textHolder;


        private void Awake()
        {
            textHolder = GetComponent<Text>();
            StartCoroutine(WriteText(input, textHolder));
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}

