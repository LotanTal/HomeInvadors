using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "DialogueData/New Dialogue", order = 1)]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public struct DialogueInfo
    {
        [TextArea(3, 10)]
        public string sentence;
        public string playerAnimation;
        public string npcAnimation;
        public string enemyAnimation;
        public string enemyEYEAnimation;
        public string soundName;
        public string virtualCameraName;
    }


    public string characterName;
    public DialogueInfo[] normalSentences;
    public DialogueInfo[] sentencesAfterTalk;
}