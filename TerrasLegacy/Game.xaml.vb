Imports System.Diagnostics.Eventing
Imports System.Windows.Threading
Imports NAudio.Wave
'Imports NAudio.CoreAudio
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar
Imports System.Collections.Generic
Imports System.Numerics
Imports System.Reflection
Imports System.Windows.Ink

Class Game
    Private Character As Character

    Dim playerFoot As Rectangle
    Dim playerHitBox As Rectangle
    Dim playerAttackRangeBox As Rectangle


    Dim isRight As Boolean
    Dim isLeft As Boolean
    Dim triggerDeath As Boolean

    Dim FirstWaveClear As Boolean

    Dim ProjectLocation As String
    Dim enemyList As New List(Of Enemy)()

    Dim isGameOver

    Private WithEvents UpdateLocationBoxTimer As DispatcherTimer
    Private WithEvents CheckFootTimer As DispatcherTimer
    Private WithEvents CharacterJumpingTimer As DispatcherTimer
    Private WithEvents MovementTimer As DispatcherTimer

    Private WithEvents UpdateEnemyBoxLocation As DispatcherTimer
    Private WithEvents PlayerAttackEnemy As DispatcherTimer
    Private WithEvents SpawnEnemy As DispatcherTimer
    Private WithEvents EnemyMovement As DispatcherTimer
    Private WithEvents EnemyAttackPlayer As DispatcherTimer
    Private WithEvents GameOverTimer As DispatcherTimer

    Private InitalPlayerX As Double
    Private InitalPlayerY As Double


    Private Sound As Sound
    Private SoundLoop As Sound
    Public Sub New()
        InitializeComponent()

        Sound = New Sound
        SoundLoop = New Sound

        ProjectLocation = AppDomain.CurrentDomain.BaseDirectory
        ProjectLocation = ProjectLocation.Replace("\bin\Debug\net8.0-windows\", "\")


        UpdateLocationBoxTimer = New DispatcherTimer()
        AddHandler UpdateLocationBoxTimer.Tick, AddressOf UpdateLocationBoxTimer_Tick
        UpdateLocationBoxTimer.Interval = TimeSpan.FromMilliseconds(5)
        UpdateLocationBoxTimer.Start()

        GameOverTimer = New DispatcherTimer()
        AddHandler GameOverTimer.Tick, AddressOf GameOverTimer_Tick
        GameOverTimer.Interval = TimeSpan.FromMilliseconds(1300)

        UpdateEnemyBoxLocation = New DispatcherTimer()
        AddHandler UpdateEnemyBoxLocation.Tick, AddressOf UpdateEnemyBoxLocation_Tick
        UpdateEnemyBoxLocation.Interval = TimeSpan.FromMilliseconds(5)
        UpdateEnemyBoxLocation.Start()

        CharacterJumpingTimer = New DispatcherTimer()
        AddHandler CharacterJumpingTimer.Tick, AddressOf CharacterJumpingTimer_Tick
        CharacterJumpingTimer.Interval = TimeSpan.FromMilliseconds(30)
        CharacterJumpingTimer.Start()



        MovementTimer = New DispatcherTimer()
        AddHandler MovementTimer.Tick, AddressOf MovementTimer_Tick
        MovementTimer.Interval = TimeSpan.FromMilliseconds(30)
        MovementTimer.Start()

        PlayerAttackEnemy = New DispatcherTimer()
        AddHandler PlayerAttackEnemy.Tick, AddressOf PlayerAttackEnemy_Tick
        PlayerAttackEnemy.Interval = TimeSpan.FromMilliseconds(50)
        PlayerAttackEnemy.Start()

        EnemyAttackPlayer = New DispatcherTimer()
        AddHandler EnemyAttackPlayer.Tick, AddressOf EnemyAttackPlayer_Tick
        EnemyAttackPlayer.Interval = TimeSpan.FromMilliseconds(100)
        EnemyAttackPlayer.Start()

        SpawnEnemy = New DispatcherTimer()
        AddHandler SpawnEnemy.Tick, AddressOf SpawnEnemy_Tick
        SpawnEnemy.Interval = TimeSpan.FromMilliseconds(50)
        SpawnEnemy.Start()

        EnemyMovement = New DispatcherTimer()
        AddHandler EnemyMovement.Tick, AddressOf EnemyMovement_Tick
        EnemyMovement.Interval = TimeSpan.FromMilliseconds(50)
        EnemyMovement.Start()


        Character = New Character(playerImage)

        playerFoot = New Rectangle()
        playerFoot.Width = Character.FootWidth
        playerFoot.Height = Character.FootHeight
        playerFoot.Name = "playerFoot"

        playerFoot.Fill = Brushes.Brown
        Canvas.SetLeft(playerFoot, 100)
        Canvas.SetTop(playerFoot, Canvas.GetTop(floorPlatform) - playerFoot.Height)

        InitalPlayerX = Character.PlayerX
        InitalPlayerY = Character.PlayerY

        Character.PlayerX = Canvas.GetLeft(playerFoot)
        Character.PlayerY = Canvas.GetTop(playerFoot)

        playerHitBox = New Rectangle()
        playerHitBox.Width = Character.HitBoxWidth
        playerHitBox.Height = Character.HitBoxHeight
        playerHitBox.Name = "playerHitBox"
        playerHitBox.Fill = Brushes.Red

        playerAttackRangeBox = New Rectangle()
        playerAttackRangeBox.Width = Character.AttackBoxWidth
        playerAttackRangeBox.Height = Character.AttackBoxHeight
        playerAttackRangeBox.Name = "playerAttackRangeBox"
        playerAttackRangeBox.Stroke = Brushes.Blue


        gameCanvas.Children.Add(playerFoot)
        gameCanvas.Children.Add(playerHitBox)
        gameCanvas.Children.Add(playerAttackRangeBox)
    End Sub




    Private Sub gameLoaded(sender As Object, e As RoutedEventArgs)
        MakeObjectsInvible()
        Dim soundPath = ProjectLocation & "Assets\Sound\Clement Panchout - Sweet 70s.wav"
        SoundLoop.PlaySoundLoop(soundPath)


        gameOverText.Visibility = Visibility.Collapsed
        gameOverText.Opacity = 0
        restartBtn.Visibility = Visibility.Collapsed
        restartBtn.Opacity = 0

        'Dim newEnemyImage As New Image()
        'Dim enemyFoot As New Rectangle()
        'Dim enemyAttackBox As New Rectangle()
        'Dim enemyHitBox As New Rectangle()

        'Dim newEnemy As New Enemy(newEnemyImage, enemyFoot, enemyAttackBox, enemyHitBox, "Goblin")


        'newEnemy.EnemyX = 300
        'newEnemy.EnemyY = Canvas.GetTop(floorPlatform) - newEnemy.FootHeight

        'enemyFoot.Width = newEnemy.FootWidth
        'enemyFoot.Height = newEnemy.FootHeight
        'enemyFoot.Fill = Brushes.Black

        'enemyAttackBox.Width = newEnemy.AttackBoxWidth
        'enemyAttackBox.Height = newEnemy.AttackBoxHeight
        'enemyAttackBox.Stroke = Brushes.Blue



        'enemyHitBox.Width = newEnemy.HitBoxWidth
        'enemyHitBox.Height = newEnemy.HitBoxHeight
        'enemyHitBox.Stroke = Brushes.Red

        'Canvas.SetLeft(newEnemy.FootBox, newEnemy.EnemyX)
        'Canvas.SetTop(newEnemy.FootBox, newEnemy.EnemyY)

        'newEnemy.EnemyX = Canvas.GetLeft(newEnemy.FootBox)
        'newEnemy.EnemyY = Canvas.GetTop(newEnemy.FootBox)

        'enemyList.Add(newEnemy)



        'gameCanvas.Children.Add(newEnemyImage)
        'gameCanvas.Children.Add(enemyFoot)
        'gameCanvas.Children.Add(enemyAttackBox)
        'gameCanvas.Children.Add(enemyHitBox)

        'Canvas.SetTop(newEnemy.HitBox, Canvas.GetTop(floorPlatform) - newEnemy.HitBoxHeight)
        'Canvas.SetLeft(newEnemy.HitBox, 200)
    End Sub

    Private Sub SpawnEnemy_Tick(sender As Object, e As EventArgs)


        If enemyList.Count > 0 AndAlso enemyList.Count Mod 10 = 0 Then
            For Each enemy As Enemy In enemyList
                enemy.EnemySpeed += Math.Floor(enemyList.Count / 20)
                enemy.Health += Math.Floor(enemyList.Count * 2)
                enemy.Damage += Math.Floor(enemyList.Count / 3)
            Next
        End If



        Dim newEnemyImage As New Image()
        Dim enemyFoot As New Rectangle()
        Dim enemyAttackBox As New Rectangle()
        Dim enemyHitBox As New Rectangle()



        Dim choiceEnemy As String() = {"Goblin", "Warhog"}



        Dim rnd As New Random()
        Dim randomIndexEnemy As Integer = rnd.Next(0, choiceEnemy.Length)

        Dim randomChoice As String = choiceEnemy(randomIndexEnemy)

        If randomChoice = "Goblin" Then
            Dim soundPath1 = ProjectLocation & "Assets\Sound\Goblins.wav"
            Sound.PlaySound(soundPath1)

        ElseIf randomChoice = "Warhog" Then
            Dim soundPath = ProjectLocation & "Assets\Sound\Hit Sound-011.wav"
            Sound.PlaySound(soundPath)
        End If

        Dim newEnemy As New Enemy(newEnemyImage, enemyFoot, enemyAttackBox, enemyHitBox, randomChoice)

        Dim randomNumber As Integer
        Dim rand As New Random()
        If Not FirstWaveClear Then
            randomNumber = rand.Next(300, 450)
            newEnemy.FaceDirection = "Right"

        Else
            randomNumber = rand.Next(Character.PlayerX - 420, Character.PlayerX + 420)

            If Character.PlayerX < randomNumber Then
                newEnemy.FaceDirection = "Right"

            Else
                newEnemy.FaceDirection = "Left"
            End If
        End If
        newEnemy.EnemyX = randomNumber
        newEnemy.EnemyY = Canvas.GetTop(floorPlatform) - newEnemy.FootHeight

        If Character.PlayerX < randomNumber Then
            newEnemy.FaceDirection = "Right"

        Else
            newEnemy.FaceDirection = "Left"
        End If

        enemyFoot.Width = newEnemy.FootWidth
        enemyFoot.Height = newEnemy.FootHeight
        enemyFoot.Fill = Brushes.Black
        enemyFoot.Opacity = 0

        enemyAttackBox.Width = newEnemy.AttackBoxWidth
        enemyAttackBox.Height = newEnemy.AttackBoxHeight
        enemyAttackBox.Stroke = Brushes.Blue
        enemyAttackBox.Opacity = 0


        enemyHitBox.Width = newEnemy.HitBoxWidth
        enemyHitBox.Height = newEnemy.HitBoxHeight
        enemyHitBox.Stroke = Brushes.Red
        enemyHitBox.Opacity = 0

        Canvas.SetLeft(newEnemy.FootBox, newEnemy.EnemyX)
        Canvas.SetTop(newEnemy.FootBox, newEnemy.EnemyY)

        newEnemy.EnemyX = Canvas.GetLeft(newEnemy.FootBox)
        newEnemy.EnemyY = Canvas.GetTop(newEnemy.FootBox)

        enemyList.Add(newEnemy)

        gameCanvas.Children.Add(newEnemyImage)
        gameCanvas.Children.Add(enemyFoot)
        gameCanvas.Children.Add(enemyAttackBox)
        gameCanvas.Children.Add(enemyHitBox)

        Canvas.SetTop(newEnemy.HitBox, Canvas.GetTop(floorPlatform) - newEnemy.HitBoxHeight)
        Canvas.SetLeft(newEnemy.HitBox, 200)

        Dim randTimer As New Random()
        Dim numbers As New List(Of Integer)() From {2200, 500, 500, 5000, 100, 4000}
        Dim randomIndex As Integer = rand.Next(0, numbers.Count)
        Dim randomNumberTimer As Integer = numbers(randomIndex)

        SpawnEnemy.Interval = TimeSpan.FromMilliseconds(randomNumberTimer)



    End Sub

    Private Sub EnemyMovement_Tick(sender As Object, e As EventArgs)
        For Each enemy In enemyList
            If enemy.EnemyType = "Warhog" And Not enemy.isDead Then
                If enemy.FaceDirection = "Right" Then
                    enemy.EnemyX -= enemy.EnemySpeed
                ElseIf enemy.FaceDirection = "Left" Then
                    enemy.EnemyX += enemy.EnemySpeed
                End If
            End If

            If enemy.EnemyType = "Goblin" And Not enemy.isDead Then

                If Character.PlayerX < enemy.EnemyX Then
                    enemy.FaceDirection = "Right"
                    enemy.EnemyX -= enemy.EnemySpeed
                Else
                    enemy.FaceDirection = "Left"
                    enemy.EnemyX += enemy.EnemySpeed
                End If
            End If
        Next
    End Sub



    Private Sub EnemyAttackPlayer_Tick(sender As Object, e As EventArgs)
        Dim playerHitboxBoxBounds As Rect = New Rect(Canvas.GetLeft(playerHitBox), Canvas.GetTop(playerHitBox), playerHitBox.ActualWidth, playerHitBox.ActualHeight)
        For Each enemy In enemyList

            Dim enemyAttackRangeBoxBounds As Rect = New Rect(Canvas.GetLeft(enemy.AttackBox), Canvas.GetTop(enemy.AttackBox), enemy.AttackBoxWidth, enemy.AttackBoxHeight)
            If enemyAttackRangeBoxBounds.IntersectsWith(playerHitboxBoxBounds) And Not enemy.isDead Then
                Character.TakeDamage(enemy.Damage)

                If Not enemy.isHitActive Then

                    If enemy.EnemyType = "Goblin" Then
                        Dim southPath1 = ProjectLocation & "Assets\Sound\Goblins.wav"
                        Sound.PlaySound(southPath1)
                    End If
                    enemy.CurrentState = EnemyState.Attacking
                End If
            ElseIf Not enemy.isDead Then

                'enemy.CurrentState = EnemyState.Running
            End If
        Next

        healthbar.Value = Character.Health
        hpNum.Content = Character.Health

        If Character.Health < 1 Then
            Dim southPath2 = ProjectLocation & "Assets\Sound\death_7_meghan.wav"
            Sound.PlaySound(southPath2)
            hpNum.Content = 0
            GameOver()

        End If
    End Sub

    Dim soundCount As Integer = 0
    Sub PlayGameOverSound()
        Dim southPath1 = ProjectLocation & "Assets\Sound\Game Over Sound Effect.wav"
        Sound.PlaySound(southPath1)
        soundCount += 1

    End Sub


    Dim gameOverCount = 1
    Private Sub GameOverTimer_Tick(sender As Object, e As EventArgs)
        If soundCount = 0 Then
            PlayGameOverSound()
        End If

        gameOverText.Visibility = Visibility.Visible
        restartBtn.Visibility = Visibility.Visible

        If gameOverCount <> 100 Then
            gameOverText.Opacity += gameOverCount
            restartBtn.Opacity += gameOverCount

        Else
            GameOverTimer.Stop()
        End If
    End Sub

    Private Sub GameOver()
        soundCount = 0
        If Not Character.isDeadAnimationPlayed Then
            Character.CurrentState = PlayerState.Dead
        End If
        PlayerAttackEnemy.Stop()
        EnemyAttackPlayer.Stop()
        SpawnEnemy.Stop()
        EnemyMovement.Stop()
        UpdateEnemyBoxLocation.Stop()
        Character.isDead = True
        isGameOver = True
        CharacterJumpingTimer.Stop()
        MovementTimer.Stop()
        gameOverCount = 1
        GameOverTimer.Start()
        SoundLoop.PausePlayback()

        For Each enemy In enemyList
            enemy.CurrentState = EnemyState.Idle
        Next

        For Each enemy In enemyList
            enemy.isDead = True
        Next

        For Each enemy In enemyList
            gameCanvas.Children.Remove(enemy.EnemyImage)
            gameCanvas.Children.Remove(enemy.FootBox)
            gameCanvas.Children.Remove(enemy.AttackBox)
            gameCanvas.Children.Remove(enemy.HitBox)

            If Not gameCanvas.Children.Contains(enemy.EnemyImage) And gameCanvas.Children.Contains(enemy.FootBox) And Not gameCanvas.Children.Contains(enemy.AttackBox) And gameCanvas.Children.Contains(enemy.HitBox) Then
                enemyList.Remove(enemy)
            End If

        Next

        enemyList.Clear()

    End Sub

    Private Sub RestartGame()
        gameOverText.Visibility = Visibility.Collapsed
        restartBtn.Visibility = Visibility.Collapsed
        gameOverText.Opacity = 0
        restartBtn.Opacity = 0
        Character.PlayerX = InitalPlayerX + 30
        Character.PlayerY = InitalPlayerY
        Character.CurrentState = PlayerState.Idle
        EnemyAttackPlayer.Start()
        SpawnEnemy.Start()
        EnemyMovement.Start()
        UpdateEnemyBoxLocation.Start()
        PlayerAttackEnemy.Start()
        GameOverTimer.Stop()

        Character.isDead = False
        Character.FaceDirection = "Right"
        isGameOver = False
        Character.Health = Character.MaxHealth
        Character.isDeadAnimationPlayed = False  ' Ensure this flag is reset
        CharacterJumpingTimer.Start()
        MovementTimer.Start()

        FirstWaveClear = False

        Dim soundPath = ProjectLocation & "Assets\Sound\Clement Panchout - Sweet 70s.wav"
        SoundLoop.PlaySoundLoop(soundPath)
    End Sub


    Private Sub PlayerAttackEnemy_Tick(sender As Object, e As EventArgs)
        isGameOver = isGameOver
        Dim playerAttackRangeBoxBounds As Rect = New Rect(Canvas.GetLeft(playerAttackRangeBox), Canvas.GetTop(playerAttackRangeBox), playerAttackRangeBox.ActualWidth, playerAttackRangeBox.ActualHeight)


        If Character.CharacterKill >= 2 Then
            FirstWaveClear = True
            SpawnEnemy.Start()
        End If

        For Each enemy In enemyList
            Dim enemyHitBoxBounds As Rect = New Rect(Canvas.GetLeft(enemy.HitBox), Canvas.GetTop(enemy.HitBox), enemy.HitBoxWidth, enemy.HitBoxHeight)
            If playerAttackRangeBoxBounds.IntersectsWith(enemyHitBoxBounds) And Character.isAttacking And Not enemy.isDead Then
                enemy.TakeDamage(10)

                'If enemy.EnemyType = "Goblin" Then
                enemy.isHitActive = True
                enemy.CurrentState = EnemyState.Hit
                'End If

                If enemy.Health < 0 Then
                    enemy.isDead = True
                    enemy.CurrentState = EnemyState.Dead
                    Character.CharacterKill += 1
                    Character.HealthRegeneration(enemy.MaxHealth)
                End If

            Else
            End If


            If enemy.isDeadAnimationPlayed Then
                gameCanvas.Children.Remove(enemy.EnemyImage)
                gameCanvas.Children.Remove(enemy.FootBox)
                gameCanvas.Children.Remove(enemy.AttackBox)
                gameCanvas.Children.Remove(enemy.HitBox)

                If Not gameCanvas.Children.Contains(enemy.EnemyImage) And gameCanvas.Children.Contains(enemy.FootBox) And Not gameCanvas.Children.Contains(enemy.AttackBox) And gameCanvas.Children.Contains(enemy.HitBox) Then
                    enemyList.Remove(enemy)
                End If

            End If
        Next
    End Sub


    Private Sub UpdateEnemyBoxLocation_Tick(sender As Object, e As EventArgs)
        For Each enemy In enemyList

            Canvas.SetLeft(enemy.FootBox, enemy.EnemyX)
            Canvas.SetTop(enemy.FootBox, enemy.EnemyY)

            Canvas.SetLeft(enemy.EnemyImage, enemy.EnemyX + enemy.ImageLeftDiffInFoot)
            Canvas.SetTop(enemy.EnemyImage, (Canvas.GetTop(enemy.FootBox) + enemy.FootHeight) - enemy.ImageTopDiffInFoot)

            Canvas.SetTop(enemy.AttackBox, Canvas.GetTop(enemy.FootBox) - enemy.AttackBoxHeight)
            Canvas.SetLeft(enemy.AttackBox, Canvas.GetLeft(enemy.FootBox) + enemy.AttackBoxLeftDiffInFoot)

            Canvas.SetLeft(enemy.HitBox, Canvas.GetLeft(enemy.FootBox) + enemy.HitBoxLeftDiffInFoot)
            Canvas.SetTop(enemy.HitBox, Canvas.GetTop(enemy.FootBox) - enemy.HitBoxHeight)
        Next

    End Sub


    Private Sub MakeEnemiesIdle()
        For Each enemy In enemyList
            enemy.CurrentState = EnemyState.Idle
        Next
    End Sub


    Private Sub MakeObjectsInvible()
        playerFoot.Opacity = 0
        playerAttackRangeBox.Opacity = 0
        playerHitBox.Opacity = 0

        For Each child As UIElement In gameCanvas.Children
            If TypeOf child Is Rectangle AndAlso DirectCast(child, Rectangle).Tag IsNot Nothing AndAlso (DirectCast(child, Rectangle).Tag.ToString() = "platformSetFoot") Then
                child.Opacity = 0
            End If
        Next
    End Sub




    Private Sub UpdateLocationBoxTimer_Tick(sender As Object, e As EventArgs)
        Canvas.SetTop(playerImage, Canvas.GetTop(playerFoot) - playerImage.ActualHeight + Character.ImageTopDiffInFoot)
        Canvas.SetLeft(playerImage, Canvas.GetLeft(playerFoot) - Character.ImageLeftDiffInFoot)

        Canvas.SetLeft(playerHitBox, Canvas.GetLeft(playerFoot) + Character.HitBoxLeftDiffInFoot)
        Canvas.SetTop(playerHitBox, Canvas.GetTop(playerFoot) - playerHitBox.ActualHeight - Character.HitBoxTopDiffInFoot)

        If Character.FaceDirection = "Right" Then
            Canvas.SetLeft(playerAttackRangeBox, Canvas.GetLeft(playerFoot) + Character.AttackBoxLeftDiffInFoot)
        Else
            Canvas.SetLeft(playerAttackRangeBox, Canvas.GetLeft(playerFoot) - playerAttackRangeBox.ActualWidth + playerFoot.ActualWidth - Character.AttackBoxLeftDiffInFoot)
        End If
        Canvas.SetTop(playerAttackRangeBox, Canvas.GetTop(playerFoot) - playerAttackRangeBox.ActualHeight)


        Canvas.SetLeft(playerFoot, Character.PlayerX)
        Canvas.SetTop(playerFoot, Character.PlayerY)
    End Sub


    Private Sub MovementTimer_Tick(sender As Object, e As EventArgs)

        If isLeft Or isRight Then
            Character.isPLayerMoving = True

        Else
            Character.isPLayerMoving = False
        End If


        If isLeft And Character.PlayerX > 30 Then
            Character.PlayerX -= Character.CharacterSpeed
            Canvas.SetLeft(playerFoot, Character.PlayerX)
        End If

        If isRight And Character.PlayerX < (gameCanvas.ActualWidth - 45) Then
            Character.PlayerX += Character.CharacterSpeed
            Canvas.SetLeft(playerFoot, Character.PlayerX)
        End If

        If isLeft And Canvas.GetLeft(bg) < 0 Then
            Canvas.SetLeft(bg, Canvas.GetLeft(bg) + Character.CharacterSpeed)
            MovePlatforms((Character.CharacterSpeed))
        End If


        If isRight And Canvas.GetLeft(bg) > -2118 Then
            Canvas.SetLeft(bg, Canvas.GetLeft(bg) - (Character.CharacterSpeed))
            MovePlatforms(-1 * ((Character.CharacterSpeed)))
        End If
        ' Check for idle state
        If Not Character.isPLayerMoving AndAlso Not Character.isAttacking AndAlso Not Character.isJumping AndAlso Not Character.isJumpFall AndAlso Not Character.isDead Then
            Character.CurrentState = PlayerState.Idle
        End If

        ' Check for running state
        If Character.isPLayerMoving Then
            Character.CurrentState = PlayerState.Running
        End If

        ' Check for attacking state
        If Character.isAttacking Then
            Character.CurrentState = PlayerState.Attacking
        End If
    End Sub

    Private Sub MovePlatforms(ByVal moveCount As Integer)
        For Each child As UIElement In gameCanvas.Children
            If TypeOf child Is Rectangle AndAlso DirectCast(child, Rectangle).Tag IsNot Nothing AndAlso (DirectCast(child, Rectangle).Tag.ToString() = "platformSetFoot" Or DirectCast(child, Rectangle).Tag.ToString() = "platform") AndAlso Not Character.isJumping Then
                Canvas.SetLeft(child, Canvas.GetLeft(child) + moveCount)
            End If
        Next
    End Sub

    Dim Gravity As Integer = 22S
    Dim JumpSpeed As Integer = 22
    Dim Force As Integer = 20
    Dim playerFootTop As Integer
    Dim isBodyPlatformCollide As Boolean = False


    Private Sub CharacterJumpingTimer_Tick(sender As Object, e As EventArgs)

        Character.PlayerY -= Force

        Canvas.SetTop(playerFoot, Character.PlayerY)


        If isBodyPlatformCollide And Not Character.isJumping Then
            Force = 0
        End If

        If Character.isJumping Then

            JumpSpeed = -12
            Force -= 1
        Else
            JumpSpeed = 12
            Force -= 1
        End If

        If Force < 0 And Character.isJumping Then
            Character.isJumping = False
        End If

        Dim playerFootBounds As Rect = playerFoot.TransformToVisual(Me).TransformBounds(New Rect(0, 0, playerFoot.ActualWidth, playerFoot.ActualHeight))
        Dim platform As Rect
        Dim platformBottom As Double

        For Each child As UIElement In gameCanvas.Children
            If TypeOf child Is Rectangle AndAlso DirectCast(child, Rectangle).Tag IsNot Nothing AndAlso DirectCast(child, Rectangle).Tag.ToString() = "platformSetFoot" AndAlso Not Character.isJumping Then
                platform = DirectCast(child, Rectangle).TransformToVisual(Me).TransformBounds(New Rect(0, 0, DirectCast(child, Rectangle).ActualWidth, DirectCast(child, Rectangle).ActualHeight))
                platformBottom = Canvas.GetBottom(child)
                If platform <> Rect.Empty AndAlso playerFootBounds.IntersectsWith(platform) Then

                    isBodyPlatformCollide = True
                    Force = 12
                    Canvas.SetTop(playerFoot, Canvas.GetTop(child) - playerFoot.ActualHeight)
                    Character.PlayerY = Canvas.GetTop(playerFoot)

                    Character.isJumping = False

                    JumpSpeed = 0
                    Exit For
                Else
                    isBodyPlatformCollide = False

                End If
            End If
        Next
    End Sub

    Private Sub windowKeyDown(sender As Object, e As KeyEventArgs)
        If isGameOver Then
            Return
        End If
        If e.Key = Key.A Or e.Key = Key.Left Then
                Character.FaceDirection = "Left"
                isLeft = True
            ElseIf e.Key = Key.D Or e.Key = Key.Right Then
                isRight = True
                Character.FaceDirection = "Right"

            ElseIf e.Key = Key.U And Not Character.isJumping Then
                Dim southPath = ProjectLocation & "Assets\Sound\swordSound.wav"
                Sound.PlaySound(southPath)
                Character.isAttacking = True
            End If
        If e.Key = Key.Space AndAlso Not Character.isJumping Then
            Dim soundPath = ProjectLocation & "Assets\Sound\Jumping.wav"
            Sound.PlaySound(soundPath)
            Character.isJumping = True
        End If
    End Sub


    Private Sub windowKeyUp(sender As Object, e As KeyEventArgs)
        If (e.Key = Key.A Or e.Key = Key.Left) Then
            isLeft = False
        End If

        If (e.Key = Key.D Or e.Key = Key.Right) Then
            isRight = False
        End If
    End Sub




    Private Sub restartHover(sender As Object, e As MouseEventArgs)
        Dim exitResourceName As String = ProjectLocation & "Assets\ui\buttons\restartHoverBtn.png"
        Dim exitImageSource As New BitmapImage(New Uri(exitResourceName))

        Canvas.SetLeft(restartBtn, Canvas.GetLeft(restartBtn) + 5)
        restartBtn.Source = exitImageSource

        Dim soundPath = ProjectLocation & "Assets\Sound\001_Hover_01.wav"
        Sound.PlaySound(soundPath)
    End Sub

    Private Sub restartNotHover(sender As Object, e As MouseEventArgs) Handles restartBtn.MouseLeave
        Dim exitResourceName As String = ProjectLocation & "Assets\ui\buttons\restartBtn.png"
        Dim exitImageSource As New BitmapImage(New Uri(exitResourceName))

        Canvas.SetLeft(restartBtn, Canvas.GetLeft(restartBtn) - 5)
        restartBtn.Source = exitImageSource
    End Sub

    Private Sub RestartBtnClick(sender As Object, e As MouseButtonEventArgs)
        Dim soundPath As String = ProjectLocation & "Assets\Sound\MI_SFX 43.wav"
        Sound.PlaySound(soundPath)
        RestartGame()
    End Sub


End Class



