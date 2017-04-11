Imports System.Runtime.InteropServices

Public Class Form2
    Dim cf As controlform
#Region "others"
#Region "declare"
    Private Const BorderWidth As Integer = 20
    Private dividerht% = 3
    Dim tb As Brush, tp As Pen
    Dim hr() As Rectangle
    Dim _cbx% : Private Property cbx%
        Get
            Return _cbx
        End Get
        Set(value%)
            If Not _cbx = value Then
                _cbx = value
                draw()
            End If
        End Set
    End Property
#End Region
#Region "DWM"
    Private dwmMargins As MARGINS
    Private _marginOk As Boolean
#Region "Ctor"
    Public Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.None
        ShowInTaskbar = 1
        cf = New controlform(Size, Location)
        Me.AddOwnedForm(cf)
    End Sub

#End Region
    Protected Overloads Overrides Sub WndProc(ByRef m As Message)

        Dim result As IntPtr
        Dim dwmHandled As Integer = DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, result)

        If dwmHandled = 1 Then
            m.Result = result
            Exit Sub
        End If

        If m.Msg = WindowsMessages.WmNcCalcSize AndAlso CInt(m.WParam) = 1 Then
            Dim nccsp As NCCALCSIZE_PARAMS = Marshal.PtrToStructure(m.LParam, GetType(NCCALCSIZE_PARAMS))

            ' Adjust (shrink) the client rectangle to accommodate the border:
            nccsp.rect0.top += 0
            nccsp.rect0.bottom += 0
            nccsp.rect0.left += 0
            nccsp.rect0.right += 0

            If Not _marginOk Then
                'Set what client area would be for passing to 
                'DwmExtendIntoClientArea. Also remember that at least 
                'one of these values NEEDS TO BE > 1, else it won't work.
                dwmMargins.cyTopHeight = BorderWidth
                dwmMargins.cxLeftWidth = BorderWidth
                dwmMargins.cyBottomHeight = BorderWidth
                dwmMargins.cxRightWidth = BorderWidth
                _marginOk = True
            End If

            Marshal.StructureToPtr(nccsp, m.LParam, False)

            m.Result = IntPtr.Zero


        ElseIf m.Msg = WindowsMessages.WmNchitTest AndAlso CInt(m.Result) = 0 Then
            m.Result = HitTestNCA(m.HWnd, m.WParam, m.LParam)

        ElseIf m.Msg = WindowsMessages.WmSetFocus Then
            'gotfocus_()
            MyBase.WndProc(m)
        ElseIf m.Msg = WindowsMessages.WmKillFocus Then
            'lostfocus_()
            MyBase.WndProc(m)

        Else : MyBase.WndProc(m)
        End If
    End Sub
    Public Function HitTestNCA(ByVal hwnd As IntPtr, ByVal wparam _
                                      As IntPtr, ByVal lparam As IntPtr) As IntPtr

        Dim p As New Point(LoWord(CInt(lparam)), HiWord(CInt(lparam)))

        Dim topleft As Rectangle = RectangleToScreen(New Rectangle(0, 0, dwmMargins.cxLeftWidth, dwmMargins.cxLeftWidth))
        Dim topright As Rectangle = RectangleToScreen(New Rectangle(Width - dwmMargins.cxRightWidth, 0, dwmMargins.cxRightWidth, dwmMargins.cxRightWidth))
        Dim botleft As Rectangle = RectangleToScreen(New Rectangle(0, Height - dwmMargins.cyBottomHeight, dwmMargins.cxLeftWidth, dwmMargins.cyBottomHeight))
        Dim botright As Rectangle = RectangleToScreen(New Rectangle(Width - dwmMargins.cxRightWidth, Height - dwmMargins.cyBottomHeight, dwmMargins.cxRightWidth, dwmMargins.cyBottomHeight))
        Dim top As Rectangle = RectangleToScreen(New Rectangle(0, 0, Width, dwmMargins.cxLeftWidth))
        Dim cap As Rectangle = RectangleToScreen(New Rectangle(0, dwmMargins.cxLeftWidth, Width, dwmMargins.cyTopHeight - dwmMargins.cxLeftWidth))
        Dim left As Rectangle = RectangleToScreen(New Rectangle(0, 0, dwmMargins.cxLeftWidth, Height))
        Dim right As Rectangle = RectangleToScreen(New Rectangle(Width - dwmMargins.cxRightWidth, 0, dwmMargins.cxRightWidth, Height))
        Dim bottom As Rectangle = RectangleToScreen(New Rectangle(0, Height - dwmMargins.cyBottomHeight, Width, dwmMargins.cyBottomHeight))


        If topleft.Contains(p) Then Return New IntPtr(HTTOPLEFT)
        If topright.Contains(p) Then Return New IntPtr(HTTOPRIGHT)
        If botleft.Contains(p) Then Return New IntPtr(HTBOTTOMLEFT)
        If botright.Contains(p) Then Return New IntPtr(HTBOTTOMRIGHT)
        If top.Contains(p) Then Return New IntPtr(HTTOP)
        If cap.Contains(p) Then Return New IntPtr(HTCAPTION)
        If left.Contains(p) Then Return New IntPtr(HTLEFT)
        If right.Contains(p) Then Return New IntPtr(HTRIGHT)
        If bottom.Contains(p) Then Return New IntPtr(HTBOTTOM)

        Return New IntPtr(HTCLIENT)
    End Function

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If DesignMode Then Exit Sub
        If Me.Width - BorderWidth > e.Location.X AndAlso
                    e.Location.X > BorderWidth AndAlso e.Location.Y > BorderWidth Then
            MoveControl(Me.Handle)

        End If
        If cbx = 0 Then End
        If cbx = 1 Then Me.WindowState = FormWindowState.Minimized
        ShowInTaskbar = 1
        MyBase.OnMouseDown(e)
    End Sub
    Private Sub MoveControl(ByVal hWnd As IntPtr)
        If DesignMode Then Exit Sub
        ReleaseCapture()
        SendMessage(hWnd, WindowsMessages.WmNcLButtonDown, HTCAPTION, 0)
    End Sub
    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
        If DesignMode Then MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        If Not DesignMode Then
            FormBorderStyle = FormBorderStyle.Sizable
        Else
            FormBorderStyle = 0
        End If
    End Sub
#End Region
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cParms As CreateParams = MyBase.CreateParams
            cParms.ExStyle = cParms.ExStyle Or WindowStyles.WS_EX_LAYERED ' Or
            'WindowStyles.WS_EX_TOOLWINDOW 'Or
            ' WindowStyles.WS_DISABLED
            Return cParms

        End Get
    End Property
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        draw()
        cf.Size = Size
        cf.Location = Location
    End Sub
    Protected Overrides Sub OnMove(e As EventArgs)
        MyBase.OnMove(e)
        cf.Size = Size
        cf.Location = Location
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        cf.Size = Size
        cf.Location = Location
    End Sub

    Sub draw()
        Dim b As New Bitmap(Width, Height)
        Dim g = Graphics.FromImage(b)

        With g
            Dim ip As New Interpolation
            .Clear(col(0, 0))

            .SetClip(rct(13, 13, Width - 2 * 13, Height - 2 * 13))
            .Clear(col(175, 0))
            .ResetClip()

            .SmoothingMode = 2 ': .PixelOffsetMode = 2
            .SetClip(rct(13, 13, Width - 2 * 13, Height - 2 * 13), Drawing2D.CombineMode.Exclude)
            For i = 0 To 13
                Dim pth As Drawing2D.GraphicsPath = DM.CreateRoundRectangle(rct(-0 + i, -0 + i, Width - 2 * i + 0, Height - 2 * i + 0), 13 - i, 1, 1, 1, 1)
                mp(col(ip.GetValue(0, 200, i + 1, 14, Type.EaseIn, EasingMethods.Exponent, 1), 0), tp)
                g.DrawPath(tp, pth)
                pth.Dispose()
            Next
            .ResetClip()
            .SmoothingMode = 0
            .DrawRectangle(Pens.Black, rct(13, 13, Width - 26, Height - 26))
            Dim r() As Rectangle = {rct(13 + 6, 13 + 6, Width - 26 - 12, 21),
                                    rct(13 + 6, 13 + 6 + 22 + dividerht - 1, Width - 26 - 12, 11),
                                    rct(13 + 6, 13 + 6 + 22 + dividerht + 10 + dividerht, Width - 26 - 12, 30),
                                    rct(13 + 6, 13 + 6 + 22 + dividerht + 10 + dividerht + 30 + dividerht, Width - 26 - 12, Height - 26 - 22 - 10 - 30 - 12 - 3 * dividerht)
                                   }
            mb(col(30, 30, 32), tb)
            For i = 0 To 3
                mb(col(30, 30, 32), tb)
                .FillRectangle(tb, r(i))
                .DrawRectangle(Pens.Black, r(i))
                mb(col(35, 255), tb)
                .FillRectangle(tb, rct(r(i).X + 1, r(i).Y + 1, r(i).Width - 1, 1))
                mb(col(80, 0), tb)
                .FillRectangle(tb, rct(r(i).X + 1, r(i).Y + r(i).Height - 1, r(i).Width - 1, 1))
            Next
        End With

        Dim hc() As Color = {col(160, 255, 25, 25), col(160, 255, 255, 128), col(160, 0, 122, 204), col(0, 0)}


        g.PixelOffsetMode = 2


        If cbx = 0 Then mb(hc(0), tb) Else mb(col(128, 255), tb)
        Dim f = New Font("Marlett", 9, FontStyle.Regular)
        g.DrawString("r", f, tb, pt(Width - 20 - 16, 6 + 18))


        If cbx = 1 Then mb(hc(2), tb) Else mb(col(128, 255), tb)
        f = New Font("Marlett", 10, FontStyle.Regular)
        ' If WindowState = FormWindowState.Maximized Then
        g.DrawString("0", f, tb, pt(Width - 40 - 16, 6 + 17)) '2
        'Else
        '    g.DrawString("0", f, tb, pt(Width - 40 - 16, 6 + 18)) '1
        'End If

        'If cbx = 2 Then mb(hc(2), tb) Else mb(col(128, 255), tb)
        'g.DrawString("0", f, tb, pt(Width - 58 - 16, 6 + 17))

        Me.SetBits(b)
        b.Dispose()
    End Sub
    Public Sub SetBits(B As Bitmap)
        If Not IsHandleCreated Or DesignMode Then Exit Sub

        If Not Bitmap.IsCanonicalPixelFormat(B.PixelFormat) OrElse Not Bitmap.IsAlphaPixelFormat(B.PixelFormat) Then
            Throw New ApplicationException("The picture must be 32bit picture with alpha channel.")
        End If

        Dim oldBits As IntPtr = IntPtr.Zero
        Dim screenDC As IntPtr = win32.GetDC(IntPtr.Zero)
        Dim hBitmap As IntPtr = IntPtr.Zero
        Dim memDc As IntPtr = win32.CreateCompatibleDC(screenDC)

        Try
            Dim topLoc As New win32.Point32(Left, Top)
            Dim bitMapSize As New win32.Size32(B.Width, B.Height)
            Dim blendFunc As New win32.BLENDFUNCTION()
            Dim srcLoc As New win32.Point32(0, 0)

            hBitmap = B.GetHbitmap(Color.FromArgb(0))
            oldBits = win32.SelectObject(memDc, hBitmap)

            blendFunc.BlendOp = win32.AC_SRC_OVER
            blendFunc.SourceConstantAlpha = 255
            blendFunc.AlphaFormat = win32.AC_SRC_ALPHA
            blendFunc.BlendFlags = 0

            win32.UpdateLayeredWindow(Handle, screenDC, topLoc, bitMapSize, memDc, srcLoc,
             0, blendFunc, win32.ULW_ALPHA)
        Finally
            If hBitmap <> IntPtr.Zero Then
                win32.SelectObject(memDc, oldBits)
                win32.DeleteObject(hBitmap)
            End If
            win32.ReleaseDC(IntPtr.Zero, screenDC)
            win32.DeleteDC(memDc)
        End Try
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        With e
            If .Y > 13 + 6 And .Y < 13 + 6 + 23 Then
                If .X < Width - 13 - 6 And .X > Width - 58 - 16 - 10 Then
                    If rct(Width - 31, 0, 18, Height).Contains(pt(.X, .Y)) Then cbx = 0
                    If rct(Width - 52, 0, 20, Height).Contains(pt(.X, .Y)) Then cbx = 1
                    If rct(Width - 73, 0, 21, Height).Contains(pt(.X, .Y)) Then cbx = 2
                Else
                    cbx = -1
                End If
            Else
                cbx = -1

            End If

        End With
    End Sub





#End Region

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        cf.Show()
        Opacity = 1
    End Sub

    Private Sub Form2_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        stopTimer()
    End Sub
End Class