using System;

namespace UI.States
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
        Instructor00Base,
        InstructorRing,
        Instructor10Ring,
        Instructor11Sun,
        Instructor12Planet1,
        Instructor13Planet2,
        Instructor14Planet3,
        Instructor15Carrier,
        Instructor16Gasket,
        Instructor17Lid,
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
                    return new Assembly.AssemblyState00_Base();
                
                case States.Assembly10Ring:
                    return new Assembly.AssemblyState10_Ring();

                case States.Assembly11Sun:
                    return new Assembly.AssemblyState11_Sun();
                
                case States.Assembly12Planet1:
                    return new Assembly.AssemblyState12_Planet1();
                
                case States.Assembly13Planet2:
                    return new Assembly.AssemblyState13_Planet2();
                
                case States.Assembly14Planet3:
                    return new Assembly.AssemblyState14_Planet3();
                
                case States.Assembly15Carrier:
                    return new Assembly.AssemblyState15_Carrier();
                
                case States.Assembly16Gasket:
                    return new Assembly.AssemblyState16_Gasket();
                
                case States.Assembly17Lid:
                    return new Assembly.AssemblyState17_Lid();
                
                case States.Instructor00Base:
                    return new Instructor.AssemblyState00_Base();
                
                case States.Instructor10Ring:
                    return new Instructor.AssemblyState10_Ring();

                case States.Instructor11Sun:
                    return new Instructor.AssemblyState11_Sun();
                
                case States.Instructor12Planet1:
                    return new Instructor.AssemblyState12_Planet1();
                
                case States.Instructor13Planet2:
                    return new Instructor.AssemblyState13_Planet2();
                
                case States.Instructor14Planet3:
                    return new Instructor.AssemblyState14_Planet3();
                
                case States.Instructor15Carrier:
                    return new Instructor.AssemblyState15_Carrier();
                
                case States.Instructor16Gasket:
                    return new Instructor.AssemblyState16_Gasket();
                
                case States.Instructor17Lid:
                    return new Instructor.AssemblyState17_Lid();
                
                default:
                    SystemException e = new SystemException("Unknown state " + state);
                    return null; // TODO: probably unnecessary here
            }
        }
        
    }
}