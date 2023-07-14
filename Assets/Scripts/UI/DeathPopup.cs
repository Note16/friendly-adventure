using Assets.Scripts.Characters.Player;
using Assets.Scripts.Dungeon;
using Assets.Scripts.Sounds;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DeathPopup : MonoBehaviour
    {
        public void OnRestartDungeon()
        {
            var dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();
            dungeonGenerator.GenerateDungeon();
            var player = FindAnyObjectByType<PlayerController>();
            player.GetComponent<SpriteRenderer>().enabled = true;
            player.GetComponent<PlayerStats>().ResetPlayer();
            SoundManager.instance.musicSource.Play();
            gameObject.SetActive(false);
        }
    }
}