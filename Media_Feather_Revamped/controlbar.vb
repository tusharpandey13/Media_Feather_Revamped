Class controlbar : Inherits customControl
    Dim _cbx% = 3 : Private Property cbx%
        Get
            Return _cbx
        End Get
        Set(value%)
            If Not _cbx = value Then
                _cbx = value
                makeimages()
            End If
        End Set
    End Property
    Dim im_arrow(3) As Bitmap
    Dim im_bar(3) As Bitmap
    Dim r() As Rectangle
    Sub New()
        MyBase.New()
        Size = New Size(261, 27)
        BackColor = col(30, 30, 32)
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        makeimages()
    End Sub
    Sub makeimages()
        r = {rct(0, 0, Width / 3, 27), rct(Width / 3, 0, Width / 3 + 1, 27), rct(2 * Width / 3, 0, Width / 3, 27), rct(-9999, -9999, 1, 1)}
        Dim c() As Color = {col(100, 255), col(160, Color.GreenYellow), col(160, Color.DodgerBlue)}

        For i = 0 To 2

            im_arrow(i) = New Bitmap(CInt(Width / 3), 27)
            mb(c(i), tb)
            With Graphics.FromImage(im_arrow(i))
                .Clear(col(0, 0))
                .SmoothingMode = 2
                Dim pts() As Point = {pt(r(0).Width / 2 - 10 + 1, 4), pt(r(0).Width / 2 + 5, 13), pt(r(0).Width / 2 - 10 + 1, Height - 5)}
                .FillPolygon(tb, pts)
            End With
            im_bar(i) = New Bitmap(CInt(Width / 3), 27)
            With Graphics.FromImage(im_bar(i))
                .Clear(col(0, 0))
                .SmoothingMode = 0
                .FillRectangle(tb, rct(r(0).Left + r(0).Width / 2 + 6, 5, 5, 17))
            End With

        Next

    End Sub
    Protected Overrides Sub PaintHook()


        G.Clear(BackColor)
        G.InterpolationMode = 7
        Dim i() As Integer

        Select Case cbx
            Case 0
                i = {2, 0, 0}
            Case 1
                i = {0, 1, 0}
            Case 2
                i = {0, 0, 2}
            Case Else
                i = {0, 0, 0}
        End Select


        mb(col(50, 0), tb)
        G.FillRectangle(tb, r(cbx))


        im_arrow(i(0)).RotateFlip(RotateFlipType.Rotate180FlipY)
        im_bar(i(0)).RotateFlip(RotateFlipType.Rotate180FlipY)
        G.DrawImageUnscaled(im_arrow(i(0)), pt(r(0)))
        G.DrawImageUnscaled(im_bar(i(0)), pt(r(0)))
        im_arrow(i(0)).RotateFlip(RotateFlipType.Rotate180FlipY)
        im_bar(i(0)).RotateFlip(RotateFlipType.Rotate180FlipY)



        '       If Manager.State = media.Media.State.Playing Then
        G.DrawImageUnscaled(im_bar(i(1)), pt(r(1).X - 3, 0))
        im_bar(i(1)).RotateFlip(RotateFlipType.Rotate180FlipY)
        G.DrawImageUnscaled(im_bar(i(1)), pt(r(1).X + 3, 0))
        im_bar(i(1)).RotateFlip(RotateFlipType.Rotate180FlipY)

        'else
        'G.DrawImageUnscaled(im_arrow(i(1)), pt(r(1).X + 4, 0))

        '        End If

        G.DrawImageUnscaled(im_arrow(i(2)), pt(r(2)))
        G.DrawImageUnscaled(im_bar(i(2)), pt(r(2)))




    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        With e
            For i = 0 To 2
                If r(i).Contains(pt(.X, .Y)) Then
                    cbx = i
                    GoTo N
                Else cbx = 3
                End If
            Next
N:          Invalidate()
        End With
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        cbx = 3
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub
End Class
