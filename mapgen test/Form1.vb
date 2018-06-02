Public Class mapGen

    Private Const xMapSize As Integer = 20
    Private Const yMapSize As Integer = 20
    Private Const deathLimit As Integer = 3
    Private Const birthLimit As Integer = 4
    Private Const starvationLimit As Integer = 3
    Private Const overPopLimit As Integer = 5
    Private Const numberOfSteps As Integer = 6
    Private Const intSize As Integer = 25

    Private blnMap(xMapSize, yMapSize) As Boolean
    Private picTiles(xMapSize, yMapSize) As PictureBox
    Private Player As PictureBox = New PictureBox

    Public Structure Grav
        Public Const intTermVel As Integer = 15
        Public Const dblGravFactor As Integer = 1
    End Structure

    Private backPanel As CustomPanel = New CustomPanel
    Private xO As Integer = 0
    Private yO As Integer = 0
    Private Sub mapGen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Randomize()
        LoadingScreen.Show()

        'Set the form size
        Me.ClientSize = New Size(500, 500)
        backPanel.Size = Me.ClientSize
        backPanel.SendToBack()
        backPanel.Visible = True

        Call Fill(xMapSize * yMapSize, 0.35, blnMap) 'Fill the screen with 50% white tiles
        For i = 0 To numberOfSteps - 1
            blnMap = doSimulationStep(blnMap)
        Next
        Call createTiles(picTiles)

        For y = 0 To yMapSize
            For x = 0 To xMapSize
                With picTiles(x, y)
                    If blnMap(x, y) = True Then
                        .Name = "Tile"
                        .BackColor = Color.White
                        .Visible = True
                        .BringToFront()
                    Else
                        'THIS NEEDS TO BE FULLY DELETED SOMEHOW
                        .Name = "Empty"
                        .Dispose()
                    End If
                End With
            Next
        Next

        Me.BackgroundImage = GetTile(My.Resources.tiles_turf1, 5, 0)
        Me.BackgroundImageLayout = ImageLayout.Tile

        For y = 0 To yMapSize
            For x = 0 To xMapSize
                If picTiles(x, y).Name <> "Tile" Then
                    With Player
                        'Setting properties
                        .Size = New Size(intSize, intSize)
                        .Visible = True
                        .Name = "Player"
                        .BackColor = Color.Red
                        .Tag = "0,0"
                        .BringToFront()
                        .Location = picTiles(x, y).Location
                    End With
                    Me.Controls.Add(Player)
                    Exit For
                    Exit For
                End If
            Next
        Next
        Debugger.Show()
        Dim tmrGravity As Timer
        tmrGravity = New Timer
        With tmrGravity
            .Enabled = True
            .Interval = 1
        End With
        AddHandler tmrGravity.Tick, AddressOf tmrGravity_Tick
    End Sub

    Private Sub tmrGravity_Tick(ByVal sender As Object, ByVal e As EventArgs)

        Dim dblVelocity As Double
        With Player

            Dim xvel = Int(Split(Player.Tag, ",")(0))
            Dim yvel = Int(Split(Player.Tag, ",")(1))

            Dim x = Player.Location.X
            Dim y = Player.Location.Y
            Dim i As Integer


            yvel += Grav.dblGravFactor 'increase gravity by the factor
            dblVelocity = Clamp(dblVelocity, -Grav.intTermVel, Grav.intTermVel) 'clamp the velocity to a terminal velocity            

            .Tag = dblVelocity & "," & Split(.Tag, ",")(0)

            Debugger.lstMap.Items.Clear()
            Debugger.lstMap.Items.Add(xvel & ", " & yvel)
            Debugger.lstMap.Items.Add(collision_point(x, y, picTiles).Name)

            Debugger.lstMap.Items.Add("  0: " & raycast_distance(x, y, 0))
            Debugger.lstMap.Items.Add(" 90: " & raycast_distance(x, y, 90))
            Debugger.lstMap.Items.Add("180: " & raycast_distance(x, y, 180))
            Debugger.lstMap.Items.Add("270: " & raycast_distance(x, y, 270))

            .Left += xvel
            .Top += yvel



            '    'right
            '    If xvel > 0 Then
            '        i = 0
            '        While collision_point(Player.Right + 1, y, picTiles).Name = "Empty" And i < xvel
            '            i += 1
            '            Player.Left += 1
            '        End While
            '    End If
            '    'left
            '    If xvel < 0 Then
            '        i = 0
            '        While collision_point(Player.Left - 1, y, picTiles).Name = "Empty" And i < Math.Abs(xvel)
            '            i += 1
            '            Player.Left -= 1
            '        End While
            '    End If
            '    'down
            '    If yvel > 0 Then
            '        i = 0
            '        While collision_point(x, Player.Bottom + 1, picTiles).Name = "Empty" And i < yvel
            '            i += 1
            '            Player.Top += 1
            '        End While
            '    End If
            '    'up
            '    If yvel < 0 Then
            '        i = 0
            '        While collision_point(x, Player.Top - 1, picTiles).Name = "Empty" And i < Math.Abs(yvel)
            '            i += 1
            '            Player.Top -= 1
            '        End While
            '    End If

        End With

    End Sub

    Public Function collision_point(ByVal x As Integer, ByVal y As Integer, collider(,) As PictureBox)
        Dim r = Nothing
        For Each tile In collider
            If tile.Bounds.IntersectsWith(New Rectangle(x, y, 1, 1)) Then
                r = tile
            End If
        Next
        Return r
    End Function
    Public Function raycast_distance(ByVal x As Integer, ByVal y As Integer, ByVal dir As Integer) As Integer
        Dim dist As Integer = 0
        While IsNothing(collision_point(x + lengthdir_x(dist, dir), y + lengthdir_y(dist, dir), picTiles))
            dist += 1
        End While
        Return dist
    End Function
    Public Function raycast_collision(ByVal x As Integer, ByVal y As Integer, ByVal dir As Integer) As PictureBox
        Dim dist As Integer = 0, xhit As Integer, yhit As Integer
        While IsNothing(collision_point(x + lengthdir_x(dist, dir), y + lengthdir_y(dist, dir), picTiles))
            dist += 1
        End While
        xhit = x + lengthdir_x(dist, dir)
        yhit = y + lengthdir_y(dist, dir)
        Return collision_point(xhit, yhit, picTiles)
    End Function
    Public Function lengthdir_x(ByVal dist As Integer, ByVal dir As Integer) As Integer
        Return Math.Cos(dir) * dist
    End Function
    Public Function lengthdir_y(ByVal dist As Integer, ByVal dir As Integer) As Integer
        Return Math.Sin(dir) * dist
    End Function
    Public Function Clamp(ByVal dblVal As Double, ByVal dblMin As Double, ByVal dblMax As Double) 'Clamps a value between two numbers.
        If dblVal > dblMax Then
            dblVal = dblMax
        ElseIf dblVal < dblMin Then
            dblVal = dblMin
        End If
        Return dblVal
    End Function
    Enum Prop As Integer
        xA = 0
        yA = 1
        xF = 2
        yF = 3
    End Enum
    Private Function toRad(ByVal deg As Integer) As Double
        Return deg * (Math.PI / 180)
    End Function

    Private Sub mapGen_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim intSpeed As Integer = 5
        Static xvel As Double = 0
        Static yvel As Double = 0

        Static grounded As Boolean = collision_point(Player.Location.X, Player.Bottom + 1, picTiles).Name = "Tile"



        xvel = Split(Player.Tag, ",")(0)
        yvel = Split(Player.Tag, ",")(1)

        If e.KeyCode = Keys.Space And grounded = True Then
            yvel -= 20
        End If
        If e.KeyCode = Keys.A Then
            xvel -= intSpeed
        End If
        If e.KeyCode = Keys.D Then

        End If


        Player.Tag = xvel & "," & yvel

    End Sub

    Private Function GetTile(ByVal img As Bitmap, ByVal x As Integer, ByVal y As Integer) 'gets a tile from the given tileset
        Dim a As Integer = 1 ' aspect ratio, for scaling later
        Return CropBitmap(img, 32 * a * x, 32 * a * y, 32 * a, 32 * a)
    End Function

    Private Function CropBitmap(ByRef bmp As Bitmap, ByVal cropX As Integer, ByVal cropY As Integer, ByVal cropWidth As Integer, ByVal cropHeight As Integer) As Bitmap 'crops a bitmap image to specified dimensions
        Dim rect As New Rectangle(cropX, cropY, cropWidth, cropHeight)
        Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)
        Return cropped
    End Function

    Private Function countAliveNeighbors(ByRef picTiles(,) As Boolean, ByVal x As Integer, ByVal y As Integer) 'returns the number off cells in a ring around (x,y) that are alive.
        Dim intCount As Integer = 0
        Dim intNeighborX As Integer
        Dim intNeighborY As Integer
        For i = -1 To 1 Step 1
            For j = -1 To 1 Step 1
                intNeighborX = x + i
                intNeighborY = y + j
                If (i = 0 And j = 0) Then
                    'middle point, do nothing
                ElseIf intNeighborX < 0 Or intNeighborY < 0 Or intNeighborX >= xMapSize Or intNeighborY >= yMapSize Then 'incase the index is off the edge of the map
                    intCount += 1
                ElseIf picTiles(intNeighborX, intNeighborY) Then ' if there is an adjacent tile
                    intCount += 1
                End If
            Next
        Next
        Return intCount
    End Function

    Function doSimulationStep(ByRef oldMap(,) As Boolean) As Boolean(,)
        Dim newMap(xMapSize, yMapSize) As Boolean
        Dim nbs As Integer
        For x = 0 To xMapSize
            For y = 0 To yMapSize
                nbs = countAliveNeighbors(oldMap, x, y)
                If oldMap(x, y) Then
                    If nbs < deathLimit Then
                        newMap(x, y) = False
                    Else
                        newMap(x, y) = True
                    End If
                Else
                    If nbs > birthLimit Then
                        newMap(x, y) = True
                    Else
                        newMap(x, y) = False
                    End If
                End If
            Next
        Next

        Return newMap
    End Function

    Sub createTiles(ByRef picTile(,) As PictureBox)
        For x = 0 To xMapSize
            For y = 0 To yMapSize
                'Creating the picturebox
                picTile(x, y) = New PictureBox
                With picTile(x, y)
                    'Setting properties
                    .Parent = backPanel
                    .Size = New Size(intSize, intSize)
                    .Location = New Point(intSize * x - intSize, intSize * y - intSize)
                    .Visible = True
                    .Tag = x.ToString + "," + y.ToString + "," + (x + xO).ToString + "," + (y + yO).ToString 'storing properties is dumb
                    .BackColor = Color.White
                    .SendToBack() 'Send these empty tiles to the back
                End With
                Me.Controls.Add(picTile(x, y))
            Next
        Next
    End Sub
    '
    'Due to the predicted forecast, Saturday's softball game against the Sega Strikers has been postphoned until June 9th.  Sorry for any inconvenience this has caused.
    '
    Sub Fill(ByVal MaxTiles As Integer, ByVal Percentage As Double, ByRef grid(,) As Boolean) 'Fill a grid with a percentage of tiles
        Dim tiles As Integer = Int(MaxTiles * Percentage)
        Dim blnCanPlace = True
        Dim rndX As Integer, rndY As Integer
        While tiles > 0
            rndX = Rand(0, xMapSize)
            rndY = Rand(0, yMapSize)
            While grid(rndX, rndY) = True
                rndX = Rand(0, xMapSize)
                rndY = Rand(0, yMapSize)
            End While
            grid(rndX, rndY) = True
            tiles -= 1
        End While
    End Sub

    Public Function Rand(ByVal intLow As Integer, ByVal intHigh As Integer) As Integer
        Return Int(Rnd() * (intHigh - intLow + 1) + intLow)
    End Function
End Class

Public Class CustomPanel
    Inherits System.Windows.Forms.Panel
    Public Sub New()
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
    End Sub
End Class