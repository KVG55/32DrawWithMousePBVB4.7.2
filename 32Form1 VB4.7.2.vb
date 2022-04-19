'This program are complited by KVG

Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Globalization

Public Class Form1


#Region "Declaration"
    Private ReadOnly Random As New Random


    ' The Bitmap and Graphics objects we will draw on.
    Private m_Bitmap As Bitmap
    Private m_Graphics As Graphics


    Private m_Drawing As Boolean

    'Private s As String = OpenFileDialog1.FileName

    ' Private bm1 As Image = New Bitmap(s)


    Private m_X As Integer
    Private m_Y As Integer

    Private s As String


    Private ControlMouse As Boolean
    Private ReadOnly B As Boolean

    ReadOnly LineCap As New LineCap


    ReadOnly penLine As New Pen(Color.Black)


    ReadOnly Brush1 As New SolidBrush(Color.Black)


    Private a As Double = 0.1

    Private originalImage As Image = Nothing

    Private currentImage As Image = Nothing

    Private M_D1 As New Point

    Private MD1, MD2, MD3 As New Point

    Private MD_1, MD_2, MD_3, MD_4 As New Point


    Private _MD_1, _MD_2, _MD_3, _MD_4, _MD_5, _MD_6, _MD_7 As New Point


    Private _MD_1_, _MD_2_, _MD_3_, _MD_4_, _MD_5_, _MD_6_, _MD_7_ As New Point


    Private MD_1_, MD_2_, MD_3_, MD_4_, MD_5_, MD_6_, MD_7_, MD_8_, MD_9_, MD_10_ As Point


    Private ReadOnly m_Points0(3) As Point


    Private ReadOnly m_Points2(7) As Point



    Private ReadOnly m_Points1(10) As Point



    Private AllPoints2 As Integer = -1 'for you point, inspite the first curve may be from 00.


    Private AllPoints() As Point
    Private PointLim As Integer
    Dim scanPoint As Point
    Const PointSize As Integer = 1



    'Besier

    Dim startP, PointC1, PointC2, EndP As New Point


    Private ReadOnly m_Points(4) As Point


    Private AllPoints1 As Integer = -1 'for you point, inspite the first Besier may be from 00.


    Private I As Integer



    'объявление и декларация переменной строки для текстурирированной браши.
    'string variable for texture Brash
    Private str As String

    ReadOnly SelectionArea As New StretchableL(Me) ' Class StretchableL

#End Region

#Region " Class RB"
    Public Class StretchableL
        ' Переменные уровня класса
        Private BasePoint As Point
        Private ExtentPoint As Point
        Private CurrentState As StretchableLState


        Private ReadOnly BaseControl As Control
        '   Текущее состояние рисования
        Public Enum StretchableLState
            Inactive
            FirstTime
            Active
        End Enum

        Public Sub New(ByVal useControl As Control)
            ' Конструктор с одним параметром
            BaseControl = useControl

        End Sub
        Public ReadOnly Property Rectangle() As Rectangle
            Get
                'Возвращаем границы области, указанной резиновой линией
                Dim result As Rectangle
                'Отсчет координат с левого верхнего угла вниз и направо.
                result.X = If(BasePoint.X < ExtentPoint.X, BasePoint.X, ExtentPoint.X)
                result.Y = If(BasePoint.Y < ExtentPoint.Y, BasePoint.Y, ExtentPoint.Y)
                result.Width = Math.Abs(ExtentPoint.X - BasePoint.X)
                result.Height = Math.Abs(ExtentPoint.Y - BasePoint.Y)
                Return result
            End Get
        End Property
        Public Sub Start(ByVal x As Integer, ByVal y As Integer)
            ' начинаем рисовать
            ' прорисовывать первое изображение линии, необходимо 
            'вызваать метод Stretch().
            BasePoint.X = x
            BasePoint.Y = y
            ExtentPoint.X = x
            ExtentPoint.Y = y
            Normalize(BasePoint)
            CurrentState = StretchableLState.FirstTime
        End Sub
        Public Sub Stretch(ByVal x As Integer, ByVal y As Integer)
            'Меняем размер линии
            Dim NewPoint As Point
            'Подготавливаем новую точку для растяжения
            NewPoint.X = x
            NewPoint.Y = y
            Normalize(NewPoint)
            Select Case CurrentState
                Case StretchableLState.Inactive
                    'Линия не активна
                    Return
                Case StretchableLState.FirstTime
                    'рисуем начальную линию
                    ExtentPoint = NewPoint
                    DrawTheRectangle()
                    CurrentState = StretchableLState.Active
                Case StretchableLState.Active
                    'удаляем предыдущуб линию
                    'потом рисуем новую
                    DrawTheRectangle()
                    ExtentPoint = NewPoint
                    DrawTheRectangle()
            End Select
        End Sub
        Public Sub Finish()
            'прекращаем рисовать линию
            DrawTheRectangle()
            CurrentState = 0
        End Sub
        Private Sub Normalize(ByRef whichPoint As Point)
            ' не даем линии покидать область видимости
            If (whichPoint.X < 0) Then whichPoint.X = 0
            If (whichPoint.X >= BaseControl.ClientSize.Width) Then whichPoint.X = BaseControl.ClientSize.Width - 1

            If (whichPoint.Y < 0) Then whichPoint.Y = 0
            If (whichPoint.Y >= BaseControl.ClientSize.Height) Then whichPoint.Y = BaseControl.ClientSize.Height - 1

        End Sub
        Private Sub DrawTheRectangle()
            'Рисуем прямоугольник на поверхности формы
            ' или элемента управления
            Dim drawArea As Rectangle
            Dim screenStart, screenEnd As Point
            'получаем квадрат площадь линии
            screenStart = BaseControl.PointToScreen(BasePoint)
            screenEnd = BaseControl.PointToScreen(ExtentPoint)
            drawArea.X = screenStart.X
            drawArea.Y = screenStart.Y
            drawArea.Width = (screenEnd.X - screenStart.X)
            drawArea.Height = (screenEnd.Y - screenStart.Y)
            ' рисуем применяя стиль
            ControlPaint.DrawReversibleFrame(drawArea, Color.Transparent, FrameStyle.Dashed)

        End Sub
    End Class

#End Region

#Region "For Form Load"

    ' Make the initial blank image.
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try


            MakeNewBitmap()

            'RemoveHandler PictureBox1.MouseMove, AddressOf PictureBox1_MouseMove
            'you may just naw create the object of graphics or RemoveHandler MouseMove to the moment of such creation 
            'so that user only after the activation of the option New, can be able to draw

            TextureBrushPictureToolStripMenuItem.Enabled = False

            CopyFromScreenRBToolStripMenuItem.Enabled = False

            Dim i1 As Integer
            For i1 = 1 To 300


                ToolStripComboBox1.Items.Add(i1)

            Next
            ToolStripComboBox1.SelectedItem = 1




            'Text

            Dim i15 As Integer
            For i15 = 1 To 300

                ComboBox2.Items.Add(i15)

            Next
            ComboBox2.SelectedItem = 12

            Dim i117 As Integer
            For i117 = 1 To 167
                'Если цвет не "Transparent":
                If i117 <> 27 Then _
            ComboBox3.Items.Add(Color.FromKnownColor((CType(i117, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox3.Sorted = True
            ' выбор пункта
            ComboBox3.SelectedItem = Color.Black



            Dim i2 As Integer
            For i2 = 1 To 300


                ComboBox4.Items.Add(i2)

            Next
            ComboBox4.SelectedItem = 10

            Dim i3 As Integer
            For i3 = 1 To 300


                ComboBox5.Items.Add(i3)

            Next
            ComboBox5.SelectedItem = 10


            Dim i4 As Integer
            For i4 = 1 To 200


                ComboBox6.Items.Add(i4)

            Next
            ComboBox6.SelectedItem = 100


            Dim i5 As Integer
            For i5 = 1 To 1000


                ComboBox7.Items.Add(i5)

            Next
            ComboBox7.SelectedItem = 150


            Dim i7 As Integer
            For i7 = 1 To 10


                ComboBox8.Items.Add(i7)

            Next
            ComboBox8.SelectedItem = 1

            ComboBox1.SelectedItem = "Times New Roman"

            RadioButton1.Checked = True

            'Dash/Cap

            For Each cap As LineCap In System.Enum.GetValues(GetType(LineCap))
                ComboBox9.Items.Add(cap) 'startCap
                ComboBox9.SelectedItem = LineCap.Flat
                ComboBox10.Items.Add(cap) 'EndCap
                ComboBox10.SelectedItem = LineCap.Round
            Next

            ' Set up Dash Cap.
            For Each dash As DashCap In System.Enum.GetValues(GetType(DashCap))
                ComboBox11.Items.Add(dash) 'Dash Style
                ComboBox11.SelectedItem = DashCap.Flat
            Next

            ' Set up Line Join
            For Each style As DashStyle In System.Enum.GetValues(GetType(DashStyle))
                ComboBox12.Items.Add(style) 'Line Style
                ComboBox12.SelectedItem = DashStyle.Solid
            Next

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ToolStripComboBox2.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ToolStripComboBox2.Sorted = True
            ' выбор пункта
            ToolStripComboBox2.SelectedItem = Color.Fuchsia



            'GR pl

            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox13.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox13.Sorted = True
            ' выбор пункта
            ComboBox13.SelectedItem = Color.Navy



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox14.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox14.Sorted = True
            ' выбор пункта
            ComboBox14.SelectedItem = Color.Green



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox15.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox15.Sorted = True
            ' выбор пункта
            ComboBox15.SelectedItem = Color.Red



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox16.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox16.Sorted = True
            ' выбор пункта
            ComboBox16.SelectedItem = Color.Crimson



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox17.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox17.Sorted = True
            ' выбор пункта
            ComboBox17.SelectedItem = Color.Lime



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox18.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox18.Sorted = True
            ' выбор пункта
            ComboBox18.SelectedItem = Color.DarkGreen



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox19.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox19.Sorted = True
            ' выбор пункта
            ComboBox19.SelectedItem = Color.Cyan



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox20.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox20.Sorted = True
            ' выбор пункта
            ComboBox20.SelectedItem = Color.Coral


            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox21.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox21.Sorted = True
            ' выбор пункта
            ComboBox21.SelectedItem = Color.HotPink

            '''''
            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ToolStripComboBox3.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ToolStripComboBox3.Sorted = True
            ' выбор пункта
            ToolStripComboBox3.SelectedItem = Color.Aqua


            '****************************************************


            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox22.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox22.Sorted = True
            ' выбор пункта
            ComboBox22.SelectedItem = Color.Fuchsia



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox23.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox23.Sorted = True
            ' выбор пункта
            ComboBox23.SelectedItem = Color.Navy



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox24.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox24.Sorted = True
            ' выбор пункта
            ComboBox24.SelectedItem = Color.Green



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox25.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox25.Sorted = True
            ' выбор пункта
            ComboBox25.SelectedItem = Color.Red


            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox26.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox26.Sorted = True
            ' выбор пункта
            ComboBox26.SelectedItem = Color.Crimson



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox27.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox27.Sorted = True
            ' выбор пункта
            ComboBox27.SelectedItem = Color.Lime



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox28.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox28.Sorted = True
            ' выбор пункта
            ComboBox28.SelectedItem = Color.DarkGreen



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox29.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox29.Sorted = True
            ' выбор пункта
            ComboBox29.SelectedItem = Color.Cyan



            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox30.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox30.Sorted = True
            ' выбор пункта
            ComboBox30.SelectedItem = Color.Coral


            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox31.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox31.Sorted = True
            ' выбор пункта
            ComboBox31.SelectedItem = Color.HotPink


            For I = 1 To 167
                'Если цвет не "Transparent":
                If I <> 27 Then _
        ComboBox32.Items.Add(Color.FromKnownColor((CType(I, KnownColor))))
            Next
            'Сортировать записи в алфавитном порядке:
            ComboBox32.Sorted = True
            ' выбор пункта
            ComboBox32.SelectedItem = Color.Aqua



        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region " Canvas, New from Menu,Copy, Past in Click"
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click, ToolStripButton4.Click
        Try

            MakeNewBitmap()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub



    ' Make a new Bitmap to fit the canvas.
    Private Sub MakeNewBitmap()
        ' Get the drawing surface's size.
        Dim wid As Integer = PictureBox1.ClientSize.Width
        Dim hgt As Integer = PictureBox1.ClientSize.Height

        ' Make a Bitmap and Graphics to fit.
        m_Bitmap = New Bitmap(wid, hgt)
        m_Graphics = Graphics.FromImage(m_Bitmap)

        ' Clear the drawing area.
        m_Graphics.Clear(Me.BackColor)

        ' Display the result.
        PictureBox1.Image = m_Bitmap


        m_Points1.ToList.Clear()

        scanPoint.ToString.ToList.Clear()


        PictureBox1.Refresh()

        PointsControlToolStripMenuItem.BackColor = Color.Red

        Delate()

        PictureBox1.Refresh()

    End Sub


    Function Delate() As Graphics
        Dim bmp As Bitmap
        bmp = New Bitmap(Width, Height)
        Dim G As Graphics
        BackgroundImage = bmp
        G = Graphics.FromImage(bmp)
        Return G
    End Function




    ''' <summary>
    ''' first copy from screen and save into the file, then past from CB
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox1_MouseDown100(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown

        Try


            If e.Button = MouseButtons.Left Then

                If RadioButton41.Checked Then

                    m_Drawing = True

                    RadioButton41.BackColor = Color.Green
                    RadioButton41.ForeColor = Color.White
                    RadioButton41.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        Dim screenPoint As Point
                        screenPoint = PointToScreen(New Point(e.X, e.Y))


                        Dim bm1 As Image = Clipboard.GetImage()



                        m_Graphics.DrawImage(bm1, screenPoint)

                    End Using



                    PictureBox1.Refresh()



                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

#End Region

#Region "Option of drawing"

#Region "Mouse down"
    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        Try
            ControlMouse = True

            If e.Button = MouseButtons.Left Then

                If RadioButton2.Checked Then

                    m_Drawing = True

                    RadioButton2.BackColor = Color.Green
                    RadioButton2.ForeColor = Color.White
                    RadioButton2.Invalidate()


                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton3.Checked Then
                    m_Drawing = True


                    RadioButton3.BackColor = Color.Green
                    RadioButton3.ForeColor = Color.White

                    RadioButton3.Invalidate()

                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton4.Checked Then
                    m_Drawing = True
                    RadioButton4.BackColor = Color.Green
                    RadioButton4.ForeColor = Color.White
                    RadioButton4.Invalidate()

                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton5.Checked Then

                    m_Drawing = True

                    RadioButton5.BackColor = Color.Green
                    RadioButton5.ForeColor = Color.White
                    RadioButton5.Invalidate()

                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton6.Checked Then
                    m_Drawing = True
                    RadioButton6.BackColor = Color.Green
                    RadioButton6.ForeColor = Color.White
                    RadioButton6.Invalidate()



                    m_X = e.X
                    m_Y = e.Y



                ElseIf RadioButton7.Checked Then
                    m_Drawing = True
                    RadioButton7.BackColor = Color.Green
                    RadioButton7.ForeColor = Color.White
                    RadioButton7.Invalidate()



                ElseIf RadioButton8.Checked Then
                    m_Drawing = True
                    RadioButton8.BackColor = Color.Green
                    RadioButton8.ForeColor = Color.White

                    RadioButton8.Invalidate()



                ElseIf RadioButton9.Checked Then
                    m_Drawing = True

                    RadioButton9.BackColor = Color.Green
                    RadioButton9.ForeColor = Color.White
                    RadioButton9.Invalidate()


                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton10.Checked Then

                    RadioButton10.BackColor = Color.Green
                    RadioButton10.ForeColor = Color.White
                    RadioButton10.Invalidate()

                    m_Drawing = True

                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton11.Checked Then
                    m_Drawing = True

                    RadioButton11.BackColor = Color.Green
                    RadioButton11.ForeColor = Color.White
                    RadioButton11.Invalidate()



                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton12.Checked Then

                    RadioButton12.BackColor = Color.Green
                    RadioButton12.ForeColor = Color.White
                    RadioButton12.Invalidate()

                    m_Drawing = True

                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton13.Checked Then
                    m_Drawing = True

                    RadioButton13.BackColor = Color.Green
                    RadioButton13.ForeColor = Color.White
                    RadioButton13.Invalidate()



                    m_X = e.X
                    m_Y = e.Y


                ElseIf RadioButton14.Checked Then
                    m_Drawing = True

                    RadioButton14.BackColor = Color.Green
                    RadioButton14.ForeColor = Color.White
                    RadioButton14.Invalidate()


                    m_X = e.X
                    m_Y = e.Y

                ElseIf RadioButton15.Checked Then
                    If ([String].IsNullOrEmpty(str)) Then Exit Sub

                    If TextureBrushPictureToolStripMenuItem.Enabled = True Then

                        If ([String].IsNullOrEmpty(str)) Then Exit Sub
                        m_Drawing = True

                        RadioButton15.BackColor = Color.Green
                        RadioButton15.ForeColor = Color.White
                        RadioButton15.Invalidate()

                        m_X = e.X
                        m_Y = e.Y
                    Else
                        Exit Sub
                    End If



                ElseIf RadioButton17.Checked Then


                    m_Drawing = True

                    RadioButton17.BackColor = Color.Green
                    RadioButton17.ForeColor = Color.White

                    RadioButton17.Invalidate()

                    PointLim = 0

                    ReDim AllPoints(PointLim)
                    AllPoints(PointLim).X = e.X
                    AllPoints(PointLim).Y = e.Y

                ElseIf RadioButton18.Checked Then

                    m_Drawing = True

                    RadioButton18.BackColor = Color.Green
                    RadioButton18.ForeColor = Color.White
                    RadioButton18.Invalidate()


                    PointLim = 0

                    ReDim AllPoints(PointLim)
                    AllPoints(PointLim).X = e.X
                    AllPoints(PointLim).Y = e.Y

                ElseIf RadioButton20.Checked Then


                    If TextureBrushPictureToolStripMenuItem.Enabled = True Then

                        m_Drawing = True

                        RadioButton20.BackColor = Color.Green
                        RadioButton20.ForeColor = Color.White
                        RadioButton20.Invalidate()

                        m_X = e.X
                        m_Y = e.Y
                    Else
                        Exit Sub
                    End If


                ElseIf RadioButton26.Checked Then

                    m_Drawing = True
                    RadioButton26.BackColor = Color.Green
                    RadioButton26.ForeColor = Color.White
                    RadioButton26.Invalidate()

                    m_X = e.X
                    m_Y = e.Y


                ElseIf RadioButton28.Checked Then

                    m_Drawing = True
                    RadioButton28.BackColor = Color.Green
                    RadioButton28.ForeColor = Color.White
                    RadioButton28.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints1 = 4 Then
                            m_Points.ToList.Clear()
                            AllPoints1 = 0
                        Else
                            AllPoints1 += 1
                        End If


                        m_Points(AllPoints1).X = e.X
                        m_Points(AllPoints1).Y = e.Y


                        startP = m_Points(0)

                        PointC1 = m_Points(1)

                        PointC2 = m_Points(2)

                        EndP = m_Points(3)



                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints1 <= 4 Then

                                For Each scanPoint In m_Points



                                    m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                Next scanPoint

                            Else
                                AllPoints1 += 1

                            End If


                        End If

                        PictureBox1.Refresh()

                    End Using
                ElseIf RadioButton29.Checked Then

                    m_Drawing = True
                    RadioButton29.BackColor = Color.Green
                    RadioButton29.ForeColor = Color.White
                    RadioButton29.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 7 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        _MD_1 = m_Points2(0)
                        _MD_2 = m_Points2(1)
                        _MD_3 = m_Points2(2)
                        _MD_4 = m_Points2(3)
                        _MD_5 = m_Points2(4)
                        _MD_6 = m_Points2(5)
                        _MD_7 = m_Points2(6)



                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 7 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                    scanPoint.X - PointSize,
                                    scanPoint.Y - PointSize,
                                    PointSize * 2, PointSize * 2)

                                Next scanPoint
                                ' m_Points.ToList.Clear()
                            Else
                                AllPoints2 += 1

                            End If


                        End If


                    End Using


                    PictureBox1.Refresh()

                ElseIf RadioButton30.Checked Then

                    m_Drawing = True
                    RadioButton30.BackColor = Color.Green
                    RadioButton30.ForeColor = Color.White
                    RadioButton30.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 7 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        _MD_1 = m_Points2(0)
                        _MD_2 = m_Points2(1)
                        _MD_3 = m_Points2(2)
                        _MD_4 = m_Points2(3)
                        _MD_5 = m_Points2(4)
                        _MD_6 = m_Points2(5)
                        _MD_7 = m_Points2(6)



                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 7 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                    scanPoint.X - PointSize,
                                    scanPoint.Y - PointSize,
                                    PointSize * 2, PointSize * 2)

                                Next scanPoint
                                ' m_Points.ToList.Clear()
                            Else
                                AllPoints2 += 1

                            End If


                        End If


                    End Using


                    PictureBox1.Refresh()


                ElseIf RadioButton31.Checked = True Then

                    m_Drawing = True
                    RadioButton31.BackColor = Color.Green
                    RadioButton31.ForeColor = Color.White
                    RadioButton31.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 4 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        MD_1 = m_Points2(0)
                        MD_2 = m_Points2(1)
                        MD_3 = m_Points2(2)
                        MD_4 = m_Points2(3)


                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 4 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                    scanPoint.X - PointSize,
                                    scanPoint.Y - PointSize,
                                    PointSize * 2, PointSize * 2)

                                Next scanPoint
                                ' m_Points.ToList.Clear()
                            Else
                                AllPoints2 += 1

                            End If


                        End If







                    End Using


                    PictureBox1.Refresh()


                ElseIf RadioButton32.Checked = True Then

                    m_Drawing = True
                    RadioButton32.BackColor = Color.Green
                    RadioButton32.ForeColor = Color.White
                    RadioButton32.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 4 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        MD_1 = m_Points2(0)
                        MD_2 = m_Points2(1)
                        MD_3 = m_Points2(2)
                        MD_4 = m_Points2(3)


                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 4 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                    scanPoint.X - PointSize,
                                    scanPoint.Y - PointSize,
                                    PointSize * 2, PointSize * 2)

                                Next scanPoint
                                ' m_Points.ToList.Clear()
                            Else
                                AllPoints2 += 1

                            End If


                        End If



                    End Using


                    PictureBox1.Refresh()

                ElseIf RadioButton33.Checked = True Then

                    m_Drawing = True
                    RadioButton33.BackColor = Color.Green
                    RadioButton33.ForeColor = Color.White
                    RadioButton33.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 7 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        _MD_1_ = m_Points2(0)
                        _MD_2_ = m_Points2(1)
                        _MD_3_ = m_Points2(2)
                        _MD_4_ = m_Points2(3)
                        _MD_5_ = m_Points2(4)
                        _MD_6_ = m_Points2(5)
                        _MD_7_ = m_Points2(6)



                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 7 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                Next scanPoint

                            Else
                                AllPoints2 += 1

                            End If


                        End If







                    End Using


                    PictureBox1.Refresh()


                ElseIf RadioButton34.Checked = True Then

                    m_Drawing = True
                    RadioButton33.BackColor = Color.Green
                    RadioButton33.ForeColor = Color.White
                    RadioButton33.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 7 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        _MD_1_ = m_Points2(0)
                        _MD_2_ = m_Points2(1)
                        _MD_3_ = m_Points2(2)
                        _MD_4_ = m_Points2(3)
                        _MD_5_ = m_Points2(4)
                        _MD_6_ = m_Points2(5)
                        _MD_7_ = m_Points2(6)



                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 7 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                Next scanPoint

                            Else
                                AllPoints2 += 1

                            End If


                        End If

                    End Using


                    PictureBox1.Refresh()


                ElseIf RadioButton35.Checked = True Then

                    m_Drawing = True
                    RadioButton35.BackColor = Color.Green
                    RadioButton35.ForeColor = Color.White
                    RadioButton35.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 7 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        _MD_1_ = m_Points2(0)
                        _MD_2_ = m_Points2(1)
                        _MD_3_ = m_Points2(2)
                        _MD_4_ = m_Points2(3)
                        _MD_5_ = m_Points2(4)
                        _MD_6_ = m_Points2(5)
                        _MD_7_ = m_Points2(6)



                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 7 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                Next scanPoint

                            Else
                                AllPoints2 += 1

                            End If


                        End If


                    End Using


                    PictureBox1.Refresh()

                ElseIf RadioButton37.Checked = True Then

                    m_Drawing = True
                    RadioButton37.BackColor = Color.Green
                    RadioButton37.ForeColor = Color.White
                    RadioButton37.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 3 Then '
                            m_Points0.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points0(AllPoints2).X = e.X
                        m_Points0(AllPoints2).Y = e.Y


                        MD1 = m_Points0(0)
                        MD2 = m_Points0(1)
                        MD3 = m_Points0(2)


                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 3 Then

                                For Each scanPoint In m_Points0



                                    m_Graphics.FillEllipse(Brushes.Black,
                                    scanPoint.X - PointSize,
                                    scanPoint.Y - PointSize,
                                    PointSize * 2, PointSize * 2)

                                Next scanPoint
                                ' m_Points.ToList.Clear()
                            Else
                                AllPoints2 += 1

                            End If


                        End If


                    End Using


                    PictureBox1.Refresh()

                ElseIf RadioButton38.Checked = True Then

                    m_Drawing = True

                    RadioButton38.BackColor = Color.White
                    RadioButton38.ForeColor = Color.Black
                    RadioButton38.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 7 Then
                            m_Points.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points2(AllPoints2).X = e.X
                        m_Points2(AllPoints2).Y = e.Y


                        _MD_1_ = m_Points2(0)
                        _MD_2_ = m_Points2(1)
                        _MD_3_ = m_Points2(2)
                        _MD_4_ = m_Points2(3)
                        _MD_5_ = m_Points2(4)
                        _MD_6_ = m_Points2(5)
                        _MD_7_ = m_Points2(6)



                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 7 Then

                                For Each scanPoint In m_Points2



                                    m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                Next scanPoint
                                ' m_Points.ToList.Clear()
                            Else
                                AllPoints2 += 1

                            End If


                        End If


                    End Using


                    PictureBox1.Refresh()


                ElseIf RadioButton39.Checked = True Then

                    m_Drawing = True

                    RadioButton39.BackColor = Color.White
                    RadioButton39.ForeColor = Color.Black
                    RadioButton39.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints2 >= 10 Then
                            m_Points1.ToList.Clear()
                            AllPoints2 = 0
                        Else
                            AllPoints2 += 1
                        End If


                        m_Points1(AllPoints2).X = e.X
                        m_Points1(AllPoints2).Y = e.Y


                        MD_1_ = m_Points1(0)
                        MD_2_ = m_Points1(1)
                        MD_3_ = m_Points1(2)
                        MD_4_ = m_Points1(3)
                        MD_5_ = m_Points1(4)
                        MD_6_ = m_Points1(5)
                        MD_7_ = m_Points1(6)
                        MD_8_ = m_Points1(7)
                        MD_9_ = m_Points1(8)
                        MD_10_ = m_Points1(9)


                        If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                            If AllPoints2 <= 10 Then

                                For Each scanPoint In m_Points1



                                    m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                Next scanPoint
                                ' m_Points.ToList.Clear()
                            Else
                                AllPoints2 += 1

                            End If


                        End If


                    End Using


                    PictureBox1.Refresh()





                ElseIf RadioButton40.Checked Then

                    If CopyFromScreenRBToolStripMenuItem.Enabled = True Then


                        m_Drawing = True

                        RadioButton40.BackColor = Color.Green
                        RadioButton40.ForeColor = Color.White

                        RadioButton40.Invalidate()

                        If e.X = 0 Then Exit Sub
                        If e.Y = 0 Then Exit Sub



                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                            SelectionArea.Start(e.X, e.Y)

                            m_X = e.X
                            m_Y = e.Y


                        End Using


                    Else
                        Exit Sub
                    End If



                ElseIf RadioButton43.Checked Then
                    m_Drawing = True

                    RadioButton43.BackColor = Color.Green
                    RadioButton43.ForeColor = Color.White

                    RadioButton16.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Start(e.X, e.Y)

                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()


                    End Using


                ElseIf RadioButton46.Checked Then
                    m_Drawing = True

                    RadioButton46.BackColor = Color.Green
                    RadioButton46.ForeColor = Color.White

                    RadioButton46.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Start(e.X, e.Y)
                    End Using

                ElseIf RadioButton47.Checked Then
                    m_Drawing = True

                    RadioButton47.BackColor = Color.Green
                    RadioButton47.ForeColor = Color.White

                    RadioButton47.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Start(e.X, e.Y)
                    End Using



                ElseIf RadioButton48.Checked Then
                    m_Drawing = True

                    RadioButton48.BackColor = Color.Green
                    RadioButton48.ForeColor = Color.White

                    RadioButton48.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Start(e.X, e.Y)
                    End Using

                ElseIf RadioButton49.Checked Then
                    m_Drawing = True

                    RadioButton49.BackColor = Color.Green
                    RadioButton49.ForeColor = Color.White

                    RadioButton49.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Start(e.X, e.Y)
                    End Using

                End If
                m_Graphics.Dispose()

            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub


#End Region

#Region "Mouse Move"
    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        Try

            ToolStripTextBox2.Text = String.Format("X = {0} или {1}",
                                     MousePosition.X, e.X)
            ToolStripTextBox3.Text = String.Format("Y = {0} или {1}",
                                               MousePosition.Y, e.Y)


            If e.Button = MouseButtons.Left Then


                ControlMouse = True


                If RadioButton2.Checked Then

                    If Not m_Drawing Then Exit Sub

                    RadioButton2.BackColor = Color.Green
                    RadioButton2.ForeColor = Color.White
                    RadioButton2.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        m_Graphics.DrawLine(penLine, m_X, m_Y, e.X, e.Y)


                        m_X = e.X
                        m_Y = e.Y


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()



                ElseIf RadioButton3.Checked Then

                    If Not m_Drawing Then Return

                    RadioButton3.BackColor = Color.Green
                    RadioButton3.ForeColor = Color.White
                    RadioButton3.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        m_Graphics.DrawLine(penLine, m_X, m_Y, e.X, e.Y)

                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()



                ElseIf RadioButton4.Checked Then

                    If Not m_Drawing Then Return

                    RadioButton4.BackColor = Color.Green
                    RadioButton4.ForeColor = Color.White
                    RadioButton4.Invalidate()


                ElseIf RadioButton5.Checked Then

                    If Not m_Drawing Then Return

                    RadioButton5.BackColor = Color.Green
                    RadioButton5.ForeColor = Color.White
                    RadioButton5.Invalidate()

                ElseIf RadioButton6.Checked Then

                    If Not m_Drawing Then Return
                    RadioButton6.BackColor = Color.Green
                    RadioButton6.ForeColor = Color.White
                    RadioButton6.Invalidate()


                ElseIf RadioButton7.Checked Then
                    m_Drawing = True
                    RadioButton7.BackColor = Color.Green
                    RadioButton7.ForeColor = Color.White
                    RadioButton7.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias




                        Dim Rect = New Rectangle(e.X, e.Y, CInt(NumericUpDown2.Value), CInt(NumericUpDown3.Value))

                        m_Graphics.FillEllipse(Brush1, Rect)



                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton8.Checked Then
                    m_Drawing = True

                    RadioButton8.BackColor = Color.Green
                    RadioButton8.ForeColor = Color.White
                    RadioButton8.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias




                        Dim Rect = New Rectangle(e.X, e.Y, CInt(NumericUpDown2.Value), CInt(NumericUpDown3.Value))

                        m_Graphics.FillRectangle(Brush1, Rect)



                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()


                ElseIf RadioButton9.Checked Then
                    If Not m_Drawing Then Return

                    RadioButton9.BackColor = Color.Green
                    RadioButton9.ForeColor = Color.White
                    RadioButton9.Invalidate()

                ElseIf RadioButton10.Checked Then

                    If Not m_Drawing Then Return
                    RadioButton10.BackColor = Color.Green
                    RadioButton10.ForeColor = Color.White
                    RadioButton10.Invalidate()

                ElseIf RadioButton11.Checked Then
                    If Not m_Drawing Then Return

                    RadioButton11.BackColor = Color.Green
                    RadioButton11.ForeColor = Color.White
                    RadioButton11.Invalidate()

                ElseIf RadioButton12.Checked Then

                    If Not m_Drawing Then Return
                    RadioButton12.BackColor = Color.Green
                    RadioButton12.ForeColor = Color.White
                    RadioButton12.Invalidate()

                ElseIf RadioButton13.Checked Then
                    m_Drawing = True
                    RadioButton13.BackColor = Color.Green
                    RadioButton13.ForeColor = Color.White
                    RadioButton13.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias




                        Dim Rect = New Rectangle(e.X, e.Y, CInt(NumericUpDown2.Value), CInt(NumericUpDown3.Value))



                        Dim GrBrush As LinearGradientBrush

                        GrBrush = New LinearGradientBrush(Rect, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color), NumericUpDown4.Value)



                        m_Graphics.FillRectangle(GrBrush, Rect)



                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()


                ElseIf RadioButton14.Checked Then

                    m_Drawing = True

                    RadioButton14.BackColor = Color.Green
                    RadioButton14.ForeColor = Color.White
                    RadioButton14.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias




                        Dim Rect = New Rectangle(e.X, e.Y, CInt(NumericUpDown2.Value), CInt(NumericUpDown3.Value))



                        Dim GrBrush As LinearGradientBrush

                        GrBrush = New LinearGradientBrush(Rect, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color), NumericUpDown4.Value)



                        m_Graphics.FillEllipse(GrBrush, Rect)



                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()


                ElseIf RadioButton15.Checked Then

                    If TextureBrushPictureToolStripMenuItem.Enabled = True Then

                        If ([String].IsNullOrEmpty(str)) Then Exit Sub

                        m_Drawing = True

                        RadioButton15.BackColor = Color.Green
                        RadioButton15.ForeColor = Color.White

                        RadioButton15.Invalidate()





                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                            Dim texture As New TextureBrush(Image.FromFile(str))





                            Dim Rect = New Rectangle(e.X, e.Y, CInt(NumericUpDown2.Value), CInt(NumericUpDown3.Value))


                            m_Graphics.FillEllipse(texture, Rect)


                        End Using

                        ' Display the result.
                        PictureBox1.Refresh()
                    Else
                        Exit Sub
                    End If



                ElseIf RadioButton17.Checked Then

                    If Not m_Drawing Then Exit Sub



                    RadioButton17.BackColor = Color.Green
                    RadioButton17.ForeColor = Color.White

                    RadioButton17.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        PointLim += 1
                        ReDim Preserve AllPoints(PointLim)
                        AllPoints(PointLim).X = e.X
                        AllPoints(PointLim).Y = e.Y

                        ' Dim demoPen As New Pen(penColor, ToolStripComboBox1.SelectedItem)
                        ' demoPen.Color = penColor

                        m_Graphics.DrawLine(penLine,
                AllPoints(PointLim - 1), AllPoints(PointLim))


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton18.Checked Then

                    If Not m_Drawing Then Exit Sub

                    RadioButton18.BackColor = Color.Green
                    RadioButton18.ForeColor = Color.White
                    RadioButton18.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        PointLim += 1
                        ReDim Preserve AllPoints(PointLim)
                        AllPoints(PointLim).X = e.X
                        AllPoints(PointLim).Y = e.Y

                        ' Dim demoPen As New Pen(penColor, ToolStripComboBox1.SelectedItem)
                        ' demoPen.Color = penColor

                        m_Graphics.DrawLine(penLine,
                AllPoints(PointLim - 1), AllPoints(PointLim))


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton20.Checked Then

                    If TextureBrushPictureToolStripMenuItem.Enabled = True Then
                        If ([String].IsNullOrEmpty(str)) Then Exit Sub

                        If Not m_Drawing Then Exit Sub

                        RadioButton20.BackColor = Color.Green
                        RadioButton20.ForeColor = Color.White

                        RadioButton20.Invalidate()


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                            Dim Rect1 = New Rectangle(e.X, e.Y, CInt(NumericUpDown8.Value), CInt(NumericUpDown9.Value))



                            Dim GrBrush As LinearGradientBrush

                            GrBrush = New LinearGradientBrush(Rect1, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color), NumericUpDown4.Value)


                            Dim texture As New TextureBrush(Image.FromFile(str))

                            Dim Rect = New Rectangle(e.X, e.Y, CInt(NumericUpDown2.Value), CInt(NumericUpDown3.Value))

                            'последовательность строк значима, the consequence of the line are meaningfull.

                            m_Graphics.FillRectangle(GrBrush, Rect1)


                            m_Graphics.FillEllipse(texture, Rect1)


                        End Using

                        ' Display the result.
                        PictureBox1.Refresh()
                    Else
                        Exit Sub
                    End If


                ElseIf RadioButton26.Checked Then

                    If Not m_Drawing Then Return

                    RadioButton26.BackColor = Color.Green
                    RadioButton26.ForeColor = Color.White
                    RadioButton26.Invalidate()

                ElseIf RadioButton28.Checked Then

                    If Not m_Drawing Then Return
                    RadioButton28.BackColor = Color.Green
                    RadioButton28.ForeColor = Color.White
                    RadioButton28.Invalidate()

                ElseIf RadioButton29.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton29.BackColor = Color.Green
                    RadioButton29.ForeColor = Color.White
                    RadioButton29.Invalidate()

                ElseIf RadioButton30.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton30.BackColor = Color.Green
                    RadioButton30.ForeColor = Color.White
                    RadioButton30.Invalidate()

                ElseIf RadioButton31.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton31.BackColor = Color.Green
                    RadioButton31.ForeColor = Color.White
                    RadioButton31.Invalidate()

                ElseIf RadioButton32.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton32.BackColor = Color.Green
                    RadioButton32.ForeColor = Color.White
                    RadioButton32.Invalidate()

                ElseIf RadioButton33.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton33.BackColor = Color.Green
                    RadioButton33.ForeColor = Color.White
                    RadioButton33.Invalidate()

                ElseIf RadioButton34.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton34.BackColor = Color.Green
                    RadioButton34.ForeColor = Color.White
                    RadioButton34.Invalidate()

                ElseIf RadioButton35.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton35.BackColor = Color.Green
                    RadioButton35.ForeColor = Color.White
                    RadioButton35.Invalidate()

                ElseIf RadioButton37.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton37.BackColor = Color.Green
                    RadioButton37.ForeColor = Color.White
                    RadioButton37.Invalidate()
                ElseIf RadioButton38.Checked Then

                    If Not m_Drawing Then Return
                    RadioButton38.BackColor = Color.Green
                    RadioButton38.ForeColor = Color.White
                    RadioButton38.Invalidate()

                ElseIf RadioButton39.Checked Then
                    If Not m_Drawing Then Return
                    RadioButton39.BackColor = Color.Green
                    RadioButton39.ForeColor = Color.White
                    RadioButton39.Invalidate()

                ElseIf RadioButton40.Checked Then
                    If CopyFromScreenRBToolStripMenuItem.Enabled = True Then



                        m_Drawing = True
                        RadioButton40.BackColor = Color.Green
                        RadioButton40.ForeColor = Color.White
                        If e.X = 0 Then Exit Sub
                        If e.Y = 0 Then Exit Sub


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                            SelectionArea.Stretch(e.X, e.Y)
                        End Using
                    Else
                        Exit Sub
                    End If


                ElseIf RadioButton43.Checked Then


                    m_Drawing = True
                    RadioButton43.BackColor = Color.Green
                    RadioButton43.ForeColor = Color.White
                    RadioButton43.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Stretch(e.X, e.Y)
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()

                    End Using
                ElseIf RadioButton46.Checked Then


                    m_Drawing = True
                    RadioButton46.BackColor = Color.Green
                    RadioButton46.ForeColor = Color.White
                    RadioButton46.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Stretch(e.X, e.Y)
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()

                    End Using

                ElseIf RadioButton47.Checked Then


                    m_Drawing = True
                    RadioButton47.BackColor = Color.Green
                    RadioButton47.ForeColor = Color.White
                    RadioButton47.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Stretch(e.X, e.Y)
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()

                    End Using



                ElseIf RadioButton48.Checked Then


                    m_Drawing = True
                    RadioButton48.BackColor = Color.Green
                    RadioButton48.ForeColor = Color.White
                    RadioButton48.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Stretch(e.X, e.Y)
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()

                    End Using


                ElseIf RadioButton49.Checked Then


                    m_Drawing = True
                    RadioButton49.BackColor = Color.Green
                    RadioButton49.ForeColor = Color.White
                    RadioButton49.Invalidate()



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Stretch(e.X, e.Y)
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()

                    End Using


                End If
                    m_Graphics.Dispose()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub


#End Region

#Region "Mouse Up"
    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        Try


            If e.Button = MouseButtons.Left Then


                ControlMouse = True


                If RadioButton2.Checked Then

                    m_Drawing = False

                    RadioButton2.BackColor = Color.White
                    RadioButton2.ForeColor = Color.Black
                    RadioButton2.Invalidate()


                ElseIf RadioButton3.Checked Then

                    m_Drawing = False

                    RadioButton3.BackColor = Color.White
                    RadioButton3.ForeColor = Color.Black
                    RadioButton3.Invalidate()

                ElseIf RadioButton4.Checked Then
                    m_Drawing = True

                    RadioButton4.BackColor = Color.White
                    RadioButton4.ForeColor = Color.Black
                    RadioButton4.Invalidate()

                    penLine.EndCap() = CType(ComboBox9.SelectedItem, LineCap)


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        m_Graphics.DrawLine(penLine, m_X, m_Y, e.X, e.Y)

                    End Using


                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton5.Checked Then
                    m_Drawing = True

                    RadioButton5.BackColor = Color.White
                    RadioButton5.ForeColor = Color.Black
                    RadioButton5.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        m_Graphics.DrawRectangle(penLine, m_X, m_Y,
                          e.X - m_X, e.Y - m_Y)




                    End Using


                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton6.Checked Then
                    m_Drawing = True

                    RadioButton6.BackColor = Color.White
                    RadioButton6.ForeColor = Color.Black
                    RadioButton6.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        m_Graphics.DrawEllipse(penLine, m_X, m_Y,
                          e.X - m_X, e.Y - m_Y)

                    End Using


                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton7.Checked Then


                    m_Drawing = False
                    RadioButton7.BackColor = Color.White
                    RadioButton7.ForeColor = Color.Black
                    RadioButton7.Invalidate()


                ElseIf RadioButton8.Checked Then

                    m_Drawing = False

                    RadioButton8.BackColor = Color.White
                    RadioButton8.ForeColor = Color.Black
                    RadioButton8.Invalidate()

                ElseIf RadioButton9.Checked Then
                    m_Drawing = True

                    RadioButton9.BackColor = Color.White
                    RadioButton9.ForeColor = Color.Black
                    RadioButton9.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias



                        Dim Rect = New Rectangle(New Point(m_X, m_Y), New Size(e.X - m_X, e.Y - m_Y))

                        m_Graphics.FillRectangle(Brush1, Rect)

                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()



                ElseIf RadioButton10.Checked Then
                    m_Drawing = True

                    RadioButton10.BackColor = Color.White
                    RadioButton10.ForeColor = Color.Black
                    RadioButton10.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias



                        Dim Rect = New Rectangle(New Point(m_X, m_Y), New Size(e.X - m_X, e.Y - m_Y))

                        m_Graphics.FillEllipse(Brush1, Rect)

                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()


                ElseIf RadioButton11.Checked Then

                    m_Drawing = True

                    RadioButton11.BackColor = Color.White
                    RadioButton11.ForeColor = Color.Black
                    RadioButton11.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                        Dim Rect = New Rectangle(New Point(m_X, m_Y), New Size(e.X - m_X, e.Y - m_Y))

                        Dim GrBrush As LinearGradientBrush

                        GrBrush = New LinearGradientBrush(Rect, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color), NumericUpDown4.Value)


                        m_Graphics.FillRectangle(GrBrush, Rect)



                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()


                ElseIf RadioButton12.Checked Then
                    m_Drawing = True

                    RadioButton12.BackColor = Color.White
                    RadioButton12.ForeColor = Color.Black
                    RadioButton12.Invalidate()

                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                        Dim Rect = New Rectangle(New Point(m_X, m_Y), New Size(e.X - m_X, e.Y - m_Y))

                        Dim GrBrush As LinearGradientBrush

                        GrBrush = New LinearGradientBrush(Rect, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color), NumericUpDown4.Value)


                        m_Graphics.FillEllipse(GrBrush, Rect)



                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton13.Checked Then

                    m_Drawing = False
                    RadioButton13.BackColor = Color.White
                    RadioButton13.ForeColor = Color.Black
                    RadioButton13.Invalidate()

                ElseIf RadioButton14.Checked Then


                    m_Drawing = False


                    RadioButton14.BackColor = Color.White
                    RadioButton14.ForeColor = Color.Black
                    RadioButton14.Invalidate()

                ElseIf RadioButton15.Checked Then
                    If TextureBrushPictureToolStripMenuItem.Enabled = True Then

                        m_Drawing = False



                        RadioButton15.BackColor = Color.White
                        RadioButton15.ForeColor = Color.Black
                        RadioButton15.Invalidate()
                    Else
                        Exit Sub
                    End If





                ElseIf RadioButton17.Checked Then

                    m_Drawing = True

                    RadioButton17.BackColor = Color.White
                    RadioButton17.ForeColor = Color.Black
                    RadioButton17.Invalidate()

                    If PointLim < 1 Then Exit Sub


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        ' Dim br As New SolidBrush(Color1)

                        m_Graphics.FillClosedCurve(Brush1, AllPoints)

                        ' Dim demoPen As New Pen(penColor, ToolStripComboBox1.SelectedItem)
                        ' demoPen.Color = penColor
                        m_Graphics.DrawLines(penLine, AllPoints)


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton18.Checked Then

                    m_Drawing = True

                    RadioButton18.BackColor = Color.White
                    RadioButton18.ForeColor = Color.Black
                    RadioButton18.Invalidate()


                    If PointLim < 1 Then Exit Sub


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias



                        Dim Rect As New Rectangle(e.X, e.Y, e.X, e.X)

                        Dim GrBrush As LinearGradientBrush

                        GrBrush = New LinearGradientBrush(Rect, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color), NumericUpDown4.Value)



                        m_Graphics.FillClosedCurve(GrBrush, AllPoints)

                        ' Dim demoPen As New Pen(penColor, ToolStripComboBox1.SelectedItem)
                        ' demoPen.Color = penColor

                        m_Graphics.DrawLines(penLine, AllPoints)


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton20.Checked Then
                    If TextureBrushPictureToolStripMenuItem.Enabled = True Then


                        m_Drawing = False



                        RadioButton20.BackColor = Color.White
                        RadioButton20.ForeColor = Color.Black
                        RadioButton20.Invalidate()
                    Else
                        Exit Sub
                    End If


                ElseIf RadioButton26.Checked Then
                    m_Drawing = True

                    RadioButton26.BackColor = Color.White
                    RadioButton26.ForeColor = Color.Black
                    RadioButton26.Invalidate()

                    penLine.StartCap() = CType(ComboBox9.SelectedItem, LineCap)

                    penLine.EndCap() = CType(ComboBox10.SelectedItem, LineCap)

                    penLine.DashCap() = CType(ComboBox11.SelectedItem, DashCap)

                    penLine.DashStyle() = CType(ComboBox12.SelectedItem, DashStyle)



                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        m_Graphics.DrawLine(penLine, m_X, m_Y, e.X, e.Y)

                    End Using


                    ' Display the result.
                    PictureBox1.Refresh()
                ElseIf RadioButton28.Checked Then


                    RadioButton28.BackColor = Color.White
                    RadioButton28.ForeColor = Color.Black
                    RadioButton28.Invalidate()

                    Using m_Bitmap = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        If AllPoints1 = 4 Then

                            Dim DrawBZPen As New Pen(CType(ToolStripComboBox2.SelectedItem, Color), CSng(ToolStripComboBox1.SelectedItem))

                            '        canvas.DrawBezier
                            m_Bitmap.DrawBezier(DrawBZPen, startP, PointC1, PointC2, EndP)


                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then


                                If AllPoints1 <= 4 Then

                                    For Each scanPoint In m_Points



                                        m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points.ToList.Clear()
                                Else
                                    AllPoints1 += 1

                                End If

                            End If


                        End If



                    End Using

                    m_Points.ToList.Clear()

                    PictureBox1.Refresh()


                ElseIf RadioButton29.Checked Then

                    m_Drawing = True

                    RadioButton29.BackColor = Color.White
                    RadioButton29.ForeColor = Color.Black
                    RadioButton29.Invalidate()


                    If AllPoints2 = 7 Then



                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim curvePoints As Point() =
                            {
                                  _MD_1,
                                  _MD_2,
                                  _MD_3,
                                  _MD_4,
                                  _MD_5,
                                  _MD_6,
                                  _MD_7
                                          }


                            'Dim PenCurve1 As New Pen(CType(ToolStripComboBox2.SelectedItem, Color), ToolStripComboBox1.SelectedItem)


                            'Dim PenCurve2 As New Pen(CType(ToolStripComboBox4.SelectedItem, Color), ToolStripComboBox3.SelectedItem)

                            ' draw curves to screen.

                            m_Graphics.DrawLines(penLine, curvePoints)


                            'm_Graphics.DrawCurve(PenCurve2, curvePoints)



                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                                If AllPoints2 <= 7 Then

                                    For Each scanPoint In m_Points2



                                        m_Graphics.FillEllipse(Brushes.Black,
                            scanPoint.X - PointSize,
                            scanPoint.Y - PointSize,
                            PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points2.ToList.Clear()
                                    scanPoint.ToString.ToList.Clear()

                                Else
                                    AllPoints2 += 1

                                End If


                            End If



                        End Using

                        m_Points2.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()



                        PictureBox1.Refresh()


                    End If

                ElseIf RadioButton30.Checked Then

                    m_Drawing = True

                    RadioButton29.BackColor = Color.White
                    RadioButton29.ForeColor = Color.Black
                    RadioButton29.Invalidate()


                    If AllPoints2 = 7 Then



                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim curvePoints As Point() =
                            {
                                  _MD_1,
                                  _MD_2,
                                  _MD_3,
                                  _MD_4,
                                  _MD_5,
                                  _MD_6,
                                  _MD_7
                                          }


                            ' Create tension and fill mode.
                            Dim tension As Single = NumericUpDown1.Value
                            Dim aFillMode As FillMode = FillMode.Alternate

                            m_Graphics.DrawClosedCurve(penLine, curvePoints, tension, aFillMode)






                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                                If AllPoints2 <= 7 Then

                                    For Each scanPoint In m_Points2



                                        m_Graphics.FillEllipse(Brushes.Black,
                            scanPoint.X - PointSize,
                            scanPoint.Y - PointSize,
                            PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points2.ToList.Clear()
                                    scanPoint.ToString.ToList.Clear()

                                Else
                                    AllPoints2 += 1

                                End If


                            End If



                        End Using

                        m_Points2.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()



                        PictureBox1.Refresh()


                    End If
                ElseIf RadioButton31.Checked = True Then


                    m_Drawing = True

                    RadioButton31.BackColor = Color.White
                    RadioButton31.ForeColor = Color.Black
                    RadioButton31.Invalidate()


                    If AllPoints2 = 4 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim curvePoints As Point() =
                                {
                                      MD_1,
                                      MD_2,
                                      MD_3,
                                      MD_4
                                             }



                            ' Create tension and fill mode.
                            Dim tension As Single = NumericUpDown1.Value
                            Dim aFillMode As FillMode = FillMode.Alternate

                            m_Graphics.FillClosedCurve(Brush1, curvePoints)







                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                                If AllPoints2 <= 4 Then

                                    For Each scanPoint In m_Points2



                                        m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points.ToList.Clear()
                                    scanPoint.ToString.ToList.Clear()

                                Else
                                    AllPoints2 += 1

                                End If


                            End If



                        End Using

                        m_Points.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()

                        PictureBox1.Refresh()

                    End If


                ElseIf RadioButton32.Checked = True Then

                    m_Drawing = True

                    RadioButton32.BackColor = Color.White
                    RadioButton32.ForeColor = Color.Black
                    RadioButton32.Invalidate()

                    If AllPoints2 = 4 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim curvePoints As Point() =
                                {
                                      MD_1,
                                      MD_2,
                                      MD_3,
                                      MD_4
                                             }

                            Dim GBrush As New LinearGradientBrush(MD_1, MD_3, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color))




                            m_Graphics.FillClosedCurve(GBrush, curvePoints)







                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                                If AllPoints2 <= 4 Then

                                    For Each scanPoint In m_Points2



                                        m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points.ToList.Clear()
                                    scanPoint.ToString.ToList.Clear()

                                Else
                                    AllPoints2 += 1

                                End If


                            End If



                        End Using

                        m_Points.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()

                        PictureBox1.Refresh()

                    End If


                ElseIf RadioButton33.Checked = True Then


                    m_Drawing = True

                    RadioButton33.BackColor = Color.White
                    RadioButton33.ForeColor = Color.Black
                    RadioButton33.Invalidate()

                    If AllPoints2 = 7 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim curvePoints As Point() =
                                {
                                      _MD_1_,
                                      _MD_2_,
                                      _MD_3_,
                                      _MD_4_,
                                      _MD_5_,
                                      _MD_6_,
                                      _MD_7_
                                              }


                            'Dim PenCurve1 As New Pen(CType(ToolStripComboBox2.SelectedItem, Color), ToolStripComboBox1.SelectedItem)



                            m_Graphics.DrawPolygon(penLine, curvePoints)




                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                                If AllPoints2 <= 7 Then

                                    For Each scanPoint In m_Points2



                                        m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points.ToList.Clear()
                                    scanPoint.ToString.ToList.Clear()

                                Else
                                    AllPoints2 += 1

                                End If


                            End If



                        End Using

                        m_Points.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()

                        PictureBox1.Refresh()

                    End If


                ElseIf RadioButton34.Checked = True Then

                    m_Drawing = True

                    RadioButton34.BackColor = Color.White
                    RadioButton34.ForeColor = Color.Black
                    RadioButton34.Invalidate()

                    If AllPoints2 = 7 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim curvePoints As Point() =
                                {
                                      _MD_1_,
                                      _MD_2_,
                                      _MD_3_,
                                      _MD_4_,
                                      _MD_5_,
                                      _MD_6_,
                                      _MD_7_
                                              }




                            m_Graphics.FillPolygon(Brush1, curvePoints)




                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                                If AllPoints2 <= 7 Then

                                    For Each scanPoint In m_Points2



                                        m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points.ToList.Clear()
                                    scanPoint.ToString.ToList.Clear()

                                Else
                                    AllPoints2 += 1

                                End If


                            End If



                        End Using

                        m_Points.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()

                        PictureBox1.Refresh()

                    End If

                ElseIf RadioButton35.Checked = True Then

                    m_Drawing = True

                    RadioButton35.BackColor = Color.White
                    RadioButton35.ForeColor = Color.Black
                    RadioButton35.Invalidate()

                    If AllPoints2 = 7 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim curvePoints As Point() =
                                {
                                      _MD_1_,
                                      _MD_2_,
                                      _MD_3_,
                                      _MD_4_,
                                      _MD_5_,
                                      _MD_6_,
                                      _MD_7_
                                              }


                            Dim GBrush As New LinearGradientBrush(_MD_1_, _MD_5_, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color))



                            m_Graphics.FillPolygon(GBrush, curvePoints)




                            If PointsControlToolStripMenuItem.BackColor = Color.Green Then

                                If AllPoints2 <= 7 Then

                                    For Each scanPoint In m_Points2



                                        m_Graphics.FillEllipse(Brushes.Black,
                                scanPoint.X - PointSize,
                                scanPoint.Y - PointSize,
                                PointSize * 2, PointSize * 2)

                                    Next scanPoint
                                    m_Points.ToList.Clear()
                                    scanPoint.ToString.ToList.Clear()

                                Else
                                    AllPoints2 += 1

                                End If


                            End If



                        End Using

                        m_Points.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()

                        PictureBox1.Refresh()

                    End If


                ElseIf RadioButton37.Checked = True Then

                    m_Drawing = True

                    RadioButton37.BackColor = Color.White
                    RadioButton37.ForeColor = Color.Black
                    RadioButton37.Invalidate()

                    If AllPoints2 >= 3 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim Points As Point() =
                                {
                                      MD1,
                                      MD2,
                                      MD3
                                   }




                            Dim path As New GraphicsPath()
                            path.AddLines(Points)


                            Dim pthGrBrush As New PathGradientBrush(path) With {
                            .CenterColor = CType(ComboBox13.SelectedItem, Color)
                        }

                            Dim colors As Color() = {
                         CType(ComboBox14.SelectedItem, Color),
                         CType(ComboBox15.SelectedItem, Color)}



                            pthGrBrush.SurroundColors = colors



                            m_Graphics.FillPath(pthGrBrush, path)


                        End Using

                        m_Points1.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()



                        PictureBox1.Refresh()



                    End If

                ElseIf RadioButton38.Checked = True Then

                    m_Drawing = True

                    RadioButton38.BackColor = Color.White
                    RadioButton38.ForeColor = Color.Black
                    RadioButton38.Invalidate()


                    If AllPoints2 >= 7 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim Points As Point() =
                                {
                                      _MD_1_,
                                      _MD_2_,
                                      _MD_3_,
                                      _MD_4_,
                                      _MD_5_,
                                      _MD_6_,
                                      _MD_7_
                                              }




                            Dim path As New GraphicsPath()
                            path.AddLines(Points)


                            Dim pthGrBrush As New PathGradientBrush(path) With {
                            .CenterColor = CType(ComboBox13.SelectedItem, Color)
                        }

                            Dim colors As Color() = {
                         CType(ComboBox14.SelectedItem, Color),
                         CType(ComboBox15.SelectedItem, Color),
                         CType(ComboBox16.SelectedItem, Color),
                         CType(ComboBox17.SelectedItem, Color),
                        CType(ComboBox18.SelectedItem, Color),
                         CType(ComboBox19.SelectedItem, Color)
                       }

                            pthGrBrush.SurroundColors = colors



                            m_Graphics.FillPath(pthGrBrush, path)







                        End Using

                        m_Points1.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()


                        PictureBox1.Refresh()


                    End If


                ElseIf RadioButton39.Checked = True Then

                    m_Drawing = True

                    RadioButton39.BackColor = Color.White
                    RadioButton39.ForeColor = Color.Black
                    RadioButton39.Invalidate()


                    If AllPoints2 >= 10 Then


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                            m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                            Dim Points As Point() =
                                {
                                        MD_1_,
                                        MD_2_,
                                        MD_3_,
                                        MD_4_,
                                        MD_5_,
                                        MD_6_,
                                        MD_7_,
                                        MD_8_,
                                        MD_9_,
                                        MD_10_
                                              }




                            Dim path As New GraphicsPath()
                            path.AddLines(Points)


                            Dim pthGrBrush As New PathGradientBrush(path) With {
                            .CenterColor = CType(ComboBox13.SelectedItem, Color)
                        }

                            Dim colors As Color() = {
                         CType(ComboBox14.SelectedItem, Color),
                         CType(ComboBox15.SelectedItem, Color),
                         CType(ComboBox16.SelectedItem, Color),
                         CType(ComboBox17.SelectedItem, Color),
                        CType(ComboBox18.SelectedItem, Color),
                         CType(ComboBox19.SelectedItem, Color),
                          CType(ComboBox20.SelectedItem, Color),
                          CType(ComboBox21.SelectedItem, Color),
                           CType(ToolStripComboBox2.SelectedItem, Color)
                       }

                            pthGrBrush.SurroundColors = colors



                            m_Graphics.FillPath(pthGrBrush, path)







                        End Using

                        m_Points1.ToList.Clear()

                        scanPoint.ToString.ToList.Clear()


                        PictureBox1.Refresh()


                    End If


                ElseIf RadioButton40.Checked Then

                    If CopyFromScreenRBToolStripMenuItem.Enabled = True Then

                        m_Drawing = False

                        RadioButton40.BackColor = Color.White
                        RadioButton40.ForeColor = Color.Black
                        RadioButton40.Invalidate()


                        If e.X = 0 Then Exit Sub
                        If e.Y = 0 Then Exit Sub


                        AllScreenToolStripMenuItem.Enabled = False


                        Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                            SelectionArea.Finish()

                            ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()


                            m_Graphics.CopyFromScreen(New Point(SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y), CType(New Size(SelectionArea.Rectangle.Width, SelectionArea.Rectangle.Height), Point), CType(New Point _
                            (SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y), Size))




                            m_Graphics.Dispose()


                        End Using

                    Else
                        Exit Sub
                    End If


                ElseIf RadioButton43.Checked Then

                    m_Drawing = False

                    RadioButton43.BackColor = Color.White
                    RadioButton43.ForeColor = Color.Black
                    RadioButton43.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Finish()
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()


                        ' m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        ' Dim Rect = New Rectangle(New Point(SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y), New Size(SelectionArea.Rectangle.Width - m_X, SelectionArea.Rectangle.Height - m_Y))



                        '  m_Graphics.DrawRectangle(penLine, Rect)


                    End Using

                    ' Display the result.
                    'PictureBox1.Refresh()

                ElseIf RadioButton46.Checked Then

                    m_Drawing = False

                    RadioButton46.BackColor = Color.White
                    RadioButton46.ForeColor = Color.Black
                    RadioButton46.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Finish()
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        Dim Rect = New Rectangle(New Point(SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y), New Size(SelectionArea.Rectangle.Width - m_X, SelectionArea.Rectangle.Height - m_Y))


                        'Dim Rect = New Rectangle(New Point(m_X, m_Y), New Size(e.X - m_X, e.Y - m_Y))

                        m_Graphics.DrawRectangle(penLine, Rect)


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()


                ElseIf RadioButton47.Checked Then

                    m_Drawing = False

                    RadioButton47.BackColor = Color.White
                    RadioButton47.ForeColor = Color.Black
                    RadioButton47.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Finish()
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        Dim Rect = New Rectangle(New Point(SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y), New Size(SelectionArea.Rectangle.Width - m_X, SelectionArea.Rectangle.Height - m_Y))



                        m_Graphics.DrawEllipse(penLine, Rect)


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()


                ElseIf RadioButton48.Checked Then

                    m_Drawing = False

                    RadioButton48.BackColor = Color.White
                    RadioButton48.ForeColor = Color.Black
                    RadioButton48.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Finish()
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        Dim Rect = New Rectangle(New Point(SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y), New Size(SelectionArea.Rectangle.Width - m_X, SelectionArea.Rectangle.Height - m_Y))



                        m_Graphics.FillEllipse(Brush1, Rect)


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()

                ElseIf RadioButton49.Checked Then

                    m_Drawing = False

                    RadioButton49.BackColor = Color.White
                    RadioButton49.ForeColor = Color.Black
                    RadioButton49.Invalidate()


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                        SelectionArea.Finish()
                        ToolStripTextBox4.Text() = SelectionArea.Rectangle.ToString()


                        m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                        Dim Rect = New Rectangle(New Point(SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y), New Size(SelectionArea.Rectangle.Width - m_X, SelectionArea.Rectangle.Height - m_Y))



                        m_Graphics.FillRectangle(Brush1, Rect)


                    End Using

                    ' Display the result.
                    PictureBox1.Refresh()



                End If
                m_Graphics.Dispose()

            End If

            Clipboard.SetImage(PictureBox1.Image)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub PictureBox1_MouseDown7337(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        Try
            m_Drawing = True

            If e.Button = MouseButtons.Left Then



                If RadioButton44.Checked Then


                    Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                        m_Graphics.CopyFromScreen(New Point(CInt(NumericUpDown6.Value), CInt(NumericUpDown7.Value)), New Point(e.X, e.Y), New Size(CInt(NumericUpDown10.Value), CInt(NumericUpDown11.Value)), CopyPixelOperation.MergePaint)


                        'CopyFromScreen(Me.Location, New Point(40, 70), New Size(500, 500))

                        '
                    End Using

                    PictureBox1.Refresh()
                End If


            End If


        Catch ex As Exception

        End Try

    End Sub









#End Region

#End Region

#Region "Option of Main Menu, Tool Strip and so on"

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            ' прозрачность формы, клик увеличение прозрачности
            If Form3.Opacity <= 0 Or Form3.Opacity >= 1 Then
                ' Stop
                a = -a
            End If
            Form3.Opacity += a
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Try
            Timer1.Enabled = False
            Form3.Opacity = 1
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Try
            Beep()
            Timer1.Enabled = Not Timer1.Enabled
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub PenColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenColorToolStripMenuItem.Click
        Try
            Dim cdlg As New ColorDialog()
            If cdlg.ShowDialog() = DialogResult.OK Then

                penLine.Color = cdlg.Color


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try


    End Sub
    Private Sub Brush1ColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Brush1ColorToolStripMenuItem.Click
        Try
            Dim cdlg As New ColorDialog()
            If cdlg.ShowDialog() = DialogResult.OK Then

                Brush1.Color = cdlg.Color


            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub


    Private Sub BackColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackColorToolStripMenuItem.Click
        Try
            Dim cdlg As New ColorDialog()
            If cdlg.ShowDialog() = DialogResult.OK Then


                Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                    m_Graphics.Clear(cdlg.Color)

                    PictureBox1.Invalidate()


                End Using

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub


    Private Sub GrBackColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GrBackColorToolStripMenuItem.Click

        Try

            Using m_Graphics = Graphics.FromImage(PictureBox1.Image)
                m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                Dim GrBrush As LinearGradientBrush


                Dim Rect = New Rectangle(0, 0, PictureBox1.ClientSize.Width, PictureBox1.ClientSize.Height)


                GrBrush = New LinearGradientBrush(Rect, CType(ToolStripComboBox2.SelectedItem, Color), CType(ToolStripComboBox3.SelectedItem, Color), NumericUpDown4.Value)



                m_Graphics.FillRectangle(GrBrush, Rect)

                PictureBox1.Invalidate()

            End Using

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try

    End Sub


    Private Sub CopyScreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyScreenToolStripMenuItem.Click
        Try

            'Copy to the ClipBoard screenshot of the active window document
            ' keybd_event(44, 1, 0, 0)
            SendKeys.Send("%{PRTSC}")


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub BackColorMultGrToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackColorMultGrToolStripMenuItem.Click
        Try

            MD_1_ = New Point(0, 0)
            MD_2_ = New Point(CInt(PictureBox1.ClientSize.Width / 6), 0)
            MD_3_ = New Point(CInt(PictureBox1.ClientSize.Width / 3), 0)


            MD_4_ = New Point(PictureBox1.ClientSize.Width, 0)

            MD_5_ = New Point(PictureBox1.ClientSize.Width, 388)



            MD_6_ = New Point(PictureBox1.ClientSize.Width, 776)


            MD_7_ = New Point(CInt(PictureBox1.ClientSize.Width / 6), 776)


            MD_8_ = New Point(CInt(PictureBox1.ClientSize.Width / 3), 776)


            MD_9_ = New Point(0, PictureBox1.ClientSize.Height)


            MD_10_ = New Point(0, 338)



            Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                Dim Points As Point() =
                                {
                                      MD_1_,
                                    MD_2_,
                                    MD_3_,
                                    MD_4_,
                                    MD_5_,
                                    MD_6_,
                                    MD_7_,
                                    MD_8_,
                                    MD_9_,
                                    MD_10_
                                              }



                Dim path As New GraphicsPath()
                path.AddLines(Points)


                Dim pthGrBrush As New PathGradientBrush(path) With {
                    .CenterColor = CType(ComboBox22.SelectedItem, Color)
                }

                Dim colors As Color() = {
                         CType(ComboBox23.SelectedItem, Color),
                         CType(ComboBox24.SelectedItem, Color),
                         CType(ComboBox25.SelectedItem, Color),
                         CType(ComboBox26.SelectedItem, Color),
                         CType(ComboBox27.SelectedItem, Color),
                         CType(ComboBox28.SelectedItem, Color),
                         CType(ComboBox29.SelectedItem, Color),
                        CType(ComboBox30.SelectedItem, Color),
                        CType(ComboBox31.SelectedItem, Color),
                        CType(ComboBox32.SelectedItem, Color)}

                pthGrBrush.SurroundColors = colors



                m_Graphics.FillPath(pthGrBrush, path)







            End Using




            PictureBox1.Refresh()





        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub


    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick


        Try
            MD_1_ = New Point(0, 0)
            MD_2_ = New Point(CInt(PictureBox1.ClientSize.Width / 6), 0)
            MD_3_ = New Point(CInt(PictureBox1.ClientSize.Width / 3), 0)


            MD_4_ = New Point(PictureBox1.ClientSize.Width, 0)

            MD_5_ = New Point(PictureBox1.ClientSize.Width, 388)



            MD_6_ = New Point(PictureBox1.ClientSize.Width, 776)


            MD_7_ = New Point(CInt(PictureBox1.ClientSize.Width / 6), 776)


            MD_8_ = New Point(CInt(PictureBox1.ClientSize.Width / 3), 776)


            MD_9_ = New Point(0, PictureBox1.ClientSize.Height)


            MD_10_ = New Point(0, 338)



            Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                m_Graphics.SmoothingMode = SmoothingMode.AntiAlias


                Dim Points As Point() =
                                    {
                                          MD_1_,
                                        MD_2_,
                                        MD_3_,
                                        MD_4_,
                                        MD_5_,
                                        MD_6_,
                                        MD_7_,
                                        MD_8_,
                                        MD_9_,
                                        MD_10_
                                                  }



                Dim path As New GraphicsPath()
                path.AddLines(Points)


                Dim pthGrBrush As New PathGradientBrush(path) With {
                    .CenterColor = Color.FromArgb(255, Random.Next(255), Random.Next(255))
                }

                Dim colors As Color() = {
                           Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                             Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                             Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                             Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                             Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                            Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                             Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                             Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                            Color.FromArgb(255, Random.Next(255), Random.Next(255)),
                            Color.FromArgb(255, Random.Next(255), Random.Next(255))}

                pthGrBrush.SurroundColors = colors



                m_Graphics.FillPath(pthGrBrush, path)







            End Using




            PictureBox1.Refresh()








        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub FuzzyMltColorGrStartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FuzzyMltColorGrStartToolStripMenuItem.Click
        Try
            If FuzzyMltColorGrStartToolStripMenuItem.Text = "Fuzzy Mlt Color Gr Start" Then

                Timer2.Enabled = True
                FuzzyMltColorGrStartToolStripMenuItem.Text = "Fuzzy Mlt Color Gr Stop"

            Else

                Timer2.Enabled = False
                FuzzyMltColorGrStartToolStripMenuItem.Text = "Fuzzy Mlt Color Gr Start"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Timer2.Enabled = True
    End Sub

    Private Sub FormatToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FormatToolStripMenuItem.Click
        Try

            If FormatToolStripMenuItem.Text = "Format" Then

                FormatToolStripMenuItem.ForeColor = Color.Black
                FormatToolStripMenuItem.Text = "Format OK"
            Else
                FormatToolStripMenuItem.ForeColor = Color.White
                FormatToolStripMenuItem.Text = "Format"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try

    End Sub



    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        Try

            If EditToolStripMenuItem.Text = "Edit" Then

                EditToolStripMenuItem.ForeColor = Color.Black
                EditToolStripMenuItem.Text = "Edit OK"
            Else
                EditToolStripMenuItem.ForeColor = Color.White
                EditToolStripMenuItem.Text = "Edit"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub DecorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DecorToolStripMenuItem.Click
        Try

            If DecorToolStripMenuItem.Text = "Decor" Then

                DecorToolStripMenuItem.ForeColor = Color.Black
                DecorToolStripMenuItem.Text = "Decor OK"
            Else
                DecorToolStripMenuItem.ForeColor = Color.White
                DecorToolStripMenuItem.Text = "Decor"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.Click
        Try

            If FileToolStripMenuItem.Text = "File" Then

                FileToolStripMenuItem.ForeColor = Color.Black
                FileToolStripMenuItem.Text = "File OK"
            Else
                FileToolStripMenuItem.ForeColor = Color.White
                FileToolStripMenuItem.Text = "File"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub CopyFromScreenRBToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyFromScreenRBToolStripMenuItem.Click
        Try




            If RadioButton40.Checked Then

                m_Drawing = True

                RadioButton40.BackColor = Color.White
                RadioButton40.ForeColor = Color.Black
                RadioButton40.Invalidate()




                Dim memoryImage As Bitmap


                Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                    Dim s As Size = New Size(SelectionArea.Rectangle.Width, SelectionArea.Rectangle.Height)
                    memoryImage = New Bitmap(s.Width, s.Height, m_Graphics)
                    Dim memoryGraphics As Graphics = Graphics.FromImage(memoryImage)
                    memoryGraphics.CopyFromScreen(SelectionArea.Rectangle.X, SelectionArea.Rectangle.Y, 0, 0, s)

                    Clipboard.SetImage(memoryImage)

                End Using




                If SaveFileDialog1.ShowDialog() = DialogResult.OK Then


                    If (SaveFileDialog1.FileName.Contains(".jpeg")) Then memoryImage.Save(SaveFileDialog1.FileName, ImageFormat.Jpeg)

                    If (SaveFileDialog1.FileName.Contains(".bmp")) Then memoryImage.Save(SaveFileDialog1.FileName, ImageFormat.Bmp)

                    If (SaveFileDialog1.FileName.Contains(".gif")) Then memoryImage.Save(SaveFileDialog1.FileName, ImageFormat.Gif)

                    If (SaveFileDialog1.FileName.Contains(".png")) Then memoryImage.Save(SaveFileDialog1.FileName, ImageFormat.Png)

                End If

            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try


    End Sub

    Private Sub RadioButton19_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton19.CheckedChanged
        Try
            AllScreenToolStripMenuItem.Enabled = True
            If RadioButton15.Checked = False Or RadioButton20.Checked = False Then
                TextureBrushPictureToolStripMenuItem.Enabled = False
                CopyFromScreenRBToolStripMenuItem.Enabled = False


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub RadioButton15_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton15.CheckedChanged
        Try
            TextureBrushPictureToolStripMenuItem.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub RadioButton20_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton20.CheckedChanged
        Try
            TextureBrushPictureToolStripMenuItem.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub RadioButton40_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton40.CheckedChanged
        Try
            CopyFromScreenRBToolStripMenuItem.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub



    Private Sub InformationToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Try
            Dim Inf = New Form2()
            Inf.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub AboutProgramToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Try
            Dim AB = New AboutBox1()
            AB.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
        Try

            penLine.Width = ToolStripComboBox1.SelectedIndex

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        Try

            Clipboard.SetImage(PictureBox1.Image)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try


    End Sub

    Private Sub PastToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PastToolStripMenuItem.Click
        Try
            If My.Computer.Clipboard.ContainsImage Then

                Dim bm1 As Image = Clipboard.GetImage()

                ' Make an associated Graphics object.
                Dim Gr As Graphics = CreateGraphics()

                Gr = Graphics.FromImage(bm1)


                ' Display the New bitmap in the PictureBox.
                PictureBox1.Image = bm1

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub AllScreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllScreenToolStripMenuItem.Click
        Try
            ToolStrip1.Visible = False
            MenuStrip1.Visible = False
            FormBorderStyle = FormBorderStyle.None

            Form3.ToolStrip1.Visible = False
            Form3.MenuStrip1.Visible = False
            Form3.FormBorderStyle = FormBorderStyle.None

            TabControl1.Visible = False


            PictureBox1.Dock = DockStyle.Fill

            If (FormBorderStyle = FormBorderStyle.None) And (Form3.FormBorderStyle = FormBorderStyle.None) Then

                Clipboard.SetImage(PictureBox1.Image)


                If My.Computer.Clipboard.ContainsImage Then

                    Dim bm1 As Image = Clipboard.GetImage()

                    ' Make an associated Graphics object.
                    Dim Gr As Graphics = CreateGraphics()

                    Gr = Graphics.FromImage(bm1)


                    ' Display the New bitmap in the PictureBox.
                    PictureBox1.Image = bm1

                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        Try
            SendKeys.Send("%{PRTSC}")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Try

            If OpenFileDialog1.ShowDialog = DialogResult.OK Then

                Dim s As String = OpenFileDialog1.FileName

                Try

                    m_Bitmap = New Bitmap(s)

                    currentImage = CType(m_Bitmap.Clone(), Image)


                    PictureBox1.Image = currentImage

                Catch ex As Exception
                    MessageBox.Show("File " + s + " has a wrong format.", "Error")
                    Return

                End Try

                Text = "DrawWithMousePB - " + s

                OpenFileDialog1.FileName = ""

            End If

            ToolStripMenuItem7.Enabled = True
            ToolStripMenuItem8.Enabled = True
            ToolStripMenuItem9.Enabled = True
            HorizontalToolStripMenuItem.Enabled = True
            VerticalToolStripMenuItem.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
            Button6.Enabled = True
            Button7.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub RotateFlip(ByVal degrees As Integer)
        Try


            If currentImage Is Nothing Then Exit Sub

            Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                Select Case degrees
                    Case 0
                        currentImage.RotateFlip(RotateFlipType.RotateNoneFlipX)
                    Case 1
                        currentImage.RotateFlip(RotateFlipType.RotateNoneFlipY)
                    Case 90
                        currentImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
                        Exit Select
                    Case 180
                        currentImage.RotateFlip(RotateFlipType.Rotate180FlipNone)
                        Exit Select
                    Case 270
                        currentImage.RotateFlip(RotateFlipType.Rotate270FlipNone)
                    Case Else
                        Exit Select
                End Select

            End Using



        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try
    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem7.Click, Button3.Click
        Try

            If ([String].IsNullOrEmpty(Text)) Then Exit Sub


            RotateFlip(90)

            PictureBox1.Refresh()

            '90 rotate
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try



    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem8.Click, Button4.Click
        Try

            If ([String].IsNullOrEmpty(Text)) Then Exit Sub
            RotateFlip(180)

            PictureBox1.Refresh()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try


        '180 rotate
    End Sub

    Private Sub ToolStripMenuItem9_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem9.Click, Button5.Click
        Try

            If ([String].IsNullOrEmpty(Text)) Then Exit Sub
            '270 rotate
            RotateFlip(270)
            PictureBox1.Refresh()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try



    End Sub

    Private Sub HorizontalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HorizontalToolStripMenuItem.Click, Button6.Click
        Try
            If ([String].IsNullOrEmpty(Text)) Then Exit Sub
            RotateFlip(0)
            PictureBox1.Refresh()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try


        'flip Horizontal

    End Sub

    Private Sub VerticalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerticalToolStripMenuItem.Click, Button7.Click
        'flip vertical

        Try
            If ([String].IsNullOrEmpty(Text)) Then Exit Sub
            RotateFlip(1)
            PictureBox1.Refresh()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try

    End Sub





    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Try
            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                Dim ContrastBmp = New Bitmap(PictureBox1.Image)


                If (SaveFileDialog1.FileName.Contains(".jpeg")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Jpeg)

                If (SaveFileDialog1.FileName.Contains(".bmp")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Bmp)

                If (SaveFileDialog1.FileName.Contains(".gif")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Gif)

                If (SaveFileDialog1.FileName.Contains(".png")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Png)

            End If




        Catch ex As Exception
            If CBool(MessageBox.Show("The file " + SaveFileDialog1.FileName + " has an inappropriate extension. Returning.")) Then
                Return
            ElseIf CBool(MessageBox.Show("The result image saved under " + SaveFileDialog1.FileName)) Then

            Else MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            End If

        End Try
    End Sub


    Private Sub ActSaveInFullScreenStateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click, ActSaveInFullScreenStateToolStripMenuItem.Click

        If (ActSaveInFullScreenStateToolStripMenuItem.Text() = "Act Save In full screen state") And (ActSaveInFullScreenStateToolStripMenuItem.BackColor = Color.White) Then
            '  Dim MBox As DialogResult = MessageBox.Show("Вернуть из БО будет нельзя, продолжить? ", "DrawWithMouse", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
            ' YES - no TextBox         NO- exit from dilog         
            ' If MBox = DialogResult.No Then Exit Sub
            ' If MBox = DialogResult.Yes Then
            ActSaveInFullScreenStateToolStripMenuItem.BackColor = Color.Lime
            ToolStripButton5.BackColor = Color.LimeGreen
            ToolStripButton5.ForeColor = Color.Black
            ToolStripButton5.Text() = "Act"
            ActSaveInFullScreenStateToolStripMenuItem.Text() = "Save"


            'End If
        Else
            ActSaveInFullScreenStateToolStripMenuItem.BackColor = Color.White
            ToolStripButton5.BackColor = Color.Navy
            ToolStripButton5.ForeColor = Color.White
            ToolStripButton5.Text() = "Save in All Scr"
            ActSaveInFullScreenStateToolStripMenuItem.Text() = "Act Save In full screen state"

        End If

    End Sub


    Private Sub PointsControlToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PointsControlToolStripMenuItem.Click

        PointsControlToolStripMenuItem.BackColor = Color.Green

    End Sub

    Private Sub PointsControlRelaxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PointsControlRelaxToolStripMenuItem.Click
        PointsControlToolStripMenuItem.BackColor = Color.Red

    End Sub

    Private Sub PictureBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDoubleClick
        Try

            If e.Button = MouseButtons.Right Then
                If ((ActSaveInFullScreenStateToolStripMenuItem.BackColor = Color.Lime) And (ActSaveInFullScreenStateToolStripMenuItem.Text = "Save") And (WindowState = FormWindowState.Maximized)) Then


                    If (SaveFileDialog1.ShowDialog() = DialogResult.Cancel) Then

                        Form3.ToolStrip1.Visible = True
                        Form3.MenuStrip1.Visible = True
                        Form3.FormBorderStyle = FormBorderStyle.Fixed3D



                        ToolStrip1.Visible = True
                        MenuStrip1.Visible = True
                        TabControl1.Visible = True
                        FormBorderStyle = FormBorderStyle.Fixed3D

                        'return for native meanings of the control
                        ToolStripButton5.BackColor = Color.Navy
                        ToolStripButton5.ForeColor = Color.White
                        ToolStripButton5.Text() = "Save in All Scr"
                        ActSaveInFullScreenStateToolStripMenuItem.Text() = "Act Save In full screen state"
                        ActSaveInFullScreenStateToolStripMenuItem.BackColor = Color.White


                        SaveFileDialog1.Dispose()


                    Else

                        Dim ContrastBmp = New Bitmap(PictureBox1.Image)


                        If (SaveFileDialog1.FileName.Contains(".bmp")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Bmp)

                        If (SaveFileDialog1.FileName.Contains(".jpeg")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Jpeg)

                        If (SaveFileDialog1.FileName.Contains(".gif")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Gif)

                        If (SaveFileDialog1.FileName.Contains(".png")) Then ContrastBmp.Save(SaveFileDialog1.FileName, ImageFormat.Png)

                        MessageBox.Show("The result image saved under " + SaveFileDialog1.FileName)

                        ContrastBmp.Dispose()


                        Form3.ToolStrip1.Visible = True
                        Form3.MenuStrip1.Visible = True
                        Form3.FormBorderStyle = FormBorderStyle.Fixed3D



                        ToolStrip1.Visible = True
                        MenuStrip1.Visible = True
                        TabControl1.Visible = True
                        FormBorderStyle = FormBorderStyle.Fixed3D


                        ToolStripButton5.BackColor = Color.Navy
                        ToolStripButton5.ForeColor = Color.White
                        ToolStripButton5.Text() = "Save in All Scr"
                        ActSaveInFullScreenStateToolStripMenuItem.Text() = "Act Save In full screen state"
                        ActSaveInFullScreenStateToolStripMenuItem.BackColor = Color.White


                    End If

                End If

            End If


        Catch ex As Exception
            If CBool(MessageBox.Show("The file " + SaveFileDialog1.FileName + " has an inappropriate extension. Returning.")) Then
                Return
            End If

        End Try



    End Sub
    Private Sub RadioButton22_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton22.CheckedChanged
        If RadioButton22.Checked Then

            RadioButton22.BackColor = Color.DarkGreen
        Else
            RadioButton22.BackColor = Color.White


        End If
    End Sub
    Private Sub PictureBox1_MouseDoubleClick1(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDoubleClick


        Try

            If Not (RadioButton22.BackColor = Color.DarkGreen) Then Exit Sub


            m_Drawing = True

            If e.Button = MouseButtons.Right Then


                Dim drawFormat As New StringFormat()


                Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                    m_Graphics.SmoothingMode = SmoothingMode.AntiAlias

                    Dim drawFont As New Font(CType(ComboBox1.SelectedItem, String), (CType(ComboBox2.SelectedItem, Integer)))

                    Dim drawBrush As New SolidBrush(CType(ComboBox3.SelectedItem, Color))

                    m_Graphics.DrawString(TextBox1.Text(), drawFont, drawBrush,
                    e.X, e.Y, drawFormat)


                    PictureBox1.Refresh()



                End Using
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Mistake", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try



    End Sub
    Private Sub PictureBox1_MouseClick(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseClick
        Try

            If e.Button = MouseButtons.Left Then


                ActSaveInFullScreenStateToolStripMenuItem.PerformClick()
                RadioButton22.BackColor = Color.White

            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try

            'Смена клавиатуры с русской раскладки на английскую
            'InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture( _ 
            'New CultureInfo("ru-RU")) 
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(
            New CultureInfo("en-US"))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            ' с английской на русскую
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(
        New CultureInfo("ru-RU"))
            'InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(
            'New CultureInfo("en-US"))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub



    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Try
            ToolStrip1.Visible = False
            TabControl1.Visible = False

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ControlVisibleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ControlVisibleToolStripMenuItem.Click
        Try
            ToolStrip1.Visible = True
            TabControl1.Visible = True
        Catch ex As Exception

        End Try
    End Sub


    Private Sub TextureBrushPictureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TextureBrushPictureToolStripMenuItem.Click
        Try
            If OpenFileDialog1.ShowDialog = DialogResult.OK Then
                'значеие переменной строки путь и имя файла для текстурированной браши
                str = OpenFileDialog1.FileName.ToString

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SystemOfCoordinateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SystemOfCoordinateToolStripMenuItem.Click, ToolStripButton6.Click
        Try

            If RadioButton24.Checked Then

                Dim LineCap As New LineCap
                'Создание объекта рисования

                'ОБъявление и декларация переменных для задания параметров системы координат
                Dim a As Integer = CInt(ComboBox4.SelectedItem)
                Dim b As Integer = CInt(ComboBox5.SelectedItem)
                Dim c As Integer = CInt(ComboBox6.SelectedItem)
                Dim d As Integer = CInt(ComboBox7.SelectedItem)
                Dim f As Integer = CInt(ComboBox8.SelectedItem)

                Using m_Graphics = Graphics.FromImage(PictureBox1.Image)

                    ' Перо для осей
                    Dim pnXY As New Pen(Color.Black, f)

                    pnXY.EndCap() = LineCap.ArrowAnchor



                    ' Ширина и высота клетки
                    Dim width As Integer = a, height As Integer = b

                    ' Число клеток по горизонтали и вертикали
                    Dim countX As Integer = c, countY As Integer = d
                    ' Рисуем линии


                    For i As Integer = 1 To countX
                        For j As Integer = 1 To countY

                            m_Graphics.DrawLine(Pens.Black, width, height * i, width * countY, height * i)
                            m_Graphics.DrawLine(Pens.Black, width * j, height, width * j, height * countX)
                            ' Рисуем оси

                            If i = Math.Ceiling(countX / 2) Then
                                m_Graphics.DrawLine(pnXY, width, height * i, width * (countY + 1), height * i)
                            End If

                            If j = Math.Ceiling(countY / 2) Then
                                m_Graphics.DrawLine(pnXY, width * j, height * countX, width * j, 0)
                            End If


                        Next j

                    Next i

                End Using

                PictureBox1.Refresh()




            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()



    End Sub

#End Region

End Class
