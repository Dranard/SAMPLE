Imports MySql.Data.MySqlClient

Public Class Suppliers

    Public supID As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Try
            dbConnection()
            sql = "INSERT INTO supplier (id, supplier_name, supplier_address, contact, email) VALUES ('" & TextBox1.Text & "', @supplier_name, @supplier_address, @contact, @email);"
            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
                .Parameters.Clear()
                .Parameters.AddWithValue("@id", TextBox1.Text)
                .Parameters.AddWithValue("@supplier_name", TextBox2.Text)
                .Parameters.AddWithValue("@supplier_address", TextBox3.Text)
                .Parameters.AddWithValue("@contact", txtCont.Text)
                .Parameters.AddWithValue("@email", txtEmail.Text)
                result = .ExecuteNonQuery()
                If result = 0 Then
                    MsgBox("Error")
                Else
                    MsgBox("Successfully added!", vbInformation)
                    getID()
                    retrieve()

                    TextBox2.Text = ""
                    TextBox3.Text = ""
                    txtCont.Text = ""
                    txtEmail.Text = ""
                End If

            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            cmd.Dispose()
        End Try

    End Sub

    Public Sub getID()

        Try
            sql = "SELECT id from supplier order by id Desc"
            dbConnection()

            cmd = New MySqlCommand(sql, sqlcon)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            If (dr.Read) = True Then
                Dim id As Integer
                id = (dr(0) + 1)
                supID = id.ToString
            End If

        Catch ex As Exception
            MsgBox(ex.Message, "getID()")
        Finally
            cmd.Dispose()
            sqlcon.Close()

            TextBox1.Text = supID

        End Try

    End Sub

    Public Sub retrieve()

        Try
            dbConnection()
            sql = "SELECT id AS'ID', supplier_name AS 'SUPPLIER NAME', supplier_address AS'ADDRESS', contact AS'CONTACT', email AS 'EMAIL' FROM supplier;"
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
            supDGV.DataSource = dt

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            mysqlDA.Dispose()
            cmd.Dispose()
        End Try

    End Sub

    Private Sub Suppliers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getID()
        retrieve()

        supDGV.Columns.Item("ID").Width = 50

    End Sub

    Private Sub supDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles supDGV.CellClick

        Try
            Dim sel As DataGridViewRow

            sel = supDGV.Rows(e.RowIndex)
            TextBox1.Text = sel.Cells(0).Value
            TextBox2.Text = sel.Cells(1).Value
            TextBox3.Text = sel.Cells(2).Value
            txtCont.Text = sel.Cells(3).Value
            txtEmail.Text = sel.Cells(4).Value

            Button3.Enabled = False
            Button2.Enabled = True

        Catch ex As Exception
            MsgBox(ex.Message)
            Return
        End Try


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Try

            dbConnection()

            sql = "UPDATE supplier SET supplier_name = @supplier_name, supplier_address = @supplier_address, contact = @contact, email= @email WHERE id = '" & TextBox1.Text & "' "

            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
                .Parameters.Clear()
                .Parameters.AddWithValue("@supplier_name", TextBox2.Text)
                .Parameters.AddWithValue("@supplier_address", TextBox3.Text)
                .Parameters.AddWithValue("@contact", txtCont.Text)
                .Parameters.AddWithValue("@email", txtEmail.Text)
                result = .ExecuteNonQuery()
                If result = 0 Then
                    MsgBox("Error in adding new product!")
                Else
                    MsgBox("Successfully Updated", vbInformation)
                    TextBox2.Text = ""
                    TextBox3.Text = ""
                    txtCont.Text = ""
                    txtEmail.Text = ""
                    getID()
                    retrieve()

                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
            Return
        Finally
            sqlcon.Close()
            cmd.Dispose()
        End Try

    End Sub


End Class