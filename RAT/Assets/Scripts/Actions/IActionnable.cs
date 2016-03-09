using System;

public interface IActionnable {

	void notifyActionShown(BaseAction action);
	void notifyActionHidden(BaseAction action);

	void notifyActionValidated(BaseAction action);

}

