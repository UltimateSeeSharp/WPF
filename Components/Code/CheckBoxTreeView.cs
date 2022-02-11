    private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
    {
        foreach (var item in treeViewCanAddresses.ItemContainerGenerator.Items)
        {
            if (!(item is CanAddress canAddress)) return;
            var count = parent.sourceChildren.Count;
            var selectedCount = parent.sourceChildren.Count(x => x.Included);
            if (count == selectedCount)
            {
                canAddress.Included = true;
            }
            else if (selectedCount > 0 && selectedCount != count)
            {
                canAddress.Included = null;
            }
            else
            {
                canAddress.Included = false;
            }
        }

        (DataContext as ViewModel)!.UpdateSelectedItemsInView();
    }
