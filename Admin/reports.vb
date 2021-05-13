Public Class reports
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        switchpanel(transactions)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click, Button4.Click

        switchpanel(stocks_log)

    End Sub


    Sub switchpanel(ByVal panel1 As Form)

        panel_reports.Controls.Clear()
        panel1.TopLevel = False
        panel_reports.Controls.Add(panel1)
        panel1.Show()

    End Sub

End Class