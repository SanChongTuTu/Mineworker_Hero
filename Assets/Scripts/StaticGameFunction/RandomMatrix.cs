using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public static class RandomMatrix
{
    public static int[,] GetARandomMatrix(int row, int col,int rednum,int bluenum,int purplenum,int hardstonenum,int lavastonenum)
    {
        int[,] matrix = new int[row, col];
        int time = 0;
        while (time<999)
        {
            time++;
            matrix = new int[row, col];
            List<(int, int)> testlist = new();

            List<(int x, int y)> canuse = new();
            for (int j = 0; j < matrix.Length; j++)
            {
                if (matrix[j / col, j % col] == 0)
                {
                    canuse.Add((j / col, j % col));
                }
            }
            #region 生成矿石
            for (int i = 0; i < hardstonenum; i++)
            {               
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = -1;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < lavastonenum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 4;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < rednum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 1;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < bluenum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 2;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < purplenum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 3;
                canuse.RemoveAt(randindex);
            }
            #endregion

            for (int j = 0; j < col; j++)
            {
                if(matrix[0, j] != -1)
                testlist.AddRange(MovePace(matrix, (0, j)).Except(testlist));
            }
            List<(int, int)> all0 = new();
            for (int j = 0; j < matrix.Length; j++)
            {
                if (matrix[j / col, j % col] != -1&&!all0.Contains((j / col, j % col)))
                {
                    all0.Add((j / col, j % col));
                }
            }
            Debug.Log(all0.Count + " " + testlist.Count);
            if (all0.Count == testlist.Count)
            {
                Debug.Log(time+"succeed");
                return matrix;
            }
        }
        Debug.Log(time);
        return matrix;
    }

    public static int[,] GetARandomMatrixWithoutRestrict(int row, int col, int rednum, int bluenum, int purplenum, int hardstonenum, int lavastonenum)
    {
        int[,] matrix = new int[row, col];
            matrix = new int[row, col];

            List<(int x, int y)> canuse = new();
            for (int j = 0; j < matrix.Length; j++)
            {
                if (matrix[j / col, j % col] == 0)
                {
                    canuse.Add((j / col, j % col));
                }
            }
            #region 生成矿石
            for (int i = 0; i < hardstonenum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = -1;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < lavastonenum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 4;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < rednum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 1;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < bluenum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 2;
                canuse.RemoveAt(randindex);
            }
            for (int i = 0; i < purplenum; i++)
            {
                int randindex = Random.Range(0, canuse.Count);
                matrix[(int)canuse[randindex].x, (int)canuse[randindex].y] = 3;
                canuse.RemoveAt(randindex);
            }
            #endregion
        return matrix;
    }

    public static string MatrixToString(int[,] matrix)
    {
        string result = "";
        int cellWidth = 4;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            System.Text.StringBuilder line = new();
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                line.Append(matrix[i, j].ToString().PadLeft(cellWidth));
            }
            result += line.ToString() + "\n";
        }
        return result;
    }


    static List<(int,int)> MovePace(int[,] matrix,(int x,int y) startpos)
    {
        if (startpos.x < 0 || startpos.x >= matrix.GetLength(0) || startpos.y < 0 || startpos.y >= matrix.GetLength(1))
        {
            return null;
        }
        int nowx = startpos.x;
        int nowy = startpos.y;
        List<(int x,int y)> savequeue = new()
        {
            (nowx, nowy)
        };

        int lastgetnum = 0;
        int getnum = 1;
        int dif = 1;
        int time = 0;

        while (time<9999)
        {
            time++;
            for(int index=lastgetnum; index < dif; index++)
            {
                nowx = savequeue[index].x;
                nowy = savequeue[index].y;
                for(int t=-1; t <= 1; t++)
                {
                    int j = t;
                    int i=t==0?1:0;
                    if (nowx + i < 0 || nowx + i >= matrix.GetLength(0) || nowy + j < 0 || nowy + j >= matrix.GetLength(1))
                    {
                        continue;
                    }
                    if (matrix[nowx + i, nowy + j] != -1 && !savequeue.Contains((nowx + i, nowy + j)))
                    {
                        savequeue.Add((nowx + i, nowy + j));
                        getnum++;
                    }
                }

                lastgetnum++;
            }

            
            if (lastgetnum == getnum)
            {
                return savequeue;
            }
            else
            {
                dif = getnum;
            }
        }
        Debug.Log("Error!");
        return null;
    }
}
