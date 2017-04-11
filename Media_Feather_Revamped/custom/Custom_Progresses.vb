Imports System.ComponentModel


Class customLinearProgress

#Region "declare"
    Inherits customControl

#Region "global"
    Private _Maximum! = 100
    Private _Value! = 0
    Private pw!
    Private hoff!
#End Region
    Dim mpos As Point

#End Region

#Region "property"
    Public Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.ResizeRedraw Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        Maximum = 100
        Value = 0
        LockHeight = 10
        animating = True
    End Sub


    <Category("Behavior")>
    Public Property Maximum!
        Get
            Return _Maximum
        End Get
        Set(ByVal V!)
            If V < 1 Then V = 1
            If V < _Value Then _Value = V
            _Maximum = V
            Invalidate()
        End Set
    End Property
    <Category("Behavior")>
    Public Property Value!
        Get
            Return _Value#
        End Get
        Set(ByVal V!)
            If V > _Maximum Then
                _Value = _Maximum
            End If
            If V < 0 Then
                _Value = 0
            End If

            If _Value <> V Then
                _Value = V
            End If

            Invalidate()
        End Set
    End Property
#End Region

#Region "draw"

    Protected Overrides Sub PaintHook()
        If hoff >= 35 Then hoff = 0
        hoff += 0.08

        simple()
    End Sub
    Private Sub simple()

        'decor
        G.SmoothingMode = 2
        pw = CInt((Value / Maximum) * (Width))
        G.Clear(col(22, 22, 24))

        If Not Value = 0 Then

            'mb(BackColor, tb)
            'G.FillRectangle(tb, rct(-1, -1, pw + 1, Height + 1))
            mb(lc(ForeColor, 30), tb)
            G.FillRectangle(tb, rct(-1, -1, pw + 1, Height + 1))
            '----------------------------------------------------------------------------------------------------------------------


            G.SetClip(rct(1, 1, pw, Height - 2))
            mb(ForeColor, tb)
            G.FillRectangle(tb, rct(0, 0, pw, Height))

            G.SetClip(rct(1, 1, pw - 1, Height - 2))
            For i = -40 To pw + 40 Step 35
                Dim pts = {pt(i + hoff, 0),
                       pt(i + 20 + hoff, 0),
                       pt(i + 30 + hoff, Height),
                       pt(i + 10 + hoff, Height)
                    }
                mb(col(70, 255), tb)
                G.FillPolygon(tb, pts)
            Next
            G.ResetClip()

            mb(lc(ForeColor, 30), tb)
            G.FillRectangle(tb, rct(pw - 1, 0, 1, Height))
            G.ResetClip()
            '-----------------------------------------------------------------------------------------------------------------------

            mp(col(100, 255), tp)
            If Not Value = 100 Then
                G.DrawLine(tp, 1, 1, pw - 1, 1)
            Else
                G.DrawLine(tp, 1, 1, pw - 2, 1)
            End If
        End If
        '-----------------------------------------------------------------------------------------------------------------------


        tp.Dispose()
        tb.Dispose()
    End Sub


    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        mpos = pt(e.X, e.Y)
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Value = clamp(mpos.X / Width * Maximum, 0, Maximum)
    End Sub
#End Region

End Class



















