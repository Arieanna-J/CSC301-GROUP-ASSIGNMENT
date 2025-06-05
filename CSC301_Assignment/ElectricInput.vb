Public Class ElectricInput
    Public Property intTextValue As Integer

    Public Property strProduct As String
    Public Property strBrand As String
    Public Property intNum As Integer

    Private Sub ElectricInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Electrical Appliance " & (intTextValue + 1)

        ComboBox1.Items.Add("Laptop")
        ComboBox1.Items.Add("Mobile Phone")
        ComboBox1.Items.Add("Tablet")
        ComboBox1.Items.Add("Electric Iron")
        ComboBox1.Items.Add("Printer")
        ComboBox1.Items.Add("Kettle")
        ComboBox1.Items.Add("Radio")

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If String.IsNullOrEmpty(TextBox1.Text) Or String.IsNullOrEmpty(TextBox2.Text) Then
                MsgBox("Please Enter a Value", 64, "Notice")
            Else
                If IsNumeric(TextBox2.Text) Then
                    intNum = Convert.ToInt32(TextBox2.Text)
                    If intNum > 0 Then
                        strProduct = ComboBox1.SelectedItem.ToString()
                        strBrand = TextBox1.Text

                        Dim electricform As New ElectricForm
                        electricform.electricalType = strProduct
                        electricform.electricalBrand = strBrand
                        electricform.electricalNum = intNum
                        MsgBox("Added Successfully", 64, "Success")

                        Me.DialogResult = DialogResult.OK
                        Me.Hide()
                    Else
                        MsgBox("Please Enter a Valid Quantity Value, No Negative Values Allowed", 64, "Notice")
                    End If
                Else
                    MsgBox("Please Enter a Valid Quantity Value, No Strings Allowed", 64, "Notice")
                End If
            End If
        Catch exception As Exception
            MsgBox(exception.Message, 64, "Alert")
        End Try
    End Sub
End Class