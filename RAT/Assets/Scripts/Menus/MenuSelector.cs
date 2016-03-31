using System;

public class MenuSelector {

	public ISelectable selectedItem { get; private set; }
	public bool isValidated { get; private set; }

	public void selectItem(object caller, ISelectable item) {

		if(item == null) {
			throw new ArgumentException();
		}

		if(item == selectedItem) {
			//same, do nothing
			return;
		}

		deselectItem();

		selectedItem = item;
		selectedItem.onSelect();
	}

	public void deselectItem() {

		if(selectedItem == null) {
			return;
		}

		cancelSelectedItem();

		ISelectable itemRef = selectedItem;
		selectedItem = null;
		itemRef.onDeselect();
	}

	public void validateSelectedItem() {

		if(isValidated) {
			return;
		}

		if(selectedItem == null) {
			return;
		}

		isValidated = true;

		//show selection popup
		selectedItem.onSelectionValidated();
	}

	public void cancelSelectedItem() {

		if(!isValidated) {
			return;
		}

		isValidated = false;

		if(selectedItem == null) {
			return;
		}

		//hide selection popup
		selectedItem.onSelectionCancelled();
	}

}

