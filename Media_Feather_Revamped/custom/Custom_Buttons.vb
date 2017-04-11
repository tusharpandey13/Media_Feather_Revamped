Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms.Design
Class CustomButton : Inherits customControl
#Region "declare"
    Dim tg, t1g As Graphics
    Dim cx, cy As Single
    Dim ms%
    Dim bt, bt1, sb As Bitmap
    Dim t%, sx!, sy!, tx!, ty!
#End Region
#Region "ctor"
    Sub New()
        DoubleBuffered = 1
        Size = New Size(100, 50)
        BackColor = Color.Gainsboro : ForeColor = col(28)
        SetStyle(ControlStyles.ResizeRedraw, 1)
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        sx = 1
        sy = 1
    End Sub
#End Region
#Region "fn"
    Protected Overrides Sub PaintHook()
        If animating Then calc()
        G.Clear(Parent.BackColor)
        If DesignMode Then G.InterpolationMode = 7 : G.DrawImageUnscaled(returnbitmap(), 0, 0)
        G.InterpolationMode = 7
        G.DrawImageUnscaled(returnbitmap(), 0, 0)
    End Sub
    Sub calc()
        If t < 100 Then
            sx = GetValue(1, 0.97, t, 100, Type.EaseInOut, EasingMethods.Sine, 1)
            sy = GetValue(1, 0.95, t, 100, Type.EaseInOut, EasingMethods.Sine, 1)
            tx = GetValue(0, 0.015 * Width, t, 100, Type.EaseInOut, EasingMethods.Sine, 1)
            ty = GetValue(0, 0.025 * Height, t, 100, Type.EaseInOut, EasingMethods.Sine, 1)
            t += 1
        Else
            t = 0
            'sx = 1 : sy = 1
            'tx = 0 : ty = 0
            animating = False
        End If
    End Sub
#End Region


    Public Function returnbitmap() As Bitmap

        Dim db% = 0 : If rescol(BackColor) = Color.White Then db = 1 Else db = 0
        Dim go% = 20 : Dim gs% = 0
        bt = New Bitmap(Width, Height)
        bt1 = New Bitmap(Width, Height)
        tg = Graphics.FromImage(bt)
        t1g = Graphics.FromImage(bt1)
        With t1g
            .Clear(Color.Transparent)

            tg.InterpolationMode = 7
            If ms = 1 Then
                gs = 255
                go = 10
            ElseIf ms = 2 Then
                gs = 0
                go = 50
                tg.ScaleTransform(sx, sy)
                tg.TranslateTransform(tx, ty)
                'tg.ScaleTransform(0.97, 0.95)
                'tg.TranslateTransform(0.015 * Width, 0.025 * Height)
            Else
                sx = 1 : sy = 1
                tx = 0 : ty = 0
                gs = 0
                go = 20
            End If




            .PixelOffsetMode = 2
            .FillRectangle(mb(BackColor), rct(1, 1, Width - 2, Height - 3))

            .SmoothingMode = 2
            tb = New LinearGradientBrush(rct(Me), col(go, gs), col(0, 0), 45.0F)
            .FillRectangle(tb, rct(1, 1, Width - 2, Height - 3))
            tb = New LinearGradientBrush(rct(Me), col(10, 0), col(0, 0), 90.0F)
            .FillRectangle(tb, rct(1, 1, Width - 2, Height - 3))

            mp(BackColor, tp)

            'offset 1 px right due to pixeloffsetmode=2	ie. +0  =  +1
            .DrawLine(tp, 1, 1, 1, Height - 2) '								left border
            'strangely offset has reset now		ie. +0  =  +0 
            .DrawLine(tp, 1, 1, Width - 1, 1) '									top border
            .DrawLine(tp, Width, 1, Width, Height - 2) '						right border
            .DrawLine(tp, 1, Height - 1, Width - 1, Height - 1) '				bottom border
            .SmoothingMode = 3




            If db Then mp(col(70, 0), tp) Else mp(col(45, 0), tp)


            .DrawLine(tp, 1, 1, Width - 1, 1)
            .DrawLine(tp, 1, 1, 1, Height - 1)
            .DrawLine(tp, Width - 0, 1, Width - 0, Height - 1)

            .DrawLine(tp, 1, Height - 1, Width - 1, Height - 1)
            .DrawLine(tp, 1, Height - 1, Width - 1, Height - 1)

            .DrawLine(tp, 1, Height - 0, Width - 1, Height - 0)


            If db Then mp(col(50, 255), tp) Else mp(col(50, 255), tp)
            .DrawLine(tp, 1, Height - 2, Width - 1, Height - 2)

            .TextRenderingHint = 5
            .DrawString(Text, Font, mb(ForeColor), pt(Width / 2, Height / 2), CenterSF)
        End With



        tg.DrawImageUnscaled(bt1, 0, 0)


        t1g.Dispose() : tg.Dispose() : bt1.Dispose()
        Return bt
        bt.Dispose()
    End Function

#Region "mouse"
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        ms = 2
        animating = True
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        ms = 0
        animating = True
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Not ms = 2 Then ms = 1
        cx = e.X : cy = e.Y
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        ms = 0
        animating = True
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
    End Sub
#End Region







End Class

Class custom_Material_Button
    Inherits customControl
    Private x As Single = 75.0
    Private y As Single = 75.0
    Private isd As Integer = 0
    Dim total As Single
    Dim d(8) As Single
    Dim t% = 0
    Dim hc As Color = col(0, 0)
    Private w! = 0.0
    Dim iso! = 0


    Private fc As Color = col(100, 0) : <System.ComponentModel.Category("Appearance")> Public Property FloodColor As Color
        Get
            Return fc
        End Get
        Set(value As Color)
            fc = value
            Invalidate()
        End Set
    End Property

    Sub New()
        Height = 150
        Width = 150
    End Sub


    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        x = e.X
        y = e.Y
        d = {x,
             y,
             Width - x,
             Height - y,
             (x ^ 2 + y ^ 2) ^ (1 / 2),
             ((Width - x) ^ 2 + y ^ 2) ^ (1 / 2),
             (x ^ 2 + (Height - y) ^ 2) ^ (1 / 2),
             ((Width - x) ^ 2 + (Height - y) ^ 2) ^ (1 / 2)}
        Dim bigi As Byte = 0
        For i = 0 To 7
            If d(i) > d(bigi) Then bigi = i
        Next
        total = d(bigi) + 2
        isd = 1
        animating = True
    End Sub
    Protected Overrides Sub PaintHook()
        Dim gd As Boolean : If rescol(BackColor) = Color.White Then gd = True Else gd = 0

        G.SmoothingMode = 2 : G.InterpolationMode = 7 : G.TextRenderingHint = 5
        G.Clear(BackColor)
        G.SmoothingMode = 2
        If gd Then tb = mb(col(15, 255)) Else tb = mb(col(10, 0))
        G.FillRectangle(tb, rct(Me))
        G.FillRectangle(mb(retdata().colorparam(1)), rct(Me))
        G.FillEllipse(mb(retdata().colorparam(0)), retdata().rctparam(0))
        G.TextRenderingHint = 5
        G.DrawString(Me.Text, Me.Font, mb(Me.ForeColor), pt(Me.Width / 2, Me.Height / 2), CenterSF)
        G.SmoothingMode = 3

    End Sub

    Public Function retdata() As drawdata
        calc()
        Dim dd As drawdata = New drawdata
        dd.colorparam = {col(Math.Abs(150 * (1.5 - (w / (total + 0.001)))), FloodColor), hc}
        dd.rctparam = {rct(x - w, y - w, w * 2, w * 2)}
        Return dd
    End Function

    Sub calc()

        If isd = 1 Then
            If Not t >= 1000 Then
                t += 1
                w = GetValue(0, total, t, 1000, Type.SmoothStep, EasingMethods.Exponent, 0.65)
                'w = GetValue(0, total, t, 1000, Type.EaseOut, EasingMethods.Exponent, 0.65)
            Else
                t = 0
                w = 0.0
                isd = 0
                If iso = 0 Then animating = False
            End If
        End If

        If iso = 1 Then
            If t1 < 400 Then
                t1 += 1
                hc = col(GetValue(0, 25, t1, 400, Type.SmoothStep, EasingMethods.Exponent, 0.65), rescol(BackColor))
            Else
                t1 = 0 : iso = 0
                If isd = 0 Then animating = False
            End If
        ElseIf iso = 2 Then
            If t1 < 250 Then
                t1 += 1
                hc = col(GetValue(25, 0, t1, 250, Type.SmoothStep, EasingMethods.Exponent, 0.65), rescol(BackColor))
            Else
                t1 = 0 : iso = 0
                If isd = 0 Then animating = False
            End If
        End If

    End Sub

    Dim t1% = 0

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        iso = 1
        animating = True
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        If Not hc.A = 0 Then iso = 2
        animating = True
    End Sub
End Class
























'		With tg
'			.Clear(Color.Transparent)
'			.PixelOffsetMode = 2
''.SmoothingMode = 2
'			.FillRectangle(mb(BackColor), rct(1, 1, Width - 2, Height - 2))
'Dim c As Color()
'' = {BackColor, Color.Black, col(vsi, 0), col(vsi, 0)} : mp(blend(c), tp)
'			mp(BackColor, tp)
''offset 1 px right due to pixeloffsetmode=2	ie. +0  =  +1
'			.DrawLine(tp, 1, 1, 1, Height - 1) '								left border
''strangely offset has reset now		ie. +0  =  +0 
'			.DrawLine(tp, 1, 1, Width - 1, 1) '									top border
'			.DrawLine(tp, Width, 1, Width, Height - 1) '						right border
'			.DrawLine(tp, 1, Height, Width - 1, Height)	'						bottom border



'			c = {col(32 + 2 * Math.Abs(vo - 60), 255)} : mp(blend(c), tp)

'			c = {col(50, 0)} : mp(blend(c), tp)
'			.DrawLine(tp, 1, 1, Width - 1, 1)
'			.DrawLine(tp, 1, 1, 1, Height - 1)
'			.DrawLine(tp, 1, Height - 0, Width - 1, Height - 0)
'			.DrawLine(tp, Width - 0, 1, Width - 0, Height - 1)

'			.DrawLine(tp, 1, Height - 0, Width - 1, Height - 0)
'			.DrawLine(tp, 1, Height - 0, Width - 1, Height - 0)
'			.DrawLine(tp, 1, Height - 0, Width - 1, Height - 0)

'		End With









'G.Clear(Parent.BackColor)
'G.SmoothingMode = SmoothingMode.AntiAlias
'G.InterpolationMode = InterpolationMode.HighQualityBicubic
'G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit


'Select Case State
'	Case MouseState.None
'              LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 2), gtn, GBN, 90.0F)
'              'LGB = New SolidBrush(GBN)
'          Case MouseState.Over
'              LGB = New SolidBrush(GBO)
'              'LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 2), GTO, GBO, 90.0F)
'          Case Else
'              LGB = New SolidBrush(GBD)
'              'LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 2), GTD, GBD, 90.0F)
'      End Select


'If Not Enabled Then
'	LGB = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), gtn, GBN, 90.0F)
'End If
'Dim buttonpath As GraphicsPath = CreateRound(Rectangle.Round(New Rectangle(0, 0, Width - 1, Height - 1)), 3)
''Dim buttonpath As GraphicsPath = createround(Rectangle.Round(LGB.Rectangle), 3)
'Dim buttonpath2 As GraphicsPath = CreateRound(Rectangle.Round(New Rectangle(1, 1, Width - 3, Height - 4)), 3)
'Dim bog = New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(120, 120, 120), Color.FromArgb(60, 60, 60), 90.0F)


'G.FillPath(bog, buttonpath)
''G.FillPath(LGB, createround(Rectangle.Round(LGB.Rectangle), 3))
'G.FillPath(LGB, CreateRound(Rectangle.Round(New Rectangle(0, 0, Width - 1, Height - 2)), 3))
'If Not Enabled Then G.FillPath(New SolidBrush(Color.FromArgb(50, Color.White)), CreateRound(Rectangle.Round(New Rectangle(0, 0, Width - 1, Height - 2)), 3))

'G.SetClip(buttonpath)

'LGB = New LinearGradientBrush(New Rectangle(0, 0, Width, Height / 6), Color.FromArgb(80, Color.White), Color.Transparent, 90.0F)
''G.FillRectangle(LGB, Rectangle.Round(LGB.Rectangle))
'G.ResetClip()


''G.DrawPath(New Pen(Bo), buttonpath)
''G.DrawPath(New Pen(Parent.BackColor), buttonpath2)


'If Enabled Then
'	Select Case State
'		Case MouseState.None
'			DrawText(New SolidBrush(TN), HorizontalAlignment.Center, 1, 0)
'		Case Else
'			DrawText(New SolidBrush(TDO), HorizontalAlignment.Center, 1, 0)
'	End Select
'Else
'	DrawText(New SolidBrush(TD), HorizontalAlignment.Center, 1, 0)
'End If