Imports System.Data.OleDb
Public Class RecieptMenu
    '' Declaration of SQL based Variables
    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap

    ''
    Public Property StudentMatriks As String

    Private Sub RecieptMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"


        conn.Open()
        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT StudentMatriks, StudentName, CollegeRecieved FROM Student WHERE StudentMatriks = '" + Me.StudentMatriks + "';"
        command.ExecuteNonQuery()

        datValueStudent = New DataTable()
        dataAdpt = New OleDbDataAdapter(command)
        dataAdpt.Fill(datValueStudent)

        '' Declare Variables From SQL Query
        Dim newMatriks As String = datValueStudent.Rows(0)("StudentMatriks").ToString()
        Dim newName As String = datValueStudent.Rows(0)("StudentName").ToString()
        Dim newCollege As String = datValueStudent.Rows(0)("CollegeRecieved").ToString()

        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT PaymentAmount, electricAmount FROM Payment WHERE StudentMatriks = '" + newMatriks + "';"
        command.ExecuteNonQuery()

        datValueStudent = New DataTable()
        dataAdpt = New OleDbDataAdapter(command)
        dataAdpt.Fill(datValueStudent)

        Dim newAmount As String = datValueStudent.Rows(0)("PaymentAmount").ToString()
        Dim newElectric As String = datValueStudent.Rows(0)("electricAmount").ToString()
        conn.Close()

        Dim dblNewAmount As Double = CDbl(newAmount)
        Dim dblNewElectric As Double = CDbl(newElectric)

        Label2.Text = newName
        Label3.Text = newMatriks
        Label8.Text = newCollege
        Label4.Text = "RM " & ((dblNewAmount / 1.001) - dblNewElectric).ToString("N")
        Label7.Text = "RM " & dblNewAmount.ToString("N")
        Label5.Text = "RM " & dblNewElectric.ToString("N")
        Label6.Text = "RM " & (dblNewAmount / 1.001).ToString("N")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click  '' Save Button
        With SaveFileDialog1
            .Filter = "Text files (*.docx)|*.docx|All files(*.*)|*.*"
            .InitialDirectory = "C:\Users\Arieanna\source\repos\CSC301_Assignment\"
            .Title = "Save File As"

            If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                ' Message box showing the file name
                MsgBox("File will be saved as: " & SaveFileDialog1.FileName)

                ' Actual file saving logic (e.g., saving some content)
                Try
                    System.IO.File.WriteAllText(SaveFileDialog1.FileName,
                    "Reciept" & Environment.NewLine &
                    "--------------------------------------------------------------------------------------------------------------------" & Environment.NewLine &
                    "Recipient Name: " & Label2.Text & Environment.NewLine &
                    "Student Matriks: " & Label3.Text & Environment.NewLine &
                    "--------------------------------------------------------------------------------------------------------------------" & Environment.NewLine &
                    "College Fee: " & Label4.Text & Environment.NewLine &
                    "Electrical Appliance Fee: " & Label5.Text & Environment.NewLine &
                    "After Tax: " & Label6.Text & Environment.NewLine &
                    "Total Fee: " & Label7.Text & Environment.NewLine &
                    "--------------------------------------------------------------------------------------------------------------------" & Environment.NewLine &
                    "College Recieved: " & Label8.Text & Environment.NewLine &
                    "--------------------------------------------------------------------------------------------------------------------" & Environment.NewLine)

                    MsgBox("File saved successfully.")
                Catch ex As Exception
                    MsgBox("Error saving the file: " & ex.Message)
                End Try

            Else
                MsgBox("File was not saved.")
            End If
        End With
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim studentmenu As New StudentMenu
        studentmenu.StudentMatriks = StudentMatriks
        studentmenu.Show()
        Me.Hide()
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem1.Click
        Dim exitResponse As Integer = MsgBox("Are you Sure You Want to Exit?", 4 + 32, "Exit")
        If exitResponse = vbYes Then
            MsgBox("System Shutting Down...", 64, "Notice")
            Application.Exit()
        End If
    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.Click
        MsgBox("Here Is Your Reciept. Kindly Click Save to Save Your Receipt so that You Can Print It Afterwards and Bring It On The Day of Registration.", 64, "About")
    End Sub
End Class