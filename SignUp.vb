Imports System.Data.OleDb
Public Class SignUp
    '' Declaration of Variables
    Dim strName As String
    Dim strMatriks As String
    Dim strAdminID As String
    Dim strPassword As String
    Dim strPhoneNumber As String
    Dim strGender As String

    '' Declaration of SQL based Variables
    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap

    Private Sub SignUp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("Male")
        ComboBox1.Items.Add("Female")
        TextBox4.PasswordChar = "*"

        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"
    End Sub
    Private Sub Label3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label2.Text = "Account ID"
            TextBox2.MaxLength = 12
        ElseIf RadioButton2.Checked Then
            Label2.Text = "Matrics Number"
            TextBox2.MaxLength = 10
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Login.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click '' Submit
        Try
            If String.IsNullOrEmpty(TextBox1.Text) = False And String.IsNullOrEmpty(TextBox2.Text) = False And String.IsNullOrEmpty(TextBox3.Text) = False And String.IsNullOrEmpty(TextBox4.Text) = False Then
                strPassword = TextBox4.Text()
                strPhoneNumber = TextBox3.Text()
                If ComboBox1.SelectedItem.ToString() = "Female" Then
                    strGender = "Female"
                ElseIf ComboBox1.SelectedItem.ToString() = "Male" Then
                    strGender = "Male"
                End If
                strName = TextBox1.Text()
                strName = strName.ToUpper()

                If RadioButton1.Checked Then '' Signing Up as Admin
                    strAdminID = TextBox2.Text()
                    conn.Open()
                    command = conn.CreateCommand()
                    command.CommandType = CommandType.Text
                    command.CommandText = "INSERT INTO Admin(ICNumber,AdminName,AdminGender,AdminPhoneNum,AdminPassword) 
                                       VALUES ('" + strAdminID + "', '" + strName + "', '" + strGender + "', '" + strPhoneNumber + "', '" + strPassword + "')"
                    command.ExecuteNonQuery()
                    conn.Close()
                    MsgBox("Account Created Successfully!", 64, "Notice")
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

                ElseIf RadioButton2.Checked Then '' Signing Up as Student
                    strMatriks = TextBox2.Text()

                    conn.Open()
                    command = conn.CreateCommand()
                    command.CommandType = CommandType.Text
                    command.CommandText = "INSERT INTO Student(StudentMatriks,StudentName,StudentGender,StudentPhoneNum,StudentPassword,CollegeApplication) 
                                       VALUES ('" + strMatriks + "', '" + strName + "', '" + strGender + "', '" + strPhoneNumber + "', '" + strPassword + "', 'None')"
                    command.ExecuteNonQuery()
                    conn.Close()
                    MsgBox("Account Created Successfully!", 64, "Notice")

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

                End If
            Else
                MsgBox("Please Enter Your Credentials Respectively!", 64, "Alert")
            End If
        Catch Exception As Exception
            MsgBox(Exception.Message, 16, "Warning")
            conn.Close()
        End Try


    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox4.PasswordChar = ""
        Else
            TextBox4.PasswordChar = "*"
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Clear()
        TextBox1.Focus()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim exitResponse As Integer = MsgBox("Are you Sure You Want to Exit?", 4 + 32, "Exit")
        If exitResponse = vbYes Then
            MsgBox("System Shutting Down...", 64, "Notice")
            Application.Exit()
        End If
    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.Click
        MsgBox("If You Don't Have an Account within this College Registration System, You're Always Free to Sign Up. If You Already Have an Account Though, Proceed with Login via the URL Given", 64, "About")
    End Sub

End Class

