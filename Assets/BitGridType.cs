using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitGridType : Grid<bool>
{

    public BitGridType(int x, int y) :base(x,y) {}

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
}
