Imports MySql.Data.MySqlClient

Public Class Admin



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Button1.BackColor = Color.FromArgb(252, 186, 3)
        Button2.BackColor = Color.Transparent
        Button3.BackColor = Color.Transparent
        Button6.BackColor = Color.Transparent

        switchpanel(users)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Button1.BackColor = Color.Transparent
        Button2.BackColor = Color.FromArgb(252, 186, 3)
        Button3.BackColor = Color.Transparent
        Button6.BackColor = Color.Transparent
        switchpanel(stock)



    End Sub

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Button6.BackColor = Color.FromArgb(252, 186, 3)

        With dashboard
            .TopLevel = False
            panel.Controls.Add(dashboard)
            .BringToFront()
            .Show()

        End With

        txtDate.Text = System.DateTime.Now.ToString("ddd, MMMM dd yyyy")
        Timer1.Start()


        Try

            dbConnection()
            sql = "select employee_id FROM employees WHERE username = '" & login.TextBox1.Text & "' "
            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
            End With

            Using sdr As MySqlDataReader = cmd.ExecuteReader
                sdr.Read()
                txtID.Text = sdr("employee_id").ToString()

            End Using



        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            mysqlDA.Dispose()
            cmd.Dispose()
        End Try


    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        txtTime.Text = TimeOfDay.ToString("h:mm:ss tt")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Button3.BackColor = Color.FromArgb(252, 186, 3)
        Button1.BackColor = Color.Transparent
        Button2.BackColor = Color.Transparent
        Button6.BackColor = Color.Transparent
        switchpanel(reports)

    End Sub


    Sub switchpanel(ByVal panel1 As Form)

        panel.Controls.Clear()
        panel1.TopLevel = False
        panel.Controls.Add(panel1)
        panel1.Show()

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Button6.BackColor = Color.FromArgb(252, 186, 3)
        Button1.BackColor = Color.Transparent
        Button2.BackColor = Color.Transparent
        Button3.BackColor = Color.Transparent

        switchpanel(dashboard)

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MsgBox("Successfully Logout", vbInformation)
        Me.Close()
        login.Show()

    End Sub


End Class
