using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoL_logic : MonoBehaviour
{

    private BitGridType game_board;
    private BitGridType next_board;

    private int[] population_stable;
    private int[] population_grow;

    //For now, we will just test with a hard coded value;
    public GoL_logic() {
        game_board = new BitGridType(100,100);
        next_board = new BitGridType(100,100);
        population_stable = new int[]{2,3};
        population_grow = new int[]{3,3};
    }

    public GridType<int> GetGameRegion(BitGridType board, int x1, int y1, int x2, int y2) {
        GridType<int> region = (BitGridType) game_board.DeepCopyRegion(x1, y1, x2, y2);
        return region;
    }

    public int GetGameRegionSum(BitGridType board, int x1, int y1, int x2, int y2) {
        GridType<int> region = GetGameRegion(board, x1, y1, x2, y2);

        int sum = 0;

        for(int j = 0; j < region.Height(); j++) {
            for(int i = 0; i < region.Width(); i++) {
                sum += region[i, j];
            }
        } return sum;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
