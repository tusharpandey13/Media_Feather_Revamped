
Class CustomTab : Inherits TabControl : Implements AnimatedObject
#Region "DECLARE"
    Private bb As Bitmap = New Bitmap(1, 1)
    Private _forecolor As Color = col(255, 82, 82)
    Private _backcolor As Color = Color.White
    Private bo, bn As Bitmap
    Private tabchanged As Boolean = 0
    Private int As New Interpolation
    Private oi%, ni%
    Public Property HighlightColor As Color
        Get
            Return _forecolor
        End Get
        Set(value As Color)
            _forecolor = value
            refreshbitmap()
            For Each p As TabPage In TabPages
                p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
            Next
        End Set
    End Property
    Public Property BackgroundColor As Color
        Get
            Return _backcolor
        End Get
        Set(value As Color)
            _backcolor = value
            refreshbitmap()
            For Each p As TabPage In TabPages
                p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
            Next
        End Set
    End Property

    Public ReadOnly Property designing As Boolean Implements AnimatedObject.designing
        Get
            Return designing
        End Get
    End Property

    Public Property animating As Boolean Implements AnimatedObject.animating

#End Region
#Region "CTOR"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        oi = 0 : ni = 0
        For Each p As TabPage In TabPages
            p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
        Next
        ItemSize = New Size(25, 30)
        bo = New Bitmap(Width, Height) : bn = bo
        addAnimatedobject(Me)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
        For Each p As TabPage In TabPages
            p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
        Next
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        bo = New Bitmap(Width, Height) : bn = bo
        refreshbitmap()
    End Sub
#End Region
    Sub refreshbitmap()
        bb = New Bitmap(Width, Height)
        Dim bg = Graphics.FromImage(bb)
        bg.Clear(col(0, 0))
        bg.SetClip(rct(0, 1, Width, Height))
        bg.DrawImage(DrawRoundRectangle1(rct(Me), 5, 4, _backcolor, col(100, 0), -0.5), 0, 0, Width, Height)
        bg.FillRectangle(mb(blendcol(_backcolor, rescol(_backcolor), 0.95)), 1, 34, Width - 3, Height - 4 - 34)
        bg.FillRectangle(mb(_backcolor), rct(1, 1, Width - 3, 34))
        bg.DrawLine(mp(col(60, 0)), 1, 33, Width - 3, 33)
        'bg.DrawLine(mp(col(30, 255)), 1, 33, Width - 3, 33)
        'bg.FillRectangle(mb(col(20, 255)), 0, 0, Width - 3, 33)
        bg.DrawLine(mp(col(60, 0)), 1, 1, Width - 3, 1)
        bg.Dispose()
        Invalidate()
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        'calc()

        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(Parent.BackColor)

        G.DrawImageUnscaled(bb, 0, 0)
        G.FillRectangle(mb(col(255, 82, 82)), rct(GetTabRect(SelectedIndex), -1, 29, 0, -27))

        For i = 0 To TabCount - 1
            Dim x2 As Rectangle = New Rectangle(GetTabRect(i).X, GetTabRect(i).Y + 2, GetTabRect(i).Width + 2, GetTabRect(i).Height - 1)
            If Not i = SelectedIndex Then
                G.DrawString(TabPages(i).Text, Font, mb(col(rescol(_backcolor), 0.7)), x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            Else
                G.DrawString(TabPages(i).Text, Font, mb(invert(_forecolor)), x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            End If
        Next

        'If animating Then
        '    G.DrawImage(bo, x, SelectedTab.Top)
        '    G.DrawImage(bn, x + (-1) * sign * Width, SelectedTab.Top)
        'End If

        'e.Graphics.DrawImage(B.Clone, 0, 0)
        'If tabchanged = 0 Then
        '    Graphics.FromImage(bo).DrawImage(copygraphics(Me, SelectedTab.DisplayRectangle), 0, 0)
        'End If
        G.Dispose() : B.Dispose()
    End Sub

    'Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
    '	MyBase.OnSelectedIndexChanged(e)
    '	ni = SelectedIndex
    '	'If Not ni = oi Then
    '	If ni > oi Then sign = -1 Else sign = +1

    '		tabchanged = 1

    '		Graphics.FromImage(bn).DrawImage(copygraphics(Me, SelectedTab.DisplayRectangle), 0, 0)
    '		SelectedTab.Hide()
    '		animating = 1
    '	'End If
    'End Sub
    Private x!, t%, sign%
    Sub calc()
        'If Not animating Then Return

        'If t < 100 Then
        '    x = int.GetValue(0, Width * sign, t, 100, Type.SmoothStep, 1)
        '    t += 1
        'Else
        '    animating = 0
        '    oi = SelectedIndex
        '    x = 0
        '    SelectedTab.Show()
        '    Graphics.FromImage(bo).DrawImage(copygraphics(Me, SelectedTab.DisplayRectangle), 0, 0)
        'End If
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        'animatedobjects.Remove(Me)
    End Sub
    Public Sub leavemouse(e As EventArgs) Implements AnimatedObject.leavemouse
        OnMouseLeave(e)
    End Sub

    Private Sub custom_invalidate() Implements AnimatedObject.invalidate
        Invalidate()
    End Sub
End Class

