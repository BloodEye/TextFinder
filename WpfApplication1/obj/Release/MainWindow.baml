   M S B A M L   `   `   `  ����   Text Finder    TextFinder.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  ] YICSharpCode.AvalonEdit, Version=5.0.1.0, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonEdit.http://icsharpcode.net/sharpdevelop/avalonedit  -     �Name$  	MainFrame�� � Title$ Text Finder��$	��600��$
��1024��#��� �� � WindowStartupLocation$ CenterScreen=�.��� .+�o� $	��Top=� o� 
IsMainMenu . $��Stretch=�$��25��$��0,0,0,1q�.@�m� $��	_Файл��$��Times New Roman)�$��0,3,0,0q�.@�m� -   $��_Відкрити��m� -   $��_Зберегти Як���� m� -   $��_Вийти��m� -   $��_Про програму��$��Times New Roman)�$��0,3,0,0q�� $
��Left=�$	��250��#��� ��$��	0,25,0,33q�.+�� $��
10,10,10,0q�$	��115��$
��Auto��$	��Top=�.+��� $	��Top=�$
��Auto=�$	��115��$��Right=�$
��Auto��$	��229��.���� -    ��Name$ 
SearchGrid��$��0q�$	��Top=�.+�(  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   $��Pattern��   	Watermark$3 -Введіть запит для пошуку��$
��Left=�$��Center=�$��Stretch=�$	��200��$��23��$��Times New Roman)����	#FF707070�� -   $��Додати поле��$��0,0,137,-26q�$��Right=�$��92��$��20��$��Bottom=�$��Times New Roman)�$��13(��� $
��Left=�$��11��$��	0,0,0,-38q�$��Bottom=�$	��230���� -	   $��
IgnoreCase��$)��#Ігнорувати регістр��$��0,0,96,-112q�$��Right=�$	��133��$��20��$��Bottom=�$��Times New Roman)�$��13(��� -
   $��Знайти��$��0,0,137,-60q�$��Right=�$��92��$��20��$��Bottom=�$��Times New Roman)�$��13(��� -   $��Знайти усі��$��0,0,137,-85q�$��Right=�$��92��$��20��$��Bottom=�$��Times New Roman)�$��13(��� -   $��Очистити ��$��	0,0,0,-60q�$��Right=�$	��122��$��20��$��Bottom=�$��Times New Roman)�$��13(��� -   $+��%Очистити результати��$��	0,0,0,-85q�$��Right=�$	��122��$��20��$��Bottom=�$��Times New Roman)�$��13(��� $��Результати:��$��10,241,141,234q�$��Times New Roman)�$��14(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Display=��� -    ��Name$ Result��$��Stretch=�$��10,271,10,10q�$��Times New Roman)�$��Disabled=�$
��Auto=� %� IsVirtualizing . 	 %� VirtualizationMode$	 	Recycling=�
 �� View
 �� .P��� $��#�� �� Width$ 40�� �� DisplayMemberBinding �� *Line+�� $��
Рядок��$	 190�� �� CellTemplate �� col1Template��.R��� $��WrapWithOverflow=��� ��� *	TextStart+�� 
��� ������ *	Pattern+�� ��� *	TextEnd+�� $ 0�� �� *Index+�� $ 0�� �� *Length+����  �� 
TargetType" ��.�-    � RowDefinitions �� $��* ��� -    ��Name$	 RTB��$��250,25,240,33q�$��
15,10,5,10q�$��0q�$��Times New Roman)�$��20(� �� IsDocumentEnabled .  �� SelectionOpacity$	 0.8Q� � Cursor$ IBeam�� +� IsFocusScope .  � ForceCursor . ��. " �� VerticalScrollBarVisibility$
 Auto=� �� SelectionBrush �	#FF51B7D4 �� 
CaretBrush
 �   �* �� #IsInactiveSelectionHighlightEnabled .  J� IsManipulationEnabled . 3  -Xceed.Wpf.Toolkit.RichTextBoxFormatBarManager   	FormatBar ,  &Xceed.Wpf.Toolkit.RichTextBoxFormatBar  . �/� .T�J� -    �Name$ RTBParagraph�� �� Padding$	 5,0q� J� 
TextIndent$ 15��.*�.*��� �� $��33��$��Bottom=�$��0,1,0,0q�.@��� $*��$Кількість символів:��$	��114��$��Times New Roman)��� -    ��Name$ QuantityChar��$��0��$��Times New Roman)���    VerticalSeparatorStyle#���   $��15���� $&�� Кількість рядків:��$	��107��$��Times New Roman)��� -   $ QuantityEndl��$��0��$��Times New Roman)��� #���   $��15��$��0,-1,0,1q��� $��Час пошуку:��$��74��$��Times New Roman)��� -   $ 
TimeSearch��$��0��$��Times New Roman)��� $��Right=��� #���   $��14��$��0,-1,0,1q��� $.��(Кількість співпадінь:��$	��127��$��Times New Roman)��� -   $ QuantityFinds��$��0��$��Times New Roman)�� $��Right=�$��	0,25,0,33q�$	��240��#��� ��.+��� -     ��Name$
  CBox��$
��Left=�$��	10,10,0,0q�$	��Top=�$	��220��$��30��! �� SelectedIndex$! 0��$��Center=����	#FF707070.@��� -   $��_Background��.��Колір фону�� -   $��_Foreground��.��Колір тексту#  Xceed.Wpf.Toolkit.ColorCanvas  -   "  Name$" CPMenu��$��1q� .  $��Stretch=�$��0q�$��Stretch=�$��Disabled=���. ���	#FFF0F0F0$��
10,55,10,0q�$	��Top=����	#FF707070�� -   $��Зафарбувати��$
��Left=�$��
10,342,0,0q�$	��Top=�$	��220��$��27���� $
��Left=�$��25��$��
10,374,0,0q�$	��Top=�$	��220���� # �� Orientation$# 
Horizontal=�$��10,399,10,0q�$
��auto��$	��Top=�.+�d� -   $��btnBold��$��32��$��32��$��5q�$ � ToolTip$$ Жирний��.���� $��Icon/Bold.png��d� -   $��	btnItalic��$��32��$��32��$��5q�$$ Курсив��.���� $��Icon/Italic.png��d� -   $��btnUnderline��$��32��$��32��$��5q�$$ Підкреслений��.���� $��Icon/Underline.png��