Imports System.Data.OleDb

Public Class Login
    '' Declaration of Variables
    Dim strMatriks As String
    Dim strAdminID As String
    Dim strPassword As String

    '' Declaration of SQL-based Variables:

    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.PasswordChar = "*"

        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click '' Login
        If RadioButton1.Checked Then
            strAdminID = TextBox1.Text
        ElseIf RadioButton2.Checked Then
            strMatriks = TextBox1.Text
        End If
        strPassword = TextBox2.Text


        If RadioButton1.Checked Then
            If String.IsNullOrEmpty(TextBox1.Text) = False And String.IsNullOrEmpty(TextBox2.Text) = False Then
                If IsNumeric(TextBox1.Text) = True Then
                    '' Checking Username and Password via SQL
                    conn.Open()
                    command = conn.CreateCommand()
                    command.CommandType = CommandType.Text
                    command.CommandText = "SELECT * FROM Admin WHERE ICNumber = '" + strAdminID + "' AND StrComp(AdminPassword, '" + strPassword + "', 0) = 0;"
                    command.ExecuteNonQuery()
                    datValueAdmin = New DataTable()
                    dataAdpt = New OleDbDataAdapter(command)
                    dataAdpt.Fill(datValueAdmin)
                    Dim checker As Integer = Convert.ToInt32(datValueAdmin.Rows.Count.ToString())
                    conn.Close()

                    If (checker <> 0) Then '' If Successful, checker shouldn't be 0
                        MsgBox("Login Successful!", 64, "Success")

                        Dim AdminMenu As New AdminMenu()
                        conn.Open()

                        command = conn.CreateCommand()
                        command.CommandType = CommandType.Text
                        command.CommandText = "SELECT AdminName FROM Admin WHERE ICNumber = '" + strAdminID + "';"
                        command.ExecuteNonQuery()

                        datValueAdmin = New DataTable()
                        dataAdpt = New OleDbDataAdapter(command)
                        dataAdpt.Fill(datValueAdmin)

                        Dim fullName As String = datValueAdmin.Rows(0)("AdminName").ToString() ' Get the AdminName value
                        AdminMenu.AdminName = fullName.Split(" "c)(0)
                        conn.Close()

                        AdminMenu.Show()
                        Me.Hide()
                    Else '' Login Failed
                        MsgBox("Login Failed! Account ID and Password Do Not Match. Please Try Again", 64, "Login Failed")
                    End If

                Else
                        MsgBox("Alert: Account ID Number is not in a Numeric Value. Please Try Again.", 64, "Alert")
                End If
            Else
                MsgBox("Please Enter Your Account ID and Password Respectively!", 64, "Alert")
            End If
        ElseIf RadioButton2.Checked Then
            If String.IsNullOrEmpty(TextBox1.Text) = False And String.IsNullOrEmpty(TextBox2.Text) = False Then
                If IsNumeric(TextBox1.Text) = True Then
                    '' Checking Username and Password via SQL
                    conn.Open()
                    command = conn.CreateCommand()
                    command.CommandType = CommandType.Text
                    command.CommandText = "SELECT * FROM Student WHERE StudentMatriks = '" + strMatriks + "' AND StrComp(StudentPassword, '" + strPassword + "', 0) = 0;"
                    command.ExecuteNonQuery()
                    datValueStudent = New DataTable()
                    dataAdpt = New OleDbDataAdapter(command)
                    dataAdpt.Fill(datValueStudent)
                    Dim checker As Integer = Convert.ToInt32(datValueStudent.Rows.Count.ToString())
                    conn.Close()

                    If (checker <> 0) Then '' If Successful, checker shouldn't be 0
                        MsgBox("Login Successful!", 64, "Success")

                        Dim StudentMenu As New StudentMenu()
                        conn.Open()

                        command = conn.CreateCommand()
                        command.CommandType = CommandType.Text
                        command.CommandText = "SELECT StudentName, StudentMatriks FROM Student WHERE StudentMatriks = '" + strMatriks + "';"
                        command.ExecuteNonQuery()

                        datValueStudent = New DataTable()
                        dataAdpt = New OleDbDataAdapter(command)
                        dataAdpt.Fill(datValueStudent)

                        Dim StudMatriks As String = datValueStudent.Rows(0)("StudentMatriks").ToString()
                        
                        StudentMenu.StudentMatriks = StudMatriks

                        conn.Close()

                        StudentMenu.Show()
                        Me.Hide()
                    Else '' Login Failed
                        MsgBox("Login Failed! Account ID and Password Do Not Match. Please Try Again", 64, "Login Failed")
                    End If
                Else
                    MsgBox("Alert: Matrics Number is not in a Numeric Value. Please Try Again.", 64, "Alert")
                End If

            Else
                MsgBox("Please Enter Your Matrics Number and Password Respectively!", 64, "Alert")
            End If
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        SignUp.Show()
        Me.Hide()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label1.Text = "Account ID"
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            Label1.Text = "Matrics Number"
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click '' Clear Button
        TextBox1.Clear()
        TextBox1.Focus()
        TextBox2.Clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox2.PasswordChar = ""
        Else
            TextBox2.PasswordChar = "*"
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
        MsgBox("This is a Login Menu Page for the College Application System. If you don't have an Account, please Sign Up with the link given below the page.", 64, "Help")
    End Sub

End Class
