   M S B A M L   `   `   `  ����H  DFlashcardsMVP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:FlashcardsMVP.ViewModelsFlashcardsMVP.ViewModels  5   	   ?$clr-namespace:FlashcardsMVP.ServicesFlashcardsMVP.Services  5   	   &     FlashcardsMVP.Views.MyFlashcards   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:FlashcardsMVP.ViewModels     5   	   7local1$clr-namespace:FlashcardsMVP.Services     5   	     � WindowStyle$  
ToolWindow=�5	   	    � Title$ MyFlashcards��5
   	   $	��450��6   $	��800��6+    � 	Resources 5      6   0FlashcardsMVP.Services.BoolToVisibilityConverter   BoolToVisibilityConverter��%   &	          5   
   6+   5       � DataContext 5      4   .FlashcardsMVP.ViewModels.MyFlashcardsViewModel  5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��300 �6   �� 5      $��* �6   .+�5   
   �� -   5        $��DeckListBox��5      $��Stretch=�5   
   $��Stretch=�5   
   ��5   
   �� *Decks+ �� SelectedItem 5   
   �� *SelectedDeck+��5       �� 5!      .R�5"      �� .+�5#      �� $
��Bold"�6:   ��6$   �� *Name+6:   �� 5$      
���������66   �� *NumberOfCards+ �� StringFormat$ 
Cards: {0}��5%      5&      5(      �� 5)   
   $��Create New Deck��6   $��Bottom=�6W   $	��200��6r   $��10q�6~   ��6+   �� *CreateDeckMethodCommand+6~   �� 5-   
   $��1��6   $��10q�5/      $
��Left=�50      $	��Top=�51      ��5.      �� *CurrentView+51      52      53      