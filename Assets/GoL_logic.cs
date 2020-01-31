using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class GoL_logic
{

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

    public BitGridType GetBoard() {
        return game_board;
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
    public void NextBoardState() {
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
    public void Populate() {
        int w = game_board.Width();
        int h = game_board.Height();

        for(int j = 0; j < h;  j++) {
            for(int i = 0; i < w;  i++) {
                game_board[i, j] = Random.Range(0,2) == 1;
            }
        }
    }


    public GoL_logic(int width, int height, int[] stability_range, int[] growth_range, bool only_test_neighbors) {
        game_board = new BitGridType(width, height);
        next_board = new BitGridType(width, height);
        population_stable = stability_range;
        population_grow = growth_range;
        should_cull = only_test_neighbors;
    }

    //Default constructor creates Life with the standard ruleset and a 100 by 100 game board;
    public GoL_logic() : this(100,100, new int[]{2,3}, new int[]{3,3}, true) {}

    //Creates a Life simulation with custom dimensions;
    public GoL_logic(int width, int height) : this(width, height, new int[]{2,3}, new int[]{3,3}, true) {}
}
