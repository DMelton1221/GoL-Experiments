using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoL_logic : MonoBehaviour
{

    private BitGridType game_board;
    private BitGridType next_board;

    private int[] population_stable;
    private int[] population_grow;
    private bool should_cull;

    //Gets a section of the game region
    public GridType<int> GetGameRegion(int x1, int y1, int x2, int y2) {
        GridType<int> region = (BitGridType) game_board.DeepCopyRegion(x1, y1, x2, y2);
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

        if ( this.CheckStability(sum) ) {
            if ( this.CheckGrowth(sum) ) {
                return true;
            }
            return game_board[x, y];
        } else {
            return false;
        }
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



    void Awake() {
        this.Populate();
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        this.NextBoardState();
    }
}
