Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes

Namespace ObtainHandlesOfAllRows
	Partial Public Class MainPage
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
			grid.DataSource = New ProductList()
			grid.GroupBy("Quantity")
			UpdateListBoxes()
		End Sub

		Private Sub TableView_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.Xpf.Grid.FocusedRowChangedEventArgs)

			UpdateListBoxes()
		End Sub

		Private Sub UpdateListBoxes()
			If listInGroupRowHandles Is Nothing OrElse listRowHandles Is Nothing Then
				Return
			End If
			listInGroupRowHandles.ItemsSource = GetDataRowHandlesInGroup(grid.View.FocusedRowHandle)
			listRowHandles.ItemsSource = GetDataRowHandles()
		End Sub

		Private Function GetDataRowHandles() As List(Of Integer)
			Dim rowHandles As New List(Of Integer)()
			For i As Integer = 0 To grid.VisibleRowCount - 1
				Dim rowHandle As Integer = grid.GetRowHandleByVisibleIndex(i)
				If grid.IsGroupRowHandle(rowHandle) Then
					If (Not grid.IsGroupRowExpanded(rowHandle)) Then
						rowHandles.AddRange(GetDataRowHandlesInGroup(rowHandle))
					End If
				Else
					rowHandles.Add(rowHandle)
				End If
			Next i
			Return rowHandles
		End Function
		Private Function GetDataRowHandlesInGroup(ByVal handle As Integer) As List(Of Integer)
			Dim rowHandles As New List(Of Integer)()
			If grid.GroupCount = 0 Then
				Return rowHandles
			End If
			Dim groupRowHandle As Integer = -1
			If grid.IsGroupRowHandle(handle) Then
				groupRowHandle = handle
			Else
				groupRowHandle = grid.GetParentRowHandle(handle)
			End If
			For i As Integer = 0 To grid.GetChildRowCount(groupRowHandle) - 1
				Dim rowHandle As Integer = grid.GetChildRowHandle(groupRowHandle, i)
				If grid.IsGroupRowHandle(rowHandle) Then
					rowHandles.AddRange(GetDataRowHandlesInGroup(rowHandle))
				Else
					rowHandles.Add(rowHandle)
				End If
			Next i
			Return rowHandles
		End Function
	End Class
End Namespace
