# pinch

# About

A repo for Elevator code test.

# Requirements

You are in charge of writing software for an elevator (lift) company.
Your task is to write a program to control the travel of a lift for a 10 storey building.
A passenger can summon the lift to go up or down from any floor, once in the lift they can choose the floor they’d like to travel to.
Your program needs to plan the optimal set of instructions for the lift to travel, stop, and open its doors.

Some test cases:

    - Passenger summons lift on the ground floor. Once in, chooses to go to level 5.
    - Passenger summons lift on level 6 to go down. Passenger on level 4 summons the lift to go down. They both choose L1.
    - Passenger 1 summons lift to go up from L2. Passenger 2 summons lift to go down from L4. Passenger 1 chooses to go to L6. Passenger 2 chooses to go to Ground Floor
    - Passenger 1 summons lift to go up from Ground. They choose L5. Passenger 2 summons lift to go down from L4. Passenger 3 summons lift to go down from L10. Passengers 2 and 3 choose to travel to Ground.

You can implement this in any language you like (.NET preferred) and submit a working solution including tests and a readme explaining how to run it. Submissions on GitHub or BitBucket are preferred.

Please let me know if you have any questions.

# How to run

Elevator.IntegrationTests is a console app which can be executed in Debug mode in VS. Scenario 4 is set up. Uncomment code in Worker.cs class for other scenarios to be simulated.

Elevator.Tests contains standard unit tests which can be executed with Test Explorer in VS.


# Assumptions

1) If elevator is more than 2 floors away then it's able to slow down and stop safely.

2) American or Australian system? Floor 1 is not Ground floor. Floor 0 is Ground Floor.

3) Assume all floor numbers will be sequential. Not all buildings contain sequential floor numbers eg: For superstitious reasons floor 13 is often skipped.

# Considerations

A) Safety requirements, eg: Acceleration and velocity likely have limits. Weight and passenger limits.

B) Wear and tear from over using one elevator. Should maybe spread the use of each elevator evenly.

C) Ancillary services such as health checking, better logging etc could be implemented.

D) The logic in Elevator class is the real meat that should be unit tested but requires re-factoring in such a way to maintain statefulness and also inject services for test. Due to limited time have opted to only note unit tests required.

# Thought Process

A) Identify main models.

B) Identify some basic actions for most basic requirement of moving to a called floor and back to ground.

C) Implement basic requirement of just 1 elevator and 2 floors and confirm with unit test.

D) Expand requirements to work with 2 elevators.

E) Optimise system for better efficiency. 


# Expected Output

As an example for Scenario 4, we should expect elevator to move to floors 5, 10, 4, 0 in that order, even though the button press order is different.

```
Passengers boarding
Doors closing...
GO!
Elevator is now at floor 1
Elevator is now at floor 2
Elevator is now at floor 3
Elevator is now at floor 4
Elevator is now at floor 5
Arrived at floor 5. Trigger opening doors
Doors opening...
Passengers boarding
Doors closing...
GO!
Elevator is now at floor 6
Elevator is now at floor 7
Elevator is now at floor 8
Elevator is now at floor 9
Elevator is now at floor 10
Arrived at floor 10. Trigger opening doors
Doors opening...
Passengers boarding
Doors closing...
GO!
Elevator is now at floor 9
Elevator is now at floor 8
Elevator is now at floor 7
Elevator is now at floor 6
Elevator is now at floor 5
Elevator is now at floor 4
Arrived at floor 4. Trigger opening doors
Doors opening...
Passengers boarding
Doors closing...
GO!
Elevator is now at floor 3
Elevator is now at floor 2
Elevator is now at floor 1
Elevator is now at floor 0
Arrived at floor 0. Trigger opening doors
Doors opening...
Passengers boarding
Doors closing...
GO!

```