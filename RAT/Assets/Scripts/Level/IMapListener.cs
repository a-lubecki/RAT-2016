using System;

public interface IMapListener {
	
	string[] getEventIds();
	
	bool isEventAchieved(string eventId);
	
	void achieveEvent(string eventId);

}

