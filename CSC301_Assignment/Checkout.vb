Imports System.Data.OleDb
Public Class Checkout
    Public Property finalbill As Double
    Public Property studentMatriks As String

    '' Declaration of Variables
    Dim strCreditNum As String
    Dim strCreditBank As String
    Dim strCreditExpiry As String
    Dim strCreditType As String
    Dim strCreditOrDebit As String
    Dim strCreditSecurity As String

    '' Declaration of SQL based Variables
    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap


    Private Sub Checkout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"

        Label10.Text = finalbill.ToString("N")
        Label8.Text = "Student Matriks: " & studentMatriks
        TextBox2.MaxLength = 5
        TextBox3.MaxLength = 3
        TextBox1.MaxLength = 16
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label1.Text = "Credit Number"
        ElseIf RadioButton2.Checked Then
            Label1.Text = "Debit Number"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click '' Clear Button
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox1.Focus()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click '' Save Button
        Dim invalid As Boolean = False
        Dim notMonth As Boolean = False '' Verification for Expiry Date [Month Only]
        Dim incomplete As Boolean = False '' When ComboBox is Empty
        Try
            If RadioButton1.Checked Then
                strCreditOrDebit = "Credit"
            ElseIf RadioButton2.Checked Then
                strCreditOrDebit = "Debit"
            End If

            strCreditBank = ComboBox1.SelectedItem.ToString()
            strCreditType = ComboBox2.SelectedItem.ToString()

            If IsNumeric(TextBox1.Text) Then '' Is Numeric !
                strCreditNum = TextBox1.Text()

                '' Process to Make Sure the Format is in MM/YY
                If TextBox2.Text.IndexOf("/") = 2 Then
                    Dim ExpiryIndex As Integer() = {0, 1, 3, 4} '' Avoiding Index 2 because it's supposed to be '/'
                    Dim monthIndex As Integer() = {0, 1}
                    For Each intCount As Integer In ExpiryIndex
                        If Not Char.IsDigit(TextBox2.Text(intCount)) Then '' Is NOT Numeric !
                            invalid = True
                            notMonth = True
                        Else
                            If Integer.Parse(TextBox2.Text(0).ToString()) > 9 Or Integer.Parse(TextBox2.Text(0).ToString()) < -1 Then
                                invalid = True
                                notMonth = True
                            ElseIf Integer.Parse(TextBox2.Text(1).ToString()) > 2 Or Integer.Parse(TextBox2.Text(0).ToString()) < -1 Then
                                invalid = True
                                notMonth = True
                            End If
                        End If
                    Next

                    If invalid = False Then
                        strCreditExpiry = TextBox2.Text '' Is Numeric !
                        If IsNumeric(TextBox3.Text) Then
                            strCreditSecurity = TextBox3.Text '' Is Numeric !

                        Else
                            invalid = True
                        End If
                    Else
                        invalid = True
                    End If
                Else
                    invalid = True
                    notMonth = True
                End If
            Else
                invalid = True
            End If

        Catch ex As Exception
            invalid = True
            incomplete = True
        End Try


        If invalid = True Then '' Error Message Statements
            If notMonth = True Then
                MsgBox("Invalid Expiry Date Detected. Please Check Back on Your Expiry Date and Make Sure it's Valid!", 64, "Notice")
            ElseIf incomplete = True Then
                MsgBox("No Items Has Been Selected. Please Add a Cardholder and Type of Card Respectively.", 64, "Alert")
            Else
                MsgBox("Invalid Input Detected. Please Make Sure your Input are all in Numeric Value!", 64, "Notice")
            End If

        Else '' if All input are Numeric and successful!
            conn.Open()
            command = conn.CreateCommand()
            command.CommandType = CommandType.Text
            command.CommandText = "INSERT INTO Credentials(CreditInfo, CreditBank, CreditType, CreditExpiry, CreditSecurity, CreditOrDebit) 
                                   VALUES ('" + strCreditNum + "', '" + strCreditBank + "', '" + strCreditType + "', '" + strCreditExpiry + "', 
                                   '" + strCreditSecurity + "', '" + strCreditOrDebit + "');"
            command.ExecuteNonQuery()
            command = conn.CreateCommand()
            command.CommandType = CommandType.Text
            command.CommandText = "UPDATE Payment SET CreditInfo = '" + strCreditNum + "', PaymentStatus = 'Paid' WHERE StudentMatriks = '" + studentMatriks + "';"
            command.ExecuteNonQuery()
            conn.Close()
            MsgBox("Successfully Paid for College Fees!", 64, "Success")
            Dim recieptmenu As New RecieptMenu
            recieptmenu.StudentMatriks = Me.studentMatriks
            recieptmenu.Show()
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
        MsgBox("Please Enter your Payment Credentials Here Before Proceeding with Payment!", 64, "About")
    End Sub
End Class