   M S B A M L   `   `   `  ����   
BookingWPF"    BookingWPF.RoomSelectionPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       N� Title$  RoomSelectionPage�� � 	Resources    ScrollBarThumb �� 
TargetType �� Property �� Value �� 
TargetType �� RadiusX �� RadiusY �� CornerRadius	 ]�Name
 ]� DecreaseRepeatButton �� PageUpCommand J� Opacity ]� Thumb ]� IncreaseRepeatButton �� PageDownCommand(  "System.Windows.Controls.DatePicker��%B  &	        '�� �     7   '  ~    �� " n�.���  �@�OverridesDefaultStyle . ��  � �� .  ��  � �� �� " n�.R��� ���#007ACC$ 4��$ 4���� " ��.���  � ��
 ���� ��  � ��$ 8����  � �� �� " ��.R�� 
������ .+��� ���#2D2D2D$ 4��]� $	 
PART_Track��
 �� #��Z 
������ $ 0Q� n� 8��   �� #��Z 
������ $ 0Q��� "  .���  � ��
 �������  � �� �#1E1E1E��  � �� �#404040��  � ��$ 1q���  � ��$ 40����  � ��$ Center=�.��� $��20q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��$��20q�.+��� -    ��Name$ HotelNameText��$��24(�$
��Bold"�
��������� -   $ HotelCityText��$��18(�
��������� -   $ WeatherText��$��16(����#007ACC$��0,5,0,0q��� -   $E��?Посмотреть достопримечательности�����#007ACC
�������$��20,10q�$��0,10,0,0q�$��0q��� $��1�����#2D2D2D$ 10�� �� Padding$ 20q�$��0,0,0,20q�._��  � ColumnDefinitions �� $��* ��� $��* ��� $
��Auto �.+��� $��0��$��0,0,10,0q�.+��� $��Дата заезда��
�������$��0,0,0,5q�  -     Name$ CheckInDatePicker��$��40�� :  4System.Windows.Controls.Primitives.DatePickerTextBox��%/   '        �� "  .���  � ��
 ������� $��1��$
��10,0q�.+��� $��Дата выезда��
�������$��0,0,0,5q�  -   $ CheckOutDatePicker��$��40�� ��%/   '        �� "  .���  � ��
 ������� -   $��2��$��Найти номера�����#007ACC
�������$��20,10q�$��40��$��0q�$��10,0,0,0q�$��Bottom=��� $��2��.���� -    ��Name$ RoomsStackPanel��