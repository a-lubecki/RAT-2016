using System;

public interface ISelectable {
	
	void onSelect();
	void onDeselect();

	void onSelectionValidated();
	void onSelectionCancelled();

}

