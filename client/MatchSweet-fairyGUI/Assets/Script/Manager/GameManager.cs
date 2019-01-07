﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Data;
using FairyGUI;

using System;

namespace Game
{

    public class GameManager : MonoBehaviour
    {

        // 大网格的行列数


        // 游戏时间
        public float gameTime = 60;
        // 填充时间
        public float fillTime;

        // 甜品字典
        public Dictionary<SweetsType, string> sweetPrefabDict;

        private GameSweet[,] _sweets;
        public GameSweet[,] Sweets
        {
            get { return _sweets; }
            set { _sweets = value; }
        }
        private UIStart _StartUI;
        public UIStart StartUI
        {
            get{ return _StartUI;}
            set { _StartUI = value; }
        }
        private UIGame _GameUI;
        public UIGame GameUI
        {
            get { return _GameUI; }
            set { _GameUI = value; }

        }

        private static GameManager instance;
        private GameManager() { }

        public static GameManager Instance
        {
            get
            {
                return instance;
            }

        }
        // Use this for initialization
        void Start()
        {
            instance = this;
            Init();
        }

        public void Init()
        {
            StartUIShow();
            _sweets = new GameSweet[PlayerInfo.xColumn, PlayerInfo.yRow];
        }

        public void StartUIShow()
        {
            _StartUI = new UIStart();
            _StartUI.Show();
        }

        public void GameUIShow()
        {
            _GameUI = new UIGame();
            _GameUI.Show();
        }
        public void StartFill()
        {
            StartCoroutine(AllFill());
        }
        public IEnumerator AllFill()
        {
            bool needRefill = true;
            while (needRefill)
            {
                while (Fill())
                {
                    yield return new WaitForSeconds(fillTime);
                }
            }

            needRefill = true;
        }

        /// <summary>
        /// 分布填充
        /// </summary>
        /// <returns></returns>
        public bool Fill()
        {
            // 判断本次填充是否完成
            bool isFiledNotFinished = false;

            for (int y = PlayerInfo.yRow - 2; y >= 0; y--)
            {
                for (int x = 0; x < PlayerInfo.xColumn; x++)
                {
                    GameSweet sweet = _sweets[x, y];
                    if (sweet.IsMove)
                    {
                        GameSweet sweetBelow = _sweets[x, y + 1];
                        if (sweetBelow.Type == SweetsType.EMPTY)// 垂直填充
                        {
                            sweetBelow.Hide();
                            sweet.Move(x, y + 1, fillTime);
                            _sweets[x, y + 1] = sweet;
                            _sweets[x, y] = PoolsManager.Instance.GetSweetObj();
                            isFiledNotFinished = true;
                        }
                        else //斜向填充
                        {
                            for (int down = -1; down <= 1; down++)
                            {
                                if (down != 0)
                                {
                                    if (down != 0)
                                    {
                                        int downX = x + down;
                                        if (downX >= 0 && downX < PlayerInfo.xColumn)
                                        {
                                            GameSweet downSweet = _sweets[downX, y - 1];
                                            if (downSweet.Type == SweetsType.EMPTY)
                                            {
                                                bool canfill = true;  // 用来判断垂直填充是否满足填充需求

                                                for (int belowY = y; belowY <= PlayerInfo.yRow; belowY++)
                                                {
                                                    GameSweet sweetUnder = _sweets[downX, belowY];

                                                    if (sweetUnder.IsMove)
                                                    {
                                                        break;
                                                    }
                                                    else if (sweet.IsMove == false && sweet.Type != SweetsType.EMPTY)
                                                    {
                                                        canfill = false;
                                                        break;
                                                    }
                                                }

                                                if (!canfill)
                                                {
                                                    sweetBelow.Hide();
                                                    sweet.Move(downX, y + 1, fillTime);
                                                    _sweets[downX, y + 1] = sweet;
                                                    _sweets[downX, y] = PoolsManager.Instance.GetSweetObj();
                                                    isFiledNotFinished = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            //最上排的特殊情况
            for (int x = 0; x < PlayerInfo.xColumn; x++)
            {
                GameSweet sweet = _sweets[x, 0];
                if (sweet.Type == SweetsType.EMPTY)
                {
                    sweet.SetSweetsType(SweetsType.NORMAL);
                    sweet.SetXY(x, -1);
                    sweet.Move(x, 0, fillTime);
                    isFiledNotFinished = true;
                }
            }
            return isFiledNotFinished;
        }
    }

}
