Imports MySql.Data.MySqlClient

Public Class users

    Public empID As String
    Public table As New DataTable


    Public Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        addEmployees()
    End Sub


    Public Sub addEmployees()

        Try

            If txtLast.Text = "" Then
                MessageBox.Show("Last name cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If txtFirst.Text = "" Then
                MessageBox.Show("First name cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If txtUser.Text = "" Then
                MessageBox.Show("Username cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If txtPass.Text = "" Then
                MessageBox.Show("Password cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If txtCont.Text = "" Then
                MessageBox.Show("Contact number cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If


            If MsgBox("Save this record?", vbYesNo + vbQuestion) = vbYes Then

                dbConnection()
                sql = "INSERT INTO employees VALUES ('" & txtID.Text & "',@lastname,@firstname,@username,@password,@user_type,@contact_number,@address, @sex) "
                cmd = New MySqlCommand
                With cmd
                    .Connection = sqlcon
                    .CommandText = sql
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@employee_id", txtID.Text)
                    .Parameters.AddWithValue("@lastname", txtLast.Text)
                    .Parameters.AddWithValue("@firstname", txtFirst.Text)
                    .Parameters.AddWithValue("@username", txtUser.Text)
                    .Parameters.AddWithValue("@password", txtPass.Text)
                    .Parameters.AddWithValue("@user_type", ComboBox1.Text)
                    .Parameters.AddWithValue("@contact_number", txtCont.Text)
                    .Parameters.AddWithValue("@address", txtAdress.Text)

                    If RadioButton1.Checked = True Then
                        .Parameters.AddWithValue("@sex", RadioButton1.Text)

                    ElseIf RadioButton2.Checked = True Then
                        .Parameters.AddWithValue("@sex", RadioButton2.Text)

                    End If
                    result = .ExecuteNonQuery()
                    If result = 0 Then
                        MsgBox("Error in adding new employee!")
                    Else
                        MsgBox("Successfully added new employee!", vbInformation)

                        txtLast.Clear()
                        txtFirst.Clear()
                        txtUser.Clear()
                        txtPass.Clear()
                        txtCont.Clear()
                        txtAdress.Clear()

                        getEmpID()

                        retrieve()
                    End If

                End With

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            cmd.Dispose()
        End Try
    End Sub

    Public Sub retrieve()

        Try
            dbConnection()
            sql = "SELECT employee_id AS 'EMPLOYEE ID', lastname AS 'LAST NAME', firstname AS 'FIRST NAME', username AS 'USERNAME', password AS 'PASSWORD', user_type AS 'USER TYPE', contact_number AS 'CONTACT NUMBER', address AS 'ADDRESS',
                    sex AS 'SEX' FROM employees;"
            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
                .ExecuteNonQuery()
            End With
            dt = New DataTable
            mysqlDA = New MySqlDataAdapter
            mysqlDA.SelectCommand = cmd
            mysqlDA.Fill(dt)
            employeeDGV.DataSource = dt

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            mysqlDA.Dispose()
            cmd.Dispose()
        End Try

    End Sub

    Public Sub users_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        getEmpID()
        retrieve()


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


    Public Sub getEmpID()
        Try

            sql = "SELECT employee_id from employees order by employee_id Desc"
            dbConnection()

            cmd = New MySqlCommand(sql, sqlcon)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            If (dr.Read) = True Then
                Dim id As Integer
                id = (dr(0) + 1)
                empID = id.ToString("000")

            ElseIf IsDBNull(dr) Then

                empID = ("0001")

            Else
                empID = ("0001")


            End If

        Catch ex As Exception
            MsgBox(ex.Message, "getEmpID()")
        Finally
            cmd.Dispose()
            sqlcon.Close()

            txtID.Text = empID

        End Try

    End Sub

    Private Sub txtCont_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCont.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack Or e.KeyChar = "+") Then
            e.Handled = True
        End If
    End Sub

    Private Sub employeeDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles employeeDGV.CellClick

        Try
            Dim sel As DataGridViewRow
            sel = employeeDGV.Rows(e.RowIndex)
            txtID.Text = sel.Cells(0).Value
            txtLast.Text = sel.Cells(1).Value
            txtFirst.Text = sel.Cells(2).Value
            txtUser.Text = sel.Cells(3).Value
            txtPass.Text = sel.Cells(4).Value
            ComboBox1.Text = sel.Cells(5).Value
            txtCont.Text = sel.Cells(6).Value
            txtAdress.Text = sel.Cells(7).Value

            If "Male" = sel.Cells(8).Value Then
                RadioButton1.Checked = True
            Else
                RadioButton2.Checked = True
            End If


            Button2.Enabled = True
            Button3.Enabled = False


        Catch ex As Exception
            Return

        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Try

            If MsgBox("Update this record?", vbYesNo + vbQuestion) = vbYes Then

                dbConnection()

                sql = "UPDATE employees SET lastname=@lastname, firstname=@firstname, username=@username, password=@password, user_type=@user_type, contact_number=@cont, address=@address, sex=@sex WHERE employee_id = '" & txtID.Text & "' "

                cmd = New MySqlCommand
                With cmd
                    .Connection = sqlcon
                    .CommandText = sql
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@lastname", txtLast.Text)
                    .Parameters.AddWithValue("@firstname", txtFirst.Text)
                    .Parameters.AddWithValue("@username", txtUser.Text)
                    .Parameters.AddWithValue("@password", txtPass.Text)
                    .Parameters.AddWithValue("@user_type", ComboBox1.Text)
                    .Parameters.AddWithValue("@cont", txtCont.Text)
                    .Parameters.AddWithValue("@address", txtAdress.Text)

                    If RadioButton1.Checked = True Then
                        .Parameters.AddWithValue("@sex", RadioButton1.Text)

                    ElseIf RadioButton2.Checked = True Then
                        .Parameters.AddWithValue("@sex", RadioButton2.Text)

                    End If

                    result = .ExecuteNonQuery()
                    If result = 0 Then
                        MsgBox("Error in adding new product!")
                    Else
                        MsgBox("Successfully Updated", vbInformation)
                        getEmpID()
                        retrieve()

                        txtLast.Text = ""
                        txtFirst.Text = ""
                        txtUser.Text = ""
                        txtPass.Text = ""
                        txtCont.Text = ""
                        txtAdress.Text = ""

                    End If
                End With
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            Return
        Finally
            sqlcon.Close()
            cmd.Dispose()
        End Try


    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Me.Controls.Clear()
        InitializeComponent()
        users_Load(e, e)
        txtSearchBox.Text = ""
    End Sub

    Private Sub txtSearchBox_TextChanged(sender As Object, e As EventArgs) Handles txtSearchBox.TextChanged

        Try
            Dim data As Integer
            dbConnection()

            If txtSearchBox.Text = Nothing Then

                sql = "SELECT employee_id AS 'EMPLOYEE ID', lastname AS 'LAST NAME', firstname AS 'FIRST NAME', username AS 'USERNAME', password AS 'PASSWORD', user_type AS 'USER TYPE', contact_number AS 'CONTACT NUMBER', address AS 'ADDRESS', 
                    sex AS 'SEX' FROM employees ORDER BY employee_id;"
            Else
                sql = "SELECT employee_id AS 'EMPLOYEE ID', lastname AS 'LAST NAME', firstname AS 'FIRST NAME', username AS 'USERNAME', password AS 'PASSWORD', user_type AS 'USER TYPE', contact_number AS 'CONTACT NUMBER', address AS 'ADDRESS', 
                    sex AS 'SEX' FROM employees WHERE firstname LIKE '" & txtSearchBox.Text & "%';"
            End If

            mysqlDA = New MySqlDataAdapter(sql, sqlcon)
            dt = New DataTable
            Data = mysqlDA.Fill(dt)
            If Data > 0 Then
                employeeDGV.DataSource = Nothing
                employeeDGV.DataSource = dt
                employeeDGV.ClearSelection()
            Else
                employeeDGV.DataSource = dt
            End If

        Catch ex As Exception
            MsgBox("Failed to search" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            sqlcon.Close()
        End Try
        sqlcon.Close()

    End Sub


End Class