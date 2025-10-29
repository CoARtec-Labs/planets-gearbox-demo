using System;

namespace statemachine
{
    public enum States
    {
        NONE,
        Assembly00Base,
        Assembly10Ring,
        Assembly11Sun,
        Assembly12Planet1,
        Assembly13Planet2,
        Assembly14Planet3,
        Assembly15Carrier,
        Assembly16Gasket,
        Assembly17Lid,
        DetectionParts,
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
                case States.Assembly00Base:
                    return new AssemblyState00_Base();
                
                case States.Assembly10Ring:
                    return new AssemblyState10_Ring();

                case States.Assembly11Sun:
                    return new AssemblyState11_Sun();
                
                case States.Assembly12Planet1:
                    return new AssemblyState12_Planet1();
                
                case States.Assembly13Planet2:
                    return new AssemblyState13_Planet2();
                
                case States.Assembly14Planet3:
                    return new AssemblyState14_Planet3();
                
                case States.Assembly15Carrier:
                    return new AssemblyState15_Carrier();
                
                case States.Assembly16Gasket:
                    return new AssemblyState16_Gasket();
                
                case States.Assembly17Lid:
                    return new AssemblyState17_Lid();
                
                case States.DetectionParts:
                    return new DetectionState();
                
                default:
                    SystemException e = new SystemException("Unknown state " + state);
                    return null; // TODO: probably unnecessary here
            }
        }
        
    }
}