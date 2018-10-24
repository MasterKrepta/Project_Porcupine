using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    float soundCooldown = 0;
	// Use this for initialization
	void Start () {
        WorldController.Instance.World.RegisterFurniture(OnFurnitureCreated);
        WorldController.Instance.World.RegisterTileChanged(OnTileChanged);
    }

    private void Update() {
        soundCooldown -= Time.deltaTime;
    }

    void OnTileChanged(Tile tile_data) {
        if(soundCooldown > 0) {
            return;
        }
        //TODO: 
        AudioClip ac =  Resources.Load<AudioClip>("Sounds/Floor_OnCreated");
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }

    public void OnFurnitureCreated(Furniture furn) {
        if (soundCooldown > 0) {
            return;
        }
        //TODO: 
        AudioClip ac = Resources.Load<AudioClip>("Sounds/"+furn.ObjectType+"_OnCreated");
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }
}
