using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WordModifier", menuName = "Word Modifier")]
public class WordModifier : ScriptableObject
{
    public float warriorModifier;
    public float mageModifier;
    public float supportModifier;
    public float rogueModifier;
}
