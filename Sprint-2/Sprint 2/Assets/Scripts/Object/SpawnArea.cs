using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnArea: MonoBehaviour
{
	[SerializeField] public BaseTile SpawnTile;
	[SerializeField] Transform SpawnLocation;
	[SerializeField] Character EnemyPrefab;
	[SerializeField] Character Destination;

	public List<Enemy> Enemies = new List<Enemy>();

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var tag = collision.gameObject.tag;

		if (tag == "Weapon")
		{
			var character = gameObject.GetComponentInParent<CharacterManager>();

			if (character == null) return;

			var weapon = character.GetCurrentWeapon();

			if (weapon == null || !weapon.CanHitEnemies) return;

			if (!GetComponent<CharacterAttributes>().TakeHitAndIsAlive(weapon.Attack, false))
			{
				MapManager.Instance.AddPoints(500);
				Destroy(gameObject);
			}
		}
	}

	private void Update()
	{
		InitializeActiveTile();
	}

	public void Spawn(List<float[]> enemies)
	{
		for(var i = 0; i < enemies.Count; i++)
		{
			var enemy = Instantiate(EnemyPrefab, SpawnLocation);

			var enemyComponent = enemy.GetComponent<Enemy>();
			enemyComponent.SetJob(this, enemies[i]);

			if (enemyComponent.IsAttacker)
			{
				enemy.GetComponent<ResourceFinderController>().enabled = false;
				enemy.GetComponent<CharacterStalkController>().enabled = true;
				enemy.GetComponent<CharacterStalkController>().SetDestination(Destination);
			}
			else
			{
				enemy.GetComponent<ResourceFinderController>().enabled = true;
				enemy.GetComponent<CharacterStalkController>().enabled = false;
			}
			Enemies.Add(enemy.GetComponent<Enemy>());
		}
	}

	void InitializeActiveTile() { if (SpawnTile == null) SpawnTile = MapManager.Instance.GetCellFromWorldPosition(SpawnLocation.position); }

}
