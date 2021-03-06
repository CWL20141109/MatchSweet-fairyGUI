﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Game.UI
{
    public class UIStart : UIBase
    {
        private GButton start_btn;
        private GButton quit_btn;

        protected override void OnShown()
        {
           
            RGResource.LoadUIAsync("UI/main/main", LoadStartPackageComplete);

        }

        public void LoadStartPackageComplete(AssetBundle ab, LoadEventData evData)
        {
            UIPackage.AddPackage(ab);
            contentPane = UIPackage.CreateObject("main", "start").asCom;

            InitCom();
            InitUI();
        }

        protected override void OnHide()
        {

        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitCom()
        {
            start_btn = contentPane.GetChild("start_btn").asButton;
            quit_btn = contentPane.GetChild("quit_btn").asButton;
        }

        /// <summary>
        /// 初始UI
        /// </summary>
        private void InitUI()
        {
            start_btn.onClick.Set(OnStartClick);
            quit_btn.onClick.Set(OnQuitClick);
        }

        private void OnStartClick()
        {
            this.Dispose();
            GameManager.Instance.GameUIShow();
        }

        private void OnQuitClick()
        {
            
        }
    }
}

