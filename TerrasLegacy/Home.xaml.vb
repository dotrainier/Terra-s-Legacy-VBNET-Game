Imports System.Numerics
Imports System.Windows.Threading
Imports System.Windows.Media
Imports System.Windows
Imports NAudio.Wave
Imports NAudio.CoreAudio

Public Class Home
    Private WithEvents OpacityTimer As DispatcherTimer
    Dim isTerraTextAnimateDone = False



    Private waveOut As WaveOut = New WaveOut()
    Private reader As WaveFileReader = Nothing

    Private Sound As Sound
    Private Sound1 As Sound

    Dim ProjectLocation As String
    Sub New()
        InitializeComponent()

        ProjectLocation = AppDomain.CurrentDomain.BaseDirectory
        ProjectLocation = ProjectLocation.Replace("\bin\Debug\net8.0-windows\", "\")

        OpacityTimer = New DispatcherTimer()
        OpacityTimer.Interval = TimeSpan.FromMilliseconds(30)

        AddHandler OpacityTimer.Tick, AddressOf OpacityTimer_Tick

        OpacityTimer.Start()
        terraText.Opacity = 0

        Sound = New Sound()
        Sound1 = New Sound()
    End Sub

    Private Sub OpacityTimer_Tick(sender As Object, e As EventArgs)
        If terraText.Opacity < 1.0 Then ' Opacity values range from 0 to 1
            terraText.Opacity += 0.02
        Else
            OpacityTimer.Stop()
            isTerraTextAnimateDone = False
        End If
    End Sub

    Private Sub startHover(sender As Object, e As MouseEventArgs)

        Dim soundPath = ProjectLocation & "Assets\Sound\001_Hover_01.wav"
        Sound1.PlaySound(soundPath)

        Dim startResourcePath As String = ProjectLocation & "Assets\ui\buttons\startBtnHover.png"
        Dim startImageSource As New BitmapImage(New Uri(startResourcePath))
        Canvas.SetLeft(startBtn, Canvas.GetLeft(startBtn) + 5)

        startBtn.Source = startImageSource

    End Sub

    Private Sub startNotHover(sender As Object, e As MouseEventArgs)

        Dim startResourcePath As String = ProjectLocation & "Assets\ui\buttons\startBtn.png"
        Dim startImageSource As New BitmapImage(New Uri(startResourcePath))
        Canvas.SetLeft(startBtn, Canvas.GetLeft(startBtn) - 5)
        startBtn.Source = startImageSource

    End Sub

    Private Sub exitHover(sender As Object, e As MouseEventArgs)
        Dim soundPath = ProjectLocation & "Assets\Sound\001_Hover_01.wav"
        Sound1.PlaySound(soundPath)

        Dim exitResourcePath As String = ProjectLocation & "Assets\ui\buttons\exitBtnHover.png"
        Dim exitImageSource As New BitmapImage(New Uri(exitResourcePath))

        Canvas.SetLeft(exitBtn, Canvas.GetLeft(exitBtn) + 5)
        exitBtn.Source = exitImageSource

    End Sub

    Private Sub exitNotHover(sender As Object, e As MouseEventArgs)
        Dim exitResourcePath As String = ProjectLocation & "Assets\ui\buttons\exitBtn.png"
        Dim exitImageSource As New BitmapImage(New Uri(exitResourcePath))

        Canvas.SetLeft(exitBtn, Canvas.GetLeft(exitBtn) - 5)
        exitBtn.Source = exitImageSource
    End Sub


    Private Sub HomeLoaded(sender As Object, e As RoutedEventArgs)
        Dim soundPath As String = ProjectLocation & "Assets\Sound\Clement Panchout _ Journey _ 2017.wav"
        Sound.PlaySoundLoop(soundPath)
    End Sub

    Private Sub startBtnClick(sender As Object, e As MouseButtonEventArgs) Handles startBtn.MouseDown
        Sound.PausePlayback()
        Dim soundPath As String = ProjectLocation & "Assets\Sound\MI_SFX 43.wav"
        Sound.PlaySound(soundPath)

        Dim gameStart As New Game()
        gameStart.Show()
        Me.Close()

    End Sub

    Private Sub exitBtnClick(sender As Object, e As MouseButtonEventArgs) Handles exitBtn.MouseDown
        Dim soundPath As String = ProjectLocation & "Assets\Sound\MI_SFX 43.wav"
        Sound.PlaySound(soundPath)
        Application.Current.Shutdown()
    End Sub
End Class
