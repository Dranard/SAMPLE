Imports MySql.Data.MySqlClient

Public Class login

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try



            Dim sql As New MySqlCommand("SELECT username, password, user_type FROM employees WHERE BINARY username = @user AND BINARY password = @pass AND user_type = @user_type", sqlcon)

            sql.Parameters.Add("@user", MySqlDbType.VarChar).Value = TextBox1.Text
            sql.Parameters.Add("@pass", MySqlDbType.VarChar).Value = txtPassword.Text
            sql.Parameters.Add("@user_type", MySqlDbType.VarChar).Value = ComboBox1.Text

            Dim adapter As New MySqlDataAdapter(sql)
            Dim table As New DataTable()

            adapter.Fill(table)

            If table.Rows.Count = 0 Then
                MessageBox.Show("Invalid username or password", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                If ComboBox1.Text = "Admin" Then
                    MsgBox("Successfully Login", vbInformation)
                    Admin.Show()
                    Me.Close()

                ElseIf ComboBox1.Text = "Cashier" Then
                    MsgBox("Successfully Login", vbInformation)
                    frmPOS.Show()
                    Me.Close()
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            Return
        Finally
            sqlcon.Close()
            cmd.Dispose()
        End Try



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Me.Close()
        End

    End Sub

    'Private Sub chkPass_CheckedChanged(sender As Object, e As EventArgs) Handles chkPass.CheckedChanged

    '    If txtPassword.UseSystemPasswordChar = True Then
    '        txtPassword.UseSystemPasswordChar = False

    '    Else
    '        txtPassword.UseSystemPasswordChar = True
    '    End If

    'End Sub

    Private Sub login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            dbConnection()
            sql = "SELECT id,user_type FROM user_type"
            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
                .ExecuteNonQuery()
            End With
            ds = New DataSet
            mysqlDA = New MySqlDataAdapter
            mysqlDA.SelectCommand = cmd
            mysqlDA.Fill(ds)
            mysqlDA.Dispose()
            cmd.Dispose()

            sqlcon.Close()

            ComboBox1.DataSource = ds.Tables(0)
            ComboBox1.ValueMember = "id"
            ComboBox1.DisplayMember = "user_type"


        Catch ex As Exception
            MessageBox.Show("Can not open connection ! ")
        End Try

    End Sub


End Class