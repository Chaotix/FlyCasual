﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubPhases
{

    public class ActionSubPhase : GenericSubPhase
    {

        public override void Start()
        {
            Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
            Name = "Action SubPhase";
            RequiredPilotSkill = PreviousSubPhase.RequiredPilotSkill;
            RequiredPlayer = PreviousSubPhase.RequiredPlayer;
            CanBePaused = true;
            UpdateHelpInfo();
        }

        public override void Initialize()
        {
            Phases.CallBeforeActionSubPhaseTrigger();

            if (!Selection.ThisShip.IsSkipsActionSubPhase)
            {
                if (!Selection.ThisShip.IsDestroyed)
                {
                    Selection.ThisShip.GenerateAvailableActionsList();
                    Triggers.RegisterTrigger(
                        new Trigger() {
                            Name = "Action",
                            TriggerOwner = Phases.CurrentPhasePlayer,
                            TriggerType = TriggerTypes.OnActionSubPhaseStart,
                            EventHandler = StartActionDecisionSubphase
                        }
                    );
                }
                else
                {
                    //Next();
                }
            }

            Phases.CallOnActionSubPhaseTrigger();
        }

        private void StartActionDecisionSubphase(object sender, System.EventArgs e)
        {
            Phases.StartTemporarySubPhase(
                "Action",
                typeof(ActionDecisonSubPhase),
                delegate () {
                    Phases.FinishSubPhase(typeof(ActionDecisonSubPhase));
                    Triggers.FinishTrigger();
                }
            );
        }

        public override void Next()
        {
            Selection.ThisShip.CallAfterActionIsPerformed(this.GetType());

            if (Phases.CurrentSubPhase.GetType() == this.GetType())
            {
                FinishPhase();
            }
        }

        public override void Pause()
        {
            Actions.CloseActionsPanel();
        }

        public override void Resume()
        {
            Actions.ShowActionsPanel();
        }

        public override void FinishPhase()
        {
            GenericSubPhase activationSubPhase = new ActivationSubPhase();
            Phases.CurrentSubPhase = activationSubPhase;
            Phases.CurrentSubPhase.Start();
            Phases.CurrentSubPhase.RequiredPilotSkill = RequiredPilotSkill;
            Phases.CurrentSubPhase.RequiredPlayer = RequiredPlayer;

            Phases.CurrentSubPhase.Next();
        }

        public override bool ThisShipCanBeSelected(Ship.GenericShip ship)
        {
            bool result = false;
            Messages.ShowErrorToHuman("Ship cannot be selected: Perform action first");
            return result;
        }

    }

}

namespace SubPhases
{

    public class ActionDecisonSubPhase : DecisionSubPhase
    {

        public override void Prepare()
        {
            infoText = "Select action";
            List<ActionsList.GenericAction> availableActions = Selection.ThisShip.GetAvailableActionsList();

            if (availableActions.Count > 0)
            {
                Roster.GetPlayer(Phases.CurrentPhasePlayer).PerformAction();
            }
            else
            {
                Messages.ShowErrorToHuman("Cannot perform any actions");
                callBack();
            }


        }

    }

}
