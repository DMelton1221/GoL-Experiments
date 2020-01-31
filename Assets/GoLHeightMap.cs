using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLHeightMap : MonoBehaviour
{

    private Texture2D tex;
    private GridType<int> HeightMap;
    private GoL_logic game;

    GoLHeightMap() {
        game = new GoL_logic();
    }


    public void RenderBoard() {
        var currentColor = this.tex.GetRawTextureData<Color32>();

        for(int i = 0; i < tex.width * tex.height; i++ ) {
            var game_board = game.GetBoard();
            int w = game_board.Width();
            int h = game_board.Height();

            var colW = new Color32(255,255,255,255);
            var colB = new Color32(0,0,0,255);

            currentColor[i] = game_board[i % w, i / h] ? colW : colB;

        }
        tex.Apply();
    }

    void Awake() {
        game.Populate();
        tex = new Texture2D(game.GetBoard().Width(), game.GetBoard().Height(), TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

    }
    // Start is called before the first frame update
    void Start() {
        RenderBoard();
    }

    // Update is called once per frame
    void Update() {
        game.NextBoardState();
        RenderBoard();
    }

    void OnRenderObject() {
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), tex);
    }
}
