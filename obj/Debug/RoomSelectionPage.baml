   M S B A M L   `   `   `  ����  
BookingWPF"    BookingWPF.RoomSelectionPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  RoomSelectionPage��5       � 	Resources 5         ScrollBarThumb �� 
TargetType �� Property �� Value �� 
TargetType �� RadiusX �� RadiusY �� CornerRadius	 ]�Name
 ]� DecreaseRepeatButton �� PageUpCommand J� Opacity ]� Thumb ]� IncreaseRepeatButton �� PageDownCommand(  "System.Windows.Controls.DatePicker��%  &	        '��     7   '  �    �� 5   
   " n�6'   .�5      ��  �@�OverridesDefaultStyle . 66   �� 5	       � �� .  6*   �� 5
       � �� 5      �� 5      " n�6&   .R�5      �� ���#007ACC$ 4��63   $ 4��6?   5      5      5      �� 5   
   " ��6   .�5      ��  � ��
 ���� 6+   �� 5       � ��$ 8��6&   �� 5       � �� 5      �� 5      " ��6&   .R�5      � 
������ .+�5      �� ���#2D2D2D$ 4��5   %   ]� 5      $	 
PART_Track��6$   
 5   "   �� 5   &   #��Z 63   
������ $ 0Q�5    1    5"   "   n� 5#   &   8��  6,    5%   "   �� 5&   &   #��Z 63   
������ $ 0Q�5(   1   5*      5+      5,      5.      5/      �� 52   
   "  6   .�53      ��  � ��
 �����6+   �� 54       � �� �#1E1E1E6+   �� 55       � �� �#4040406,   �� 56       � ��$ 1q�60   �� 57       � ��$ 40��6'   �� 58       � ��$ Center=�69   59      5:      .��5<      � $��20q�6    � RowDefinitions 5=   
   �� 5>      $
��Auto �6   �� 5?      $
��Auto �6   �� 5@      $��* �6   .+�5D   
   �� $��0��6   $��20q�6"   .+�5E      �� -   5         ��Name$ HotelNameText��5E      $��24(�5F      $
��Bold"�5G      
�������5H      �� 5I      -   5        $ HotelCityText��5I      $��18(�5J      
�������5K      �� 5L      -   5        $ WeatherText��5L      $��16(�5M      ���#007ACC$��0,5,0,0q�5O      �� 5P      -   5        $E��?Посмотреть достопримечательности��5P      ���#007ACC
�������$��20,10q�5S      $��0,10,0,0q�5T      $��0q�5U      5V      5W      �� 5Z   
   $��1��6   ���#2D2D2D$ 10��5\       �� Padding$ 20q�5]      $��0,0,0,20q�5^      ._�5_      �  � ColumnDefinitions 5`      �� 5a      $��* �6'   �� 5b      $��* �6'   �� 5c      $
��Auto �6'   .+�5g      �� $��0��6   $��0,0,10,0q�6-   .+�5h      �� $��Дата заезда��6    
�������$��0,0,0,5q�5j        5k      -   5          Name$ CheckInDatePicker��5k   !   $��40��5l        5m      :  4System.Windows.Controls.Primitives.DatePickerTextBox��%T   '        �� 5n      "  6$   .�5o   "   ��  � ��
 �����6?   5p      5q      5r      5s      �� 5v      $��1��6   $
��10,0q�6-   .+�5w      �� $��Дата выезда��6    
�������$��0,0,0,5q�5y        5z      -   5        $ CheckOutDatePicker��5z   !   $��40��5{        5|      ��%T   '        �� 5}      "  6$   .�5~   "   ��  � ��
 �����6?   5      5�      5�      5�      �� 5�      -   5        $��2��5�      $��Найти номера��5�      ���#007ACC
�������$��20,10q�5�      $��40��5�      $��0q�5�      $��10,0,0,0q�5�      $��Bottom=�5�      5�      5�      5�      �� 5�   
   $��2��6   .��5�      �� -   5         ��Name$ RoomsStackPanel��5�      5�      5�      5�      5�      