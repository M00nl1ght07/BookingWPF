   M S B A M L   `   `   `  ����  
BookingWPF    BookingWPF.ProfilePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  ProfilePage��5       � 	Resources 5         CustomCheckBoxStyle �� 
TargetType �� Property �� Value �� 
TargetType �Name � ColumnDefinitions ��Name	 �� CornerRadius
 F�Name �� Triggers P� Property P� Value �� 
TargetName��%D  &	        �� 5   
   " ��6,   .�5      ��  � ��
 �����6+   �� 5	       � �� �#1E1E1E6+   �� 5
       � �� �#4040406,   �� 5       � ��$ 0,0,0,10q�6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      � $ templateRoot��6   
������  5      �� 5   "   $
��Auto �63   �� 5   "   $��* �63   .+�5      �� $ checkBoxBorder��6%   #��z��5   %   #��z��5   %   $��1q�5   %   $��18��5   %   $��18��5   %   $��0,0,8,0q�5   %   $	 3��5   %   ._�5   "   F� $
 	checkMark��6'   "����@B @T� р�[ @]� р�� ��- 	���#007ACC$��2��5   '   $y�	Collapsed=�5    '   5!      �� 5"      $��1��6/   $��Center=�5#   -   $
��Left=�5$   -   5%       5&      P� 5'       �@d�	IsChecked$
 trueY�6;   . �5(   "   �� $ 	checkMark��6O    � y�$ Visible=�6?   6O   5)      P� 5*       �@J�IsMouseOver . . �5+   "   �� $ checkBoxBorder��6O    � �� �#2D2D2D�� 5,   "    � �� �#007ACC6@   5-      5/      51      52      53      .��55      � $��20q�6    � RowDefinitions 56   
   �� 57      $
��Auto �6   �� 58      $��* �6   .+�5<   
   �� $!��Личный кабинет��6   $��28(�5=      
�������$��0,0,0,20q�5?      � 5B   
   $��1��6    5C      �� 5D      $	��300 �6#   �� 5E      $��* �6#   .+�5I      �� ���#2D2D2D$	 10��5J       �� Padding$ 20q�5K      $��0,0,20,0q�5L      ._�5M      �� .+�5O      �� $	��150��6   $	��150��5P      ���#1E1E1E$	 75��5R      $��0,0,0,20q�5S      �� 5V      -   5         ��Name$ UserNameText��5V       $��24(�5W      
�������$��Center=�5Y      $��0,0,0,5q�5Z      �� 5\      -   5        $ UserEmailText��5\       $��16(�5]      ���#E0E0E0$��Center=�5_      $��0,0,0,20q�5`      �� 5c      -   5        $/��)Редактировать профиль��5c      ���#007ACC
�������$��40��5f      $��0q�5g      $��0,0,0,10q�5h      5i      �� 5k      -   5        $��
Выйти��5k      ���#DC3545
�������$��40��5n      $��0q�5o      5p      5q      5r      �� 5u      $��1��6   .+�5w      �� ���#2D2D2D$	 10��5x      $ 20q�5y      $��0,0,0,20q�5z      ._�5{      �� .+�5|      �� $��Статистика��6$   $��20(�5}   #   
�������$��0,0,0,15q�5   #   � 5�       5�      �� 5�   "   $��* �63   �� 5�   "   $��* �63   .+�5�      �� $��0��6)   .+�5�   "   �� -   5        $ BookingsCountText��5�   ,   $��32(�5�   +   ���#00A0FF$��Center=�5�   +   �� 5�   "   $��Бронирований��6,   ���#E0E0E0$��Center=�5�   +   5�      �� 5�      $��1��6)   .+�5�   "   �� -   5        $ BonusPointsText��5�   ,   $��32(�5�   +   ���#00A0FF$��Center=�5�   +   �� 5�   "   $��Бонусов��6,   ���#E0E0E0$��Center=�5�   +   5�      5�      5�      5�      �� 5�      ���#2D2D2D$	 10��5�      $ 20q�5�      ._�5�      �� .+�5�      �� $��Настройки��6$   $��20(�5�   #   
�������$��0,0,0,15q�5�   #   �� 5�      $`��ZПолучать уведомления о специальных предложениях��6#   #��[  5�   #   �� 5�      $X��RПолучать уведомления о статусе бронирования��6#   #��[  5�   #   �� 5�      $=��7Получать новости и обновления��6#   #��[  5�   #   5�      5�      5�      5�      5�      5�      