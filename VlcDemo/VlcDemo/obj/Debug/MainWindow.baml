   M S B A M L   `   `   `  ����U  QVlc.DotNet.Wpf, Version=3.0.0.0, Culture=neutral, PublicKeyToken=84529da31f4eb963G4clr-namespace:Vlc.DotNet.Wpf;assembly=Vlc.DotNet.WpfVlc.DotNet.Wpf  5   	    VlcDemo   VlcDemo.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Dvlc4clr-namespace:Vlc.DotNet.Wpf;assembly=Vlc.DotNet.Wpf     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5	      �� 5
      $
��auto �6   .+�5   
   �  � ColumnDefinitions 5      �� 5      �� 5      $
��auto �6#   �� 5      $
��auto �6#   .+�5      �� -   5         ��Name$ tbUrl��5      $��
10,10,0,10q�6%   �� 5       �� Orientation$ 
Horizontal=�6   $��1��62   $��Right=�6B   .+�5      �� -   5         ��Name$ btnPlay��5      $
��Play��6*   $��Center=�69   $��10,0,0,0q�6T   $��60��6f   5      �� 5      -   5        $ btnStop��5      $
��Stop��6*   $��Center=�69   $��10,0,0,0q�6T   $��60��6f   5      5      �� 5      -   5        $ btnSnapshot��5      $��2��6*   $��Snapshot��6:   $��Right=�6M   $��Center=�6i   $��	10,0,10,0q�6�   $��60��6�   5      5         Vlc.DotNet.Wpf.VlcControl  5   
   -   5          Name$ 
vlcControl��5      $��1��6-   �� 5   
   -   5         ��Name$ txtWatermark��5      $��1��6*   $��Hello World��67   
�������$��Center=�6]   $��Center=�6z    J� Opacity$	 0.9Q�6�   $��16(�6�   �� 5   
   -   5        	 ��Name$	 
timeSlider��5      $��2��6%   
 �� IsMoveToPointEnabled
 . 62   5       5!      