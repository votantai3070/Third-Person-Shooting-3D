using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> levelParts;
    [SerializeField] private SnapPoint nextSnapPoint;


    [ContextMenu("Generate Next Level Part")]
    private void GenerationNextLevelPart()
    {
        Transform newPart = Instantiate(ChooseRandomPart());
        LevelPart levelPart = newPart.GetComponent<LevelPart>();

        levelPart.SnapAndAlignPartTo(nextSnapPoint);

        nextSnapPoint = levelPart.GetExitSnapPoint();
    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, levelParts.Count);

        Transform choosePart = levelParts[randomIndex];

        levelParts.RemoveAt(randomIndex);

        return choosePart;
    }
}
