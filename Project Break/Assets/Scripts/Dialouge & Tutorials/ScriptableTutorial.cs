﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Dialouge Tutorial System/New Tutorial")]
public class ScriptableTutorial : ScriptableObject
{
    public string Name;
    public string[] Lines;
}