Imports System.Data.OleDb

Public Class GamePage

    'Game Elements
    'Snake Variables
    Dim Btn(3) As Button        'Used for the Structure of the Snake
    Dim BtnCount As Integer     'Used to Current Length of the Snake
    Dim BodyPart As Button      'Used during Snake Assembly
    Dim NewBtn As Button        'Used during the Growth of the Snake
    Dim Growthlimit As Integer  'Used to limit the Snake Growth
    Dim Index As Integer        'Used as a Subscript in SnakeSkinPattern

    'Food Variables
    Dim Food As Button          'Used to Indicate Food of the Snake

    'Snake Movement Variables
    Dim Pixel As Integer = 16   'Used as a Size of BodyPart of the Snake
    Dim Direction As String     'Used to Indicate the Direction of the Snake
    Dim PermitUD As Boolean     'Used to Permit UP and DOWN Direction
    Dim PermitLR As Boolean     'Used to Permit LEFT and RIGHT Direction

    'Score Variables
    Dim Score As Integer            'Used to Indicate the Current Score
    Dim HighScore As Integer        'Used to Indicate the Highest Score
    Dim cn As New OleDbConnection   'Used to Build Connection with the Database
    Dim bs As New BindingSource     'Used to Bind the Score with the Database

    'Game Over Elements
    Dim Star(2) As PictureBox       'Used as a Collection of the Stars
    Dim StarCount As Integer = 0    'Used to Store Count of the Stars
    Dim ScorePercent As Integer     'Used as Score relative to HighScore
    Dim Subscript As Integer = 0
    Dim Once As Integer             'Used to
    Dim Selection2 As Integer = 1

    Dim Sc As Integer = 0

    'Pause Menu Variables
    Dim Mode As String = "Resume"               '
    Dim Label(3) As Label                       '
    Dim LabelCount As Integer = 0               '
    Dim Selection As Integer = 1                '
    Private showRectangle As Boolean = False    '
    Private op As Single = 0.3F                 '



    'Entry Point of MenuPage
    Private Sub GamePage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        ByteCore.FormRadius(Me, 55)
        GameEngine(4, True, True, 1, 1, 0, 0)
    End Sub

    'Setups a GamePage
    Sub GameEngine(ByVal BCount As Integer, ByVal PerUD As Boolean, ByVal PerLR As Boolean, ByVal One As Integer, ByVal Growlimit As Integer, ByVal Sc As Integer, ByVal HScore As Integer)
        BtnCount = BCount - 1
        PermitUD = PerUD
        PermitLR = PerLR
        Once = One
        Growthlimit = Growlimit
        Score = Sc
        HighScore = HScore
        GameUI()
        PauseUI()
        FetchHighScoreRecord()
        Timer1.Interval = 100
        Timer1.Start()
    End Sub

    Sub GameUI()
        Button1.FlatAppearance.BorderSize = 0
        Btn(0) = Button1
        SnakeBodyAssemble()
        Food = Button2
        Food.FlatAppearance.BorderSize = 0
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        SnakeSuicide()
        SnakeTeleport()
        FoodCollid()
        SnakeDirect()
        FoodCollid()
        SnakeSuicide()
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Timer2.Stop()
        FoodSpawn()
        Growthlimit = 1
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        If Index <= BtnCount Then
            SnakeSkinPattern(Index)
            Index += 1
        Else
            Timer3.Stop()
        End If
    End Sub

    Sub SnakeBodyAssemble()
        ReDim Preserve Btn(BtnCount)
        For i As Integer = 1 To BtnCount
            BodyPart = New Button
            BodyPart.Name = "Button" & BtnCount
            BodyPart.Size = New Size(Pixel, Pixel)
            BodyPart.FlatStyle = FlatStyle.Flat
            BodyPart.FlatAppearance.BorderSize = 0
            BodyPart.Enabled = False
            BodyPart.Location = New Point(Btn(i - 1).Location.X - Pixel, Btn(i - 1).Location.Y)
            Btn(i) = BodyPart
            SnakeSkinPattern(i)
            Me.Controls.Add(BodyPart)
            Label1.SendToBack()
            Label2.SendToBack()
            Label3.SendToBack()
            Label4.SendToBack()
        Next
    End Sub

    Sub SnakeSuicide()
        For i As Integer = 3 To BtnCount
            If Btn(0).Bounds.IntersectsWith(Btn(i).Bounds) Then
                Timer1.Stop()
                If Once = 1 Then
                    Once += 1
                    PermitLR = False
                    PermitUD = False
                    GameOverUI()
                End If
            End If
        Next
    End Sub

    Sub SnakeTeleport()
        For i As Integer = 0 To BtnCount
            If Btn(i).Location.X < -16 Then
                Btn(i).Location = New Point(Me.ClientSize.Width, Btn(i).Location.Y)
            ElseIf Btn(0).Location.Y < -16 Then
                Btn(i).Location = New Point(Btn(i).Location.X, Me.ClientSize.Height)
            ElseIf Btn(i).Location.X > Me.ClientSize.Width Then
                Btn(i).Location = New Point(1, Btn(i).Location.Y)
            ElseIf Btn(i).Location.Y > Me.ClientSize.Height Then
                Btn(i).Location = New Point(Btn(i).Location.X, 1)
            End If
        Next
    End Sub

    Sub SnakeDirect()
        If Direction = "UP" Then
            BtnFollow()
            SnakeCrawl(0, -1)
        ElseIf Direction = "DOWN" Then
            BtnFollow()
            SnakeCrawl(0, 1)
        ElseIf Direction = "LEFT" Then
            BtnFollow()
            SnakeCrawl(-1, 0)
        ElseIf Direction = "RIGHT" Then
            BtnFollow()
            SnakeCrawl(1, 0)
        End If
    End Sub

    Sub SnakeCrawl(ByVal a As Integer, ByVal b As Integer)
        For i As Integer = 1 To Pixel
            Btn(0).Location = New Point(Btn(0).Location.X + a, Btn(0).Location.Y + b)
            SnakeSuicide()
            FoodCollid()
        Next
    End Sub

    Sub BtnFollow()
        Dim i As Integer
        For i = BtnCount To 1 Step -1
            Btn(i).Location = New Point(Btn(i - 1).Location.X, Btn(i - 1).Location.Y)
        Next
    End Sub

    Sub FoodCollid()
        If Btn(0).Bounds.IntersectsWith(Food.Bounds) Then
            SnakeGrowth()
            ScoreCount()
            Food.Location = New Point(Me.ClientSize.Width + 200, Me.ClientSize.Height + 200)
            Timer2.Interval = 3000
            Timer2.Start()
        End If
    End Sub

    Sub SnakeGrowth()
        If Growthlimit = 1 Then
            BtnCount += 1
            ReDim Preserve Btn(BtnCount)
            NewBtn = New Button
            NewBtn.Name = "Button" & BtnCount
            NewBtn.Size = New Size(Pixel, Pixel)
            NewBtn.FlatStyle = FlatStyle.Flat
            NewBtn.BackColor = Color.Black
            NewBtn.FlatAppearance.BorderSize = 0
            NewBtn.Enabled = False
            NewBtn.Location = Btn(BtnCount - 1).Location
            Btn(BtnCount) = NewBtn
            SnakeSkinPattern(BtnCount)
            PatternAnimation()
            Me.Controls.Add(NewBtn)
            Label1.SendToBack()
            Label2.SendToBack()
            Label3.SendToBack()
            Label4.SendToBack()
            Growthlimit += 1
        End If
    End Sub

    Sub SnakeSkinPattern(ByVal i As Integer)
        If Score < 1 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(0, 255, 0) 'Lime
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 2 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(200, 255, 0) 'Green-Yellow
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 3 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(255, 255, 0) 'Yellow
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 4 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(250, 196, 2) 'Yellow-Orange
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 5 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(255, 128, 0) 'Orange
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 6 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(255, 69, 0) 'Orange-Red
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 7 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(255, 0, 0) 'Red
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 8 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(255, 0, 128) 'Red-Pink
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 9 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(255, 0, 234) 'Pink
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 10 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(200, 0, 255) 'Pink-Purple
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 11 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(144, 0, 255) 'Purple
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 12 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(81, 38, 252) 'Purple-Violet
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 13 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(2, 56, 250) 'Violet
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 14 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(2, 163, 250) 'Violet-Cyan(Blue)
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 15 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(2, 250, 233) 'Cyan
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 16 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(158, 255, 237) 'Cyan-White
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 17 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(255, 255, 255) 'White
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 18 Then
            If ((i + 1) Mod 2) = 0 Then
                Btn(i).BackColor = Color.FromArgb(135, 135, 135) 'Gray
            Else
                Btn(i).BackColor = Color.Black
            End If
        ElseIf Score < 19 Then
            Btn(i).BackColor = Color.Black
        Else
            Btn(i).BackColor = Color.White
        End If
    End Sub

    Sub PatternAnimation()
        Index = 0
        Timer3.Interval = 100
        Timer3.Start()
    End Sub

    Sub FoodSpawn()
        RandomLocation()
        While (Not ValidSpot())
            RandomLocation()
        End While
    End Sub

    Sub RandomLocation()
        Dim Random As Random = New Random()
        Dim RandX, RandY As Integer
        RandX = Random.Next(0, Me.ClientSize.Width - Pixel + 1)
        RandY = Random.Next(0, Me.ClientSize.Height - Pixel + 1)
        Food.Location = New Point(RandX, RandY)
    End Sub

    Function ValidSpot() As Boolean
        For i As Integer = 0 To BtnCount
            If Food.Bounds.IntersectsWith(Btn(i).Bounds) Then
                Return False
            End If
        Next
        Return True
    End Function

    Sub FetchHighScoreRecord()
        cn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\heman\OneDrive\Documents\Sumanth C#\Snake Game\SnakeByteRecord.accdb"
        Dim qs As String
        qs = "Select * From HighScoreRecord Where PlayerID = 1"
        Dim da As New OleDbDataAdapter(qs, cn)
        Dim ds As New DataSet
        da.Fill(ds, "HighScoreRecord")
        bs.DataSource = ds.Tables("HighScoreRecord")
        Label2.DataBindings.Clear()
        Label2.DataBindings.Add("Text", bs, "HighScore")
        HighScore = Val(Label2.Text)
    End Sub

    Sub ScoreCount()
        Score += 1
        If Score > HighScore Then
            Label1.Text = Score
            Label2.Text = Score
        Else
            Label1.Text = Score
        End If
    End Sub

    Sub UpdateHighScore()
        If Score > HighScore Then
            Console.WriteLine("Score Updating")
            HighScore = Score
            cn.Open()
            Dim cmd As New OleDbCommand
            cmd.Connection = cn
            Dim qs As String
            qs = "Update HighScoreRecord Set HighScore ='" & HighScore & "' Where PlayerID = 1"
            cmd.CommandText = qs
            cmd.ExecuteNonQuery()
            cn.Close()
            FetchHighScoreRecord()
        End If
    End Sub

    Sub GameOverUI()
        UpdateHighScore()
        Mode = "GameOver"
        showRectangle = Not showRectangle
        Me.Invalidate()
        Panel1.Visible = True
        Label9.ForeColor = Color.FromArgb(148, 148, 148)
        Star(0) = PictureBox1
        Star(1) = PictureBox2
        Star(2) = PictureBox3
        ScorePercent = Score / HighScore * 100
        For i = 0 To 2
            Star(i).Image = ImageList1.Images(0)
        Next
        If ScorePercent >= 25 And ScorePercent <= 50 Then
            StarCount = 1
        ElseIf ScorePercent > 50 And ScorePercent <= 100 Then
            StarCount = 2
        ElseIf ScorePercent > 100 Then
            StarCount = 3
        Else
            StarCount = 0
        End If
        Timer4.Interval = 1500
        Timer4.Start()
        Timer5.Interval = 100
        Timer5.Start()
    End Sub

    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        If Subscript < StarCount Then
            FillStar(Subscript)
            Subscript += 1
        Else
            Timer4.Stop()
        End If
    End Sub

    Sub FillStar(ByVal i As Integer)
        Star(i).Image = ImageList1.Images(1)
    End Sub

    Private Sub Timer5_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer5.Tick
        Label7.Text = Sc
        Sc = Sc + 1
        If Sc > Score Then
            Timer5.Stop()
        End If
    End Sub

    Sub PauseUI()
        Panel2.Visible = False
        Label(0) = Label10
        Label(1) = Label11
        Label(2) = Label12
        Label(0).ForeColor = Color.White
        Label(1).ForeColor = Color.FromArgb(148, 148, 148)
        Label(2).ForeColor = Color.FromArgb(148, 148, 148)
    End Sub

    Sub PauseMenuStructure()
        Panel2.Visible = Not Panel2.Visible
    End Sub

    Sub ResumeGame()
        Timer1.Start()
        Timer3.Start()
        Mode = "Resume"
        showRectangle = Not showRectangle
        Me.Invalidate()
        PauseMenuStructure()
    End Sub

    Sub RestartGame()
        Dim MenuWindow As New MenuPage
        Dim GameWindow As New GamePage
        UpdateHighScore()
        Mode = "Resume"
        showRectangle = Not showRectangle
        Me.Invalidate()
        Me.Close()
        GameWindow.Show()
    End Sub

    Sub ExitGame()
        Dim MenuWindow As New MenuPage
        UpdateHighScore()
        Me.Close()
        ByteCore.Loading("MenuPage")
        LoadingPage.Show()
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If Mode = "Resume" Then
            Select Case e.KeyCode
                Case Keys.O
                    Timer1.Stop()
                    Mode = "Pause"
                    showRectangle = Not showRectangle
                    Me.Invalidate()
                    PauseMenuStructure()
                Case Keys.Up
                    If PermitUD Then
                        Direction = "UP"
                        BtnFollow()
                        SnakeCrawl(0, -1)
                        PermitUD = False
                        PermitLR = True
                    End If
                Case Keys.Down
                    If PermitUD Then
                        Direction = "DOWN"
                        BtnFollow()
                        SnakeCrawl(0, 1)
                        PermitUD = False
                        PermitLR = True
                    End If
                Case Keys.Left
                    If PermitLR Then
                        Direction = "LEFT"
                        BtnFollow()
                        SnakeCrawl(-1, 0)
                        PermitUD = True
                        PermitLR = False
                    End If
                Case Keys.Right
                    If PermitLR Then
                        Direction = "RIGHT"
                        BtnFollow()
                        SnakeCrawl(1, 0)
                        PermitUD = True
                        PermitLR = False
                    End If
            End Select
        ElseIf Mode = "Pause" Then
            Select Case e.KeyCode
                Case Keys.Up
                    If LabelCount = 0 Then
                        Selection = 1
                        PictureBox6.Location = New Point(242, 117)
                        Label(LabelCount).ForeColor = Color.White
                        Label(LabelCount + 1).ForeColor = Color.FromArgb(148, 148, 148)
                        Label(LabelCount + 2).ForeColor = Color.FromArgb(148, 148, 148)
                    ElseIf LabelCount = 1 Then
                        Selection = 1
                        LabelCount -= 1
                        PictureBox6.Location = New Point(242, 117)
                        Label(LabelCount).ForeColor = Color.White
                        Label(LabelCount + 1).ForeColor = Color.FromArgb(148, 148, 148)
                        Label(LabelCount + 2).ForeColor = Color.FromArgb(148, 148, 148)
                    ElseIf LabelCount = 2 Then
                        Selection = 2
                        LabelCount -= 1
                        PictureBox6.Location = New Point(237, 167)
                        Label(LabelCount).ForeColor = Color.White
                        Label(LabelCount - 1).ForeColor = Color.FromArgb(148, 148, 148)
                        Label(LabelCount + 1).ForeColor = Color.FromArgb(148, 148, 148)
                    Else
                        Exit Sub
                    End If
                Case Keys.Down
                    If LabelCount = 2 Then
                        Selection = 3
                        PictureBox6.Location = New Point(254, 218)
                        Label(LabelCount).ForeColor = Color.White
                        Label(LabelCount - 1).ForeColor = Color.FromArgb(148, 148, 148)
                        Label(LabelCount - 2).ForeColor = Color.FromArgb(148, 148, 148)
                    ElseIf LabelCount = 1 Then
                        Selection = 3
                        LabelCount += 1
                        PictureBox6.Location = New Point(254, 218)
                        Label(LabelCount).ForeColor = Color.White
                        Label(LabelCount - 1).ForeColor = Color.FromArgb(148, 148, 148)
                        Label(LabelCount - 2).ForeColor = Color.FromArgb(148, 148, 148)
                    ElseIf LabelCount = 0 Then
                        Selection = 2
                        LabelCount += 1
                        PictureBox6.Location = New Point(237, 167)
                        Label(LabelCount).ForeColor = Color.White
                        Label(LabelCount - 1).ForeColor = Color.FromArgb(148, 148, 148)
                        Label(LabelCount + 1).ForeColor = Color.FromArgb(148, 148, 148)
                    Else
                        Exit Sub
                    End If
                Case Keys.Enter
                    If Selection = 1 Then
                        ResumeGame()
                    ElseIf Selection = 2 Then
                        RestartGame()
                    ElseIf Selection = 3 Then
                        ExitGame()
                    End If
                    Selection = 1
                Case Keys.Escape
                    End
            End Select
        ElseIf Mode = "GameOver" Then
            Select Case e.KeyCode
                Case Keys.Left
                    If Selection2 = 1 Then
                        Selection2 = 1
                        PictureBox5.Location = New Point(142, 278)
                        Label8.ForeColor = Color.White
                        Label9.ForeColor = Color.FromArgb(148, 148, 148)
                    ElseIf Selection2 = 2 Then
                        Selection2 -= 1
                        PictureBox5.Location = New Point(142, 278)
                        Label8.ForeColor = Color.White
                        Label9.ForeColor = Color.FromArgb(148, 148, 148)
                    End If
                Case Keys.Right
                    If Selection2 = 1 Then
                        Selection2 += 1
                        PictureBox5.Location = New Point(364, 278)
                        Label8.ForeColor = Color.FromArgb(148, 148, 148)
                        Label9.ForeColor = Color.White
                    ElseIf Selection2 = 2 Then
                        Selection2 = 2
                        PictureBox5.Location = New Point(364, 278)
                        Label8.ForeColor = Color.FromArgb(148, 148, 148)
                        Label9.ForeColor = Color.White
                    End If
                Case Keys.Enter
                    If Selection2 = 1 Then
                        RestartGame()
                    ElseIf Selection2 = 2 Then
                        ExitGame()
                    End If
            End Select
        End If
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Check if the rectangle should be shown
        If showRectangle Then
            ' Create a semi-transparent FromArgb(148,148,148) color (30% opacity)
            Dim transparentColor As Color = Color.FromArgb(CInt(op * 255), Color.Black)

            ' Create a solid brush with the transparent FromArgb(148,148,148) color
            Using brush As New SolidBrush(transparentColor)
                ' Draw a full-size FromArgb(148,148,148) rectangle with the specified opacity
                e.Graphics.FillRectangle(brush, 0, 0, Me.Width, Me.Height)
            End Using
        End If
    End Sub

End Class