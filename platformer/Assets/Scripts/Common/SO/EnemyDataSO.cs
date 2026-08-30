using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyDataSO", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyDataSO : CharacterDataSO
{

    [SerializeField] private MovementType type;
    [SerializeField] private VectorRange detectRange;
    [SerializeField] private VectorRange detectLastingRange;

    [SerializeReference, SubclassSelector]
    private List<DetectCondition> detectConditions;



    public IReadOnlyList<DetectCondition> DetectConditions => detectConditions;
    public MovementType MovementType => type;
    public VectorRange DetectRange => detectRange;
    public VectorRange DetectLastingRange => detectLastingRange;

}