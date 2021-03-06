﻿using System.ComponentModel;
using System.Data;
using System.Linq;
using SmartClient.Core.Container;
using SmartClient.Core.Views.Custom.Model;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;

namespace SmartClient.Core.Views.Custom
{
    public partial class UserSettingsView : RibbonView
    {
        private readonly BindingList<UserSettings> _dataSource = new BindingList<UserSettings>();

        public UserSettingsView()
        {
            InitializeComponent();
            userSettingsBindingSource.DataSource = _dataSource;
        }

        public override void OnNavigatedTo(INavigationArgs args)
        {
            base.OnNavigatedTo(args);

            RefreshData();
        }

        private void _delBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var rowId = gridView1.FocusedRowHandle;

            if (gridView1.IsDataRow(rowId) &&
                ServiceContainer.Default.DialogService.Ask("Удалить?") == true)
            {
                var us = (UserSettings)gridView1.GetRow(rowId);
                ServiceContainer.Default
                .UserSettingsService.Set(us.Key, null);

                RefreshData();
            }
        }

        private void _btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            _dataSource.Clear();

            var source = ServiceContainer.Default
                .UserSettingsService.GetKeys()
                .Select(x => new UserSettings()
                {
                    Key = x,
                    Value = ServiceContainer.Default.UserSettingsService.Get<string>(x),
                })
                .ToArray();

            foreach (var setting in source)
                _dataSource.Add(setting);
        }
    }
}
