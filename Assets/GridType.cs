using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T> {
   
    private T[,] board;
    private int width;
    private int height;

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
        board = new T[width, height];
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
                throw new ArgumentOutOfRangeException("x", "first indexer value to object Grid during get is out of range");
            }
            
            if (y > height || y < 0) {
                throw new ArgumentOutOfRangeException("y", "second indexer value to object Grid during get is out of range");
            }
            return board[x, y];
        }
        set {
            if (x > width || x < 0) {
                throw new ArgumentOutOfRangeException("x", "first indexer value to object Grid during set is out of range");
            }

            if (y > height || y < 0) {
                throw new ArgumentOutOfRangeException("y", "second indexer value to object Grid during set is out of range");
            }

            board[x, y] = value;
        }
    }
};
