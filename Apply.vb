Imports System.Data.OleDb
Public Class Apply
    '' Declaration of Variables
    Dim dblEMerit As Double
    Dim dblCGPA As Double
    Dim dblSalary As Double
    Dim intSemester As Integer

    Dim strMatriks As String
    Private studentMatriks As String

    '' Declaration of SQL based Variables
    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap

    Public Sub New(studentMatriks As String)
        InitializeComponent()
        Me.studentMatriks = studentMatriks
    End Sub

    Private Sub Apply_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label9.Text = studentMatriks
        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"
    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.Click
        MsgBox("Please Refer to Student Portal if you are unsure of your CGPA and E-Merit.", 64, "Help")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim exitResponse As Integer = MsgBox("Are you Sure You Want to Exit?", 4 + 32, "Exit")
        If exitResponse = vbYes Then
            MsgBox("System Shutting Down...", 64, "Notice")
            Application.Exit()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click '' Go Back Button
        Dim StudentMenu As New StudentMenu()
        StudentMenu.StudentMatriks = Me.studentMatriks
        StudentMenu.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox2.Focus()
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click '' Submit Button
        Try
            dblEMerit = CDbl(TextBox2.Text())
            dblCGPA = CDbl(TextBox3.Text())
            dblSalary = CDbl(TextBox4.Text())
            intSemester = CInt(TextBox5.Text())

            conn.Open()
            command = conn.CreateCommand()
            command.CommandType = CommandType.Text
            command.CommandText = "UPDATE Student SET StudentEMerit = Merit, StudentCGPA = CGPA, StudentSalary = Salary, Semester = newSemester, CollegeApplication = 'Pending' WHERE StudentMatriks = '" + studentMatriks + "'"
            command.Parameters.AddWithValue("Merit", dblEMerit)
            command.Parameters.AddWithValue("CGPA", dblCGPA)
            command.Parameters.AddWithValue("Salary", dblSalary)
            command.Parameters.AddWithValue("newSemester", intSemester)
            command.ExecuteNonQuery()
            conn.Close()
            MsgBox("Application Sent In Successfully! Please Come Back in 2-3 Business Days.", 0, "Application Sent")

            Dim StudentMenu As New StudentMenu()
            StudentMenu.StudentMatriks = Me.studentMatriks
            StudentMenu.Show()
            Me.Hide()
        Catch exception As Exception
            MsgBox(exception.Message, 16, "Alert")
        End Try



    End Sub
End Class