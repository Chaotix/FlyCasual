﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;

namespace UpgradesList
{

    public class IonCannonTurret : GenericSecondaryWeapon
    {
        public IonCannonTurret() : base()
        {
            IsHidden = true;

            Type = UpgradeSlot.Turret;

            Name = "Ion Cannon Turret";
            ShortName = "Ion Cannon Turret";
            ImageUrl = "https://vignette4.wikia.nocookie.net/xwing-miniatures/images/f/f3/Ion_Cannon_Turret.png";
            Cost = 5;

            MinRange = 1;
            MaxRange = 2;
            AttackValue = 3;
        }

        public override void AttachToShip(Ship.GenericShip host)
        {
            base.AttachToShip(host);
        }

        public override bool IsShotAvailable(Ship.GenericShip anotherShip)
        {
            bool result = true;

            if (isDiscarded) return false;

            Board.ShipShotDistanceInformation shotInfo = new Board.ShipShotDistanceInformation(Host, anotherShip);
            int range = shotInfo.Range;
            if (range < MinRange) return false;
            if (range > MaxRange) return false;

            return result;
        }

    }

}