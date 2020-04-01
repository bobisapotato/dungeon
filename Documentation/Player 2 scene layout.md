# Supine: Player 2 - Web app design document

![Player 2 diagram](https://i.imgur.com/5k0dA4d.png)

- **Map**
  - Live updates with icons for player, enemies, and icons
  - When items are dropped by player, they lock to nearest character on map
  - When enemies are hit they flash to show impact and sound plays to give feedback to player
- **Inventory**
  - Icons show items in inventory
  - One icon for each instance of item 
  - Icons can be clicked and dragged to the map
  - When items are dropped, message is sent to unity game with details of effect and area impacted
  - When enemies are hit by player, there’s a chance to drop an item, which the player can tap to add to inventory 
- **Attack**
  - Recharge for the tap attack
  - Has icon for attack
  - When used, recharge bar around icon drops to zero, and slowly fills up until attack can be used again 
  - When charged, player can tap on enemies or player to do small amount of damage
  - When player attacks, message is sent to unity to carry out effect