   M S B A M L   `   `   `  ����H  DFlashcardsMVP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:FlashcardsMVP.ViewModelsFlashcardsMVP.ViewModels  5      -    'FlashcardsMVP.Views.DeckInformationView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      8local&clr-namespace:FlashcardsMVP.ViewModels     5      $	��300��5      $	��500��6     � DataContext  5      �� 5	   
    �� Path$ CurrentViewModel�6   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��5q�6"   .+�5      �� $��20(�62   $
��Bold"�6@   ��6   �� *
DeckName+6@   �� 5      $��14(�6S   ��6   �� *NumberOfCards+ �� StringFormat$ 	{0} cards��6S   5      �� 5!   
   $��1��6   $��5q�6$   .��5"      �� $
��Auto=�64   ��6   �� *Cards+��5#      �� 5$      .R�5&      �� .+�5'      �� $��16(�6?   $
��Bold"�6M   ��6(   �� *Front+6M   �� 5)      $��5,0,0,0q�6>   ��6(   �� *Back+6>   5*      5+      5-      5.      �� 51   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��5q�6W   .+�52      �� $��Learn��6   $��5q�6%   ���#448E64$��50��6E   $��25��6P   �� 53      $
��Edit��6   $��5q�6$   $��50��6/   $��25��6:   ��6F   �� *EditDeckCommand+�� 54      $��Export��6   $��5q�6&   $��50��61   $��25��6<   ��6H   �� *ExportDeckCommand+�� 55      $��Delete��6   $��5q�6&   ���#C74A4C$��50��6F   $��25��6Q   ��6]   �� *DeleteDeckCommand+56      57      58      