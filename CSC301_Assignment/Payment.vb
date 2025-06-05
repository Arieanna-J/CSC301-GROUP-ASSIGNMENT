Imports System.Data.OleDb
Public Class Payment
    '' Declaration of SQL based Variables
    Dim datValueStudent As New DataTable
    Dim datValueAdmin As New DataTable

    Dim conn As New OleDbConnection
    Dim command As OleDbCommand
    Dim dataAdpt As New OleDbDataAdapter(command)

    Private btmap As Bitmap

    '' Declaration of Local Variables

    Dim dblDormFees As Double
    Dim dblElectricPrice As Double
    Dim dblAfterTax As Double
    Dim dblTotalFees As Double
    Dim localMatriks As String '' Local Variable since I dunno why PaymentStudentMatriks can't be used in DatabaseInsert() ???

    ''Declaration of Property Variables
    Public Property PaymentStudentMatriks As String
    Public Property RetrieveName As String
    Public Property ElectricalNum As Integer
    Public Property intQuantity As Integer

    '' Declaration of Variables

    Private Sub Payment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' SQL Connection
        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Arieanna\source\repos\CSC301_Assignment\CSC301_Assignment\CollegeSystem.accdb"

        '' Changing Label Text
        dblDormFees = CalculateDormFees()
        dblElectricPrice = 10.0 * intQuantity
        dblAfterTax = dblDormFees + (dblDormFees * 0.001)
        dblTotalFees = dblAfterTax + dblElectricPrice

        Dim electricform As New ElectricForm


        Label7.Text = dblDormFees.ToString("N")
        Label8.Text = dblElectricPrice.ToString("N")
        Label9.Text = dblAfterTax.ToString("N")
        Label10.Text = dblTotalFees.ToString("N")

        Me.PaymentStudentMatriks = electricform.StudentMatriks
    End Sub

    '' Sub Function
    Private Function CalculateDormFees() As Double
        conn.Open()
        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT StudentMatriks, StudentCGPA, StudentEMerit, Semester, StudentSalary FROM Student WHERE StudentMatriks = '" + PaymentStudentMatriks + "';"
        command.ExecuteNonQuery()

        datValueStudent = New DataTable()
        dataAdpt = New OleDbDataAdapter(command)
        dataAdpt.Fill(datValueStudent)

        '' Declare Variables From SQL Query
        Dim newMatriks As String = datValueStudent.Rows(0)("StudentMatriks").ToString()
        localMatriks = newMatriks
        Dim CalcCGPA As String = datValueStudent.Rows(0)("StudentCGPA").ToString()
        Dim CalcEMerit As String = datValueStudent.Rows(0)("StudentEMerit").ToString()
        Dim CalcSemester As String = datValueStudent.Rows(0)("Semester").ToString()
        Dim CalcSalary As String = datValueStudent.Rows(0)("StudentSalary").ToString()
        conn.Close()

        '' Declare Variables
        Dim dblCalcCGPA As Double = CDbl(CalcCGPA)
        Dim dblCalcEMerit As Double = CDbl(CalcEMerit)
        Dim intCalcSemester As Integer = CInt(CalcSemester)
        Dim dblCalcSalary As Double = CDbl(CalcSalary)

        Dim PaymentCollege As Double = 455.0

        '' Processing [For Salary to Determine Either: B40, M40 or T20]
        If dblCalcSalary < 2000.0 Then '' 0.05 Discount for Low-B40 Students
            PaymentCollege = PaymentCollege - (PaymentCollege * 0.05)

        ElseIf dblCalcSalary < 5249.0 Then '' 0.03 Discount for Standard-B40 Students
            PaymentCollege = PaymentCollege - (PaymentCollege * 0.03)

        ElseIf dblCalcSalary >= 5250.0 And dblCalcSalary < 8949.0 Then '' 0.001 Discount for Low-M40 Students
            PaymentCollege = PaymentCollege - (PaymentCollege * 0.001)

        ElseIf dblCalcSalary >= 8950.0 And dblCalcSalary < 11819.0 Then '' Standard Price for Standrd-M40 Students
            PaymentCollege = PaymentCollege

        Else '' Premium based on T20 Student's Salary
            Dim PremiumPercent = dblCalcSalary / (PaymentCollege * 1000) ''So that the Decimal should look like 0.0xx
            PaymentCollege = PaymentCollege + (PaymentCollege * PremiumPercent)
        End If

        '' Processing [For CGPA: Dean's List Requirement]
        If dblCalcCGPA > 3.5 Then '' If Student had gotten Dean's List last Semester, They'd get a 0.0003% Discount
            PaymentCollege = PaymentCollege - (PaymentCollege * 0.0003)
        End If

        '' Processing [For EMerit]
        If dblCalcEMerit > 3.5 Then ''If Student's EMerit is more 3.5, They'll get a Discount based on the EMerit

            Dim intCounter As Integer
            Dim dblEMeritDiscount As Double = 0

            '' For Loop
            For intCounter = 0 To dblCalcEMerit
                dblEMeritDiscount += 0.0002
            Next

            PaymentCollege = PaymentCollege - (PaymentCollege * dblEMeritDiscount)
        End If

        Return PaymentCollege
    End Function

    '' Sub Function to Check if PaymentID exists, if Not, Create One.
    Private Sub DatabaseInsert()
        conn.Open()
        command = conn.CreateCommand()
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT StudentMatriks FROM Student WHERE StudentMatriks = '" + localMatriks + "' AND PaymentID IS NULL;"
        command.ExecuteNonQuery()

        datValueStudent = New DataTable()
        dataAdpt = New OleDbDataAdapter(command)
        dataAdpt.Fill(datValueStudent)


        Dim checker As Integer = Convert.ToInt32(datValueStudent.Rows.Count.ToString())
        conn.Close()

        Try
            conn.Open()
            command = conn.CreateCommand()
            command.CommandType = CommandType.Text
            command.CommandText = "INSERT INTO Payment(PaymentAmount, StudentMatriks, PaymentStatus, electricAmount) VALUES(@Payment, @Matriks, 'Not Paid', @ElectricPrice);"
            command.Parameters.Clear()

            Dim paymentParam As New OleDbParameter("@Payment", OleDbType.Double)
            paymentParam.Value = dblTotalFees ' Ensure this value is valid
            command.Parameters.Add(paymentParam)

            Dim matriksParam As New OleDbParameter("@Matriks", OleDbType.VarWChar)
            matriksParam.Value = localMatriks
            command.Parameters.Add(matriksParam)

            Dim ElectricPrice As New OleDbParameter("@ElectricPrice", OleDbType.Double)
            ElectricPrice.Value = dblElectricPrice
            command.Parameters.Add(ElectricPrice)

            command.ExecuteNonQuery()

            command = conn.CreateCommand()
            command.CommandType = CommandType.Text

            command.CommandText = "UPDATE Student INNER JOIN Payment ON Student.StudentMatriks = Payment.StudentMatriks 
                                       SET Student.PaymentID = Payment.PaymentID WHERE Student.StudentMatriks = @StudentMatriks;"
            command.Parameters.Clear()

            matriksParam = New OleDbParameter("@Matriks", OleDbType.VarWChar)
            matriksParam.Value = localMatriks
            command.Parameters.Add(matriksParam)

            command.ExecuteNonQuery()
            conn.Close()

        Catch exception As Exception
            MsgBox(exception.Message, 64, "Error")
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DatabaseInsert()
        Dim checkout As New Checkout With {
            .finalbill = dblTotalFees,
            .studentMatriks = localMatriks
        }
        checkout.Show()
        Me.Hide()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim exitResponse As Integer = MsgBox("Are you Sure You Want to Exit?", 4 + 32, "Exit")
        If exitResponse = vbYes Then
            MsgBox("System Shutting Down...", 64, "Notice")
            Application.Exit()
        End If
    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.Click
        MsgBox("These are Your Fees That You'd Be Paying For This Semester.", 64, "About")
    End Sub
End Class