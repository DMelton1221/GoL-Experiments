using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLHeightMap : MonoBehaviour
{

    private Texture2D texBoard;
    private Texture2D texMap;
    private GridType<int> heightMap;
    private GoL_logic game;
    private HashSet<float> means;

    GoLHeightMap() {
        game = new GoL_logic();
        heightMap = new GridType<int>(game.GetBoard().Width(), game.GetBoard().Height());
        means = new HashSet<float>();
    }


    public void RenderBoard() {
        var currentColor = texBoard.GetRawTextureData<Color32>();
        var game_board = game.GetBoard();
        int w = game_board.Width();
        int h = game_board.Height();
        var colW = new Color32(255,255,255,255);
        var colB = new Color32(0,0,0,255);

        for(int i = 0; i < texBoard.width * texBoard.height; i++ ) {
            currentColor[i] = game_board[i % w, i / h] ? colW : colB;
        }
        texBoard.Apply();
    }

    public void RenderHeightMap() {
        var currentColor = texMap.GetRawTextureData<Color32>();
        int w = heightMap.Width();
        int h = heightMap.Height();

        for(int i = 0; i < texBoard.width * texBoard.height; i++ ) {
            byte component = (byte) heightMap[i % w, i / h];

            currentColor[i] = new Color32(component, component, component, 255);
        }
        texMap.Apply();
    }

    public void SumBoards() {
        var h = heightMap.Height();
        var w = heightMap.Width();
        for(int j = 0; j < h; j++ ) {
            for(int i = 0; i < w;  i++) {
                heightMap[i, j] += game.GetBoard()[i, j] ? 1 : 0;
            }
        }
    }

    public GridType<int> SumBoards(GridType<int> heightMap) {
        var h = heightMap.Height();
        var w = heightMap.Width();
        for(int j = 0; j < h; j++ ) {
            for(int i = 0; i < w;  i++) {
                heightMap[i, j] += game.GetBoard()[i, j] ? 1 : 0;
            }
        }

        return heightMap;
    }

    public BitGridType UnionSteps(int iter) {
        BitGridType region  = new BitGridType(game.GetBoard().Width(), game.GetBoard().Height());
        for(int i = 0; i < iter;  i++) {
            game.NextBoardState();
            region |= game.GetBoard();
        }

        return region;
    }

    void Awake() {
        game.Populate();
        texBoard = new Texture2D(game.GetBoard().Width(), game.GetBoard().Height(), TextureFormat.RGBA32, false);
        texBoard.filterMode = FilterMode.Point;

        texMap = new Texture2D(heightMap.Width(), heightMap.Height(), TextureFormat.RGBA32, false);

    }
    // Start is called before the first frame update
    void Start() {
        RenderBoard();
    }

    // Update is called once per frame
    void Update() {
        var w = game.game_board.Width();
        var h = game.game_board.Height();
        var sum = game.GetGameRegionSum(0,0,w-1, h-1);
        var mean = (w*h/((float)sum));
        game.game_board = UnionSteps(2);
        RenderBoard();
        SumBoards();
        RenderHeightMap();

        if (!means.Contains(mean)) {
            means.Add(mean);
            Debug.Log(mean.ToString());
            Debug.Log(means.Count);
        }
    }

    void OnRenderObject() {
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), texBoard);
        Graphics.DrawTexture(new Rect(1, 0, 1, 1), texMap);
    }
}
