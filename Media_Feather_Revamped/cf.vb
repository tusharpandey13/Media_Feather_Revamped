Class controlform : Inherits Form
    Dim seekbar As customLinearProgress = New customLinearProgress With {.Left = 13 + 7, .Top = 13 + 6 + 22 + 3, .Width = Width - 26 - 14, .BackColor = Color.Fuchsia, .ForeColor = col(90, 120, 204), .Value = 50}
    Dim cb1 As controlbar = New controlbar With {.Top = 13 + 6 + 22 + 3 + 10 + 3 + 2, .Left = 20}
    Sub New(s As Size, p As Point)
        FormBorderStyle = 0
        Size = s
        Location = p
        ShowInTaskbar = 0
        Text = ""
        TransparencyKey = Color.Fuchsia
        BackColor = TransparencyKey

        Controls.Add(seekbar)
        Controls.Add(cb1)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        seekbar.Width = Width - 26 - 14
        cb1.Width = Width - 26 - 14

    End Sub
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        End
    End Sub
End Class
