﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Movement
{

    public class KoiogranTurnMovement : StraightMovement
    {
        public KoiogranTurnMovement(int speed, ManeuverDirection direction, ManeuverBearing bearing, ManeuverColor color) : base(speed, direction, bearing, color)
        {

        }

        protected override void ManeuverEndRotation(Action callBack)
        {
            if (!Selection.ThisShip.IsBumped)
            {
                Phases.StartTemporarySubPhase("Koiogran Turn", typeof(SubPhases.KoiogranTurnSubPhase), callBack);
            }
            else
            {
                Messages.ShowError("Koiogran Turn is failed due to collision");
                callBack();
            }
        }

    }

}

