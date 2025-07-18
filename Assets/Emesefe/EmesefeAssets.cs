using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Emesefe
{
    public class EmesefeAssets : MonoBehaviour 
    {

        // Internal instance reference
        private static EmesefeAssets instance; 

        // Instance reference
        public static EmesefeAssets Instance {
            get {
                if (instance == null) instance = Instantiate(Resources.Load<EmesefeAssets>("EmesefeAssets")); 
                return instance; 
            }
        }
        
        public Sprite whiteSprite;
        public Sprite whiteCircleSprite;
        public Material whiteMaterial;
        
        public TMP_FontAsset defaultFont;

    }
}

