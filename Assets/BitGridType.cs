using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Name is currently a misnomer, it is a GridType with bool specializations unlike a type which actually sets bits. Would prefer to use C++ Template Specializations, but inheritence is sufficient.
public class BitGridType : GridType<bool>
{

    public BitGridType(int x, int y) :base(x,y) {}


    //Finds the least dimension of two input grids, and returns the result of the | operator on each bool stored in each underlying array
    public static BitGridType operator |(BitGridType a, BitGridType b) {
        int w, h;

        if (a.Width() <= b.Width()) {
            w = a.Width();
        } else {
            w = b.Width();
        }

        if (a.Height() <= b.Height()) {
            h = a.Height();
        } else {
            h = b.Height();
        }

        BitGridType to_return = new BitGridType(w, h);

        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {

                to_return[i, j] = a[i, j] | b[i, j];
            }
        }

        return to_return;
    }

    //Finds the least dimension of two input grids, and returns the result of the & operator on each bool stored in each underlying array.
    //Not necessary for current usage, but the class as a standalone would have such an operator.
    public static BitGridType operator &(BitGridType a, BitGridType b) {
        int w, h;

        if (a.Width() <= b.Width()) {
            w = a.Width();
        } else {
            w = b.Width();
        }

        if (a.Height() <= b.Height()) {
            h = a.Height();
        } else {
            h = b.Height();
        }

        BitGridType to_return = new BitGridType(w, h);

        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                to_return[i, j] = a[i, j] & b[i, j];
            }
        }

        return to_return;
    }


    public static BitGridType operator ^(BitGridType a, BitGridType b) {
        int w, h;

        if (a.Width() <= b.Width()) {
            w = a.Width();
        } else {
            w = b.Width();
        }

        if (a.Height() <= b.Height()) {
            h = a.Height();
        } else {
            h = b.Height();
        }

        BitGridType to_return = new BitGridType(w, h);

        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                to_return[i, j] = a[i, j] ^ b[i, j];
            }
        }

        return to_return;
    }


    //C# bools aren't castable to ints, and the choices are to either make a helper object to perform the cast, or provide a method to this class. Long term will probably create a generic helper object. 
    public static implicit operator GridType<int>(BitGridType t) {
        int w, h;
        w = t.Width();
        h = t.Height();
        GridType<int> val = new GridType<int>(w, h);

        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                val[i, j] = t[i, j] ? 1 : 0;
            }
        }

        return val;
    }
}
