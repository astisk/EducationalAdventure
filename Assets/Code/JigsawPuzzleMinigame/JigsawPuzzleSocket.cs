﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JigsawPuzzleSocket : MonoBehaviour
{

    //Default value ! means error
    public int ExcpectedPuzzleID = -1;

    public JigsawPuzzlePiece Piece
    {
        get { return piece; }
        set { piece = value; }
    }
    private JigsawPuzzlePiece piece;
}

