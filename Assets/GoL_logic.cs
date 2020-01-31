using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class GoL_logic : MonoBehaviour
{

    Texture2D tex;

    private BitGridType game_board;
    private BitGridType next_board;

    private int[] population_stable;
    private int[] population_grow;
    private bool should_cull;

    //Gets a section of the game region
    public GridType<int> GetGameRegion(int x1, int y1, int x2, int y2) {
        var region_temp = game_board.DeepCopyRegion(x1, y1, x2, y2);
        var region = new GridType<int>(region_temp.Width(), region_temp.Height());

        for(int j = 0; j < region_temp.Height(); j++ ) {
            for( int i = 0; i < region_temp.Width(); i++ ) {
                region[i, j] = region_temp[i, j] ? 1 : 0;
            }
        }

        return region;
    }

    //Gets the sum of the values in a game region
    public int GetGameRegionSum(int x1, int y1, int x2, int y2) {
        GridType<int> region = GetGameRegion( x1, y1, x2, y2);

        int sum = 0;

        for(int j = 0; j < region.Height(); j++) {
            for(int i = 0; i < region.Width(); i++) {
                sum += region[i, j];
            }
        } return sum;
    }

    //Check if a value is in the population stability range.
    public bool CheckStability(int val) {
        return ( val >= population_stable[0] && val <= population_stable[1] );
    }

    //Check if a value is in the population growth range.
    public bool CheckGrowth(int val) {
        return ( val >= population_grow[0] && val <= population_grow[1] );
    }

    //Get the next state for an indivudal cell
    public bool NextCellState( int x, int y) {
        int sum = GetGameRegionSum(x - 1, y - 1, x + 1, y + 1);

        if ( should_cull )
            sum -= game_board[x, y] ? 1 : 0;

        if ( CheckGrowth(sum) ) 
            return true;

        if ( CheckStability(sum) )
            return game_board[x, y];

        return false;
    }

    //Mutate game_board to store the next state for all game board cells.
    void NextBoardState() {
        int w = game_board.Width();
        int h = game_board.Height();

        for(int j = 0; j < h;  j++) {
            for(int i = 0; i < w;  i++) {
                next_board[i,j] = NextCellState(i, j);
            }
        }

        game_board = next_board;
        next_board = new BitGridType(next_board.Width(), next_board.Height());
    }


    //Populate the game board with random noise.
    void Populate() {
        int w = game_board.Width();
        int h = game_board.Height();

        for(int j = 0; j < h;  j++) {
            for(int i = 0; i < w;  i++) {
                game_board[i, j] = Random.Range(0,2) == 1;
            }
        }
    }


    //For now, we will just test with a hard coded state;
    public GoL_logic() {
        game_board = new BitGridType(100,100);
        next_board = new BitGridType(100,100);
        population_stable = new int[]{2,3};
        population_grow = new int[]{3,3};
        should_cull = true;
    }

    public void RenderBoard() {
        var currentColor = this.tex.GetRawTextureData<Color32>();

        for(int i = 0; i < tex.width * tex.height; i++ ) {
            int w = game_board.Width();
            int h = game_board.Height();

            var colW = new Color32(255,255,255,255);
            var colB = new Color32(0,0,0,255);

            currentColor[i] = game_board[i % w, i / h] ? colW : colB;

        }
        tex.Apply();
    }

    void Awake() {
        Populate();
        tex = new Texture2D(game_board.Width(), game_board.Height(), TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

    }
    // Start is called before the first frame update
    void Start() {
        RenderBoard();
    }

    // Update is called once per frame
    void Update() {
        NextBoardState();
        RenderBoard();
    }

    void OnRenderObject() {
        GL.PushMatrix();
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), tex);
        GL.PopMatrix();
    }

}
