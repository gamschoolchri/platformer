using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyDataSO", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyDataSO : CharacterDataSO
{

    [SerializeField] private MovementType type;
    [SerializeField] private VectorRange targetDetectRange;
    [SerializeField] private VectorRange targetSwitchRange;
    [SerializeField] private VectorRange targetMaintainRange;

    [SerializeReference, SubclassSelector]
    private List<DetectCondition> detectConditions;



    public IReadOnlyList<DetectCondition> DetectConditions => detectConditions;
    public MovementType MovementType => type;
    public VectorRange TargetDetectRange => targetDetectRange;
    public VectorRange TargetSwitchRange => targetSwitchRange;
    public VectorRange TargetMaintainRange => targetMaintainRange;

}