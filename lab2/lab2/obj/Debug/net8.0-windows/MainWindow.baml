   M S B A M L   `   `   `  ����  lab2    lab2.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$/  )Визуализация рекурсии��5   	   $	��900��6'   $
��1600��64    � 
ResizeMode$ NoResize=�6A    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5	      �� 5
      $ /UI/Styles.xaml?�6%   %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      -   5         ��Name$ LeftPanelColumn��5   +   $	��400 �6   6+   �� 5      $��* �6   .+�5   
   �� ���#F5F5F5$��0��6&    �� Padding$ 20, 20,20,0q�66   ._�5      �� -   5        $��	LeftPanel��5      $��Stretch=�6*   $
��Left=�6F   .+�5      �� $#��Выберите задачу��6   $	��240��63   $��24��6?   $��SemiBold"�6K   $��20(�6a   $��0,0,0,16q�5      $
��Left=�6.      EuclidSquareSemiBoldBold#��[  6I   	� 5      -   5        $+��%Отрисовка фракталов��5       	� 	GroupName$ AlgorithmType��6<   $	��300��6V   $��30��6b   $
��Left=�6n   $��Regular"�6�   $��16(�6�   $��0,0,0,10q�5        EuclidSquareRegular#��[ 60   6b   	� 5      -   5        $#��Ханойские башни��5      $ AlgorithmType��68   $	��300��6R   $��30��6^   $
��Left=�6j   $��Regular"�6�   $��16(�6�   $��0,0,0,10q�5      #��[ 60   6b   �� 5       -   5        	 ��Name$	 PageContentControl��5    !   $��0,20,0,0q�6=   5!      5"       � 5%   
   -   5        $��	MainFrame��5%      $��1��6!   
  � NavigationUIVisibility$
 Hidden=�61   5&      5'      