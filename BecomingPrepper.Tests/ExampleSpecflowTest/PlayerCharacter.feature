Feature: PlayerCharacter
	In order to play the game
	as a human player
	I want my character attribute to be correctly represented

Background: 
	Given I'm a new player

@mytag @Ignore
Scenario: Taking too much damage results in player Death
	
	When Player takes 100 damage
	Then Player should die

@elf @Ignore
Scenario: Elf race character gets additional 20 damage resistance
		And Player has damage resistance of 10
		And Player is an elf
	When Player takes 40 damage
	Then Players health should be 90

@elf @Ignore
Scenario: Elf race character gets additional 20 damage resistance using data table
		And Player has the following attributes
		| attribute  | value |
		| Race       | Elf   |
		| Resistance | 10    |
	When Player takes 40 damage
	Then Players health should be 90

@Ignore
Scenario Outline: Health Reduction
	When Player takes <damage> damage
	Then Players health should be <health>

	Examples:
	| damage | health |
	| 0      | 100    |
	| 40     | 60     |
	| 50     | 50     |


@Ignore
Scenario: Healers restore all health
	Given Player characterType is Healer
	When Player takes 40 damage
		And Casts a healing Spell
	Then Players health should be 100

@Ignore
Scenario: Total magical power
	Given Player has the following magical items
	| name   | value | power |
	| Ring   | 200   | 100   |
	| Amulet | 400   | 200   |
	| Gloves | 100   | 400   |
	Then my total magical power should be 700

@Ignore
Scenario: Reading a restore health scroll when over tired has no effect
	Given Player last slept 3 days ago
	When Player takes 40 damage
		And Player reads a restore health scroll
	Then Players health should be 60

@Ignore
Scenario: Weapons are worth Money
	Given Player has the following weapons
	| name  | value |
	| Sword | 50    |
	| Pick  | 40    |
	| Knife | 10    |
	Then Players weapons should be worth 100
