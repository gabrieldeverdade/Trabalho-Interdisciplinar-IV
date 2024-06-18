using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterAttributes))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(BaseInventory))]
public class Character : MonoBehaviour
{
	public bool IsPlayer = false;
	public bool CanFly = false;

	public BaseTile ActiveTile;
	public Vector2 Position;
	public Vector2 PositionWithHeight => Position + (ActiveTile.Height * new Vector2(0, 0.25f));

	CharacterAttributes CharacterAttributes;
	public BaseInventory BaseInventory;

	private void Start()
	{
		CharacterAttributes = GetComponent<CharacterAttributes>();
		BaseInventory = GetComponent<BaseInventory>();
	}

	public virtual void TakeHit(Character character)
	{
		if (!CharacterAttributes.TakeHitAndIsAlive(10, true, character))
		{
			if (IsPlayer)
			{
				Destroy(gameObject);
				SceneManager.LoadScene(2);
			} 
			else
			{
				Destroy(gameObject);
				MapManager.Instance.AddPoints(10);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var tag = collision.gameObject.tag;
		if (tag == "Enemy" && gameObject.tag == "Player")
			TakeHit(collision.gameObject.GetComponent<Character>());
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var tag = collision.gameObject.tag;
		var canUseWeapon = collision.gameObject.GetComponent<RacketAttacker>() != null && collision.gameObject.GetComponent<RacketAttacker>().IsRotating;
		if (tag == "Weapon" && gameObject.tag == "Enemy" && canUseWeapon)
			TakeHit(collision.gameObject.GetComponent<Character>());
	}

	public void UpdatePosition(Vector2 position)
	{
		Position = position;
		GetComponentInChildren<Rigidbody2D>().position = PositionWithHeight;
		transform.position = new Vector3(transform.position.x, transform.position.y, ActiveTile.Height);
		ActiveTile = MapManager.Instance.GetCellFromWorldPosition(position);
	}

	public bool CanReachHeight(BaseTile tile)
		=> Mathf.Abs(tile.Height - ActiveTile.Height) > CharacterAttributes.JumpHeight;

	public bool HasResourceNearby()
		=> ActiveTile != null && ActiveTile.Neighbours.Any(c => c.Value.Resourceable);

	public bool HasWorkbenchNearby()
		=> ActiveTile != null && ActiveTile.Neighbours.Any(c => c.Value.WorkBench);

	public BaseTile GetClosestResource() => GetClosest(c => c.Resourceable);
	public BaseTile GetClosestWorkbench() => GetClosest(c => c.WorkBench);

	public BaseTile GetClosest(Func<BaseTile, bool> condition)
	{
		if (!ActiveTile.Neighbours.Any()) return null;

		var tile = ActiveTile.Neighbours.Values.FirstOrDefault(condition);

		if (tile == null) return null;

		return tile;
	}
}
