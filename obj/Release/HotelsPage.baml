   M S B A M L   `   `   `  ����   
BookingWPF    BookingWPF.HotelsPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       N� Title$  
HotelsPage�� � 	Resources    CustomCheckBoxStyle �� 
TargetType �� Property �� Value �� 
TargetType �Name � ColumnDefinitions ��Name	 �� CornerRadius
 F�Name �� Triggers P� Property P� Value �� 
TargetName  ScrollBarThumb �� RadiusX �� RadiusY ]�Name ]� DecreaseRepeatButton �� PageUpCommand J� Opacity ]� Thumb ]� IncreaseRepeatButton �� PageDownCommand��%�  &	        &	 �    '��     7  �� " ��.���  � ��
 �������  � �� �#1E1E1E��  � �� �#404040��  � ��$ 0,0,0,10q���  � �� �� " ��.R�� $ templateRoot��
������  �� $
��Auto ��� $��* �.+��� $ checkBoxBorder��#��z��#��z��$��1q�$��18��$��18��$��0,0,8,0q�$	 3��._�F� $
 	checkMark��"����@B @T� р�[ @]� р�� ��- 	���#007ACC$��2��$y�	Collapsed=��� $��1��$��Center=�$
��Left=� P�  �@d�	IsChecked$
 trueY�. ��� $ 	checkMark�� � y�$ Visible=�P�  �@J�IsMouseOver . . ��� $ checkBoxBorder�� � �� �#2D2D2D��  � �� �#007ACC�� " n�.���  �@�OverridesDefaultStyle . ��  � �� .  ��  � �� �� " n�.R��� ���#007ACC$ 4��$ 4���� " ��.���  � �� �#2D2D2D��  � ��$ 8����  � �� �� " ��.R�]� $ 
PART_Track����.  �� #��Z $ 0Q� n� 8��   �� #��Z $ 0Q�.���  �� $	��300 ��� $��* �.+��� ���#2D2D2D$	 15�� �� Padding$ 20q�$��0,0,20,0q�._��� .+��� $��Фильтры��$��24(�
�������$��0,0,0,20q��� $��Цена за ночь��$��16(�
�������$��0,0,0,10q�� $��0,0,0,20q� �� $��* ��� $
��Auto ��� $��* �.+��� -    ��Name$ MinPriceTextBox��$��0�����#1E1E1E
������� �� 
CaretBrush
 ��������#404040$��1q�$��40��$
��10,0q�$��Center=��� $��1��$	�� - ��
�������$��Center=�$
��10,0q��� -   $ MaxPriceTextBox��$��2�����#1E1E1E
�������
 ��������#404040$��1q�$��40��$
��10,0q�$��Center=��� $��Рейтинг��$��16(�
�������$��0,0,0,10q��� -    ��Name$ Rating5CheckBox��$��5 звезд��#��[  �� -   $ Rating4CheckBox��$��4 звезды��#��[  �� -   $ Rating3CheckBox��$��3 звезды��#��[  �� -   $ Rating2CheckBox��$��2 звезды��#��[  �� -   $'��!Применить фильтры�����#007ACC
�������$��20,10q�$��0q��� $��1��$
��Auto=�$��Disabled=�.��� -    �Name$ HotelsWrapPanel��