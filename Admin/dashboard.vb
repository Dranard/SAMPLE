Imports MySql.Data.MySqlClient

Public Class dashboard


    Private Sub dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ShowData()
        TotalItems()

    End Sub

    Public Sub ShowData()

        Try
            dbConnection()
            sql = "SELECT product_id AS 'PRODUCT ID', product_name AS 'PRODUCT NAME', brand AS 'BRAND', category AS 'CATEGORY', 
                    supplier AS 'SUPPLIER', product_price AS 'PRICE(PHP)', product_qty AS 'QUANTITY', date_added AS 'DATE ADDED', user_type AS 'USER', employee_id AS 'EMPLOYEE ID' FROM stocks;"
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
            DataGridView1.DataSource = dt

            DataGridView1.Columns.Item("PRODUCT ID").Width = 80
            DataGridView1.Columns.Item("SUPPLIER").Width = 150
            DataGridView1.Columns.Item("PRICE(PHP)").Width = 100
            DataGridView1.Columns.Item("QUANTITY").Width = 70
            DataGridView1.Columns.Item("DATE ADDED").Width = 150
            DataGridView1.Columns.Item("USER").Width = 90

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            mysqlDA.Dispose()
            cmd.Dispose()
        End Try

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting

        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.Cells(6).Value <= 10 Then
                row.DefaultCellStyle.ForeColor = Color.White
                row.DefaultCellStyle.BackColor = Color.Red

            ElseIf row.Cells(6).Value > 10 And row.Cells(6).Value <= 20 Then
                row.DefaultCellStyle.ForeColor = Color.Black
                row.DefaultCellStyle.BackColor = Color.Yellow

            Else
                row.DefaultCellStyle.ForeColor = Color.Black
                'row.DefaultCellStyle.BackColor = Color.Yellow

            End If

        Next

    End Sub





    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Suppliers.Show()

    End Sub


    Public Sub TotalItems()

        Try
            dbConnection()
            sql = "SELECT SUM(product_qty) FROM stocks"
            cmd = New MySqlCommand
            With cmd
                .Connection = sqlcon
                .CommandText = sql
                .ExecuteNonQuery()
            End With

            lblTotalItems.Text = cmd.ExecuteScalar().ToString()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            mysqlDA.Dispose()
            cmd.Dispose()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click, Button6.Click

        brands.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        categories.Show()

    End Sub
End Class