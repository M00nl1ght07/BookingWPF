   M S B A M L   `   `   `  ����e  aMicrosoft.Web.WebView2.Wpf, Version=1.0.3124.44, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf  5       
BookingWPF"   BookingWPF.AttractionMapPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      \wpfLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf     5        N� Title$  Места рядом��5       � 	Resources 5         ThinScrollBarStyle �� 
TargetType �� Property �� Value �� 
TargetType ]�Name ]� Thumb �� RadiusX	 �� RadiusY��%�  &	        �� 5   
   " ��6+   .�5	      ��  � ��$ 8��6&   �� 5
       � ��
 ���� 6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $��8��6(   ���#1A1A1A5   &   ]� 5      $ 
PART_Track��6$    5   "   n� 5   &   ��5   *   �� 5   .   .R�5   2   �� $��8��6<   #��z��5   :   ���#0078D7$ 4��5   :   $	 4��5   :   5   /   5   '   5      5       5!      5#      5$      5%      .��5'      � 
 � ColumnDefinitions
 5(   
   �� 5)      $	��300 �6   �� 5*      $��* �6   .+�5.   
   ]� $��0��6   ��. .+�50      �� $	��Top=�6   ���#2D2D2D$��
10,10,10,0q�52       �� Padding$ 10q�53       �� CornerRadius$ 10��54      ._�55      � 
 56      �� 57      $��* �6+   �� 58      $
��Auto �6+   .+�5:      �� -   5         ��Name$ 	SearchBox��5:      $��0��5;      $��0,0,5,0q�5<      $��5q�5=      ���#404040
�������$��0q�5@      5A      �� 5B      -   5        $��1��5B      $
��🔍��5C      
������ 
�������$��0q�5F      5G      5H      5I      �� 5L      $
��Auto=�6   $��Disabled=�5M      ��5N      �� 5O      " ��6   .�5P      ��  � �� 5Q      �� 5R   "   " ��62   .R�5S   &   � 
 5T   *   �� 5U   .   $��* �6?   �� 5V   .   $
��Auto �6?   .+�5X   *   �� $��0��6A   �� 5Y   *    ��Name$ PART_VerticalScrollBar��64   $��1��5Z   2    �� VerticalOffset � Value# z 5[   2    �� ScrollableHeight � Maximum# z 5\   2    �� ViewportHeight �� ViewportSize# z 5]   2   #��[  5^   2   * �� #ComputedVerticalScrollBarVisibility#y�z 5_   2   5`   '   5a   #   5c      5d      .��5f      �� -   5         ��Name$ CategoriesPanel��5f      $��10q�66   5h      5i      5j      �� 5m   
   $��1��6   ���#333333$��10q�5o      ._�5p      )   #Microsoft.Web.WebView2.Wpf.WebView2  -   5          Name$ MapView��5p      5q      5r      5s      