﻿using System;
using System.Windows.Forms;
using SmartClient.Core.Forms;
using Microsoft.Practices.Unity;
using System.Threading;

namespace SmartClient.Core.AppModel
{
    public partial class App : UnityContainer
    {
        public static App Instance = new App();

        private App()
        {
        }

        public IMainForm MainForm => this.Resolve<IMainForm>();

        partial void Init();

        partial void DeInit();

        partial void Start();

        public void Run(Action onInitilized = null, Action onClosing = null)
        {
            MainForm.Instance.Load += (sender, args) =>
            {
                onInitilized?.Invoke();
                Start();
            };

            MainForm.Instance.FormClosing += (sender, args) =>
              {
                  onClosing?.Invoke();
              };

            Init();
            Application.Run(MainForm.Instance);
        }

        protected override void Dispose(bool disposing)
        {
            DeInit();

            base.Dispose(disposing);
        }
    }
}