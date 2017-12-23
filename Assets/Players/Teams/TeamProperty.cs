using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TeamProperty : ScriptableObject {
    public PlayerTeam team;
    public Material bulletMat;
    public Color teamColor;
	public int distortionIndex;

    [Header("Controls")]
    public string horizontalInput;
	public string verticalInput;
	public string shotInput;
}

public enum PlayerTeam
{
    CYAN, MAGENTA, NEITHER
}