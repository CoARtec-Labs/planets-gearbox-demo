using System;

namespace statemachine
{
    public enum States
    {
        NONE,
        AssemblyStepBase,
        AssemblyStepRing,
        AssemblyStepWheelSun,
        AssemblyStepWheelPlanet1,
        AssemblyStepWheelPlanet2,
        AssemblyStepWheelPlanet3,
        AssemblyStepCarrier,
        AssemblyStepGasket,
        AssemblyStepLid,
        StagingParts,
    }
    
    /// <summary>
    /// This class instantiates new states based on specifiers like the class id.
    /// </summary>
    public class StateFactory
    {
        public StateFactory()
        {
            
        }

        /// <summary>
        /// Create a new state instance based on given state name.
        /// </summary>
        /// <param name="stateName">State name as string.</param>
        /// <returns></returns>
        public static BaseState CreateState(string stateName)
        {
            // Turn string into enum
            States state = (States)Enum.Parse(typeof(States), stateName);
            
            return CreateState(state);
        }

        /// <summary>
        /// Create a new state instance based on given state.
        /// </summary>
        /// <param name="state">State as enum.</param>
        /// <returns></returns>
        public static BaseState CreateState(States state)
        {
            switch (state)
            {
                case States.AssemblyStepBase:
                    return new AssemblyStateStepBase();
                
                case States.AssemblyStepRing:
                    return new AssemblyStateStepRing();

                case States.AssemblyStepWheelSun:
                    return new AssemblyStateStepWheelSun();
                
                case States.AssemblyStepWheelPlanet1:
                    return new AssemblyStateStepWheelPlanet1();
                
                case States.AssemblyStepWheelPlanet2:
                    return new AssemblyStateStepWheelPlanet2();
                
                case States.AssemblyStepWheelPlanet3:
                    return new AssemblyStateStepWheelPlanet3();
                
                case States.AssemblyStepCarrier:
                    return new AssemblyStateStepCarrier();
                
                case States.AssemblyStepGasket:
                    return new AssemblyStateStepGasket();
                
                case States.AssemblyStepLid:
                    return new AssemblyStateStepLid();
                
                case States.StagingParts:
                    return new StagingState();
                
                default:
                    SystemException e = new SystemException("Unknown state " + state);
                    return null; // TODO: probably unnecessary here
            }
        }
        
    }
}