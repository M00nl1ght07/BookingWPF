   M S B A M L   `   `   `  ����  
BookingWPF    BookingWPF.HomePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  HomePage��5       � 	Resources 5	         ScrollBarThumb �� 
TargetType �� Property �� Value �� 
TargetType �� RadiusX �� RadiusY ]�Name	 ]� DecreaseRepeatButton
 �� PageUpCommand J� Opacity ]� Thumb ]� IncreaseRepeatButton �� PageDownCommand��%�  &	        '��     7   �� 5
   
   " n�6'   .�5      ��  �@�OverridesDefaultStyle . 66   �� 5       � �� .  6*   �� 5       � �� 5      �� 5      " n�6&   .R�5      �� ���#007ACC$ 4��63   $ 4��6?   5      5      5      �� 5   
   " ��6   .�5      ��  � �� �#2D2D2D6+   �� 5       � ��$ 8��6&   �� 5       � �� 5      �� 5      " ��6&   .R�5      ]� $ 
PART_Track��6    ��. 	 5      �� 5   "   #��Z
 6/   $ 0Q�6\    5       n� 5!   "   8��  6(    5#      �� 5$   "   #��Z 6/   $ 0Q�6^   5&      5'      5)      5*      5+      .��5-      �� .��5.   
   �� .+�50      �� $]��WДобро пожаловать в систему бронирования отелей��6   $��28(�51      
�������$��20,30,20,20q�53       �� TextAlignment$ Center=�54      �� 57      ���#2D2D2D$��20q�58       �� Padding$ 20q�59       �� CornerRadius$ 10��5:      ._�5;      �  � ColumnDefinitions 5<      �� 5=      $��* �6+   �� 5>      $��* �6+   �� 5?      $
��Auto �6+   .+�5B      �� -   5         ��Name$ CityComboBox��5B      $��0��5C      $��5q�5D      $��10q�5E      ��5F      �� 5G      " ��6$   .�5H   "   ��  � ��
 �����6?   �� 5I   "    � �� �#1E1E1E6?   5J      .@�5L      ��  �� 
IsSelected . ���#666666.��6N   Выберите город6^   5M      (  "System.Windows.Controls.DatePicker  5O      -   5          Name$ CheckInDatePicker��5O   !   $��1��5P   !   $��5q�5Q   !   ��5R      �� 5S      "  6$   .�5T   "   ��  � ��
 �����6?   �� 5U   "    � �� �#1E1E1E6?   5V      5X        5Z      -   5        $ CheckOutDatePicker��5Z   !   $��2��5[   !   $��5q�5\   !   ��5]      �� 5^      "  6$   .�5_   "   ��  � ��
 �����6?   �� 5`   "    � �� �#1E1E1E6?   5a      5c      �� 5e      -   5        $��2��5e      $��Найти отели��5f      ���#007ACC
�������$��20,10q�5i      $	��200��5j      $��Center=�5k      $��0q�5l      5m      5n      5o      �� 5r      $%��Популярные отели��6   $��24(�5s      
�������$��
20,0,20,20q�5u      �� 5w      $
��Auto=�6   $��
20,0,20,40q�5x       5y        HorizontalScrollBarThumb �� PageLeftCommand �� PageRightCommand��%  &	       '��     7  �� 5{      " n�6=   .�5|      ��  �@�OverridesDefaultStyle . 6B   �� 5}       � �� .  66   �� 5~       � �� 5      �� 5�   "   " n�62   .R�5�   &   �� ���#007ACC$ 4��6?   $ 4��6K   5�   #   5�      5�      �� 5�      " ��6   .�5�      ��  � ��
 ���� 67   �� 5�       � ��$ 8��63   �� 5�       � �� 5�      �� 5�   "   " ��62   .R�5�   &   � 
������ .+�5�   *   �� ���#2D2D2D$ 4��6F   ]� 5�   *   $ 
PART_Track��60   	 5�   .   �� 5�   2   #��Z 6?   
������ $ 0Q�5�   =    5�   .   n� 5�   2   8��  68    5�   .   �� 5�   2   #��Z 6?   
������ $ 0Q�5�   =   5�   +   5�   '   5�   #   5�      5�      5�      .��5�      �� -   5         ��Name$ PopularHotelsPanel��5�       �� Orientation$ 
Horizontal=�5�      5�      5�      �� 5�      $3��-Специальные предложения��6   $��24(�5�      
�������$��
20,0,20,20q�5�      � 5�      $��
20,0,20,20q�6    5�      �� 5�      $��* �6'   �� 5�      $��* �6'   .+�5�      �� $��0��6   $	��200��5�      $��0,0,10,0q�5�      ���#2D2D2D$ 10��5�      ._�5�      �  5�      �� 5�      $��* �6/   �� 5�      $	��200 �6/   .+�5�      �� $��15q�6%   $��Center=�61   .+�5�      �� $��Скидка 20%��6(   $��24(�5�   (   ���#007ACC$
��Bold"�5�   (   �� 5�      $8��2на бронирование от 3-х ночей��6(   $��16(�5�   (   
�������$
��Wrap=�5�   (   $
��0,10q�5�   (   �� 5�      -   5        $��Узнать больше��5�   %   ���#007ACC
�������$
��20,5q�5�   %   $��0q�5�   %   $
��Left=�5�   %   5�   %   5�      �� 5�      $��1��6!   ���#1E1E1E$ 	0,10,10,0��5�   !   ._�5�      �� -   5         ��Name$ Image1��5�   $   $D��>pack://application:,,,/BookingWPF;component/Photos/skidka1.jpg��5�   $   $��UniformToFill=�5�   $   $��Center=�5�   $   $��Center=�5�   $   5�      5�      5�      �� 5�      $��1��6   $	��200��5�      $��10,0,0,0q�5�      ���#2D2D2D$ 10��5�      ._�5�      �  5�      �� 5�      $��* �6/   �� 5�      $	��200 �6/   .+�5�      �� $��15q�6%   $��Center=�61   .+�5�      �� $)��#Бонусная программа��6(   $��24(�5�   (   ���#007ACC$
��Bold"�5�   (   �� 5�      $[��UПолучайте до 10% кешбэка за каждое бронирование��6(   $��16(�5�   (   
�������$
��Wrap=�5�   (   $
��0,10q�5�   (   �� 5�      -   5        $��Подробнее��5�   %   ���#007ACC
�������$
��20,5q�5�   %   $��0q�5�   %   $
��Left=�5�   %   5�   %   5�      �� 5�      $��1��6!   ���#1E1E1E$ 	0,10,10,0��5�   !   ._�5      �� -	   5        $ Image2��5   $   $G��Apack://application:,,,/BookingWPF;component/Photos/cashback10.png��5  $   $��UniformToFill=�5  $   $��Center=�5  $   $��Center=�5  $   5     5     5     5     5	     5
     5     