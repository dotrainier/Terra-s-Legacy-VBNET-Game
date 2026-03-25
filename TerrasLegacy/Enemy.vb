Imports System.Windows.Threading

Public Class Enemy

    Inherits BodyBoxes

    Dim ProjectLocation As String

    Public Property CurrentState As EnemyState
    Public Property EnemyType As String
    Public Property EnemyX As Double
    Public Property EnemyY As Double

    Public Property Health As Integer
    Public Property MaxHealth As Integer = 100

    Public Property FaceDirection As String = "Right"

    Private CostumeCount As Integer = 1

    Public Property isDead As Boolean

    Public Property isDeadAnimationDone As Boolean

    Public Property isEnemyVanish As Boolean

    Public Property isHitActive As Boolean

    Public Property Damage As Integer

    Private Property WalkCostumeCount As Integer
    Private Property RunCostumeCount As Integer
    Private Property DeadCostumeCount As Integer

    Private Property AttackCostumeCount As Integer

    Private Property IdleCostumeCount As Integer


    Private Property HitCostumeCount As Integer


    Public EnemyImage As Image
    Public FootBox As Rectangle
    Public AttackBox As Rectangle
    Public HitBox As Rectangle

    Public Property EnemySpeed As Integer

    Private WithEvents EnemyAnimationTimer As DispatcherTimer

    Public Property isDeadAnimationPlayed As Boolean = False

    Sub New(ByVal image As Image, ByVal rect1 As Rectangle, ByVal rect2 As Rectangle, ByVal rect3 As Rectangle, ByVal Type As String)
        EnemyImage = image
        FootBox = rect1
        AttackBox = rect2
        HitBox = rect3

        EnemyType = Type
        TypeCharacteristics()

        Health = MaxHealth
        Health = MaxHealth
        CurrentState = EnemyState.Running

        If EnemyType = "Warhog" Then
            RunCostumeCount = 4
            AttackCostumeCount = 4
            DeadCostumeCount = 4
            IdleCostumeCount = 4
            HitCostumeCount = 2


        ElseIf EnemyType = "Goblin" Then
            AttackCostumeCount = 8
            DeadCostumeCount = 4
            RunCostumeCount = 8
            IdleCostumeCount = 4
            HitCostumeCount = 4

        End If

        ProjectLocation = AppDomain.CurrentDomain.BaseDirectory
        ProjectLocation = ProjectLocation.Replace("\bin\Debug\net8.0-windows\", "\")
        EnemyAnimationTimer = New DispatcherTimer()
        AddHandler EnemyAnimationTimer.Tick, AddressOf EnemyAnimationTimer_Tick
        EnemyAnimationTimer.Interval = TimeSpan.FromMilliseconds(50)
        EnemyAnimationTimer.Start()


    End Sub



    Sub TakeDamage(ByVal amount As Integer)
        Health -= amount
    End Sub


    Private Sub EnemyAnimationTimer_Tick(sender As Object, e As EventArgs)
        Select Case CurrentState
            Case EnemyState.Attacking
                SpriteAnimation(AttackCostumeCount, "Attack")

            Case EnemyState.Dead
                If Not isDeadAnimationPlayed Then
                    UpdateAnimation(200)
                    SpriteAnimation(DeadCostumeCount, "Dead")
                    End If
                    Case EnemyState.Running
                SpriteAnimation(RunCostumeCount, "Run")
            Case EnemyState.Idle
                SpriteAnimation(IdleCostumeCount, "Idle")

            Case EnemyState.Hit
                UpdateAnimation(20)
                SpriteAnimation(HitCostumeCount, "Hit")

        End Select
    End Sub

    Private Sub UpdateAnimation(ByVal interval As Integer)
        EnemyAnimationTimer.Interval = TimeSpan.FromMilliseconds(interval)
    End Sub

    Sub StopEnemyTimers()
        EnemyAnimationTimer.Stop()
    End Sub

    Private Function SpriteAnimation(ByVal CostumeEnd As Integer, ByVal Action As String) As Boolean
        If CostumeCount <= CostumeEnd Then
            Dim resourceName As String = $"{Me.ProjectLocation}Assets\Mob\{EnemyType}\{Action}{FaceDirection}\{Action}{FaceDirection}{CostumeCount}.png"
            Dim imageSource As New BitmapImage()
            imageSource.BeginInit()
            imageSource.UriSource = New Uri(resourceName, UriKind.Absolute)
            imageSource.CacheOption = BitmapCacheOption.OnLoad
            imageSource.EndInit()

            EnemyImage.Source = imageSource

            CostumeCount += 1
            Return False
        Else
            If CurrentState = EnemyState.Dead Then
                isDeadAnimationPlayed = True
            End If

            If CurrentState = EnemyState.Hit Then
                isHitActive = False
            End If
            If CurrentState = EnemyState.Attacking Then
                CurrentState = EnemyState.Running
            End If

            CostumeCount = 1
            Return True
        End If
    End Function

    Sub TypeCharacteristics()
        Select Case EnemyType
            Case "Warhog"
                EnemySpeed = 2
                Damage = 1
                WarhogBoxesPlacement()
            Case "Goblin"
                EnemySpeed = 3
                Damage = 2
                GoblinBoxesPlacement()
        End Select
    End Sub

    Sub GoblinBoxesPlacement()
        Me.FootWidth = 20
        Me.FootHeight = 1

        Me.AttackBoxWidth = 60
        Me.AttackBoxHeight = 35
        Me.AttackBoxLeftDiffInFoot = -25

        Me.ImageTopDiffInFoot = 10000
        Me.ImageLeftDiffInFoot = -70

        Me.HitBoxWidth = 15
        Me.HitBoxHeight = 30
        Me.HitBoxLeftDiffInFoot = -5

        Me.ImageTopDiffInFoot = 100

        If FaceDirection = "Left" Then
            Me.AttackBoxLeftDiffInFoot = -23
        End If
    End Sub

    Sub WarhogBoxesPlacement()
        Me.FootWidth = 20
        Me.FootHeight = 1

        Me.AttackBoxWidth = 10
        Me.AttackBoxHeight = 15
        Me.AttackBoxLeftDiffInFoot = -13


        Me.ImageTopDiffInFoot = 50
        Me.ImageLeftDiffInFoot = -23

        Me.HitBoxWidth = 25
        Me.HitBoxHeight = 10
        Me.HitBoxLeftDiffInFoot = -5


        If FaceDirection = "Left" Then
            Me.AttackBoxLeftDiffInFoot = FootWidth
        End If
    End Sub

End Class


Public Enum EnemyState
    Walking
    Attacking
    Running
    Dead
    Hit
    Idle
End Enum
