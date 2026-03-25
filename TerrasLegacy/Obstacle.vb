Public Class Obstacle
    Public Property CurrentState As ObstacleState

    Public Property Damage As Integer

    Public AttackBox As Rectangle

    Sub New(ByVal rect1 As Rectangle)
        AttackBox = rect1
    End Sub


End Class

Public Enum ObstacleState
    Rest
    Attack
End Enum