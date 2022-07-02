# VB.NET KingFisher Desktop Pet

<img width="100%" src="https://i.postimg.cc/1t11rWkN/1.png">

<img alt="Windows" src="https://img.shields.io/badge/-Windows-0078D6?style=flat&logo=windows&logoColor=white"/> <img alt="NET" src="https://img.shields.io/badge/-Visual%20Basic-blue?style=flat&logo=.net&logoColor=white"/>

Translate this page to english: https://bit.ly/3A8G7zz

Esta aplicación es un clon escrito en Visual Basic.NET del programa <b>Desktop Bird KingFisher</b> de <b>SEGA Enterprises, LTD</b>.

Los <b>desktop pets</b> o <b>Screenmates</b> fueron muy populares en la época de Windows95/98/XP pero debido a la inclusion de software malicioso por parte de terceros y la saturación de aplicaciones de este tipo, hicieron que al final la gente perdiera el interés y cayeran en desuso.

Esta aplicación es un pequeño tributo a los screenmates de antaño y a sus creadores.

# Funcionamiento interno
- Todas las animaciones son arrays de imágenes (Bitmaps) que se guardan como recursos incrustados en un archivo .resx.
- Al ejecutar el programa, se inicia un reloj (Timer) y con cada tick del reloj se calcula la animación y las imágenes a mostrar.
- Una función genera puntos aleatorios dentro de la pantalla y otra función hace que el pájaro vuele hacia ellos uno tras otro.
- Cada cierto tiempo el pájaro ejecuta animaciones aleatorias como pescar, volar por la pantalla, o posarse en una ventana.
- El cambio de animación se controla mediante un contador que incrementa cada vez que se alcanza uno de los puntos aleatorios.

# Animaciones implementadas
- El pájaro volará aleatoriamente de lado a lado de la pantalla.
- Si el pájaro se acerca a los bordes de la pantalla, ejecutará una animación de frenado para no estrellarse.
- El pájaro detectará ventanas abiertas y se posará sobre ellas hasta que la ventana pierda el foco o se minimize o se cierre.
- Cada cierto tiempo el pájaro bajará en picado hasta la barra de tareas y pescará un pececillo.
- Si encuentra una ventana sobre la que posarse, se comerá allí el pececillo, luego volverá a volar aleatoriamente de lado a lado dentro de la pantalla.
- Si pasado cierto tiempo no encuentra una ventana abierta, el pájaro bajará hasta la barra de tareas y se comerá allá el pececillo.

# Sonidos
- El pájaro reproduce sonidos cada vez que una animación se ejecuta o cada vez que pesca un pececillo, los sonidos se pueden desactivar.

# APIs de Windows implementadas
- GetWindowRectangle() <-- Substituye a GetWindowRect() porque obtiene el tamaño de la ventana sin sombras en Win10.<br>
- DwmGetWindowAttribute()<br>
- GetWindow()<br>
- GetWindowText()<br>
- GetTopWindow()<br>
- IsWindowVisible()<br>
- EnumWindows()<br>
- EnumWindowsDelegate()<br>

# Fallos conocidos
- La transparencia del control Picturebox no acaba de ser correcta, sigo buscando la manera de ajustarla correctamente.
- Sigo buscando a alguien que me ayude a depurar la implementación de las APIs de Windows para detectar ventanas abiertas.
- Algunas animacions son algo bruscas, me gustaría mejorarlas y hacerlas más suaves.

# Agradecimientos
- <b>Mr.MonkeyBoy</b> por compartir su clase "MathHelp": https://social.msdn.microsoft.com/profile/mr.%20monkeyboy/?ws=usercard-mini<br>
- <b>AdrianoTiger</b> por el código fuente de eSheep de donde he sacado algún recorte de código: https://github.com/Adrianotiger<br>
- <b>StackOverflow</b> por la ayuda y códigos fuente: https://stackoverflow.com<br>
