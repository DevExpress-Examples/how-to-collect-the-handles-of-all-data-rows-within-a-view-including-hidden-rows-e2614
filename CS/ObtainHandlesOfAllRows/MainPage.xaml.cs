using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ObtainHandlesOfAllRows {
    public partial class MainPage : UserControl {
        public MainPage() {
            InitializeComponent();
            grid.ItemsSource = new ProductList();
            grid.GroupBy("Quantity");
            UpdateListBoxes();
        }
        
        private void TableView_FocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e) {
            
            UpdateListBoxes();
        }

        private void UpdateListBoxes() {
            if (listInGroupRowHandles == null || listRowHandles == null) return;
            listInGroupRowHandles.ItemsSource = GetDataRowHandlesInGroup(grid.View.FocusedRowHandle);
            listRowHandles.ItemsSource = GetDataRowHandles();
        }

        private List<int> GetDataRowHandles() {
            List<int> rowHandles = new List<int>();
            for (int i = 0; i < grid.VisibleRowCount; i++) {
                int rowHandle = grid.GetRowHandleByVisibleIndex(i);
                if (grid.IsGroupRowHandle(rowHandle)) {
                    if (!grid.IsGroupRowExpanded(rowHandle)) {
                        rowHandles.AddRange(GetDataRowHandlesInGroup(rowHandle));
                    }
                }
                else
                    rowHandles.Add(rowHandle);
            }
            return rowHandles;
        }
        private List<int> GetDataRowHandlesInGroup(int handle) {
            List<int> rowHandles = new List<int>();
            if (grid.GroupCount == 0) return rowHandles;
            int groupRowHandle = -1;
            if (grid.IsGroupRowHandle(handle))
                groupRowHandle = handle;
            else
                groupRowHandle = grid.GetParentRowHandle(handle);
            for (int i = 0; i < grid.GetChildRowCount(groupRowHandle); i++) {
                int rowHandle = grid.GetChildRowHandle(groupRowHandle, i);
                if (grid.IsGroupRowHandle(rowHandle)) {
                    rowHandles.AddRange(GetDataRowHandlesInGroup(rowHandle));
                }
                else
                    rowHandles.Add(rowHandle);
            }
            return rowHandles;
        }
    }
}
