Imports MySql.Data.MySqlClient

Public Class stock

    Public dateAndTime As Date
    Public stockID As String
    Public catSelect As String

    Public Sub stock_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        getEmpID()
        ShowData()

        Try

            dbConnection()
            sql = "SELECT id,category_name FROM categories"
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

            categoryBox.DataSource = ds.Tables(0)
            categoryBox.ValueMember = "id"
            categoryBox.DisplayMember = "category_name"


        Catch ex As Exception
            MessageBox.Show("Can not open connection ! ")
        End Try

        Try

            dbConnection()
            sql = "SELECT id,supplier_name FROM supplier"
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

            supplierBox.DataSource = ds.Tables(0)
            supplierBox.ValueMember = "id"
            supplierBox.DisplayMember = "supplier_name"


        Catch ex As Exception
            MessageBox.Show("Can not open connection ! ")
        End Try

        Try

            dbConnection()
            sql = "SELECT brand_id,brand_name FROM tbl_brand"
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

            brandBox.DataSource = ds.Tables(0)
            brandBox.ValueMember = "brand_id"
            brandBox.DisplayMember = "brand_name"


        Catch ex As Exception
            MessageBox.Show("Can not open connection ! ")
        End Try


        stocksDGV.Columns.Item("PRODUCT ID").Width = 80
        stocksDGV.Columns.Item("SUPPLIER").Width = 130
        stocksDGV.Columns.Item("PRICE(PHP)").Width = 100
        stocksDGV.Columns.Item("QUANTITY").Width = 70
        stocksDGV.Columns.Item("USER").Width = 90



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
            stocksDGV.DataSource = dt

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlcon.Close()
            mysqlDA.Dispose()
            cmd.Dispose()
        End Try

    End Sub

    Public Sub addStocks()

        If itemNameText.Text = "" Then
            MessageBox.Show("Item name cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If priceText.Text = "" Then
            MessageBox.Show("Price cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If stockText.Text = "" Then
            MessageBox.Show("Quantity cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try


            If MsgBox("Save this record?", vbYesNo + vbQuestion) = vbYes Then

                dbConnection()
                sql = "INSERT INTO stocks (product_id,product_name,brand,category,supplier,product_price,product_qty,date_added,user_type,employee_id) 
                    VALUES ('" & TextBox1.Text & "',@product_name,@brand,@category,@supplier,@product_price,@product_qty,@date_added,@user_type,@employee_id)"

                cmd = New MySqlCommand
                With cmd
                    .Connection = sqlcon
                    .CommandText = sql
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@product_id", TextBox1.Text)
                    .Parameters.AddWithValue("@product_name", itemNameText.Text)
                    .Parameters.AddWithValue("@brand", brandBox.Text)
                    .Parameters.AddWithValue("@category", categoryBox.Text)
                    .Parameters.AddWithValue("@supplier", supplierBox.Text)
                    .Parameters.AddWithValue("@product_price", priceText.Text)
                    .Parameters.AddWithValue("@product_qty", stockText.Text)
                    .Parameters.AddWithValue("@date_added", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").ToString)
                    .Parameters.AddWithValue("@user_type", Admin.Label5.Text)
                    .Parameters.AddWithValue("@employee_id", Admin.txtID.Text)
                    result = .ExecuteNonQuery()
                    If result = 0 Then
                        MsgBox("Error in adding new product!")
                    Else
                        MsgBox("Successfully added new product!", vbInformation)
                        getEmpID()
                        ShowData()

                        itemNameText.Text = ""
                        priceText.Text = ""
                        stockText.Text = ""

                        dashboard.ShowData()

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

    Public Sub getEmpID()
        Try

            sql = "SELECT product_id from stocks order by product_id Desc"
            dbConnection()

            cmd = New MySqlCommand(sql, sqlcon)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            If (dr.Read) = True Then
                Dim id As Integer
                id = (dr(0) + 1)
                stockID = id.ToString("0000000")

            ElseIf IsDBNull(dr) Then

                stockID = ("1000001")

            Else
                stockID = ("1000001")


            End If

        Catch ex As Exception
            MsgBox(ex.Message, "getEmpID()")
        Finally
            cmd.Dispose()
            sqlcon.Close()

            TextBox1.Text = stockID

        End Try

    End Sub


    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        addStocks()
    End Sub


    Private Sub priceText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles priceText.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack Or e.KeyChar = ".") Then
            e.Handled = True
        End If
    End Sub

    Private Sub stockText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles stockText.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack Or e.KeyChar = "+") Then
            e.Handled = True
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        If CheckBox1.Checked = True Then
            CheckBox2.Checked = False
            CheckBox3.Checked = False
            txtSearchBox.ReadOnly = False
        End If

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

        If CheckBox2.Checked = True Then
            CheckBox1.Checked = False
            CheckBox3.Checked = False
            txtSearchBox.ReadOnly = False
        End If
    End Sub
    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged

        If CheckBox3.Checked = True Then
            CheckBox1.Checked = False
            CheckBox2.Checked = False
            txtSearchBox.ReadOnly = False
        End If

    End Sub

    Private Sub txtSearchBox_TextChanged(sender As Object, e As EventArgs) Handles txtSearchBox.TextChanged

        Dim data As Integer

        If CheckBox1.Checked = True Then
            If txtSearchBox.Text = Nothing Then
                sql = "SELECT product_id AS 'PRODUCT ID', product_name AS 'PRODUCT NAME', brand AS 'BRAND', category AS 'CATEGORY', 
                    supplier AS 'SUPPLIER', product_price AS 'PRICE(PHP)', product_qty AS 'QUANTITY', date_added AS 'DATE ADDED', user_type AS 'USER', employee_id AS 'EMPLOYEE ID' FROM stocks ORDER BY product_name;"


            Else
                sql = "SELECT product_id AS 'PRODUCT ID', product_name AS 'PRODUCT NAME', brand AS 'BRAND', category AS 'CATEGORY', 
                    supplier AS 'SUPPLIER', product_price AS 'PRICE(PHP)', product_qty AS 'QUANTITY', date_added AS 'DATE ADDED', user_type AS 'USER', employee_id AS 'EMPLOYEE ID' FROM stocks WHERE category LIKE '" & txtSearchBox.Text & "%';"

            End If
        End If

        If CheckBox2.Checked = True Then
            If txtSearchBox.Text = Nothing Then
                sql = "SELECT product_id AS 'PRODUCT ID', product_name AS 'PRODUCT NAME', brand AS 'BRAND', category AS 'CATEGORY', 
                    supplier AS 'SUPPLIER', product_price AS 'PRICE(PHP)', product_qty AS 'QUANTITY', date_added AS 'DATE ADDED', user_type AS 'USER', employee_id AS 'EMPLOYEE ID' FROM stocks ORDER BY product_name;"

            Else
                sql = "SELECT product_id AS 'PRODUCT ID', product_name AS 'PRODUCT NAME', brand AS 'BRAND', category AS 'CATEGORY', 
                    supplier AS 'SUPPLIER', product_price AS 'PRICE(PHP)', product_qty AS 'QUANTITY', date_added AS 'DATE ADDED', user_type AS 'USER', employee_id AS 'EMPLOYEE ID' FROM stocks WHERE supplier LIKE '" & txtSearchBox.Text & "%';"

            End If
        End If

        If CheckBox3.Checked = True Then
            If txtSearchBox.Text = Nothing Then
                sql = "SELECT product_id AS 'PRODUCT ID', product_name AS 'PRODUCT NAME', brand AS 'BRAND', category AS 'CATEGORY', 
                    supplier AS 'SUPPLIER', product_price AS 'PRICE(PHP)', product_qty AS 'QUANTITY', date_added AS 'DATE ADDED', user_type AS 'USER', employee_id AS 'EMPLOYEE ID' FROM stocks ORDER BY product_name;"

            Else
                sql = "SELECT product_id AS 'PRODUCT ID', product_name AS 'PRODUCT NAME', brand AS 'BRAND', category AS 'CATEGORY', 
                    supplier AS 'SUPPLIER', product_price AS 'PRICE(PHP)', product_qty AS 'QUANTITY', date_added AS 'DATE ADDED', user_type AS 'USER', employee_id AS 'EMPLOYEE ID' FROM stocks WHERE product_name LIKE '" & txtSearchBox.Text & "%';"

            End If
        End If

        Try
            mysqlDA = New MySqlDataAdapter(sql, sqlcon)
            dt = New DataTable
            data = mysqlDA.Fill(dt)
            If data > 0 Then
                stocksDGV.DataSource = Nothing
                stocksDGV.DataSource = dt
                stocksDGV.ClearSelection()
            Else
                stocksDGV.DataSource = dt
            End If

            stocksDGV.Columns.Item("ID").Width = 60
            stocksDGV.Columns.Item("SUPPLIER").Width = 130
            stocksDGV.Columns.Item("PRICE(PHP)").Width = 100
            stocksDGV.Columns.Item("QUANTITY").Width = 70
            stocksDGV.Columns.Item("USER").Width = 90

        Catch ex As Exception
            MsgBox("Failed to search" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            sqlcon.Close()

        End Try
        sqlcon.Close()
    End Sub

    Public Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Me.Controls.Clear()
        InitializeComponent()
        stock_Load(e, e)
        txtSearchBox.Text = ""
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox3.Checked = False

    End Sub
    Private Function AllCellsSelected(dgv As DataGridView) As Boolean
        AllCellsSelected = (stocksDGV.SelectedCells.Count = (stocksDGV.RowCount * stocksDGV.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
    End Function

    Private Sub stocksDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles stocksDGV.CellClick

        Try
            Dim sel As DataGridViewRow

            sel = stocksDGV.Rows(e.RowIndex)
            TextBox1.Text = sel.Cells(0).Value
            itemNameText.Text = sel.Cells(1).Value
            brandBox.Text = sel.Cells(2).Value
            categoryBox.Text = sel.Cells(3).Value
            supplierBox.Text = sel.Cells(4).Value
            priceText.Text = sel.Cells(5).Value
            stockText.Text = sel.Cells(6).Value

            btnAdd.Enabled = False
            btnClear.Enabled = True
            Button1.Enabled = True

            categoryBox.Enabled = False
            supplierBox.Enabled = False
            stockText.Enabled = False

        Catch ex As Exception
            Return

        End Try


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If MsgBox("Update this record?", vbYesNo + vbQuestion) = vbYes Then
                dbConnection()

                sql = "UPDATE stocks SET product_name=@product_name, product_price=@product_price WHERE product_id = '" & TextBox1.Text & "' "

                cmd = New MySqlCommand
                With cmd
                    .Connection = sqlcon
                    .CommandText = sql
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@product_name", itemNameText.Text)
                    .Parameters.AddWithValue("@product_price", priceText.Text)
                    result = .ExecuteNonQuery()
                    If result = 0 Then
                        MsgBox("Error in adding new product!")
                    Else
                        MsgBox("Successfully Updated", vbInformation)
                        getEmpID()
                        ShowData()
                        itemNameText.Text = ""
                        priceText.Text = ""
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

    Private Sub stocksDGV_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles stocksDGV.CellFormatting


        For Each row As DataGridViewRow In stocksDGV.Rows
            If row.Cells(6).Value <= 10 Then
                row.DefaultCellStyle.ForeColor = Color.White
                row.DefaultCellStyle.BackColor = Color.Red

            ElseIf row.Cells(6).Value > 10 And row.Cells(6).Value <= 20 Then
                row.DefaultCellStyle.ForeColor = Color.Black
                row.DefaultCellStyle.BackColor = Color.Yellow
            End If

        Next
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        itemNameText.Text = ""
        priceText.Text = ""

    End Sub


End Class