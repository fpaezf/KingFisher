Public Class MathHelp
    Public Const TWO_PI As Single = Math.PI * 2

    Protected Sub New()
    End Sub

    Public Shared Function GetDistance(ByVal source As PointF, ByVal target As PointF) As Single
        Dim squareX As Double = CDbl(target.X - source.X)
        squareX *= squareX
        Dim squareY As Double = CDbl(target.Y - source.Y)
        squareY *= squareY
        Return CSng(Math.Sqrt(squareX + squareY))
    End Function

    Public Shared Function GetRadiansTo(ByVal source As PointF, ByVal target As PointF) As Single
        Return WrapAngle(CSng(Math.Atan2(target.Y - source.Y, target.X - source.X)))
    End Function

    Public Shared Function GetPointToward(ByVal source As PointF, ByVal target As PointF, ByVal distance As Single) As PointF
        Dim angle As Single = GetRadiansTo(source, target)
        Return source + New SizeF(CSng(Math.Cos(angle) * distance), CSng(Math.Sin(angle) * distance))
    End Function

    Public Shared Function WrapAngle(ByVal radians As Single) As Single
        While radians < -Math.PI
            radians += TWO_PI
        End While
        While radians > Math.PI
            radians -= TWO_PI
        End While
        Return radians
    End Function
End Class
