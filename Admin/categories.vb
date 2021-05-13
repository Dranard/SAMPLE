Imports MySql.Data.MySqlClient


Public Class categories

    Public catID As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Try
            dbConnection()
            sql = "INSERT INTO categories (id,category_name) VALUES ('" & TextBox1.Text & "',@category_name); "
            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
                .Parameters.Clear()
                .Parameters.AddWithValue("@id", TextBox1.Text)
                .Parameters.AddWithValue("@category_name", TextBox2.Text)
                result = .ExecuteNonQuery()
                If result = 0 Then
                    MsgBox("Error")
                Else
                    MsgBox("Successfully added!")
                    getID()
                    retrieve()

                    TextBox2.Text = ""
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
            sql = "SELECT id from categories order by id Desc"
            dbConnection()

            cmd = New MySqlCommand(sql, sqlcon)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            If (dr.Read) = True Then
                Dim id As Integer
                id = (dr(0) + 1)
                catID = id.ToString
            End If

        Catch ex As Exception
            MsgBox(ex.Message, "getID()")
        Finally
            cmd.Dispose()
            sqlcon.Close()

            TextBox1.Text = catID

        End Try

    End Sub

    Private Sub categories_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getID()
        retrieve()



    End Sub

    Public Sub retrieve()

        Try
            dbConnection()
            sql = "SELECT id AS'ID', category_name AS 'CATEGORY' FROM categories;"
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
            catDGV.DataSource = dt

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            mysqlDA.Dispose()
            cmd.Dispose()
        End Try

    End Sub

    Private Sub catDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles catDGV.CellClick

        Try

            Dim sel As DataGridViewRow

            sel = catDGV.Rows(e.RowIndex)
            TextBox1.Text = sel.Cells(0).Value
            TextBox2.Text = sel.Cells(1).Value

            Button3.Enabled = False

            Button2.Enabled = True


        Catch ex As Exception
            Return
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Try
            dbConnection()

            sql = "UPDATE categories SET category_name = @category_name WHERE id = '" & TextBox1.Text & "' "

            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
                .Parameters.Clear()
                .Parameters.AddWithValue("@category_name", TextBox2.Text)
                result = .ExecuteNonQuery()
                If result = 0 Then
                    MsgBox("Error in adding new product!")
                Else
                    MsgBox("Successfully Updated")
                    TextBox2.Text = ""
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