using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStates
{
    [CreateAssetMenu(fileName = "New level", menuName = "Levels/New Level", order = 0)]
    public class LevelObject : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _preview;
        [SerializeField] private int _levelLoadIndex;
        [SerializeField] private LevelType Type;
        public string LevelName => _name;
        public string Description => _description;
        public int LevelLoadIndex => _levelLoadIndex;
        public Sprite Preview => _preview;
        
        public enum LevelType
        {
            TankBattle,
            ArtilleryPreparation,
            ArtileryDuel
        }
    }
}