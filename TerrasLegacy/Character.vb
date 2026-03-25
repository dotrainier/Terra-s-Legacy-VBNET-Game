Imports System.Windows.Threading

Public Class Character
    Inherits BodyBoxes


    Public Property PlayerX As Double
    Public Property PlayerY As Double

    Public Property CharacterName As String
    Public Property CurrentState As PlayerState
    Public Property FaceDirection As String

    Public Property isAttacking As Boolean
    Public Property isJumping As Boolean
    Public Property isJumpFall As Boolean
    Public Property isPLayerMoving As Boolean

    Public Property isDead As Boolean

    Public Property CharacterKill As Integer



    Private CostumeCount As Integer = 1
    Private CharacterImage As Image

    Public Property Health As Integer
    Public Property MaxHealth As Integer = 300


    Public isDeadAnimationPlayed As Boolean = False

    Public Property CharacterSpeed As Integer = 8

    Public GameCanvas As Canvas
    Dim ProjectLocation As String

    'Character'

    Private WithEvents CharacterAnimationTimer As DispatcherTimer
    Private WithEvents BodyBoxAdjustingTimer As DispatcherTimer
    Private WithEvents DelayDeadTimer As DispatcherTimer

    Sub New(ByVal image As Image)
        Health = MaxHealth

        FootWidth = 25
        FootHeight = 1
        HitBoxWidth = 10
        HitBoxHeight = 45
        AttackBoxWidth = 40
        AttackBoxHeight = 53
        ImageTopDiffInFoot = 15
        ImageLeftDiffInFoot = 24
        HitBoxLeftDiffInFoot = 4
        HitBoxTopDiffInFoot = 0
        AttackBoxLeftDiffInFoot = 0
        AttackBoxTopDiffInFoot = 0


        ProjectLocation = AppDomain.CurrentDomain.BaseDirectory
        ProjectLocation = ProjectLocation.Replace("\bin\Debug\net8.0-windows\", "\")
        CurrentState = PlayerState.Idle
        FaceDirection = "Right"
        CharacterImage = image

        BodyBoxAdjustingTimer = New DispatcherTimer()
        AddHandler BodyBoxAdjustingTimer.Tick, AddressOf BodyBoxAdjustingTimer_Tick
        BodyBoxAdjustingTimer.Interval = TimeSpan.FromMilliseconds(50)
        BodyBoxAdjustingTimer.Start()


        CharacterAnimationTimer = New DispatcherTimer()
        AddHandler CharacterAnimationTimer.Tick, AddressOf CharacterAnimationTimer_Tick
        CharacterAnimationTimer.Interval = TimeSpan.FromMilliseconds(50)
        CharacterAnimationTimer.Start()
    End Sub

    Private Sub CharacterAnimationTimer_Tick(sender As Object, e As EventArgs)
        Select Case CurrentState
            Case PlayerState.Idle
                SpriteAnimation(4, "Idle")

            Case PlayerState.Attacking
                UpdateAnimation(50)
                If SpriteAnimation(8, "Attack") Then
                    CurrentState = PlayerState.Idle
                End If
            Case PlayerState.Running
                SpriteAnimation(8, "Run")
            Case PlayerState.Dead
                If Not isDeadAnimationPlayed Then
                    UpdateAnimation(100)
                    SpriteAnimation(8, "Dead")
                End If

        End Select
    End Sub


    Private Function SpriteAnimation(ByVal CostumeEnd As Integer, ByVal Action As String) As Boolean
        If CostumeCount <= CostumeEnd Then
            Dim resourceName As String = $"{Me.ProjectLocation}Assets\characterActions\Terra\{Action}{Me.FaceDirection}\{Action}{Me.FaceDirection}{Me.CostumeCount}.png"
            Dim imageSource As New BitmapImage()
            imageSource.BeginInit()
            imageSource.UriSource = New Uri(resourceName, UriKind.Absolute)
            imageSource.CacheOption = BitmapCacheOption.OnLoad
            imageSource.EndInit()

            CharacterImage.Source = imageSource

            CostumeCount += 1
            Return False
        Else
            If CurrentState = PlayerState.Jumping Or CurrentState = PlayerState.JumpFall Or CurrentState = PlayerState.Dead Then
                CostumeCount = CostumeEnd
                If CurrentState = PlayerState.Dead Then
                    isDeadAnimationPlayed = True
                End If
                Return True

            Else
                CostumeCount = 1

                If CurrentState = PlayerState.Attacking Then
                    isAttacking = False
                End If
                Return True
            End If
        End If
    End Function

    Sub TakeDamage(ByVal amount As Integer)
        Health -= amount
    End Sub

    Sub HealthRegeneration(ByVal enemyMaxHealth As Integer)
        Dim regen = enemyMaxHealth * 0.2
        Health += regen
        If Health > MaxHealth Then
            Health = MaxHealth
        End If
    End Sub



    Private Sub BodyBoxAdjustingTimer_Tick(sender As Object, e As EventArgs)
        If Me.FaceDirection = "Right" Then
            AttackBoxLeftDiffInFoot = 2
            Select Case CurrentState
                Case PlayerState.Idle
                    ImageLeftDiffInFoot = 24

                Case PlayerState.Attacking
                    ImageLeftDiffInFoot = 40

                Case PlayerState.Running
                    ImageLeftDiffInFoot = 37
            End Select

        Else
            AttackBoxLeftDiffInFoot = 5
            Select Case CurrentState
                Case PlayerState.Idle
                    ImageLeftDiffInFoot = 24

                Case PlayerState.Attacking
                    ImageLeftDiffInFoot = 35

                Case PlayerState.Running
                    ImageLeftDiffInFoot = 20
            End Select
        End If
    End Sub

    Private Sub UpdateAnimation(ByVal interval As Integer)
        CharacterAnimationTimer.Interval = TimeSpan.FromMilliseconds(interval)
    End Sub

End Class


Public Enum PlayerState
    Idle
    Walking
    Jumping
    JumpFall
    Attacking
    Running
    Dead
End Enum