Imports System.Runtime.InteropServices

Public Class Bird

    'Some Windows API to enumerate open windows and to detect bird colision with these windows
    Public Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwnd As IntPtr, ByVal lpString As System.Text.StringBuilder, ByVal cch As Integer) As Integer
    Private Declare Function IsWindowVisible Lib "user32" (ByVal hWnd As IntPtr) As Boolean
    Private Declare Function EnumWindows Lib "user32.dll" (ByVal lpfn As EnumWindowsDelegate, ByVal lParam As Integer) As Boolean
    Private Delegate Function EnumWindowsDelegate(ByVal hWnd As System.IntPtr, ByVal parametro As Integer) As Boolean
    Private Declare Function GetTopWindow Lib "user32.dll" (ByVal hWnd As Long) As Long
    Private Declare Function GetWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal uCmd As UInteger) As IntPtr
    'Private Declare Function GetWindowRect Lib "user32" Alias "GetWindowRect" (ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean    'Replaced with GetWindowRectangle()
    Public Declare Function DwmGetWindowAttribute Lib "dwmapi.dll" (ByVal hwnd As IntPtr, ByVal dwAttribute As Integer, <Out> ByRef pvAttribute As RECT, ByVal cbAttribute As Integer) As Integer

    'Better than GetWindowRect() because not messes with windows shadows
    Public Shared Function GetWindowRectangle(ByVal hWnd As IntPtr) As RECT
        Dim rect As RECT
        Dim size As Integer = Marshal.SizeOf(GetType(RECT))
        DwmGetWindowAttribute(hWnd, 9, rect, size)
        Return rect
    End Function

    'Structure to define a rectangle limits
    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    'All the bird animations, each one in a diferent bitmap array
    Dim FlyLeft As Bitmap() = New Bitmap(3) {My.Resources.fly1, My.Resources.fly2, My.Resources.fly3, My.Resources.fly2}
    Dim FlyRight As Bitmap() = New Bitmap(3) {My.Resources.fly4, My.Resources.fly5, My.Resources.fly6, My.Resources.fly5}
    'Dim FlyUpLeft As Bitmap() = New Bitmap(3) {My.Resources.fly_up_Left1, My.Resources.fly_up_Left2, My.Resources.fly_up_Left3, My.Resources.fly_up_Left2}
    'Dim FlyUpRight As Bitmap() = New Bitmap(3) {My.Resources.fly4, My.Resources.fly5, My.Resources.fly6, My.Resources.fly5}
    Dim FlyStopLeft As Bitmap() = New Bitmap(3) {My.Resources.Breaking_left1, My.Resources.Breaking_left2, My.Resources.Breaking_left3, My.Resources.Breaking_left2}
    Dim FlyStopRight As Bitmap() = New Bitmap(3) {My.Resources.Breaking_Right1, My.Resources.Breaking_Right2, My.Resources.Breaking_Right3, My.Resources.Breaking_Right2}
    Dim FlyWithFishLeft As Bitmap() = New Bitmap(3) {My.Resources.FlyWithFish_Left1, My.Resources.FlyWithFish_Left2, My.Resources.FlyWithFish_Left3, My.Resources.FlyWithFish_Left2}
    Dim FlyWithFishRight As Bitmap() = New Bitmap(3) {My.Resources.FlyWithFish_Right1, My.Resources.FlyWithFish_Right2, My.Resources.FlyWithFish_Right3, My.Resources.FlyWithFish_Right2}
    Dim FlyWithFishStopLeft As Bitmap() = New Bitmap(3) {My.Resources.Breaking_left_wf1, My.Resources.Breaking_left_wf2, My.Resources.Breaking_left_wf3, My.Resources.Breaking_left_wf2}
    Dim FlyWithFishStopRight As Bitmap() = New Bitmap(3) {My.Resources.Breaking_Right_wf1, My.Resources.Breaking_Right_wf2, My.Resources.Breaking_Right_wf3, My.Resources.Breaking_Right_wf2}
    Dim Looking_Down_Left As Bitmap() = New Bitmap(3) {My.Resources.Looking_Down_Left1, My.Resources.Looking_Down_Left2, My.Resources.Looking_Down_Left3, My.Resources.Looking_Down_Left2}
    Dim Looking_Down_Right As Bitmap() = New Bitmap(3) {My.Resources.Looking_Down_Right1, My.Resources.Looking_Down_Right2, My.Resources.Looking_Down_Right3, My.Resources.Looking_Down_Right2}
    Dim FallLeft As Bitmap() = New Bitmap(2) {My.Resources.fall_left1, My.Resources.fall_left2, My.Resources.fall_left3}
    Dim FallRight As Bitmap() = New Bitmap(2) {My.Resources.fall_right1, My.Resources.fall_right2, My.Resources.fall_right3}
    Dim SplashLeft As Bitmap() = New Bitmap(2) {My.Resources.splash_Left1, My.Resources.splash_Left2, My.Resources.splash_Left3}
    Dim SplashRight As Bitmap() = New Bitmap(2) {My.Resources.splash_Right1, My.Resources.splash_Right2, My.Resources.splash_Right3}
    Dim EatingFishLeft As Bitmap() = New Bitmap(14) {My.Resources.Eat_Fish_Left1, My.Resources.Eat_Fish_Left2, My.Resources.Eat_Fish_Left3, My.Resources.Eat_Fish_Left4, My.Resources.Eat_Fish_Left3, My.Resources.Eat_Fish_Left4, My.Resources.Eat_Fish_Left3, My.Resources.Eat_Fish_Left4, My.Resources.Eat_Fish_Left5, My.Resources.Eat_Fish_Left6, My.Resources.Eat_Fish_Left5, My.Resources.Eat_Fish_Left7, My.Resources.Eat_Fish_Left8, My.Resources.Eat_Fish_Left9, My.Resources.Eat_Fish_Left10}
    Dim EatingFishRight As Bitmap() = New Bitmap(14) {My.Resources.Eat_Fish_Right1, My.Resources.Eat_Fish_Right2, My.Resources.Eat_Fish_Right3, My.Resources.Eat_Fish_Right4, My.Resources.Eat_Fish_Right3, My.Resources.Eat_Fish_Right4, My.Resources.Eat_Fish_Right3, My.Resources.Eat_Fish_Right4, My.Resources.Eat_Fish_Right5, My.Resources.Eat_Fish_Right6, My.Resources.Eat_Fish_Right5, My.Resources.Eat_Fish_Right7, My.Resources.Eat_Fish_Right8, My.Resources.Eat_Fish_Right9, My.Resources.Eat_Fish_Right10}
    Dim Chirp As Bitmap() = New Bitmap(4) {My.Resources.Chirp1, My.Resources.Chirp2, My.Resources.Chirp3, My.Resources.Chirp4, My.Resources.Chirp5}

    Dim Frame As Integer = 0

    'Variables to handle bird mouse drag
    Dim IsDragging As Boolean = False
    Dim MouseX As Integer
    Dim MouseY As Integer

    'Variables for random screen point generation
    Dim ScreenPointX As Double
    Dim ScreenPointY As Double
    Dim R As New Random
    Dim myRect As RECT

    'Variables for moving bird on desktop
    Dim OldLocation As Point
    Dim NewLocation As Point
    Dim Recta As Rectangle
    Dim BirdSpeed As Single
    Dim Count As Integer = 0
    Dim CountTrigger As Integer
    Dim FlightDirection As String = ""
    Dim ChirpPercentage As Integer = 100

    'Bird animation triggers
    Dim Is_Flying As Boolean = True
    Dim Is_Flying_WithFish As Boolean = False
    Dim Is_LookingDown As Boolean = False
    Dim Is_Falling As Boolean = False
    Dim Is_FallingToTaskBar As Boolean = False
    Dim Is_Splashing As Boolean = False
    Dim Is_EatingFish_OnWindow As Boolean = False
    Dim Is_EatingFish_OnTaskbar As Boolean = False
    Dim Is_Chirping As Boolean = False
    Public Is_Dead As Boolean = False

    'Enable to view debug tooltip and screen points
    Public ShowDebug As Boolean = False
    Public PlaySounds As Boolean = True

    'List of open and visible windows
    Dim OpenWindows As New System.Collections.Specialized.StringDictionary

    'SOME USEFUL GRAPHIC TRICKS HERE  ;)
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000      'NO_IDEA            <- Unsure about this shit, disabled.
            cp.ExStyle = cp.ExStyle Or &H80           'WS_EX_TOOLWINDOW   <- Remove sheep(s) from ALT-TAB list.
            cp.ExStyle = cp.ExStyle Or &H8            'WS_EX_TOPMOST      <- Set sheep(s) TopMost on Z index.
            cp.ExStyle = cp.ExStyle Or &H80000        'WS_EX_LAYERED      <- Increase overall paint performance (Still not sure if it works).
            'cp.ExStyle = cp.ExStyle Or &H20          'WS_EX_TRANSPARENT  <- Do not draw window (Makes sheep unclickable, discarded).
            cp.ExStyle = cp.ExStyle Or &H8000000      'WS_EX_NOACTIVATE   <- prevent focus when created (Not sure if it's nedded).
            cp.Style = cp.Style Or &H80000000         'WS_POPUP           <- Unsure about this shit, disabled.
            cp.ClassStyle = cp.ClassStyle Or &H20000  'CS_DROPSHADOW      < -Cancel Drop shadow
            Return cp
        End Get
    End Property

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Public Shared Function GetRandomBool(ByVal R As Random, ByVal Optional TruePercentage As Integer = 50) As Boolean
        Return R.NextDouble() < TruePercentage / 100.0
    End Function

    Private Sub GenerateNewRandomScreenPoint()
        ScreenPointX = GetRandom(50, Screen.PrimaryScreen.WorkingArea.Width - Me.Width)
        ScreenPointY = GetRandom(50, Screen.PrimaryScreen.WorkingArea.Height - Me.Height)
    End Sub

    Private Sub StartMovingToNextScreenPoint()
        OldLocation = Me.Location
        NewLocation = New Point(ScreenPointX, ScreenPointY)
        Recta = New Rectangle(New Point(NewLocation.X - CInt(Math.Ceiling(CDec(BirdSpeed))), NewLocation.Y - CInt(Math.Ceiling(CDec(BirdSpeed)))), New Size(CInt(Math.Ceiling(CDec(BirdSpeed * 2))), CInt(Math.Ceiling(CDec(BirdSpeed * 2)))))
    End Sub

    Private Function EnumerateOpenWindows(ByVal hWnd As System.IntPtr, ByVal parametro As Integer) As Boolean
        Dim Titulo As New System.Text.StringBuilder(New String(" "c, 256))
        Dim ret As Integer = GetWindowText(hWnd, Titulo, Titulo.Length)
        Dim nombreVentana As String = Titulo.ToString.Substring(0, ret)
        If Not hWnd = IntPtr.Zero AndAlso IsWindowVisible(hWnd) AndAlso nombreVentana <> Nothing AndAlso nombreVentana.Length > 0 Then
            OpenWindows.Add(hWnd, nombreVentana)
        End If
        Return True
    End Function

    Private Function IAmInTheTopBorder(ByVal hwndWindow As IntPtr) As Boolean
        If Not hwndWindow = IntPtr.Zero Then
            Dim Ventana As RECT = GetWindowRectangle(hwndWindow)
            Dim NextWindowHandle As IntPtr = GetTopWindow(IntPtr.Zero)
            While NextWindowHandle <> IntPtr.Zero
                If NextWindowHandle = hwndWindow Then
                    Return True
                End If
                Dim Bird As RECT = GetWindowRectangle(NextWindowHandle)
                If IsWindowVisible(NextWindowHandle) Then
                    If Bird.Top < Ventana.Top AndAlso Bird.Bottom > Ventana.Top Then
                        Return False
                    End If
                End If
                NextWindowHandle = GetWindow(NextWindowHandle, 2) 'Get next window
            End While
        End If
        Return False
    End Function

    Private Function CheckTopWindow() As Boolean
        For Each s As String In OpenWindows.Keys
            myRect = GetWindowRectangle(s)
            If Me.Location.Y + Me.Height < myRect.Top AndAlso
               Me.Location.Y + Me.Height + BirdSpeed >= myRect.Top AndAlso
               Me.Location.X >= myRect.Left AndAlso
               Me.Location.X <= myRect.Right AndAlso
               IAmInTheTopBorder(s) = True Then
                Return True
            End If
        Next
    End Function

    Public Sub DrawRectangle(ByVal a As Rectangle)
        a.Inflate(2, 2)
        Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
            Using lgb As New Drawing2D.LinearGradientBrush(a, Color.Red, Color.Red, 90, True)
                g.FillRectangle(lgb, a)
            End Using
        End Using

    End Sub

    Public Sub StopAllAnimations()
        Is_Flying = False
        Is_Flying_WithFish = False
        Is_LookingDown = False
        Is_Falling = False
        Is_FallingToTaskBar = False
        Is_Splashing = False
        Is_EatingFish_OnWindow = False
        Is_EatingFish_OnTaskbar = False
        Is_Chirping = False
        Count = 0
        Frame = 0
        CountTrigger = GetRandom(5, 10)
        BirdSpeed = GetRandom(6, 8)
    End Sub

    Private Sub Animation_Tick(sender As Object, e As EventArgs) Handles Animation.Tick
        Me.TopMost = True 'Ensure bird is always top most

        If Is_Flying = True Then '---------------------------------------------------------------------------------------------------------------------------------
            'Move bird one step to the target screen point 
            Dim PBL As PointF = MathHelp.GetPointToward(New PointF(OldLocation.X, OldLocation.Y), New PointF(NewLocation.X, NewLocation.Y), BirdSpeed)
            Me.Location = New Point(CInt(PBL.X), CInt(PBL.Y))
            Me.Update()

            If Me.Location.X > OldLocation.X Then           'Is bird flying to the right?
                FlightDirection = "Right"
            ElseIf Me.Location.X < OldLocation.X Then       'Is bird flying to the left?
                FlightDirection = "Left"
            End If


            If Me.Location.X < 200 And FlightDirection = "Left" Then
                If Frame < FlyStopLeft.Count - 1 Then
                    PictureBox1.Image = FlyStopLeft(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FlyStopLeft(Frame)
                    Frame = 0
                End If
            ElseIf Me.Location.X > Screen.PrimaryScreen.WorkingArea.Width - 200 And FlightDirection = "Right" Then
                If Frame < FlyStopRight.Count - 1 Then
                    PictureBox1.Image = FlyStopRight(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FlyStopRight(Frame)
                    Frame = 0
                End If
            Else
                If FlightDirection = "Right" Then
                    If Frame < FlyRight.Count - 1 Then
                        PictureBox1.Image = FlyRight(Frame)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = FlyRight(Frame)
                        Frame = 0
                    End If
                ElseIf FlightDirection = "Left" Then
                    If Frame < FlyLeft.Count - 1 Then
                        PictureBox1.Image = FlyLeft(Frame)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = FlyLeft(Frame)
                        Frame = 0
                    End If
                End If
            End If

            'Update bird location
            OldLocation = Me.Location

            OpenWindows.Clear()
            EnumWindows(AddressOf EnumerateOpenWindows, 0)
            If CheckTopWindow() = True Then 'And GetRandomBool(R, ChirpPercentage) = True Then
                StopAllAnimations()
                PlayChirp()
                Is_Chirping = True
                Me.Location = New Point(Me.Location.X, myRect.Top - Me.Height - 1)
                Exit Sub
            End If

            'If bird reached the random point on the screen, generate a new random point and fly to it
            If Recta.Contains(Me.Location) Then
                If Count >= CountTrigger Then    'If CountTrigger is reached the do fishing animation
                    StopAllAnimations()
                    Is_LookingDown = True
                    PlayChirp()
                Else
                    Count = Count + 1
                    GenerateNewRandomScreenPoint()
                    StartMovingToNextScreenPoint()
                End If
            End If




        ElseIf Is_LookingDown = True Then '---------------------------------------------------------------------------------------------------------------------------------

            Is_LookingDown = False

            If FlightDirection = "Right" Then
                ScreenPointX = Me.Location.X + 200
                If ScreenPointX >= Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 100 Then
                    ScreenPointX = Screen.PrimaryScreen.WorkingArea.Width - GetRandom(200, 300)
                End If
            ElseIf FlightDirection = "Left" Then
                ScreenPointX = Me.Location.X - 200
                If ScreenPointX <= Me.Width + 100 Then
                    ScreenPointX = Screen.PrimaryScreen.WorkingArea.Width / 2 + GetRandom(200, 300)
                End If
            End If

            ScreenPointY = Screen.PrimaryScreen.WorkingArea.Height - Me.Height
            NewLocation = New Point(ScreenPointX, ScreenPointY)
            Recta = New Rectangle(New Point(NewLocation.X - CInt(Math.Ceiling(CDec(BirdSpeed))), NewLocation.Y - CInt(Math.Ceiling(CDec(BirdSpeed)))), New Size(CInt(Math.Ceiling(CDec(BirdSpeed * 2))), CInt(Math.Ceiling(CDec(BirdSpeed * 2)))))

            'Update bird location
            OldLocation = Me.Location

            If NewLocation.X > OldLocation.X Then
                FlightDirection = "Right"
            Else
                FlightDirection = "Left"
            End If


            For i As Integer = 0 To 21
                If FlightDirection = "Right" Then
                    If Frame < Looking_Down_Right.Count - 1 Then
                        PictureBox1.Image = Looking_Down_Right(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(50)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = Looking_Down_Right(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(50)
                        Frame = 0
                    End If
                ElseIf FlightDirection = "Left" Then
                    If Frame < Looking_Down_Left.Count - 1 Then
                        PictureBox1.Image = Looking_Down_Left(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(50)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = Looking_Down_Left(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(50)
                        Frame = 0
                    End If
                End If
            Next

            StopAllAnimations()
            Is_Falling = True

        ElseIf Is_Falling = True Then '---------------------------------------------------------------------------------------------------------------------------------
            Dim PBL As PointF = MathHelp.GetPointToward(New PointF(OldLocation.X, OldLocation.Y), New PointF(NewLocation.X, NewLocation.Y), 10)
            Me.Location = New Point(CInt(PBL.X), CInt(PBL.Y))
            Me.Update()


            If Me.Location.X < OldLocation.X Then
                FlightDirection = "Left"
                If Frame < FallLeft.Count - 1 Then
                    PictureBox1.Image = FallLeft(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FallLeft(2)
                    'Frame = 0
                End If
            ElseIf Me.Location.X > OldLocation.X Then
                FlightDirection = "Right"
                If Frame < FallRight.Count - 1 Then
                    PictureBox1.Image = FallRight(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FallRight(2)
                    'Frame = 0
                End If
            End If

            OldLocation = Me.Location


            'If bird reached the random point on the screen, generate a new random point and fly to it
            If Recta.Contains(Me.Location) Then
                StopAllAnimations()
                Is_Splashing = True
            End If


        ElseIf Is_Splashing = True Then '---------------------------------------------------------------------------------------------------------------------------------
            Is_Splashing = False
            Me.Location = New Point(Me.Location.X, Screen.PrimaryScreen.WorkingArea.Height - Me.Height)
            PlaySplash()
            For i As Integer = 0 To 2
                If FlightDirection = "Right" Then
                    If Frame < SplashRight.Count - 1 Then
                        PictureBox1.Image = SplashRight(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(100)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = SplashRight(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(100)
                        Frame = 0
                    End If
                ElseIf FlightDirection = "Left" Then
                    If Frame < SplashLeft.Count - 1 Then
                        PictureBox1.Image = SplashLeft(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(100)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = SplashLeft(Frame)
                        Application.DoEvents()
                        Threading.Thread.Sleep(100)
                        Frame = 0
                    End If
                End If


            Next

            OldLocation = Me.Location

            GenerateNewRandomScreenPoint()
            StartMovingToNextScreenPoint()

            StopAllAnimations()
            Is_Flying_WithFish = True


        ElseIf Is_Flying_WithFish = True Then '---------------------------------------------------------------------------------------------------------------------------------
            Dim PBL As PointF = MathHelp.GetPointToward(New PointF(OldLocation.X, OldLocation.Y), New PointF(NewLocation.X, NewLocation.Y), BirdSpeed)
            Me.Location = New Point(CInt(PBL.X), CInt(PBL.Y))
            Me.Update()

            If Me.Location.X > OldLocation.X Then           'Is bird flying to the right?
                FlightDirection = "Right"
            ElseIf Me.Location.X < OldLocation.X Then       'Is bird flying to the left?
                FlightDirection = "Left"
                'ElseIf Me.Location.X = OldLocation.X Then       'Is bird flying vertival (up/down)?
                'Flying vertical animation
            End If


            If Me.Location.X < 200 And FlightDirection = "Left" Then
                If Frame < FlyWithFishStopLeft.Count - 1 Then
                    PictureBox1.Image = FlyWithFishStopLeft(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FlyWithFishStopLeft(Frame)
                    Frame = 0
                End If
            ElseIf Me.Location.X > Screen.PrimaryScreen.WorkingArea.Width - 200 And FlightDirection = "Right" Then
                If Frame < FlyWithFishStopRight.Count - 1 Then
                    PictureBox1.Image = FlyWithFishStopRight(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FlyWithFishStopRight(Frame)
                    Frame = 0
                End If

            Else

                'Set animation based on the bird flight direction
                If FlightDirection = "Right" Then
                    If Frame < FlyWithFishRight.Count - 1 Then
                        PictureBox1.Image = FlyWithFishRight(Frame)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = FlyWithFishRight(Frame)
                        Frame = 0
                    End If
                ElseIf FlightDirection = "Left" Then
                    If Frame < FlyWithFishLeft.Count - 1 Then
                        PictureBox1.Image = FlyWithFishLeft(Frame)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = FlyWithFishLeft(Frame)
                        Frame = 0
                    End If
                End If
            End If


            'Update bird location
            OldLocation = Me.Location


            OpenWindows.Clear()
            EnumWindows(AddressOf EnumerateOpenWindows, 0)


            If CheckTopWindow() = True Then
                StopAllAnimations()
                PlayChirp()
                Is_EatingFish_OnWindow = True
                Exit Sub
            End If





            If Recta.Contains(Me.Location) Then
                If Count >= CountTrigger Then
                    StopAllAnimations()
                    Is_FallingToTaskBar = True

                    If FlightDirection = "Right" Then
                        ScreenPointX = Me.Location.X + 200
                        If ScreenPointX >= Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 50 Then
                            ScreenPointX = Screen.PrimaryScreen.WorkingArea.Width - GetRandom(100, 200)
                        End If
                    ElseIf FlightDirection = "Left" Then
                        ScreenPointX = Me.Location.X - 200
                        If ScreenPointX <= Me.Width + 50 Then
                            ScreenPointX = Screen.PrimaryScreen.WorkingArea.Width / 2 + GetRandom(100, 200)
                        End If
                    End If
                    ScreenPointY = Screen.PrimaryScreen.WorkingArea.Height - Me.Height
                    NewLocation = New Point(ScreenPointX, ScreenPointY)
                    Recta = New Rectangle(New Point(NewLocation.X - CInt(Math.Ceiling(CDec(BirdSpeed))), NewLocation.Y - CInt(Math.Ceiling(CDec(BirdSpeed)))), New Size(CInt(Math.Ceiling(CDec(BirdSpeed * 2))), CInt(Math.Ceiling(CDec(BirdSpeed * 2)))))

                Else
                    Count = Count + 1
                    GenerateNewRandomScreenPoint()
                    StartMovingToNextScreenPoint()
                End If
            End If



        ElseIf Is_FallingToTaskBar = True Then '---------------------------------------------------------------------------------------------------------------------------------
            Dim PBL As PointF = MathHelp.GetPointToward(New PointF(OldLocation.X, OldLocation.Y), New PointF(NewLocation.X, NewLocation.Y), BirdSpeed)
            Me.Location = New Point(CInt(PBL.X), CInt(PBL.Y))
            Me.Update()

            If Me.Location.X < OldLocation.X Then
                FlightDirection = "Left"
                If Frame < FlyWithFishLeft.Count - 1 Then
                    PictureBox1.Image = FlyWithFishLeft(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FlyWithFishLeft(Frame)
                    Frame = 0
                End If
            ElseIf Me.Location.X > OldLocation.X Then
                FlightDirection = "Right"
                If Frame < FlyWithFishRight.Count - 1 Then
                    PictureBox1.Image = FlyWithFishRight(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = FlyWithFishRight(Frame)
                    Frame = 0
                End If
            End If

            OldLocation = Me.Location


            'If bird reached the random point on the screen, generate a new random point and fly to it
            If Recta.Contains(Me.Location) Then
                StopAllAnimations()
                Is_EatingFish_OnTaskbar = True
            End If






        ElseIf Is_EatingFish_OnWindow = True Then '---------------------------------------------------------------------------------------------------------------------------------
            Is_EatingFish_OnWindow = False

            For i As Integer = 0 To 14
                If Frame < EatingFishLeft.Count - 1 Then
                    PictureBox1.Image = EatingFishLeft(Frame)
                    Application.DoEvents()
                    Threading.Thread.Sleep(150)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = EatingFishLeft(Frame)
                    Application.DoEvents()
                    Threading.Thread.Sleep(150)
                    Frame = 14
                End If

                If CheckTopWindow() = False Then
                    PlayChirp()
                    StopAllAnimations()
                    Is_Flying_WithFish = True
                    GenerateNewRandomScreenPoint()
                    StartMovingToNextScreenPoint()
                    Exit Sub
                End If
            Next



            StopAllAnimations()
            Is_Flying = True
            GenerateNewRandomScreenPoint()
            StartMovingToNextScreenPoint()







        ElseIf Is_EatingFish_OnTaskbar = True Then '---------------------------------------------------------------------------------------------------------------------------------

            Is_EatingFish_OnTaskbar = False
            Me.Location = New Point(Me.Location.X, Screen.PrimaryScreen.WorkingArea.Height - Me.Height)

            For i As Integer = 0 To 14
                If Frame < EatingFishLeft.Count - 1 Then
                    PictureBox1.Image = EatingFishLeft(Frame)
                    Application.DoEvents()
                    Threading.Thread.Sleep(150)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = EatingFishLeft(Frame)
                    Application.DoEvents()
                    Threading.Thread.Sleep(150)
                    Frame = 14
                End If
            Next

            StopAllAnimations()
            Is_Flying = True
            GenerateNewRandomScreenPoint()
            StartMovingToNextScreenPoint()
            PlayChirp()


        ElseIf Is_Chirping = True Then '---------------------------------------------------------------------------------------------------------------------------------
            'StopAllAnimations()


            Dim Anim As Integer = GetRandom(0, 5)
            Select Case Anim
                Case 0
                    PictureBox1.Image = Chirp(0)
                Case 1
                    PictureBox1.Image = Chirp(1)
                Case 2
                    PictureBox1.Image = Chirp(2)
                Case 3
                    PictureBox1.Image = Chirp(3)
                Case 4
                    PictureBox1.Image = Chirp(4)
                Case Else
                    PictureBox1.Image = Chirp(0)
            End Select
            Application.DoEvents()
            Threading.Thread.Sleep(500)


            OpenWindows.Clear()
            EnumWindows(AddressOf EnumerateOpenWindows, 0)

            If CheckTopWindow() = False Then
                StopAllAnimations()
                GenerateNewRandomScreenPoint()
                PlayChirp()
                Is_Flying = True
                Exit Sub
            End If





        ElseIf Is_Dead = True Then '---------------------------------------------------------------------------------------------------------------------------------

            Me.Location = New Point(Me.Location.X, Me.Location.Y + 12)
            ToolTip1.Show("Bye bye!", PictureBox1, -5, -10, 100000)

            If Me.Location.Y >= Screen.PrimaryScreen.WorkingArea.Height Then
                About.NotifyIcon1.Visible = False
                End
            End If

        End If


        If ShowDebug = True Then
            DrawRectangle(Recta)
            ToolTip1.Show(
                "CheckTopWindow: " & CheckTopWindow() & vbCrLf &
                "FlightDirection: " & FlightDirection & vbCrLf &
                "BirdSpeed: " & BirdSpeed.ToString & vbCrLf &
                "-----------------------------" & vbCrLf &
                "Count: " & Count.ToString & vbCrLf &
                "CountTrigger: " & CountTrigger.ToString & vbCrLf &
                "-----------------------------" & vbCrLf &
                "Is_Flying: " & Is_Flying.ToString & vbCrLf &
                "Is_Flying_WithFish: " & Is_Flying_WithFish.ToString & vbCrLf &
                "Is_LookingDown: " & Is_LookingDown.ToString & vbCrLf &
                "Is_Falling: " & Is_Falling.ToString & vbCrLf &
                "Is_FallingToDesktop: " & Is_FallingToTaskBar.ToString & vbCrLf &
                "Is_Splashing: " & Is_Splashing.ToString & vbCrLf &
                "Is_EatingFish_OnTaskbar: " & Is_EatingFish_OnTaskbar.ToString & vbCrLf &
                "Is_EatingFish_OnWindow: " & Is_EatingFish_OnWindow.ToString & vbCrLf &
                "Is_Chirping: " & Is_Chirping.ToString,
            PictureBox1, -40, -280, 100000)
        End If

    End Sub

    Private Sub Bird_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Set transparency key on form for transparent PNG images
        Me.DoubleBuffered = True
        Me.BackColor = Color.DarkSlateBlue
        Me.TransparencyKey = Color.DarkSlateBlue
        Frame = 0
        CountTrigger = GetRandom(5, 10)
        BirdSpeed = GetRandom(6, 8)
        GenerateNewRandomScreenPoint()
        StartMovingToNextScreenPoint()
        Animation.Enabled = True
    End Sub





    Private Sub PlayChirp()
        If PlaySounds = True Then
            My.Computer.Audio.Play(My.Resources.chirp, AudioPlayMode.Background)
        End If
    End Sub

    Private Sub PlaySplash()
        If PlaySounds = True Then
            My.Computer.Audio.Play(My.Resources.splash, AudioPlayMode.Background)
        End If
    End Sub

    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        If Is_Dead = False Then
            IsDragging = True
            Animation.Enabled = False
            PictureBox1.Image = My.Resources.drag
            MouseX = Windows.Forms.Cursor.Position.X - Me.Left
            MouseY = Windows.Forms.Cursor.Position.Y - Me.Top
        End If
    End Sub

    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        IsDragging = False
        Animation.Enabled = True
        GenerateNewRandomScreenPoint()
        StartMovingToNextScreenPoint()
    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        If IsDragging = True Then
            Me.Top = Windows.Forms.Cursor.Position.Y - MouseY
            Me.Left = Windows.Forms.Cursor.Position.X - MouseX
        End If
    End Sub

    Private Sub PictureBox1_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox1.MouseLeave
        IsDragging = False
        Animation.Enabled = True
    End Sub






End Class
