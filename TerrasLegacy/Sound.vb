Imports NAudio.Wave

Public Class Sound
    Private waveOut As WaveOut = New WaveOut()
    Private reader As WaveFileReader = Nothing




    Private waveOut1 As WaveOut = New WaveOut()


    Private soundPath As String = ""
    Public Sub PlaySound(soundPath As String)
        reader = New WaveFileReader(soundPath)
        waveOut.Init(reader)
        waveOut.Play()
    End Sub

    Public Sub PlaySound1(soundPath As String)
        reader = New WaveFileReader(soundPath)
        waveOut1.Init(reader)
        waveOut1.Play()
    End Sub

    Public Sub PlaySoundLoop(soundPath As String)
        If reader IsNot Nothing Then
            reader.Dispose()
        End If

        reader = New WaveFileReader(soundPath)
        waveOut.Init(reader)
        AddHandler waveOut.PlaybackStopped, AddressOf OnPlaybackStopped

        waveOut.Play()
    End Sub

    Private Sub OnPlaybackStopped(sender As Object, e As StoppedEventArgs)
        PlaySoundLoop(soundPath)
    End Sub

    Public Sub PausePlayback()
        If waveOut.PlaybackState = PlaybackState.Playing Then
            waveOut.Pause()
        End If
    End Sub
End Class
