using UnityEngine; 
using System.Collections;


public class NpcCollider : EntityCollider { 

	public float moveSpeed = 1;

	protected override Vector2 getNewMoveVector() {

		return new Vector2(0, 0);//TODO
	}
	
	protected override bool canRun() {
		return true;
	}
	
	protected override CharacterAction getCurrentCharacterAction() {
		return new CharacterAction(false, 100);
	}
	
	protected override BaseCharacterState getNextState() {
		return BaseCharacterState.WAIT;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		
		collide(other);		
	}
	
	void OnTriggerStay2D(Collider2D other) {
		
		collide(other);		
	}
	
	private void collide(Collider2D other) {
		
		if(Constants.GAME_OBJECT_NAME_PLAYER_COLLIDER.Equals(other.name)) {
			
			Player player = GameHelper.Instance.getPlayer();
			
			if(!player.isDead()) {
				//TODO TEST remove player life
				gameObject.GetComponent<Npc>().takeDamages(10);
			}
			
		}
	}
}

