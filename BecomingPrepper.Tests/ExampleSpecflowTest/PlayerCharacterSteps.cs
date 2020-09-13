using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BecomingPrepper.Tests.ExampleSpecflowTest
{
    [Binding]
    public class PlayerCharacterSteps
    {
        private PlayerCharacter _playerCharacter;

        [Given(@"I'm a new player")]
        public void GivenImANewPlayer()
        {
            _playerCharacter = new PlayerCharacter();
        }

        [When(@"Player takes (.*) damage")]
        public void WhenPlayerTakesDamage(int damage)
        {
            _playerCharacter.Hit(damage);
        }

        [Then(@"Players health should be (.*)")]
        public void PlayersHealthShouldBe(int health)
        {
            _playerCharacter.Health.Should().Be(health);
        }

        [Then(@"Player should die")]
        public void ThenPlayerShouldDie()
        {
            _playerCharacter.IsDead.Should().Be(true, "player took too much damage");
        }

        [Given(@"Player has damage resistance of (.*)")]
        public void GivenPlayerHasDamageResistanceOf(int damageResistance)
        {
            _playerCharacter.DamageResistance = damageResistance;
        }

        [Given(@"Player is an elf")]
        public void GivenPlayerIsAnElf()
        {
            _playerCharacter.Race = "Elf";
        }

        [Given(@"Player has the following attributes")]
        public void GivenPlayerHasTheFollowingAttributes(Table table)
        {
            //var attributes = table.CreateInstance<PlayerAttributes>();

            dynamic attributes = table.CreateDynamicInstance();

            _playerCharacter.Race = attributes.Race;
            _playerCharacter.DamageResistance = attributes.Resistance;
        }

        [Given(@"Player characterType is (.*)")]
        public void GivenPlayerCharacterTypeIsHealer(CharacterType characterType)
        {
            _playerCharacter.CharacterType = characterType;
        }

        [When(@"Casts a healing Spell")]
        public void WhenCastsAHealingSpell()
        {
            _playerCharacter.CastHealingSpell();
        }

        [Given(@"Player has the following magical items")]
        public void GivenPlayerHasTheFollowingMagicalItems(Table table)
        {
            IEnumerable<MagicalItem> items = table.CreateSet<MagicalItem>();
            _playerCharacter.MagicalItems.AddRange(items);
        }

        [Then(@"my total magical power should be (.*)")]
        public void ThenMyTotalMagicalPowerShouldBe(int expectedPower)
        {
            _playerCharacter.MagicalPower.Should().Be(expectedPower, $"we had various magical Items");
        }

        [Given(@"Player last slept (.* days ago)")]
        public void GivenPlayerLastSleptDaysAgo(DateTime lastSlept)
        {
            _playerCharacter.LastSleepTime = lastSlept;
        }

        [When(@"Player reads a restore health scroll")]
        public void WhenPlayerReadsARestoreHealthScroll()
        {
            _playerCharacter.ReadHealthScroll();
        }

        [Given(@"Player has the following weapons")]
        public void GivenPlayerHasTheFollowingWeapons(IEnumerable<Weapon> weapons)
        {
            _playerCharacter.Weapons.AddRange(weapons);
        }

        [Then(@"Players weapons should be worth (.*)")]
        public void ThenPlayersWeaponsShouldBeWorth(int value)
        {
            _playerCharacter.Weapons.Sum(x => x.Value).Should().Be(value, "Player has numerous weapons");
        }
    }
}
