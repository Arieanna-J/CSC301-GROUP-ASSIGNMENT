Imports System.Data.OleDb

Public Class StudentMenu

    Public Property StudentMatriks As String

    Dim paymentStatus As String = Nothing
    Dim paymentID As String = Nothing
    Dim paymentAmount As String = Nothing

    '' Declaration of SQL-based Variables:

    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        conn.Open()
        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT CollegeApplication FROM Student WHERE StudentMatriks = '" + StudentMatriks + "';"
        command.ExecuteNonQuery()

        datValueStudent = New DataTable()
        dataAdpt = New OleDbDataAdapter(command)
        dataAdpt.Fill(datValueStudent)

        Dim applicationStatus As String = datValueStudent.Rows(0)("CollegeApplication").ToString() ' Get the StudentName value
        conn.Close()

        Dim Apply As New Apply(StudentMatriks)


        If applicationStatus <> "None" Then
            MsgBox("You Already Applied for a College. You can only Apply it Again Next Semester.", 64, "Notice")
        Else
            Apply.Show()
            Me.Hide()
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click '' Logout Button
        Dim logoutResponse As Integer = MsgBox("Are You Sure you want to Logout?", 4 + 32, "Logout")
        If logoutResponse = vbYes Then
            MsgBox("Logging Out...", 64, "Notice")
            Login.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub StudentMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"

        conn.Open()
        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT StudentName FROM Student WHERE StudentMatriks = '" + StudentMatriks + "';"
        command.ExecuteNonQuery()

        datValueStudent = New DataTable()
        dataAdpt = New OleDbDataAdapter(command)
        dataAdpt.Fill(datValueStudent)

        Dim StudentName As String = datValueStudent.Rows(0)("StudentName").ToString() ' Get the StudentName value
        conn.Close()
        StudentName = StudentName.Split(" "c)(0)
        Label3.Text = $"Welcome, {StudentName}!"
        Label1.Text = "Matriks Number: " & StudentMatriks


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click '' Check for Status
        conn.Open()
        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT CollegeApplication, StudentName, PaymentID FROM Student WHERE StudentMatriks = '" + StudentMatriks + "';"
        command.ExecuteNonQuery()

        datValueStudent = New DataTable()
        dataAdpt = New OleDbDataAdapter(command)
        dataAdpt.Fill(datValueStudent)

        Dim retrieveName As String = datValueStudent.Rows(0)("StudentName").ToString()
        Dim applicationStatus As String = datValueStudent.Rows(0)("CollegeApplication").ToString()
        Dim PaymentID As String = datValueStudent.Rows(0)("PaymentID").ToString()

        If applicationStatus = "Approved" And String.IsNullOrEmpty(PaymentID) = False Then
            command = conn.CreateCommand()
            command.CommandType = CommandType.Text
            command.CommandText = "SELECT PaymentStatus, PaymentID, PaymentAmount FROM Payment WHERE StudentMatriks = '" + StudentMatriks + "';"
            command.ExecuteNonQuery()

            datValueStudent = New DataTable()
            dataAdpt = New OleDbDataAdapter(command)
            dataAdpt.Fill(datValueStudent)

            paymentStatus = datValueStudent.Rows(0)("PaymentStatus").ToString()
            PaymentID = datValueStudent.Rows(0)("PaymentID").ToString()
            paymentAmount = datValueStudent.Rows(0)("PaymentAmount").ToString()
            conn.Close()
        Else
            conn.Close()
        End If

        If applicationStatus = "None" Then
            MsgBox("You Haven't Applied a College for This Semester!", 64, "Notice")
        ElseIf applicationStatus = "Pending" Or applicationStatus = "Appeal" Then
            MsgBox("Your Application Request is Still Pending. Please Come Back in 2-3 Business Days.", 64, "Pending")
        ElseIf applicationStatus = "Declined" Then
            MsgBox("Your Application got Rejected! You Are Not Eligible for a Dorm This Semester.", 48, "Application Rejected")
        ElseIf applicationStatus = "Approved" Then
            If paymentStatus <> "Paid" Then
                If String.IsNullOrEmpty(paymentID) Then
                    MsgBox("Your Application is Approved!", 64, "Application Accepted")
                    Dim electricform As New ElectricForm
                    electricform.StudentMatriks = Me.StudentMatriks
                    electricform.StudentName = retrieveName
                    electricform.Show()
                    Me.Hide()
                Else '' Resume Where you Last Went Off.
                    Dim checkout As New Checkout
                    checkout.finalbill = paymentAmount
                    checkout.studentMatriks = Me.StudentMatriks
                    checkout.Show()
                    Me.Hide()

                End If
            Else
                    Dim recieptmenu As New RecieptMenu
                recieptmenu.StudentMatriks = Me.StudentMatriks
                recieptmenu.Show()
                Me.Hide()
            End If
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
        MsgBox("Welcome Student! This is Your Student Menu Where You Can Apply for a College This Semester (If you haven't Already) and Wait For Results to Show. Hopefully You're Qualified This Semester!", 64, "About")
    End Sub
End Class