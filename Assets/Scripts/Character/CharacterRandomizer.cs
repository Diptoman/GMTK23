using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRandomizer : MonoBehaviour
{
    [Serializable]
    public struct Reference
    {
        public MeshMarker[] body;
        public MeshMarker[] face;
        public MeshMarker[] torso;
        public MeshMarker[] hands;
        public MeshMarker[] hat;
        public MeshMarker[] boots;
        public MeshMarker[] weapon;
    }

    public CharacterClass selectedClass;

    public Reference reference;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RandomizeCharacter(selectedClass);
        }
    }

    public void RandomizeCharacter(CharacterClass charClass)
    {
        var canHatBeEmpty = charClass == CharacterClass.Warrior;
        var canHaveNoShirt = charClass == CharacterClass.Warrior || charClass == CharacterClass.Rogue;
        var canHaveNoHat = charClass == CharacterClass.Warrior || charClass == CharacterClass.Rogue;
        var canHaveNoWeapon = charClass == CharacterClass.Warrior || charClass == CharacterClass.Support;

        RandomizePart(charClass, reference.body);
        RandomizePart(charClass, reference.face);
        RandomizePart(charClass, reference.torso, canHaveNoShirt);
        RandomizePart(charClass, reference.hands);
        RandomizePart(charClass, reference.hat, canHatBeEmpty);
        RandomizePart(charClass, reference.boots, true);
        RandomizePart(charClass, reference.weapon, canHaveNoWeapon);
    }

    private void RandomizePart(CharacterClass characterClass, MeshMarker[] parts, bool canUseNone = false)
    {
        var validParts = new List<MeshMarker>();

        foreach (var mesh in parts)
        {
            mesh.gameObject.SetActive(false);

            if (IsValidPart(characterClass, mesh.classFlag))
                validParts.Add(mesh);
        }

        if (validParts.Count == 0)
        {
            Debug.Log("No valid part found");
            return;
        }

        var minIndex = canUseNone ? -1 : 0;
        var targetIndex = UnityEngine.Random.Range(minIndex, validParts.Count);

        if (targetIndex >= 0)
        {
            validParts[targetIndex].gameObject.SetActive(true);
            validParts[targetIndex].GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private bool IsValidPart(CharacterClass charClass, MeshMarker.ClassFlag flags)
    {
        switch (charClass)
        {
            case CharacterClass.Warrior:
                return flags.HasFlag(MeshMarker.ClassFlag.Warrior);
            case CharacterClass.Mage:
                return flags.HasFlag(MeshMarker.ClassFlag.Mage);
            case CharacterClass.Rogue:
                return flags.HasFlag(MeshMarker.ClassFlag.Rogue);
            case CharacterClass.Support:
                return flags.HasFlag(MeshMarker.ClassFlag.Support);
        }

        return false;
    }
}
