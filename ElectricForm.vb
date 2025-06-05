Imports System.Data.OleDb
Public Class ElectricForm
    Public Property StudentMatriks As String
    Public Property StudentName As String

    Public Property electricalType As String
    Public Property electricalBrand As String
    Public Property electricalNum As Integer

    Dim strCancelClicked As String
    Dim intTotalQuantity As Integer

    '' Declaration of SQL based Variables
    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim strNumElectrical As String = InputBox("How Many Electrical Appliance Do You Want to Register?", "Electrical Appliance Input Menu", "")
        If strNumElectrical <> strCancelClicked Then
            If IsNumeric(strNumElectrical) Then
                Dim intNumElectrical = Convert.ToInt32(strNumElectrical)
                If intNumElectrical > 0 Then
                    AddElectricalAppliance(intNumElectrical)
                Else
                    MsgBox("Please Enter a Non-Negative Value", 48, "Error")
                End If
            Else
                    MsgBox("Please Enter a Numeric Value", 48, "Error")
            End If
        End If
    End Sub

    '' Sub Procedure
    Private Sub AddElectricalAppliance(intNumElectrical As Integer)

        Dim intStartValue As Integer = 0

        '' Do Until Loop
        Do Until intStartValue >= intNumElectrical
            Dim electricinput As New ElectricInput With {
                .intTextValue = intStartValue
            }

            Dim result As DialogResult = electricinput.ShowDialog() '' Pop-up Form to Insert Electrical Appliances

            If result = DialogResult.OK Then
                electricalType = electricinput.strProduct
                electricalBrand = electricinput.strBrand
                electricalNum = electricinput.intNum

                ListBox1.Items.Add(electricalType)
                ListBox2.Items.Add(electricalBrand)
                ListBox3.Items.Add(electricalNum)

                intStartValue += 1
            Else
                MsgBox("You Have Pressed the X Button. Process Has Been Reseted.", 64, "Process Reset")
                Exit Do
            End If

        Loop
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click '' Clear Box
        Dim clearButton As Integer = MsgBox("Are You Sure you want to Clear all the Items in the List?", 4 + 32, "Clear")
        If clearButton = vbYes Then
            ListBox1.Items.Clear()
            ListBox2.Items.Clear()
            ListBox3.Items.Clear()
            MsgBox("Items Cleared Successfully!", 64, "Success")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click '' Submit Button

        If ListBox1.Items.Count = 0 Then
            Dim proceedSubmit As Integer = MsgBox("You Didn't Register an Electrical Appliance, Are You Sure You Would Like to Proceed? (You Wouldn't be Charged with Electrical Fees)", 4 + 32, "Confirm")
            If proceedSubmit = vbYes Then
                Dim payment As New Payment With {
                    .PaymentStudentMatriks = Me.StudentMatriks,
                    .intQuantity = Me.intTotalQuantity,
                    .RetrieveName = Me.StudentName
                }
                payment.Show()
                Me.Hide()
            End If
        Else '' If Student Went Through with Adding Electrical Appliances

            '' For Loop To Calculate Total Quantity
            Dim intCalcQuantity As Integer
            For intCalcQuantity = 0 To ListBox3.Items.Count - 1
                intTotalQuantity = intTotalQuantity + Convert.ToInt32(ListBox3.Items(intCalcQuantity))
            Next

            Dim payment As New Payment With {
                .PaymentStudentMatriks = Me.StudentMatriks,
                .intQuantity = Me.intTotalQuantity,
                .RetrieveName = Me.StudentName
            }
            payment.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim exitResponse As Integer = MsgBox("Are you Sure You Want to Exit?", 4 + 32, "Exit")
        If exitResponse = vbYes Then
            MsgBox("System Shutting Down...", 64, "Notice")
            Application.Exit()
        End If
    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.Click
        MsgBox("If You Have Electrical Appliances You Are Going to Bring for this Semester, Please Register Them Here! If You Don't Have Any, You Can Directly Click the Submit Button.", 64, "About")
    End Sub
End Class