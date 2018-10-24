using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour {

    Dictionary<Character, GameObject> characterGOMap;

    Dictionary<string, Sprite> characterSprites;

    public World World { get { return WorldController.Instance.World; } }

    void Start() {

        LoadSprites();

        //Instantiate Dictionary to track data to objects

        characterGOMap = new Dictionary<Character, GameObject>();

        World.RegisterCharacter(OnCharacterCreated);

        Character c = World.CreateCharacter(World.GetTileAt(World.Width / 2, World.Height / 2));
        //c.SetDest(World.GetTileAt(World.Width / 2 + 5, World.Height / 2));
    }

    private void LoadSprites() {
        characterSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/Characters");

        foreach (Sprite s in sprites) {
            //Debug.Log(s);
            characterSprites[s.name] = s;
        }
    }

    public void OnCharacterCreated(Character c) {
        GameObject char_go = new GameObject();
        characterGOMap.Add(c, char_go); // Pair to dictionary

        char_go.name = "Character";

        char_go.transform.position = new Vector3(c.X, c.Y, 0);

        char_go.transform.SetParent(this.transform, true);


        SpriteRenderer sr = char_go.AddComponent<SpriteRenderer>();
        sr.sprite = characterSprites["p1_front"];
        sr.sortingLayerName = "Characters";

        c.RegisterOnChanged(OnCharacterChanged);
    }

    void OnCharacterChanged(Character c) {

        //Make sure furniture graphics are correct
        if (characterGOMap.ContainsKey(c) == false) {
            Debug.LogError("OnCharacterChanged: -- trying to change visuals for character not in map");
            return;
        }

        GameObject char_go = characterGOMap[c];

        char_go.transform.position = new Vector3(c.X, c.Y, 0);
    }

}
