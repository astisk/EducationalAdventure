﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class JigsawPuzzleManager : MonoBehaviour
{
    //Image that will be devided into puzzles
    [SerializeField]
    Texture2D inputImage;

    //Describes how many puzzles there will be in x and y axises
    [SerializeField]
    Vector2Int jigsawPuzzleCount;

    [SerializeField]
    Transform unusedPuzzlePieceContainer;

    [SerializeField]
    Transform puzzleSocketContainer;

    [SerializeField]
    GameObject socketPrefab;

    [SerializeField]
    GameObject piecePrefab;

    List<JigsawPuzzleSocket> jigsawPuzzleSockets = new List<JigsawPuzzleSocket>();

    // Start is called before the first frame update
    void Start()
    {

        //Difficulty settings
        if(Difficulty.diff == DIFFICULTY.EASY)
        {
            jigsawPuzzleCount.x = 3;
            jigsawPuzzleCount.y = 3;
        }
        else if(Difficulty.diff == DIFFICULTY.MEDIUM)
        {

            jigsawPuzzleCount.x = 5;
            jigsawPuzzleCount.y = 5;
        }
        else if (Difficulty.diff == DIFFICULTY.HARD)
        {

            jigsawPuzzleCount.x = 7;
            jigsawPuzzleCount.y = 7;
        }



        //calculate one piece's texture size
        Vector2Int texSize = new Vector2Int(Mathf.FloorToInt((float)inputImage.width / (float)jigsawPuzzleCount.x),
        Mathf.FloorToInt((float)inputImage.height / (float)jigsawPuzzleCount.y));

        //calculate piece's UI element size
        Rect containerBounds = puzzleSocketContainer.GetComponent<RectTransform>().rect;
        GridLayoutGroup containerLayout = puzzleSocketContainer.GetComponent<GridLayoutGroup>();

        Vector2 objSize = new Vector2((containerBounds.width - containerLayout.padding.left - containerLayout.padding.right) / jigsawPuzzleCount.x,
           (containerBounds.height - containerLayout.padding.bottom - containerLayout.padding.top) / jigsawPuzzleCount.y);
        containerLayout.cellSize = objSize;

        //take the input image and devide it into puzzle pieces
        for (int i = 0; i < jigsawPuzzleCount.x; i++)
        {
            for (int j = 0; j < jigsawPuzzleCount.y; j++)
            {
                //Create puzzle piece
                GameObject puzzlePiece = Instantiate(piecePrefab, unusedPuzzlePieceContainer);

                //get portion of a main picture to assign it as a texture for our new piece
                Texture2D pieceTex = new Texture2D(texSize.x, texSize.y);
                pieceTex.SetPixels(inputImage.GetPixels(i * texSize.x, j * texSize.y,
                texSize.x, texSize.y));
                pieceTex.Apply();

                //Set texture for piece
                puzzlePiece.GetComponent<Image>().sprite = Sprite.Create(pieceTex, new Rect(0.0f, 0.0f, pieceTex.width, pieceTex.height), new Vector2(0.5f, 0.5f));

                //Set fields
                puzzlePiece.GetComponent<JigsawPuzzlePiece>().SetChangeStateHandler(OnStateChanged);
                puzzlePiece.GetComponent<JigsawPuzzlePiece>().SetUnusedContainer(unusedPuzzlePieceContainer);
                puzzlePiece.GetComponent<JigsawPuzzlePiece>().puzzlePieceID = (i * jigsawPuzzleCount.x) + j;

                //Create puzzle socket
                GameObject puzzleSocket = Instantiate(socketPrefab, puzzleSocketContainer);
                //puzzleSocket.transform.localScale = new Vector3(1, 1, 1);
                puzzleSocket.GetComponent<JigsawPuzzleSocket>().ExcpectedPuzzleID = (jigsawPuzzleCount.x - i - 1) + (j * jigsawPuzzleCount.y);
                jigsawPuzzleSockets.Add(puzzleSocket.GetComponent<JigsawPuzzleSocket>());

            }
        }


    }

    void OnStateChanged()
    {
        if (jigsawPuzzleSockets.All(x => x.Piece != null) == true)
        {
            if (jigsawPuzzleSockets.All(x => x.Piece.puzzlePieceID == x.ExcpectedPuzzleID) == true)
            {
                Win();
            }
        }
    }

    void Win()
    {
        //Todo: Win screen?
        Debug.Log("Win");
        SceneChanger.instance.ChangeSceneOnWin("country_scene");
        StateSaver.instance.SetFlag("minigame_jigsaw", true);
    }

}
