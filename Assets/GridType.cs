using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


//TO-DO: finish the class, currently only has a minimal set of functionality that I will be using or may use at this time.
public class GridType<T> {
   
    private T[,] board;
    private int width;
    private int height;

    public GridType(int width, int height)
    {
        this.width = width;
        this.height = height;
        board = new T[width, height];
    }

    virtual public GridType<T> ShallowCopy() {
        return (GridType<T>) this.MemberwiseClone();
    }

    virtual public GridType<T> DeepCopy() {
        GridType<T> t = new GridType<T>(width, height);

        for(int j = 0; j < height; j++) {
            for(int i = 0; i < width; i++) {
                t[i, j] = this[i, j];
            }
        }

        return t;
    }

    //will copy a region and will wrap using a toroidial configuration to copy elements out of range. Does so by finding (index + size) % size
    public GridType<T> DeepCopyRegion(int x1, int y1, int x2, int y2) {
        GridType<T> temp = new GridType<T>(x2 - x1 + 1, y2 - y1 + 1);
        int regIterX = 0;
        int regIterY = 0;
        int w = width;
        int h = height;

        for(int j = y1; j <= y2; j++) {
            regIterX = 0;
            for(int i = x1; i <= x2; i++) {
                temp[regIterX++, regIterY] = this[(i + w) % w, (j + h) % h];
            } regIterY++;
        }

        return temp;
    }

    public int Width() {
        return width;
    }

    public int Height() {
        return height;
    }

    public T this[int x, int y] {
        get {
            if (x > width || x < 0) {
                throw new ArgumentOutOfRangeException("x", "first indexer value to type GridType during get is out of range");
            }
            
            if (y > height || y < 0) {
                throw new ArgumentOutOfRangeException("y", "second indexer value to type GridType during get is out of range");
            }
            return board[x, y];
        }
        set {
            if (x > width || x < 0) {
                throw new ArgumentOutOfRangeException("x", "first indexer value to type GridType during set is out of range");
            }

            if (y > height || y < 0) {
                throw new ArgumentOutOfRangeException("y", "second indexer value to type GridType during set is out of range");
            }

            board[x, y] = value;
        }
    }
};
