Imports System.Data.OleDb
Public Class AdminMenu
    '' Declaration of Variables
    Public Property AdminName As String
    Dim strStudentName As String = Nothing
    Dim strCancelClicked As String

    '' Declaration of SQL-based Variables:
    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable
    Dim strCommandPending As String = "SELECT StudentName, StudentMatriks, StudentCGPA, StudentEMerit, StudentSalary, StudentGender, Semester FROM Student WHERE CollegeApplication = 'Pending';"

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap

    '' Start of Form
    Private Sub AdminMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Text = $"Welcome, {AdminName}!"

        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"
        viewData()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            strStudentName = DataGridView1.SelectedRows(0).Cells(0).Value.ToString()

        Catch exception As Exception
            MsgBox(exception.Message, 16, "Student Database")
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click '' Approve Button
        If String.IsNullOrEmpty(strStudentName) = False Then
            Dim approveResponse As Integer = MsgBox("Are you Sure you want to Approve " + strStudentName + "'s College Application?", 4 + 32, "Notice")
            If approveResponse = vbYes Then
                Try
                    conn.Open()
                    command = conn.CreateCommand()
                    command.CommandType = CommandType.Text
                    command.CommandText = "UPDATE Student SET CollegeApplication = 'Approved' WHERE StudentName = '" + strStudentName + "'"
                    command.ExecuteNonQuery()

                    datValueStudent = New DataTable()
                    dataAdpt = New OleDbDataAdapter(strCommandPending, conn)
                    dataAdpt.Fill(datValueStudent)
                    DataGridView1.DataSource = datValueStudent

                    conn.Close()

                    MsgBox("Approved " + strStudentName + "'s Successfully!", 64, "Success")
                    Dim strCollege As String = InputBox("Where Would You Like to Assign " & strStudentName & "'s Dorm at?", "College Provision", "")
                    If strCollege <> strCancelClicked Then
                        conn.Open()
                        command = conn.CreateCommand()
                        command.CommandType = CommandType.Text
                        command.CommandText = "UPDATE Student SET CollegeRecieved = '" + strCollege + "' WHERE StudentName = '" + strStudentName + "'"
                        command.ExecuteNonQuery()
                        conn.Close()
                        MsgBox("Success!", 64, "Success")
                        strStudentName = Nothing
                    End If

                Catch exception As Exception
                    MsgBox(exception.Message, 16, "Student Database")
                End Try
            End If
        Else
            MsgBox("You haven't selected a Student to Appeal or Deny their Application", 64, "Notice")
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click '' Decline Button
        If String.IsNullOrEmpty(strStudentName) = False Then
            Dim declineResponse As Integer = MsgBox("Are you Sure you want to Decline " + strStudentName + "'s College Application?", 4 + 32, "Notice")
            If declineResponse = vbYes Then
                Try
                    conn.Open()
                    command = conn.CreateCommand()
                    command.CommandType = CommandType.Text
                    command.CommandText = "UPDATE Student SET CollegeApplication = 'Declined' WHERE StudentName = '" + strStudentName + "'"
                    command.ExecuteNonQuery()

                    datValueStudent = New DataTable()
                    dataAdpt = New OleDbDataAdapter(strCommandPending, conn)
                    dataAdpt.Fill(datValueStudent)
                    DataGridView1.DataSource = datValueStudent

                    conn.Close()

                    MsgBox("Declined " + strStudentName + "'s Successfully!", 64, "Success")


                Catch exception As Exception
                    MsgBox(exception.Message, 16, "Student Database")
                End Try
            End If
        Else
            MsgBox("You haven't selected a Student to Appeal or Deny their Application", 64, "Notice")
        End If
    End Sub

    '' Sub Procedure [Does not Return Value]
    Private Sub viewData()
        conn.Open()
        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        dataAdpt = New OleDbDataAdapter(strCommandPending, conn)
        dataAdpt.Fill(datValueStudent)
        DataGridView1.DataSource = datValueStudent
        conn.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click '' Logout Button
        Dim logoutResponse As Integer = MsgBox("Are You Sure you want to Logout?", 4 + 32, "Logout")
        If logoutResponse = vbYes Then
            MsgBox("Logging Out...", 64, "Notice")
            Login.Show()
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
        MsgBox("Welcome Admin! This is Admin Menu Where You are Tasked to Approve or Deny Student's College Applications Based on their CGPA, EMerit, Salary and Semesters.", 64, "Welcome")
    End Sub
End Class